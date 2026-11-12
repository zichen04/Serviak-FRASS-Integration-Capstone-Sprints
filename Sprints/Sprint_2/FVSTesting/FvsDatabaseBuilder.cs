using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using FRASS.FVS.Models;

namespace FRASS.FVS.Services
{
    /// <summary>
    /// Robust SQLite database generator for the US National Forest Service's 
    /// Forest Vegetation Simulator (FVS) Database Extension.
    /// Implements transactional inserts with parameterized queries for security and performance.
    /// </summary>
    public class FvsDatabaseBuilder
    {
        private const string FVS_STANDINIT_TABLE = @"
            CREATE TABLE IF NOT EXISTS FVS_StandInit (
                Stand_ID TEXT PRIMARY KEY NOT NULL,
                Variant TEXT,
                Inv_Year INTEGER,
                Groups TEXT,
                Location INTEGER,
                Site_Species TEXT,
                Site_Index REAL,
                Basal_Area_Factor REAL,
                Inv_Plot_Size REAL,
                Num_Plots INTEGER
            );";

        private const string FVS_TREEINIT_TABLE = @"
            CREATE TABLE IF NOT EXISTS FVS_TreeInit (
                Stand_ID TEXT NOT NULL,
                Plot_ID TEXT,
                Tree_ID INTEGER NOT NULL,
                Tree_Count REAL NOT NULL,
                History INTEGER NOT NULL,
                Species TEXT NOT NULL,
                DBH REAL NOT NULL,
                DG REAL,
                HT REAL NOT NULL,
                HTG REAL,
                Cr_Ratio INTEGER NOT NULL,
                FOREIGN KEY (Stand_ID) REFERENCES FVS_StandInit(Stand_ID)
            );";

