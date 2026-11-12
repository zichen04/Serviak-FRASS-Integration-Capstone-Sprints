using System;

namespace FRASS.FVS.Models
{
    /// <summary>
    /// Represents an individual tree record for the FVS_TreeInit table.
    /// Contains individual tree measurements required by the Forest Vegetation Simulator.
    /// </summary>
    public class TreeRecord
    {
        /// <summary>
        /// Stand identifier - Must match Stand_ID in FVS_StandInit.
        /// </summary>
        public string Stand_ID { get; set; }

        /// <summary>
        /// Plot identifier within the stand.
        /// </summary>
        public string Plot_ID { get; set; }

        /// <summary>
        /// Unique tree number within the plot.
        /// </summary>
        public int Tree_ID { get; set; }

        /// <summary>
        /// Expansion factor (Trees per acre) - number of trees this record represents.
        /// </summary>
        public double Tree_Count { get; set; }

        /// <summary>
        /// Tree status code: 1 = Live, 8 = Dead, etc.
        /// </summary>
        public int History { get; set; }

        /// <summary>
        /// FVS species code (e.g., 'DF' for Douglas-fir, 'PP' for Ponderosa Pine).
        /// </summary>
        public string Species { get; set; }

        /// <summary>
        /// Diameter at breast height (inches).
        /// </summary>
        public double DBH { get; set; }

        /// <summary>
        /// Diameter growth (optional, inches/year).
        /// </summary>
        public double? DG { get; set; }

        /// <summary>
        /// Total tree height (feet).
        /// </summary>
        public double HT { get; set; }

        /// <summary>
        /// Height growth (optional, feet/year).
        /// </summary>
        public double? HTG { get; set; }

        /// <summary>
        /// Crown ratio percent (0-100).
        /// </summary>
        public int Cr_Ratio { get; set; }
    }
}
