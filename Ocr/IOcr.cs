namespace Ocr
{
    public interface IOcr
    {
        public string TesseractDataPath { get; set; }
        string ProcessOcr(string filename);
    }
}
