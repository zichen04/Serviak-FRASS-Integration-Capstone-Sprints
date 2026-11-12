using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FRASS.FVS.Examples;


namespace FVSTesting
{
    public static class FVSKeyFile
    {
        static string R_PATH = FvsUtils.R_PATH;
        static string KEY_FILE_BUILDER_SCRIPT_PATH = FvsUtils.GetKeyFileBuilderScriptPath();
        /// <summary>
        /// Uses an R script to create an rFVS keyfile
        /// </summary>
        /// <param name="keyFileName">The name of the key file</param>
        /// <param name="runTitle">the name of the generated FVS run</param>
        /// <param name="standIDs">the IDs of the timberstands</param>
        /// <param name="inDataBase">The input database</param>
        /// <param name="outDataBase">The output database</param>
        /// <param name="ncycles">Total number of cycles run</param>
        /// <param name="moreKeywords">Placeholder for non-specified FVS Keywords</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task CreateAsync(
            string keyFileName,
            string runTitle,
            string[] standIDs,
            string inDataBase,
            string outDataBase,
            int ncycles,
            string[] moreKeywords = null)
        {
            string rScript = R_PATH;
            string keyFileBuilderPath = KEY_FILE_BUILDER_SCRIPT_PATH;


            // Setup the parameters for our R terminal
            var startInfo = new ProcessStartInfo
            {
                FileName = rScript,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true

            };

            // Pass our parameters as command-line arguments
            startInfo.Arguments =
                "\"" + keyFileBuilderPath + "\" " +
                "\"" + keyFileName + "\" " +
                "\"" + runTitle + "\" " +
                "\"" + string.Join(",", standIDs) + "\" " +
                "\"" + inDataBase + "\" " +
                "\"" + outDataBase + "\" " +
                ncycles + " " +
                "\"" + string.Join("\n", moreKeywords ?? new string[0]) + "\"";

            Console.WriteLine("Starting R process...");
            Console.WriteLine("R executable: " + rScript);
            Console.WriteLine("Arguments: " + startInfo.Arguments);

            using (Process process = new Process())
            {
                process.StartInfo = startInfo;
                process.Start();


                string output = await process.StandardOutput.ReadToEndAsync();
                string errors = await process.StandardError.ReadToEndAsync();

                await Task.Run(() => process.WaitForExit());

                // Change to a try/catch where the try returns the location of the key file
                if (process.ExitCode != 0)
                {
                    throw new Exception("R failed  to create keyfile with the exit code" + process.ExitCode + ": \n" + errors);
                }
                // Currently just prints, remove once we are returning the location of the key file
                Console.WriteLine(output);
            }
            ;
        }
        public static async Task CreateYearZeroAsync(
            string runTitle,
            string[] standIDs,
            string inDataBase,
            string outDataBase,
            string[] moreKeywords = null)
        {
            string rScript = R_PATH;
            string keyFileBuilderPath = KEY_FILE_BUILDER_SCRIPT_PATH;

            // Setup the parameters for our R terminal
            var startInfo = new ProcessStartInfo
            {
                FileName = rScript,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true

            };

            // Pass our parameters as command-line arguments
            // Year 0 run has some more specific parameters
            startInfo.Arguments =
                "\"" + keyFileBuilderPath + "\" " +
                "\"" + "year_zero.key" + "\" " +
                "\"" + runTitle + "\" " +
                "\"" + string.Join(",", standIDs) + "\" " +
                "\"" + inDataBase + "\" " +
                "\"" + outDataBase + "\" " +
                0 + " " +
                "\"" + string.Join("\n", moreKeywords ?? new string[0]) + "\"";

            using (Process process = new Process())
            {
                process.StartInfo = startInfo;
                process.Start();


                string output = await process.StandardOutput.ReadToEndAsync();
                string errors = await process.StandardError.ReadToEndAsync();

                await Task.Run(() => process.WaitForExit());

                // Change to a try/catch where the try returns the location of the key file
                if (process.ExitCode != 0)
                {
                    throw new Exception("R failed to create year 0 keyfile with the exit code" + process.ExitCode + ": \n" + errors);
                }
                // Currently just prints, remove once we are returning the location of the key file
                Console.WriteLine(output);
            }
            ;
        }
    }
}