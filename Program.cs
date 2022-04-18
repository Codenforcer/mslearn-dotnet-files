using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

var currentDirectory = Directory.GetCurrentDirectory(); //Get the current directory.
var storesDirectory = Path.Combine(currentDirectory, "stores");

var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);

var salesFiles = FindFiles(storesDirectory);

var salesTotal = CalculateSalesTotal(salesFiles);

File.WriteAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal}{Environment.NewLine}");//create an empty file with salestotaldir folder.


// Program iterates through the folders storing the individual sales.json files from the store 
// folder and save it the above variable salesFiles for printing.
IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);
        // The file name will contain the full path, so only check the end of it.
        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;
    // Loops each file path in slaesFiles.
    foreach (var file in salesFiles)
    {
        //read contents of file.
        string salesJson = File.ReadAllText(file);

        // Parse the contents as Json.
        salesData? data = JsonConvert.DeserializeObject<salesData?>(salesJson);

        // Add the amount from the Total field to the salesTotal variable.
        salesTotal += data?.Total ?? 0;
    }

    return salesTotal;
}
record salesData(double Total);
