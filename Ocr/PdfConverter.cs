using ImageMagick;

namespace Ocr
{
    public class PdfConverter
    {
        public static string MakeTiff(string pdfPath)
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

                    //image.ColorType = ColorType.Grayscale;
                    //image.Contrast();
                    //image.AutoThreshold(AutoThresholdMethod.OTSU);

                    image.ColorType = ColorType.Grayscale;

                    //Enhance contrast and brightness
                    image.BrightnessContrast(new Percentage(20), new Percentage(50));

                    //Denoise (remove artifacts)
                    image.Despeckle();

                    //Adaptive Thresholding - Use Otsu for better binarization
                    image.AutoThreshold(AutoThresholdMethod.OTSU);

                    //This code turned background to black
                    // 5. Morphology Edge Enhancement to sharpen text
                    //IMorphologySettings morphologySettings = new MorphologySettings();
                    //morphologySettings.Method = MorphologyMethod.EdgeOut;
                    //morphologySettings.Kernel = Kernel.Diamond;
                    //image.Morphology(morphologySettings);

                }

                images.Write(tiffPath, MagickFormat.Tiff);
            }
            return tiffPath;
        }
    }
}
