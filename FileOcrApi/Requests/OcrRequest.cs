namespace FileOcrApi.Requests
{
    public class OcrRequest
    {
        public string Filename { get; set; } = string.Empty;
        public byte[] File { get; set; } = Array.Empty<byte>();
    }
}
