namespace ImgConverter
{
    using System;
    using System.Drawing;

    public class Helper
    {
        /// <summary>
        ///     Structure to pass information to the background worker
        /// </summary>
        public struct ConversionInformation
        {
            /// <summary>
            ///     File Names
            /// </summary>
            public string[] FileNames;

            /// <summary>
            ///     Destination Path
            /// </summary>
            public string DestinationPath;

            /// <summary>
            ///     Pixel Format
            /// </summary>
            public string PixelFormat;

            /// <summary>
            ///     Image Type
            /// </summary>
            public string ImageType;
        }

        /// <summary>
        ///     Convert the input image to required image
        /// </summary>
        /// <param name="fileName">input image</param>
        /// <param name="destinationFolder">destination folder to save required image</param>
        /// <param name="pixelFormat">Required Pixel Format</param>
        /// <param name="imageFormat">Required Image Format</param>
        /// <returns>true or false based on conversion success</returns>
        public static bool Convert(string fileName, string destinationFolder, string pixelFormat, string imageFormat)
        {
            try
            {
                Bitmap fillImage = new Bitmap(fileName);

                System.Drawing.Imaging.PixelFormat defaultPixelFormat = System.Drawing.Imaging.PixelFormat.Format32bppArgb;

                switch (pixelFormat)
                {
                    case "8":
                        defaultPixelFormat = System.Drawing.Imaging.PixelFormat.Format8bppIndexed;
                        break;

                    case "16":
                        defaultPixelFormat = System.Drawing.Imaging.PixelFormat.Format16bppArgb1555;
                        break;

                    case "24":
                        defaultPixelFormat = System.Drawing.Imaging.PixelFormat.Format24bppRgb;
                        break;

                    case "32":
                        defaultPixelFormat = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
                        break;
                }

                Bitmap skeletonImage = new Bitmap(fillImage.Width, fillImage.Height, defaultPixelFormat);
                Graphics g = Graphics.FromImage(skeletonImage);
                g.DrawImage(fillImage, 0, 0);
                string targetPath = destinationFolder + "/" + System.IO.Path.GetFileNameWithoutExtension(fileName);

                System.Drawing.Imaging.ImageFormat defaultImageFormat = null;

                switch (imageFormat)
                {
                    case "bmp":
                        defaultImageFormat = System.Drawing.Imaging.ImageFormat.Bmp;
                        targetPath += ".bmp";
                        break;

                    case "jpg":
                        defaultImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                        targetPath += ".jpg";
                        break;

                    case "png":
                        defaultImageFormat = System.Drawing.Imaging.ImageFormat.Png;
                        targetPath += ".png";
                        break;
                }

                skeletonImage.Save(targetPath, defaultImageFormat);

                return true;
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show("Image Conversion failed. Please try again." + exception.Message);
                return false;
            }
        }
    }
}
