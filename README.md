# OCR Extraction API

## Overview
This project provides an OCR extraction API that processes PDFs and image files using **ImageMagick** for preprocessing and **Tesseract OCR** for text recognition. It is designed to extract text from scanned documents and images efficiently.

## Features
- Supports **PDFs and various image formats** (PNG, JPG, TIFF, etc.).
- **ImageMagick preprocessing** to enhance OCR accuracy.
- **Tesseract OCR** for text extraction.
- REST API for easy integration.

## Prerequisites
Before running the application, ensure the following dependencies are installed:

1. **.NET 8.0 SDK** (or later) - [Download](https://dotnet.microsoft.com/download)
2. **ImageMagick** - [Install Guide](https://imagemagick.org/script/download.php)
3. **Tesseract OCR** - [Install Guide](https://github.com/tesseract-ocr/tesseract)
4. **Ghostscript** (Required for PDF processing) - [Download](https://www.ghostscript.com/download.html)

## Installation
1. Clone the repository:
   ```sh
   git clone https://github.com/yourusername/your-repo.git
   cd your-repo
   ```
2. Install dependencies:
   ```sh
   dotnet restore
   ```
3. Configure `appsettings.json`:
   ```json
   {
     "TesseractDataPath": "C:/Users/yourusername/source/repos/FileOcr/bin/Debug/net8.0/tessdata",
     "WorkingDirectory": "C:/temp"
   }
   ```
4. Build and run:
   ```sh
   dotnet run
   ```

## API Endpoints
### 1. Process OCR (Multipart File Upload)
**Endpoint:**
```
POST /api/ocr
```
**Request (multipart/form-data):**
```http
POST http://localhost:5041/api/ocr
Content-Type: multipart/form-data; boundary=boundary123

--boundary123
Content-Disposition: form-data; name="file"; filename="sample.pdf"
Content-Type: application/pdf

< C:\temp\sample.pdf
--boundary123--
```

### 2. Process OCR (Base64 JSON Request)
**Endpoint:**
```
POST /api/ocr
```
**Request (JSON):**
```json
{
  "filename": "sample.pdf",
  "file": "BASE64_ENCODED_CONTENT"
}
```

## How It Works
1. The uploaded file is **preprocessed** using ImageMagick (format conversion, resolution adjustments, thresholding, etc.).
2. The processed image is **fed into Tesseract OCR** to extract text.
3. The extracted text is returned in the API response or saved to a file.

## Troubleshooting
### 1. "FailedToExecuteCommand gswin64c.exe (127)"
- Ensure **Ghostscript** is installed and available in the system `PATH`.

### 2. "Could not find the language file './tessdata/eng.traineddata'"
- Ensure **Tesseract language files** are present in the configured `TesseractDataPath`.

## Contributing
Feel free to submit **issues, feature requests, or pull requests** to improve the project.

## License
This project is licensed under the **MIT License**.

---

### 🔗 Stay Connected
- **GitHub**: [github.com/jkcpineda](https://github.com/jkcpineda)

