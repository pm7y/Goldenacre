using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Encoder = System.Drawing.Imaging.Encoder;

// ReSharper disable CheckNamespace
namespace Goldenacre.Extensions
// ReSharper restore CheckNamespace
{
    /// <summary>
    ///     This class contains static methods which can be used to manipulate images.
    /// </summary>
    /// <remarks>n/a.</remarks>
    public static class ImageExtensions
    {
        public static Image Resize(this Image @this, Size size)
        {
            if (@this == null)
            {
                throw new ArgumentNullException();
            }

            using (var bitmap = new Bitmap(@this.Clone() as Image))
            {
                var sourceWidth = bitmap.Width;
                var sourceHeight = bitmap.Height;

                float nPercent = 0;
                float nPercentW = 0;
                float nPercentH = 0;

                nPercentW = (size.Width / (float)sourceWidth);
                nPercentH = (size.Height / (float)sourceHeight);

                if (nPercentH < nPercentW)
                    nPercent = nPercentH;
                else
                    nPercent = nPercentW;

                var destWidth = (int)(sourceWidth * nPercent);
                var destHeight = (int)(sourceHeight * nPercent);

                var b = new Bitmap(destWidth, destHeight);
                using (var g = Graphics.FromImage(b))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    g.DrawImage(bitmap, 0, 0, destWidth, destHeight);
                    g.Dispose();
                }
                return b;
            }
        }

        public static Bitmap Grayscale(this Image @this)
        {
            if (@this == null)
            {
                throw new ArgumentNullException();
            }

            using (var bitmap = new Bitmap(@this.Clone() as Image))
            {
                //create a blank bitmap the same size as original
                var newBitmap = new Bitmap(bitmap.Width, bitmap.Height);

                //get a graphics object from the new image
                using (var g = Graphics.FromImage(newBitmap))
                {
                    //create the grayscale ColorMatrix
                    var colorMatrix = new ColorMatrix(
                        new[]
                        {
                            new[] {.3f, .3f, .3f, 0, 0},
                            new[] {.59f, .59f, .59f, 0, 0},
                            new[] {.11f, .11f, .11f, 0, 0},
                            new[] {0f, 0f, 0f, 1f, 0f},
                            new[] {0f, 0f, 0f, 0f, 1f}
                        });

                    //create some image attributes
                    using (var attributes = new ImageAttributes())
                    {
                        //set the color matrix attribute
                        attributes.SetColorMatrix(colorMatrix);

                        //draw the original image on the new image
                        //using the grayscale color matrix
                        g.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height), 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, attributes);

                        return newBitmap;
                    }
                }
            }
        }

        public static Image Compress(this Image @this, long compressionFactor)
        {
            if (@this == null)
            {
                throw new ArgumentNullException();
            }

            using (var bitmap = new Bitmap(@this.Clone() as Image))
            {
                compressionFactor = compressionFactor.EnsureBetween(0, 100);

                var objCodecInfo = GetImageCodecInfo("image/jpeg");

                using (var objCompressionRatio = new EncoderParameter(Encoder.Quality, compressionFactor))
                using (var objEncoderParams = new EncoderParameters(1))
                {
                    objEncoderParams.Param[0] = objCompressionRatio;

                    using (Stream objStream = new MemoryStream())
                    {
                        bitmap.Save(objStream, objCodecInfo, objEncoderParams);

                        return new Bitmap(Image.FromStream(objStream).Clone() as Image);
                    }
                }
            }
        }

        /// <summary>
        ///     Adds either header or footer text to an image.
        /// </summary>
        /// <param name="originalBitmap">Bitmap object to add text to.</param>
        /// <param name="text">Text string to add.</param>
        /// <param name="textBgColour">The background colour of the text.</param>
        /// <param name="textColour">The colour of the text.</param>
        /// <param name="addDateTime">Indicates whether a time stamp should be added.</param>
        /// <param name="placeTop">Indicates whether the text should be a header (true) or a footer (false).</param>
        /// <returns>Returns an Image object.</returns>
        /// <remarks>n/a.</remarks>
        public static Image AddTextToImage(this Image @this,
            string text,
            Color textBgColour,
            Brush textColour,
            bool addDateTime,
            bool placeTop)
        {
            Rectangle rect;
            var intX = 0;
            int y;
            int height;


            if (@this == null)
            {
                throw new ArgumentNullException();
            }

            using (var bitmap = new Bitmap(@this.Clone() as Image))
            {
                var sb = new StringBuilder();
                sb.Append(text);

                using (var graphics = Graphics.FromImage(bitmap))
                {
                    var originalSize = bitmap.Size;

                    using (var font = new Font("Courier New", 12, FontStyle.Regular, GraphicsUnit.Pixel))
                    {
                        var fontHeight = Convert.ToInt32(font.GetHeight(graphics));
                        var width = originalSize.Width;

                        if (placeTop)
                        {
                            y = fontHeight + 2;
                            height = y;
                            rect = new Rectangle(intX, 0, width, height);
                        }
                        else
                        {
                            y = (bitmap.Height - (fontHeight + 2));
                            height = bitmap.Height - y;
                            rect = new Rectangle(intX, y, width, height);
                        }

                        if (addDateTime)
                        {
                            sb.Append(DateTime.Now.ToString(CultureInfo.CurrentCulture));
                        }

                        graphics.DrawRectangle(new Pen(textBgColour), rect);
                        graphics.FillRectangle(new SolidBrush(textBgColour), rect);

                        graphics.DrawString(sb.ToString(), font, textColour, placeTop ? new PointF(2, 0) : new PointF(2, (originalSize.Height - height)));

                        graphics.Save();
                    }
                }
                return new Bitmap(bitmap.Clone() as Image);
            }
        }

        /// <summary>
        ///     Adds either header or footer text to an image.
        /// </summary>
        /// <param name="originalBitmap">Bitmap object to add text to.</param>
        /// <param name="text">Text string to add.</param>
        /// <returns>Returns an Image object.</returns>
        /// <remarks>This will add the specified text (white on black) to the bottom of the image.</remarks>
        public static Image AddTextToImage(this Image @this, string text)
        {
            return AddTextToImage(@this, text, Color.Black, Brushes.White, false, false);
        }

        /// <summary>
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        /// <remarks>n/a.</remarks>
        private static ImageCodecInfo GetImageCodecInfo(string mimeType)
        {
            return
                ImageCodecInfo.GetImageEncoders()
                    .FirstOrDefault(t => t.MimeType.ToUpper(CultureInfo.CurrentCulture).Equals(mimeType.ToUpper(CultureInfo.CurrentCulture)));
        }
    }
}