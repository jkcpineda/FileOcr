namespace Ocr
{
    public class OcrProcessor
    {
        private readonly IOcr pdfOcrProcessor;
        private readonly IOcr imageOcrProcessor;

        public OcrProcessor(IPdfOcrProcessor pdfOcrProcessor, IImageOcrProcessor imageOcrProcessor)
        {
            this.pdfOcrProcessor = pdfOcrProcessor;
            this.imageOcrProcessor = imageOcrProcessor;
        }

        public async Task<string> ProcessOcr(string filename)
        {
            var fileExtension = Path.GetExtension(filename);

            IOcr ocrProcessor;

            switch (fileExtension.ToLower())
            {
                case ".pdf":
                    ocrProcessor = pdfOcrProcessor;
                    break;
                default:
                    ocrProcessor = imageOcrProcessor;
                    break;
            }

            var result = await Task.Run(() => ocrProcessor.ProcessOcr(filename));

            return result;
        }
    }
}