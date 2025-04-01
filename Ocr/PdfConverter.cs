using ImageMagick;

namespace Ocr
{
    public class PdfConverter
    {
        public static async Task<string> MakeTiff(string pdfPath)
        {
            return await Task.Run(() =>
            {
                //300 dpi will create an image with a better quality
                var settings = new MagickReadSettings
                {
                    Density = new Density(300, 300)
                };

                var tiffPath = $"{pdfPath}.tiff";
                FileUtility.TryDelete(tiffPath);

                using (var images = new MagickImageCollection())
                {
                    images.Read(pdfPath, settings);

                    foreach (var image in images)
                    {
                        image.BackgroundColor = MagickColors.White;
                        image.Alpha(AlphaOption.Remove);

                        image.ColorType = ColorType.Grayscale;

                        //Enhance contrast and brightness
                        image.BrightnessContrast(new Percentage(20), new Percentage(50));

                        //Denoise (remove artifacts)
                        image.Despeckle();

                        //Adaptive Thresholding - Use Otsu for better binarization
                        image.AutoThreshold(AutoThresholdMethod.OTSU);
                    }

                    images.Write(tiffPath, MagickFormat.Tiff);
                }
                return tiffPath;
            });
        }
    }
}
