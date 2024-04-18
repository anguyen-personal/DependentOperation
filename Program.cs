using DependentOperation.Services;
using System.Net.Http.Headers;

try
{
    var filename = args.Length >= 1 ? args[0] : GetUserInput("Please enter file name, including full path");
    bool hasHeader = args.Length >= 2 ? Convert.ToBoolean(args[1]) : GetBoolInput("Does the file contain the headers 'Dependency' and 'Item'? (N/y)");

    var input = DependencyService.OpenCsvFile(filename, hasHeader);
    var order = DependencyService.CreateDependencyResult(input);
    foreach (var line in order)
    {
        Console.WriteLine(string.Join(", ", line));
    }
}
catch(CsvHelper.HeaderValidationException) { Console.WriteLine("The file did not have the correct headers: Dependency, Item!"); }
catch (Exception ex) { Console.WriteLine($"An error occurred processing the result: {ex.Message}"); }


string GetUserInput(string message)
{
    var input = string.Empty;
    while (string.IsNullOrEmpty(input))
    {
        Console.WriteLine($"{message}. Control-C to end.");
        input = Console.ReadLine();
    }
    return input;
}
bool GetBoolInput(string message)
{
    var input = string.Empty;
    Console.WriteLine(message);
    input = Console.ReadLine()?.ToLower();
    if(string.IsNullOrEmpty(input)) return false;
    return input?.StartsWith("y") == true;
}