using CsvHelper;
using CsvHelper.Configuration;
using DependentOperation.Models;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DependentOperation.Services
{
    public static class DependencyService
    {
        private const string ITEM = "item";
        private const string DEPENDENCY = "dependency";
        static List<string> listErors = new List<string>();
        /// <summary>
        /// Opens a csv file at the given path. If path is not provided, assume file is in same folder as executable
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>2 dimensional array of the file</returns>
        public static string[,] OpenCsvFile(string filename = "items.csv", bool hasHeader = false)
        {
            string[,] result;
            var fileExtension = Path.GetExtension(filename);
            string[] validExtension = { ".csv", ".txt" };
            if (!validExtension.Contains(fileExtension)) throw new Exception("File name must be .csv or .txt");
            //check if only the filename was provided without a fully qualified path ie c:\temp\items.csv
            //assume that the file is in the same location as the executable
            if (!Path.IsPathFullyQualified(filename)) filename = $"{Environment.CurrentDirectory}\\{filename}";
            if (!File.Exists(filename)) throw new Exception("File provided does not exist");

            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ",", HasHeaderRecord = hasHeader };
            using (var reader = new StreamReader(filename))
            using (var csv = new CsvReader(reader, config, false)) 
            {
                //load the contents of the file into an array of the model
                var records = csv.GetRecords<DependencyModel>().ToArray();
                // Get the properties of the first object to determine the number of columns
                var properties = records.First().GetType().GetProperties();

                // Create a 2D array based on the number of rows (records) and columns (properties.Length)
                result = new string[records.Count(), properties.Length];

                // Populate the array with string representations of object properties
                for (int i = 0; i < records.Count(); i++)
                {
                    var obj = records[i];
                    for (int j = 0; j < properties.Length; j++)
                    {
                        // Get the property value as string
                        var value = properties[j].GetValue(obj)?.ToString() ?? string.Empty;
                        result[i, j] = value;
                    }
                }
               
            }
            return result;
        }
        /// <summary>
        /// Sort a 2 dimensional array of items to create a hiearical order based on dependent itms
        /// </summary>
        /// <param name="listOfItems"></param>
        /// <returns>list of strings</returns>
        public static List<List<string>> CreateDependencyResult(string[,] listOfItems)
        {
            var result = new List<List<string>>();
            if (listOfItems != null)
            {
                var chart = new Dictionary<string, List<string>>();
                var level = new Dictionary<string, int>();
                

                // Build the chart and calculate level for each item
                for (int i = 0; i < listOfItems.GetLength(0); i++)
                {
                    string dependency = listOfItems[i, 0];
                    string item = listOfItems[i, 1];
                    ValidateDependencyItem(dependency, ITEM,item, DEPENDENCY);
                    ValidateDependencyItem(item, DEPENDENCY, dependency, ITEM);
                    if (!chart.ContainsKey(dependency)) //dependency was not found, at to chart with level 0
                    {
                        chart[dependency] = new List<string>();
                        level[dependency] = 0;
                    }

                    if (!chart.ContainsKey(item)) //item was not found, at to chart with level 0
                    {
                        chart[item] = new List<string>();
                        level[item] = 0;
                    }

                    chart[dependency].Add(item);
                    level[item]++;
                }
                //check if the ValidateDependencyItem generated error with the rows
                if (listErors.Count > 0) { throw new Exception($"The file contains error(s):\n{string.Join("\n", listErors)}"); }

                //items that do not have dependencies
                var initialItems = level.Where(kv => kv.Value == 0).Select(kv => kv.Key).ToList();

                while (initialItems.Any()) //loop if there are any items
                {
                    var currentLevel = new List<string>();
                    var nextItems = new List<string>();

                    foreach (var item in initialItems)
                    {
                        currentLevel.Add(item);

                        foreach (var dependent in chart[item]) //loop through all dependent items
                        {
                            level[dependent]--; //decrease the level by 1
                            if (level[dependent] == 0)
                            {
                                nextItems.Add(dependent);
                            }
                        }
                    }

                    currentLevel.Sort(); // Sort items alphabetically within the same level
                    result.Add(currentLevel);
                    initialItems = nextItems; //prepare for next set of items
                }

            }

            return result;

        }

        static void ValidateDependencyItem(string value, string label, string found, string element)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                listErors.Add($"The {label} '{found}' has a missing or blank {element}.");
            }
        }
    }
}
