using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FVSTesting
{
    /// <summary>
    /// This class holds functions to communicate with R to run FVS on a database and keyfile
    /// </summary>
    public static class FVSRunner
    {
        private const string R_PATH = FvsUtils.R_PATH;
        // Path to script in FRASS
        private static string FVS_RUN_PATH = FvsUtils.GetRunFVSScriptPath();
        /// <summary>
        /// This function runs FVS on a keyfile using a specific model
        /// </summary>
        /// <param name="model">The model to use</param>
        /// <param name="bin">The folder that holds all models</param>
        /// <param name="keyFile">The keyfile passed to FVS</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task RunAsync(
            string model,
            string bin,
            string keyFile)
        {
            // Setup the parameters for our R terminal
            var startInfo = new ProcessStartInfo
            {
                FileName = R_PATH,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true

            };

            startInfo.Arguments = "\"" + FVS_RUN_PATH + "\" " + "\"" + model + "\" " + "\"" + bin + "\" " + "\"" + keyFile + "\"";

            Console.WriteLine("Starting R process...");
            Console.WriteLine("R executable: " + R_PATH);
            Console.WriteLine("Arguments: " + startInfo.Arguments);

            using (Process process = new Process())
            {
                process.StartInfo = startInfo;
                process.Start();


                Task<string> outputTask =
                        process.StandardOutput.ReadToEndAsync();

                Task<string> errorTask =
                    process.StandardError.ReadToEndAsync();

                await Task.WhenAll(outputTask, errorTask);
                await process.WaitForExitAsync();

                string output = outputTask.Result;
                string errors = errorTask.Result;


                // Change to a try/catch where the try returns the location of the key file
                if (process.ExitCode != 0)
                {
                    throw new Exception("R failed  to create run FVS with error code: " + process.ExitCode + ": \n" + errors);
                }
                // Currently just prints, remove once we are returning the location of the output database
                Console.WriteLine("R output:\n" + output);
                //Console.WriteLine("R errors:\n" + errors);
            }
            ;

        }
        public static void DoDefaultFVSRuns()
        {
            // Do a year 0 run
                // this is how we get our tree numbering

            // anually, 15 years

            // Every 5 years, for 250
        }
    }
}
