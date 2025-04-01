
namespace Ocr
{
    public class OcrProcessor
    {
        private readonly IOcr pdfOcrProcessor;
        private readonly IOcr imageOcrProcessor;

        public OcrProcessor(PdfOcrProcessor pdfOcrProcessor, ImageOcrProcessor imageOcrProcessor)
        {
            this.pdfOcrProcessor = pdfOcrProcessor;
            this.imageOcrProcessor = imageOcrProcessor;

            InitializeDirectories();
        }

        private void InitializeDirectories()
        {
            if (!Directory.Exists(pdfOcrProcessor.WorkingDirectory))
                Directory.CreateDirectory(pdfOcrProcessor.WorkingDirectory);


            if (!Directory.Exists(imageOcrProcessor.WorkingDirectory))
                Directory.CreateDirectory(imageOcrProcessor.WorkingDirectory);
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


        public async Task<string> ProcessOcr(string filename, byte[] file)
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

            var result = await Task.Run(() => ocrProcessor.ProcessOcr(filename, file));

            return result;
        }
    }
}