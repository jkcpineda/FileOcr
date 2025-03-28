// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Ocr;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

using var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                // Ensure appsettings.json is loaded
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })

            .ConfigureServices((context, services) =>
            {
                var tesseractDataPath = context.Configuration["TesseractDataPath"] ?? string.Empty;

                services.AddSingleton(tesseractDataPath);
                services.AddScoped<IPdfOcrProcessor>(provider => new PdfOcrProcessor(provider.GetRequiredService<string>()));

                services.AddScoped<IImageOcrProcessor>(provider => new ImageOcrProcessor(tesseractDataPath));
                services.AddScoped<OcrProcessor>();
            })
            .Build();

var ocrProcessor = host.Services.GetRequiredService<OcrProcessor>();

Console.WriteLine(await ocrProcessor.ProcessOcr(@"C:\temp\sample.pdf"));
Console.WriteLine(await ocrProcessor.ProcessOcr(@"C:\temp\sample2-0.png"));
Console.WriteLine(await ocrProcessor.ProcessOcr(@"C:\temp\sample2-1.png"));
Console.WriteLine(await ocrProcessor.ProcessOcr(@"C:\temp\sample2.pdf"));

await host.RunAsync();
