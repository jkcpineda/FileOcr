using FileOcrApi.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Ocr;

namespace FileOcrApi.Controllers
{
    [Route("api/ocr")]
    [ApiController]
    public class OcrController : ControllerBase
    {
        private readonly ILogger<OcrController> _logger;

        private readonly OcrProcessor _ocrProcessor;

        private readonly int _fileLengthLimit;

        public OcrController(ILogger<OcrController> logger, IOptions<OcrOptions> options,
            OcrProcessor ocrProcessor)
        {
            _ocrProcessor = ocrProcessor;
            _logger = logger;
            _fileLengthLimit = options.Value.FileLengthLimit;
        }

        [HttpPost]
        public async Task<IActionResult> Process([FromBody] OcrRequest request)
        {
            if (request.File == null || request.File.Length == 0)
            {
                return BadRequest("File content is empty.");
            }

            if (request.File.Length > _fileLengthLimit)
            {
                return StatusCode(StatusCodes.Status413PayloadTooLarge, $"File is too large {request.File.Length}");
            }

            _logger.LogInformation($"Processing {request.Filename}-{request.File.Length}");

            var result = await _ocrProcessor.ProcessOcr(request.Filename, request.File);

            return Ok(result);
        }
    }
}
