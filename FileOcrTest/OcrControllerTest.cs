using FileOcrApi.Controllers;
using FileOcrApi.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Ocr;

namespace FileOcrTest
{
    [TestClass]
    public sealed class OcrControllerTest
    {
        private readonly Mock<ILogger<OcrController>> logger = new Mock<ILogger<OcrController>>();
        private readonly OcrProcessor ocrProcessor;
        private readonly Mock<IOptions<OcrOptions>> options = new Mock<IOptions<OcrOptions>>();
        private readonly string ExpectedLoremIpsumPdfText;

        public OcrControllerTest()
        {
            options.Setup(opt => opt.Value)
                .Returns(new OcrOptions
            {
                FileLengthLimit = 1000000,
                TesseractDataPath = "tessdata",
                WorkingDirectory = "temp"
            });
            
            ocrProcessor = new OcrProcessor(new PdfOcrProcessor(options.Object),
                new ImageOcrProcessor(options.Object));

            ExpectedLoremIpsumPdfText = File.ReadAllText(@"Pdf/Lorem Ipsum.pdf.txt");
        }

        [TestMethod]
        public async Task Ocr_ShouldNotExtractText_WhenFileIsEmpty()
        {
            var controller = new OcrController(logger.Object, options.Object, ocrProcessor);
            var request = new OcrRequest
            {
                Filename = "Large.pdf",
                File = Array.Empty<byte>()
            };

            var result = await controller.Process(request);

            if (result is BadRequestObjectResult badResult)
            {
                Assert.AreEqual(StatusCodes.Status400BadRequest, badResult.StatusCode);
            }
            else
            {
                Assert.Fail("Result is not BadRequestObjectResult");
            }
        }

        [TestMethod]
        public async Task Ocr_ShouldExtractText_WhenPdfFileSizeIsOK()
        {
            var controller = new OcrController(logger.Object, options.Object, ocrProcessor);
            var request = new OcrRequest
            {
                Filename = "Lorem Ipsum.pdf",
                File = await File.ReadAllBytesAsync(@"Pdf/Lorem Ipsum.pdf")
            };

            var result = await controller.Process(request);

            if (result is OkObjectResult okResult)
            {
                Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
                Assert.AreEqual(ExpectedLoremIpsumPdfText, okResult.Value);
            }
            else
            {
                Assert.Fail("Result is not ContentResult");
            }
        }

        [TestMethod]
        public async Task Ocr_ShouldNotExtractText_WhenPdfFileSizeExceededLimit()
        {
            var controller = new OcrController(logger.Object, options.Object, ocrProcessor);
            var request = new OcrRequest
            {
                Filename = "Large.pdf",
                File = new byte[options.Object.Value.FileLengthLimit+1]
            };

            var result = await controller.Process(request);

            if (result is ObjectResult badResult)
            {
                Assert.AreEqual(StatusCodes.Status413PayloadTooLarge, badResult.StatusCode);
                Assert.AreEqual($"File is too large {request.File.Length}", badResult.Value?.ToString());
            }
            else
            {
                Assert.Fail("Result is not ObjectResult");
            }
        }

        [TestMethod]
        public async Task Ocr_ShouldExtractText_WhenFileIsPng()
        {
            var controller = new OcrController(logger.Object, options.Object, ocrProcessor);
            var request = new OcrRequest
            {
                Filename = "Lorem Ipsum.png",
                File = await File.ReadAllBytesAsync(@"Pdf/Lorem Ipsum.png")
            };

            var result = await controller.Process(request);

            if (result is OkObjectResult okResult)
            {
                Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
                Assert.AreEqual(ExpectedLoremIpsumPdfText, okResult.Value);
            }
            else
            {
                Assert.Fail("Result is not ContentResult");
            }
        }
    }
}
