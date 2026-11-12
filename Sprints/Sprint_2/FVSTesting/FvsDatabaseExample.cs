using System;
using System.Collections.Generic;
using System.IO;
using FRASS.FVS.Models;
using FRASS.FVS.Services;

namespace FRASS.FVS.Examples
{
    /// <summary>
    /// Example usage of the FvsDatabaseBuilder service.
    /// Demonstrates how to create a FVS-compatible SQLite database with sample forestry inventory data.
    /// </summary>
    public class FvsDatabaseExample
    {
        /// <summary>
        /// Creates a sample FVS database with example forestry inventory data.
        /// </summary>
        /// <param name="outputPath">Path where the database file should be created. 
        /// If null, creates in the current directory as "FVS_Input.db".</param>
        /// <returns>The full path to the created database file.</returns>
        public static string CreateSampleDatabase(string outputPath = null)
        {
            // Default output path if none provided
            if (string.IsNullOrWhiteSpace(outputPath))
            {
                Console.WriteLine("Using default output path");
                outputPath = Path.Combine(Directory.GetCurrentDirectory(), "FVS_Input.db");
            }

            Console.WriteLine("Building default trees and timberstands");
            // Create sample stand data
            var stands = CreateSampleStands();

            // Create sample tree data
            var trees = CreateSampleTrees();

            // Build the database
            var builder = new FvsDatabaseBuilder();
            Console.WriteLine("Attempting to build database");
            try
            {
                builder.BuildDatabase(outputPath, stands, trees);
                Console.WriteLine($"Successfully created FVS database at: {outputPath}");
                Console.WriteLine($"  - Inserted {stands.Count} stand record(s)");
                Console.WriteLine($"  - Inserted {trees.Count} tree record(s)");
                return outputPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating FVS database: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Creates sample stand-level inventory data.
        /// Represents typical forestry stand parameters for the Pacific Northwest (PN variant).
        /// </summary>
        private static List<StandRecord> CreateSampleStands()
        {
            return new List<StandRecord>
            {
                new StandRecord
                {
                    Stand_ID = "STAND001",
                    Variant = "PN",  // Pacific Northwest variant
                    Inv_Year = 2024,
                    Groups = "A",
                    Location = 614,  // Gifford Pinchot National Forest
                    Site_Species = "DF",  // Douglas-fir
                    Site_Index = 120.0,  // 120 feet at base age 50
                    Basal_Area_Factor = 40.0,  // BAF 40 variable radius plot
                    Inv_Plot_Size = null,  // Not used with variable radius
                    Num_Plots = 5
                },
                new StandRecord
                {
                    Stand_ID = "STAND002",
                    Variant = "PN",
                    Inv_Year = 2024,
                    Groups = "B",
                    Location = 614,
                    Site_Species = "WH",  // Western Hemlock
                    Site_Index = 110.0,
                    Basal_Area_Factor = 0,  // Fixed plot
                    Inv_Plot_Size = 20.0,  // 1/20th acre plot
                    Num_Plots = 3
                }
            };
        }

        /// <summary>
        /// Creates sample tree-level inventory data.
        /// Represents individual tree measurements for multiple species.
        /// </summary>
        private static List<TreeRecord> CreateSampleTrees()
        {
            return new List<TreeRecord>
            {
                // Stand 001 - Douglas-fir dominated stand
                new TreeRecord
                {
                    Stand_ID = "STAND001",
                    Plot_ID = "P1",
                    Tree_ID = 1,
                    Tree_Count = 12.5,  // Trees per acre expansion factor
                    History = 1,  // Live tree
                    Species = "DF",  // Douglas-fir
                    DBH = 18.5,  // inches
                    DG = null,  // No growth data
                    HT = 95.0,  // feet
                    HTG = null,
                    Cr_Ratio = 45  // 45% crown ratio
                },
                new TreeRecord
                {
                    Stand_ID = "STAND001",
                    Plot_ID = "P1",
                    Tree_ID = 2,
                    Tree_Count = 12.5,
                    History = 1,
                    Species = "DF",
                    DBH = 22.3,
                    DG = null,
                    HT = 105.0,
                    HTG = null,
                    Cr_Ratio = 50
                },
                new TreeRecord
                {
                    Stand_ID = "STAND001",
                    Plot_ID = "P1",
                    Tree_ID = 3,
                    Tree_Count = 12.5,
                    History = 1,
                    Species = "WH",  // Western Hemlock
                    DBH = 14.2,
                    DG = null,
                    HT = 78.0,
                    HTG = null,
                    Cr_Ratio = 55
                },
                new TreeRecord
                {
                    Stand_ID = "STAND001",
                    Plot_ID = "P2",
                    Tree_ID = 1,
                    Tree_Count = 12.5,
                    History = 8,  // Dead tree (snag)
                    Species = "DF",
                    DBH = 16.0,
                    DG = null,
                    HT = 65.0,  // Broken top
                    HTG = null,
                    Cr_Ratio = 0  // No crown
                },
                
                // Stand 002 - Western Hemlock dominated stand
                new TreeRecord
                {
                    Stand_ID = "STAND002",
                    Plot_ID = "P1",
                    Tree_ID = 1,
                    Tree_Count = 20.0,  // Fixed plot expansion
                    History = 1,
                    Species = "WH",
                    DBH = 24.5,
                    DG = 0.15,  // 0.15 inches/year growth
                    HT = 110.0,
                    HTG = 1.2,  // 1.2 feet/year height growth
                    Cr_Ratio = 60
                },
                new TreeRecord
                {
                    Stand_ID = "STAND002",
                    Plot_ID = "P1",
                    Tree_ID = 2,
                    Tree_Count = 20.0,
                    History = 1,
                    Species = "RC",  // Western Redcedar
                    DBH = 28.7,
                    DG = 0.12,
                    HT = 115.0,
                    HTG = 0.8,
                    Cr_Ratio = 65
                },
                new TreeRecord
                {
                    Stand_ID = "STAND002",
                    Plot_ID = "P2",
                    Tree_ID = 1,
                    Tree_Count = 20.0,
                    History = 1,
                    Species = "WH",
                    DBH = 12.8,
                    DG = 0.18,
                    HT = 72.0,
                    HTG = 1.5,
                    Cr_Ratio = 70
                }
            };
        }

        /// <summary>
        /// Example of creating a database with separate Create and Insert operations.
        /// Demonstrates more granular control over the database generation process.
        /// </summary>
        public static void CreateDatabaseStepByStep(string databasePath)
        {
            var builder = new FvsDatabaseBuilder();

            // Step 1: Create the empty database with schema
            Console.WriteLine("Step 1: Creating database schema...");
            builder.CreateDatabase(databasePath);
            Console.WriteLine("  Schema created successfully.");

            // Step 2: Prepare data
            Console.WriteLine("Step 2: Preparing inventory data...");
            var stands = CreateSampleStands();
            var trees = CreateSampleTrees();
            Console.WriteLine($"  Prepared {stands.Count} stands and {trees.Count} trees.");

            // Step 3: Insert data
            Console.WriteLine("Step 3: Inserting inventory data...");
            builder.InsertInventoryData(databasePath, stands, trees);
            Console.WriteLine("  Data inserted successfully.");

            Console.WriteLine($"\nDatabase ready at: {databasePath}");
        }

        /// <summary>
        /// Example of error handling when working with the database builder.
        /// </summary>
        public static void DemonstrateErrorHandling()
        {
            var builder = new FvsDatabaseBuilder();
            var testPath = Path.Combine(Path.GetTempPath(), "test_fvs.db");

            try
            {
                // Attempt to insert data into non-existent database
                var stands = new List<StandRecord>
                {
                    new StandRecord 
                    { 
                        Stand_ID = "TEST", 
                        Variant = "PN", 
                        Inv_Year = 2024 
                    }
                };
                var trees = new List<TreeRecord>();

                builder.InsertInventoryData("nonexistent.db", stands, trees);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Expected error caught: {ex.Message}");
            }

            try
            {
                // Create database successfully
                builder.CreateDatabase(testPath);
                Console.WriteLine($"Successfully created test database at: {testPath}");

                // Clean up
                if (File.Exists(testPath))
                {
                    File.Delete(testPath);
                    Console.WriteLine("Test database cleaned up.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
