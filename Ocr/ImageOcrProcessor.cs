using Microsoft.Extensions.Options;
using Tesseract;

namespace Ocr
{
    public class ImageOcrProcessor : IOcr
    {
        public string TesseractDataPath { get; set; }
        public string WorkingDirectory { get; set; }

        public ImageOcrProcessor(IOptions<OcrOptions> settings)
        {
            TesseractDataPath = settings.Value.TesseractDataPath;
            WorkingDirectory = settings.Value.WorkingDirectory;
        }

        public async Task<string> ProcessOcr(string filename, byte[] file)
        {
            var fullPath = Path.Combine(WorkingDirectory, filename);
            await File.WriteAllBytesAsync(fullPath, file);

            return await ExtractTextFromImage(filename);
        }

        public async Task<string> ProcessOcr(string filename)
        {
            return await ExtractTextFromImage(filename);
        }

        private async Task<string> ExtractTextFromImage(string filename)
        {
            return await Task.Run(() =>
            {
                var fullPath = Path.Combine(WorkingDirectory, filename);

                using (TesseractEngine engine = new TesseractEngine(TesseractDataPath, "eng"))
                using (var image = Pix.LoadFromFile(fullPath))
                using (var page = engine.Process(image))
                {
                    var txtPath = $"{fullPath}.txt";
                    FileUtility.TryDelete(txtPath);

                    var ocrContent = page.GetText();
                    File.WriteAllText(txtPath, ocrContent);

                    return ocrContent;
                }
            });
        }
    }
}
