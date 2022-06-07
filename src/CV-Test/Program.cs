// put your Cognitive Service Key1 value
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

string Key = "facee84a442a408c86c020dbbb91d9f0";
// put your Cognitive Service URL
string url = "https://caydev-cv-service.cognitiveservices.azure.com/";


var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(Key));
client.Endpoint = url;

var myfile = File.OpenRead(@"C:\Users\MN35\Desktop\test3a.jpg");
//var result = client.RecognizePrintedTextInStreamAsync(false, myfile);
//result.Wait();

var textHeaders = await client.ReadInStreamAsync(myfile);
string operationLocation = textHeaders.OperationLocation;

Thread.Sleep(2000);

// Retrieve the URI where the extracted text will be stored from the Operation-Location header.
// We only need the ID and not the full URL
const int numberOfCharsInOperationId = 36;
string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

// Extract the text
ReadOperationResult results;
Console.WriteLine($"Extracting text from URL file...");
Console.WriteLine();
do
{
    results = await client.GetReadResultAsync(Guid.Parse(operationId));
}
while ((results.Status == OperationStatusCodes.Running ||
    results.Status == OperationStatusCodes.NotStarted));

// Display the found text.
Console.WriteLine();
var textUrlFileResults = results.AnalyzeResult.ReadResults;
foreach (ReadResult page in textUrlFileResults)
{
    foreach (Line line in page.Lines)
    {
        var texts = line.Text.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var text in texts)
        {
            var isDigit = text.All(e => char.IsDigit(e));

            if (isDigit)
                Console.WriteLine("\t\t" + text);
            else
                Console.WriteLine(line.Text);
        }



    }
}


//var rst = result.Result;
//foreach (var r in rst.Regions)
//{
//    foreach (var t in r.Lines)
//    {
//        foreach (var w in t.Words)
//        {
//            var coordinates = w.BoundingBox.Split(',');
//            Console.WriteLine($"Found word {w.Text} at" +
//            $" X {coordinates[0]} and Y {coordinates[1]}");
//        }
//    }
//}

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


