using System.Text;
using Tesseract;

namespace Ocr
{
    public interface IPdfOcrProcessor : IOcr { }
    public class PdfOcrProcessor : IPdfOcrProcessor
    {
        public string TesseractDataPath { get; set; }

        public PdfOcrProcessor(string tesseractDataPath)
        {
            this.TesseractDataPath = tesseractDataPath;
        }
        
        public string ProcessOcr(string filename)
        {
            var tiffFile = PdfConverter.MakeTiff(filename);
            var outputFile = DoOcr(tiffFile);

            return outputFile;
        }

        private string DoOcr(string filename)
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
        }
    }
}
