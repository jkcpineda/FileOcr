using Microsoft.Extensions.Options;
using System.Text;
using Tesseract;

namespace Ocr
{
    public class PdfOcrProcessor : IOcr
    {
        public string TesseractDataPath { get; set; }

        public string WorkingDirectory { get; set; }

        public PdfOcrProcessor(IOptions<OcrOptions> settings)
        {
            TesseractDataPath = settings.Value.TesseractDataPath;
            WorkingDirectory = settings.Value.WorkingDirectory;
        }

        public async Task<string> ProcessOcr(string filename, byte[] file)
        {
            var fullPath = Path.Combine(WorkingDirectory, filename);
            await File.WriteAllBytesAsync(fullPath, file);

            var tiffFile = await PdfConverter.MakeTiff(fullPath);
            var outputFile = await DoOcr(tiffFile);

            return outputFile;
        }

        public async Task<string> ProcessOcr(string filename)
        {
            var tiffFile = await PdfConverter.MakeTiff(filename);
            var outputFile = await DoOcr(tiffFile);

            return outputFile;
        }

        private async Task<string> DoOcr(string filename)
        {
            return await Task.Run(() =>
            {
                var tiffBytes = File.ReadAllBytes(filename);
                var extractedText = new StringBuilder();

                using (TesseractEngine engine = new TesseractEngine(TesseractDataPath, "eng"))
                {
                    using (PixArray pages = PixArray.LoadMultiPageTiffFromFile(filename))
                    {
                        foreach (Pix p in pages)
                        {
                            using (Page page = engine.Process(p))
                            {
                                var text = page.GetText();
                                extractedText.Append(text);
                            }
                        }
                    }
                }

                var txtPath = $"{filename}.txt";
                FileUtility.TryDelete(txtPath);

                var ocrContent = extractedText.ToString();
                File.WriteAllText(txtPath, ocrContent);

                return ocrContent;
            });
        }
    }
}
