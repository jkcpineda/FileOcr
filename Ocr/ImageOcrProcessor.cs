using Tesseract;

namespace Ocr
{
    public interface IImageOcrProcessor : IOcr { }
    public class ImageOcrProcessor : IImageOcrProcessor
    {
        public string TesseractDataPath { get; set; }

        public ImageOcrProcessor(string tesseractDataPath)
        {
            this.TesseractDataPath = tesseractDataPath;
        }

        public string ProcessOcr(string filename)
        {
            using (TesseractEngine engine = new TesseractEngine(TesseractDataPath, "eng"))
            using (var image = Pix.LoadFromFile(filename))
            using (var page = engine.Process(image))
            {
                var txtPath = $"{filename}.txt";
                FileUtility.TryDelete(txtPath);

                var ocrContent = page.GetText();
                File.WriteAllText(txtPath, ocrContent);

                return ocrContent;
            }
        }
    }
}
