//// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.Json;

var client = new HttpClient();

var jsonContentPdf = new StringContent(
    JsonSerializer.Serialize(new
    {
        filename = $"{Guid.NewGuid()}.pdf",
        file = Convert.ToBase64String(File.ReadAllBytes(@"C:\temp\Lorem Ipsum.pdf"))
    }), Encoding.UTF8, "application/json");

Console.WriteLine(await client.PostAsync("http://localhost:5041/api/ocr", jsonContentPdf));

var jsonContentPng = new StringContent(
    JsonSerializer.Serialize(new
    {
        filename = $"{Guid.NewGuid()}.png",
        file = Convert.ToBase64String(File.ReadAllBytes(@"C:\temp\Lorem Ipsum.png"))
    }), Encoding.UTF8, "application/json");

Console.WriteLine(await client.PostAsync("http://localhost:5041/api/ocr", jsonContentPng));

Console.ReadKey();