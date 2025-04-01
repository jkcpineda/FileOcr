
namespace Ocr
{
    public class OcrOptions
    {
        public const string Ocr = "Ocr";
        public string TesseractDataPath { get; set; }

        public string WorkingDirectory { get; set; }

        public int FileLengthLimit { get; set; }
    }
}
