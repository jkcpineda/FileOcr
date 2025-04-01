namespace Ocr
{
    public interface IOcr
    {
        public string TesseractDataPath { get; set; }

        public string WorkingDirectory { get; set; }
        Task<string> ProcessOcr(string filename);
        Task<string> ProcessOcr(string filename, byte[] file);
    }
}
