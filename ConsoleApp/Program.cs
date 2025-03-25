// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Ocr;
using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                var tesseractDataPath = @"C:\Users\judit\Downloads\tessdata-main\tessdata-main";
                services.AddSingleton(tesseractDataPath);

                services.AddScoped<IPdfOcrProcessor>(provider => new PdfOcrProcessor(provider.GetRequiredService<string>()));
                services.AddScoped<IImageOcrProcessor>(provider => new ImageOcrProcessor(provider.GetRequiredService<string>()));
                services.AddScoped(provider => new OcrProcessor(
                    provider.GetRequiredService<IPdfOcrProcessor>(),
                    provider.GetRequiredService<IImageOcrProcessor>()
                ));
            })
            .Build();

var ocrProcessor = host.Services.GetRequiredService<OcrProcessor>();

Console.WriteLine(await ocrProcessor.ProcessOcr(@"C:\temp\sample.pdf"));
Console.WriteLine(await ocrProcessor.ProcessOcr(@"C:\temp\sample2-0.png"));
Console.WriteLine(await ocrProcessor.ProcessOcr(@"C:\temp\sample2-1.png"));
Console.WriteLine(await ocrProcessor.ProcessOcr(@"C:\temp\sample2.pdf"));

await host.RunAsync();