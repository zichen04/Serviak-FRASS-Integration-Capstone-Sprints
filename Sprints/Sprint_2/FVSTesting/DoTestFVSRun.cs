using FRASS.FVS.Examples;
using System;
using System.Collections.Generic;
using System.Text;

namespace FVSTesting
{
    /// <summary>
    /// Makes a test FVS run using the FVS in serviak and the data in serviak.  Runs on serviak
    /// </summary>
    internal class DoTestFVSRun
    {
        public async void ExecuteTestRun()
        {
            string model = "FVSpn";
            string bin = FvsUtils.FVS_SERVER_PATH;
            string keyFilePath = "";


            // GET FVS Location on serviak
            string dbLocation = FvsUtils.FVS_DATA_PATH + FvsUtils.TARGET_TEST_DATABASE;
            if (File.Exists(dbLocation))
            {
                await FVSTesting.FVSKeyFile.CreateAsync("TestFile.key", "TestRun", ["STAND001"], dbLocation, "output.db", 10);

                keyFilePath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "TestFile.key"
                );

                await FVSTesting.FVSRunner.RunAsync(model, bin, keyFilePath);
            }
            else
            {
                Console.WriteLine("Database does not exist");
            }

            Console.ReadKey();
        }
    }
}
