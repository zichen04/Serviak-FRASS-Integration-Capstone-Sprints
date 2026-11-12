using System;

namespace FRASS.FVS.Models
{
    /// <summary>
    /// Represents a stand-level record for the FVS_StandInit table.
    /// Contains stand-level forestry inventory parameters required by the Forest Vegetation Simulator.
    /// </summary>
    public class StandRecord
    {
        /// <summary>
        /// Stand identifier - Primary Key/Foreign Key link to tree records.
        /// </summary>
        public string Stand_ID { get; set; }

        /// <summary>
        /// 2-letter FVS variant code (e.g., 'PN' for Pacific Northwest, 'IE' for Inland Empire).
        /// </summary>
        public string Variant { get; set; }

        /// <summary>
        /// 4-digit inventory year.
        /// </summary>
        public int Inv_Year { get; set; }

        /// <summary>
        /// Optional FVS grouping codes.
        /// </summary>
        public string Groups { get; set; }

        /// <summary>
        /// National Forest location code.
        /// </summary>
        public int? Location { get; set; }

        /// <summary>
        /// Species code for site index calculation.
        /// </summary>
        public string Site_Species { get; set; }

        /// <summary>
        /// Site index value (feet at base age).
        /// </summary>
        public double? Site_Index { get; set; }

        /// <summary>
        /// Basal Area Factor (use 0 or NULL for fixed plots).
        /// </summary>
        public double? Basal_Area_Factor { get; set; }

        /// <summary>
        /// Inverse of the fixed plot size (1/plot_size_acres).
        /// </summary>
        public double? Inv_Plot_Size { get; set; }

        /// <summary>
        /// Number of plots in the stand.
        /// </summary>
        public int? Num_Plots { get; set; }
    }
}