        /// <summary>
        /// Creates a new FVS-compatible SQLite database with the required schema.
        /// </summary>
        /// <param name="databasePath">Full path to the .db file to create.</param>
        /// <exception cref="ArgumentException">Thrown when databasePath is null or empty.</exception>
        /// <exception cref="SQLiteException">Thrown when database creation or schema creation fails.</exception>
        /// <exception cref="IOException">Thrown when file system operations fail.</exception>
        public void CreateDatabase(string databasePath)
        {
            if (string.IsNullOrWhiteSpace(databasePath))
            {
                throw new ArgumentException("Database path cannot be null or empty.", nameof(databasePath));
            }

            try
            {
                // Safely handle existing file - delete if present
                if (File.Exists(databasePath))
                {
                    File.Delete(databasePath);
                }

                // Ensure directory exists
                string directory = Path.GetDirectoryName(databasePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Create the database file
                SQLiteConnection.CreateFile(databasePath);

                // Create schema
                using (var connection = new SQLiteConnection($"Data Source={databasePath};Version=3;"))
                {
                    connection.Open();

                    using (var command = new SQLiteCommand(connection))
                    {
                        // Create FVS_StandInit table
                        command.CommandText = FVS_STANDINIT_TABLE;
                        command.ExecuteNonQuery();

                        // Create FVS_TreeInit table
                        command.CommandText = FVS_TREEINIT_TABLE;
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SQLiteException ex)
            {
                throw new SQLiteException($"Failed to create FVS database: {ex.Message}", ex);
            }
            catch (IOException ex)
            {
                throw new IOException($"File system error while creating database: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Inserts forestry inventory data into an existing FVS database using robust 
        /// transactional operations with parameterized queries.
        /// </summary>
        /// <param name="databasePath">Full path to the existing .db file.</param>
        /// <param name="stands">Collection of stand-level records to insert.</param>
        /// <param name="trees">Collection of individual tree records to insert.</param>
        /// <exception cref="ArgumentException">Thrown when databasePath is null/empty or collections are null.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the database file doesn't exist.</exception>
        /// <exception cref="SQLiteException">Thrown when database operations fail.</exception>
        public void InsertInventoryData(
            string databasePath, 
            IEnumerable<StandRecord> stands, 
            IEnumerable<TreeRecord> trees)
        {
            if (string.IsNullOrWhiteSpace(databasePath))
            {
                throw new ArgumentException("Database path cannot be null or empty.", nameof(databasePath));
            }

            if (stands == null)
            {
                throw new ArgumentNullException(nameof(stands), "Stand records collection cannot be null.");
            }

            if (trees == null)
            {
                throw new ArgumentNullException(nameof(trees), "Tree records collection cannot be null.");
            }

            if (!File.Exists(databasePath))
            {
                throw new FileNotFoundException("Database file not found. Create the database first.", databasePath);
            }

            // Convert to lists for better performance (avoid multiple enumerations)
            var standList = stands.ToList();
            var treeList = trees.ToList();

            if (!standList.Any() && !treeList.Any())
            {
                return; // Nothing to insert
            }

            SQLiteTransaction transaction = null;

            try
            {
                using (var connection = new SQLiteConnection($"Data Source={databasePath};Version=3;"))
                {
                    connection.Open();

                    // Begin transaction for bulk insert performance and data integrity
                    transaction = connection.BeginTransaction();

                    try
                    {
                        // Insert stand records
                        if (standList.Any())
                        {
                            InsertStandRecords(connection, transaction, standList);
                        }

                        // Insert tree records
                        if (treeList.Any())
                        {
                            InsertTreeRecords(connection, transaction, treeList);
                        }

                        // Commit the transaction
                        transaction.Commit();
                    }
                    catch
                    {
                        // Rollback on any error to maintain data integrity
                        transaction?.Rollback();
                        throw;
                    }
                }
            }
            catch (SQLiteException ex)
            {
                throw new SQLiteException($"Failed to insert inventory data: {ex.Message}", ex);
            }
            finally
            {
                transaction?.Dispose();
            }
        }

        /// <summary>
        /// Creates a complete FVS database with inventory data in one operation.
        /// </summary>
        /// <param name="databasePath">Full path to the .db file to create.</param>
        /// <param name="stands">Collection of stand-level records to insert.</param>
        /// <param name="trees">Collection of individual tree records to insert.</param>
        public void BuildDatabase(
            string databasePath,
            IEnumerable<StandRecord> stands,
            IEnumerable<TreeRecord> trees)
        {
            CreateDatabase(databasePath);
            InsertInventoryData(databasePath, stands, trees);
        }

        /// <summary>
        /// Inserts stand records using parameterized queries to prevent SQL injection and formatting errors.
        /// </summary>
        private void InsertStandRecords(
            SQLiteConnection connection, 
            SQLiteTransaction transaction, 
            List<StandRecord> stands)
        {
            const string insertSql = @"
                INSERT INTO FVS_StandInit 
                (Stand_ID, Variant, Inv_Year, Groups, Location, Site_Species, 
                 Site_Index, Basal_Area_Factor, Inv_Plot_Size, Num_Plots)
                VALUES 
                (@Stand_ID, @Variant, @Inv_Year, @Groups, @Location, @Site_Species, 
                 @Site_Index, @Basal_Area_Factor, @Inv_Plot_Size, @Num_Plots);";

            using (var command = new SQLiteCommand(insertSql, connection, transaction))
            {
                // Add parameters once, then reuse for each record
                command.Parameters.Add("@Stand_ID", System.Data.DbType.String);
                command.Parameters.Add("@Variant", System.Data.DbType.String);
                command.Parameters.Add("@Inv_Year", System.Data.DbType.Int32);
                command.Parameters.Add("@Groups", System.Data.DbType.String);
                command.Parameters.Add("@Location", System.Data.DbType.Int32);
                command.Parameters.Add("@Site_Species", System.Data.DbType.String);
                command.Parameters.Add("@Site_Index", System.Data.DbType.Double);
                command.Parameters.Add("@Basal_Area_Factor", System.Data.DbType.Double);
                command.Parameters.Add("@Inv_Plot_Size", System.Data.DbType.Double);
                command.Parameters.Add("@Num_Plots", System.Data.DbType.Int32);

                foreach (var stand in stands)
                {
                    command.Parameters["@Stand_ID"].Value = stand.Stand_ID ?? (object)DBNull.Value;
                    command.Parameters["@Variant"].Value = stand.Variant ?? (object)DBNull.Value;
                    command.Parameters["@Inv_Year"].Value = stand.Inv_Year;
                    command.Parameters["@Groups"].Value = stand.Groups ?? (object)DBNull.Value;
                    command.Parameters["@Location"].Value = stand.Location.HasValue ? (object)stand.Location.Value : DBNull.Value;
                    command.Parameters["@Site_Species"].Value = stand.Site_Species ?? (object)DBNull.Value;
                    command.Parameters["@Site_Index"].Value = stand.Site_Index.HasValue ? (object)stand.Site_Index.Value : DBNull.Value;
                    command.Parameters["@Basal_Area_Factor"].Value = stand.Basal_Area_Factor.HasValue ? (object)stand.Basal_Area_Factor.Value : DBNull.Value;
                    command.Parameters["@Inv_Plot_Size"].Value = stand.Inv_Plot_Size.HasValue ? (object)stand.Inv_Plot_Size.Value : DBNull.Value;
                    command.Parameters["@Num_Plots"].Value = stand.Num_Plots.HasValue ? (object)stand.Num_Plots.Value : DBNull.Value;

                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Inserts tree records using parameterized queries to prevent SQL injection and formatting errors.
        /// </summary>
        private void InsertTreeRecords(
            SQLiteConnection connection, 
            SQLiteTransaction transaction, 
            List<TreeRecord> trees)
        {
            const string insertSql = @"
                INSERT INTO FVS_TreeInit 
                (Stand_ID, Plot_ID, Tree_ID, Tree_Count, History, Species, 
                 DBH, DG, HT, HTG, Cr_Ratio)
                VALUES 
                (@Stand_ID, @Plot_ID, @Tree_ID, @Tree_Count, @History, @Species, 
                 @DBH, @DG, @HT, @HTG, @Cr_Ratio);";

            using (var command = new SQLiteCommand(insertSql, connection, transaction))
            {
                // Add parameters once, then reuse for each record
                command.Parameters.Add("@Stand_ID", System.Data.DbType.String);
                command.Parameters.Add("@Plot_ID", System.Data.DbType.String);
                command.Parameters.Add("@Tree_ID", System.Data.DbType.Int32);
                command.Parameters.Add("@Tree_Count", System.Data.DbType.Double);
                command.Parameters.Add("@History", System.Data.DbType.Int32);
                command.Parameters.Add("@Species", System.Data.DbType.String);
                command.Parameters.Add("@DBH", System.Data.DbType.Double);
                command.Parameters.Add("@DG", System.Data.DbType.Double);
                command.Parameters.Add("@HT", System.Data.DbType.Double);
                command.Parameters.Add("@HTG", System.Data.DbType.Double);
                command.Parameters.Add("@Cr_Ratio", System.Data.DbType.Int32);

                foreach (var tree in trees)
                {
                    command.Parameters["@Stand_ID"].Value = tree.Stand_ID ?? (object)DBNull.Value;
                    command.Parameters["@Plot_ID"].Value = tree.Plot_ID ?? (object)DBNull.Value;
                    command.Parameters["@Tree_ID"].Value = tree.Tree_ID;
                    command.Parameters["@Tree_Count"].Value = tree.Tree_Count;
                    command.Parameters["@History"].Value = tree.History;
                    command.Parameters["@Species"].Value = tree.Species ?? (object)DBNull.Value;
                    command.Parameters["@DBH"].Value = tree.DBH;
                    command.Parameters["@DG"].Value = tree.DG.HasValue ? (object)tree.DG.Value : DBNull.Value;
                    command.Parameters["@HT"].Value = tree.HT;
                    command.Parameters["@HTG"].Value = tree.HTG.HasValue ? (object)tree.HTG.Value : DBNull.Value;
                    command.Parameters["@Cr_Ratio"].Value = tree.Cr_Ratio;

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
