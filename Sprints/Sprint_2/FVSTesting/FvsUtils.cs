using System;
using System.Collections.Generic;
using System.Text;

namespace FVSTesting
{
    /// <summary>
    /// This class implements shared functions and constants between rFVS c# classes
    /// </summary>
    public static class FvsUtils
    {
        // Path to RScript, assums true path
        // Replace this with the FVS inside of R
        public const string R_PATH = @"C:\FVS\FVSSoftware\R\R-4.5.0\bin\Rscript.exe";
        // Depricate old references
        //public const string KEY_FILE_BUILDER_SCRIPT_PATH = @"C:\Users\Tizio\Documents\Repos\Capstone\FrassEnv\FVSIntegration\TestEnv\FVSTesting\FVSKeyFileBuilder.r";
        //public const string RUN_FVS_SCRIPT_PATH = "C:\\Users\\Tizio\\Documents\\Repos\\Capstone\\FrassEnv\\FVSIntegration\\TestEnv\\FVSTesting\\RunFVS.r";
        // Replace them with these local paths
        static string keyFileBuilderScriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FVSKeyFileBuilder.r");
        static string runFvsScriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RunFVS.r");
        public const string FVS_SERVER_PATH = "C:\\FVS\\FVSSoftware\\FVSbin";
        public const string FVS_DATA_PATH = @"F:\FRASS_FVS\DATABASES";
        // Test database (Most recent in folder)
        public const string TARGET_TEST_DATABASE = "FVS_STAND001_20261029_125048";

        public static string GetKeyFileBuilderScriptPath()
        {
            return keyFileBuilderScriptPath;
        }
        public static string GetRunFVSScriptPath()
        {
            return runFvsScriptPath;
        }

    }
}