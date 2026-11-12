/// This program tests Serviak's FVS capability by using recorded FVS data to create a year 0 (1 cycle) run.

using FRASS.FVS.Examples;
using FVSTesting;

string model = "FVSpn";
string bin = @"C:\FVS\FVSSoftware\FVSbin";
string keyFilePath = "";


//string dbLocation = FvsUtils.FVS_DATA_PATH + "\\" + FvsUtils.TARGET_TEST_DATABASE;

string dbLocation = @"F:\FRASS_FVS\DATABASES\FVS_STAND001_20261029_125048.db";

if(Path.Exists(dbLocation))
{
    await FVSTesting.FVSKeyFile.CreateAsync("TestFile.key", "TestRun", ["STAND001"], dbLocation, "output.db", 1);

    keyFilePath = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "TestFile.key"
    );

    await FVSTesting.FVSRunner.RunAsync(model, bin, keyFilePath);

}
else
{
    Console.WriteLine("Database did not exist\n");
}

// Year 0 run example
//await FVSTesting.FVSKeyFile.CreateAsync("year_zero.key", "year_zero", ["Stand002"], dbLocation, "year_zero.db", 0);

//keyFilePath = Path.Combine(
//    AppDomain.CurrentDomain.BaseDirectory,
//    "year_zero.key"
//);

//await FVSTesting.FVSRunner.RunAsync(model, bin, keyFilePath);