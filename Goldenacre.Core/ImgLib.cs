using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Goldenacre.Extensions;
using Encoder = System.Drawing.Imaging.Encoder;

namespace Goldenacre.Core
{
    /// <summary>
    ///     This class contains static methods which can be used to manipulate images.
    /// </summary>
    /// <remarks>n/a.</remarks>
    public sealed class ImgLib
    {
        /// <summary>
        ///     Alters colour of the specified Bitmap.
        /// </summary>
        /// <param name="originalBitmap">Bitmap to colourise.</param>
        /// <param name="redLevel">Red colour level.</param>
        /// <param name="greenLevel">Green colour level.</param>
        /// <param name="blueLevel">Blue colour level..</param>
        /// <returns>Returns altered Bitmap.</returns>
        /// <remarks>n/a.</remarks>
        public static Bitmap Colorize(Bitmap originalBitmap, int redLevel, int greenLevel, int blueLevel)
        {
            const int maxColorLevel = 255;
            const int minColorLevel = 0;

            redLevel = redLevel.EnsureBetween(minColorLevel, maxColorLevel);
            greenLevel = greenLevel.EnsureBetween(minColorLevel, maxColorLevel);
            blueLevel = blueLevel.EnsureBetween(minColorLevel, maxColorLevel);

            if (originalBitmap == null)
            {
                throw new ArgumentNullException("originalBitmap");
            }

            var bmData = originalBitmap.LockBits(new Rectangle(0, 0, originalBitmap.Width, originalBitmap.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);

            var stride = bmData.Stride;
            var scan0 = bmData.Scan0;

            unsafe
            {
                var p = (byte*) (void*) scan0;

                var nOffset = stride - originalBitmap.Width*3;

                for (var y = 0; y < originalBitmap.Height; ++y)
                {
                    for (var x = 0; x < originalBitmap.Width; ++x)
                    {
                        var nPixel = p[2] + redLevel;
                        nPixel = Math.Max(nPixel, 0);
                        p[2] = (byte) Math.Min(255, nPixel);

                        nPixel = p[1] + greenLevel;
                        nPixel = Math.Max(nPixel, 0);
                        p[1] = (byte) Math.Min(255, nPixel);

                        nPixel = p[0] + blueLevel;
                        nPixel = Math.Max(nPixel, 0);
                        p[0] = (byte) Math.Min(255, nPixel);

                        p += 3;
                    }
                    p += nOffset;
                }
            }

            originalBitmap.UnlockBits(bmData);


            return originalBitmap;
        }

        #region INVERT FUNCTIONS

        /// <summary>
        ///     Inverts the colours of an Bitmap object.
        /// </summary>
        /// <param name="originalBitmap">Bitmap object to invert.</param>
        /// <returns>Returns an Bitmap object.</returns>
        /// <remarks>n/a.</remarks>
        public static Bitmap Invert(Bitmap originalBitmap)
        {
            if (originalBitmap == null)
            {
                throw new ArgumentNullException("originalBitmap");
            }

            var bmData = originalBitmap.LockBits(new Rectangle(0, 0, originalBitmap.Width, originalBitmap.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);

            var stride = bmData.Stride;
            var scan0 = bmData.Scan0;

            unsafe
            {
                var p = (byte*) (void*) scan0;

                var nOffset = stride - originalBitmap.Width*3;
                var nWidth = originalBitmap.Width*3;

                for (var y = 0; y < originalBitmap.Height; ++y)
                {
                    for (var x = 0; x < nWidth; ++x)
                    {
                        p[0] = (byte) (255 - p[0]);
                        ++p;
                    }
                    p += nOffset;
                }
            }

            originalBitmap.UnlockBits(bmData);

            return originalBitmap;
        }

        #endregion

        #region RESIZE FUNCTIONS

        /// <summary>
        ///     Resizes the specified Bitmap object.
        /// </summary>
        /// <param name="originalBitmap">The Bitmap object to resize.</param>
        /// <param name="width">The new width of the object.</param>
        /// <param name="height">The new height of the object.</param>
        /// <returns>Returns a Bitmap object.</returns>
        /// <remarks>n/a.</remarks>
        public static Bitmap Resize(Bitmap originalBitmap, int width, int height)
        {
            if (originalBitmap == null)
            {
                throw new ArgumentNullException("originalBitmap");
            }

            var bTemp = (Bitmap) originalBitmap.Clone();
            originalBitmap = new Bitmap(width, height, bTemp.PixelFormat);
            var nXFactor = bTemp.Width/(double) width;
            var nYFactor = bTemp.Height/(double) height;

            for (var x = 0; x < originalBitmap.Width; ++x)
            {
                for (var y = 0; y < originalBitmap.Height; ++y)
                {
                    var floorX = (int) Math.Floor(x*nXFactor);
                    var floorY = (int) Math.Floor(y*nYFactor);
                    var ceilX = floorX + 1;

                    if (ceilX >= bTemp.Width)
                    {
                        ceilX = floorX;
                    }

                    var ceilY = floorY + 1;

                    if (ceilY >= bTemp.Height)
                    {
                        ceilY = floorY;
                    }

                    var fractionX = x*nXFactor - floorX;
                    var fractionY = y*nYFactor - floorY;
                    var oneMinusX = 1.0 - fractionX;
                    var oneMinusY = 1.0 - fractionY;

                    var c1 = bTemp.GetPixel(floorX, floorY);
                    var c2 = bTemp.GetPixel(ceilX, floorY);
                    var c3 = bTemp.GetPixel(floorX, ceilY);
                    var c4 = bTemp.GetPixel(ceilX, ceilY);

                    // Blue
                    var b1 = (byte) (oneMinusX*c1.B + fractionX*c2.B);
                    var b2 = (byte) (oneMinusX*c3.B + fractionX*c4.B);
                    var blue = (byte) (oneMinusY*(b1) + fractionY*(b2));

                    // Green
                    b1 = (byte) (oneMinusX*c1.G + fractionX*c2.G);
                    b2 = (byte) (oneMinusX*c3.G + fractionX*c4.G);
                    var green = (byte) (oneMinusY*(b1) + fractionY*(b2));

                    // Red
                    b1 = (byte) (oneMinusX*c1.R + fractionX*c2.R);
                    b2 = (byte) (oneMinusX*c3.R + fractionX*c4.R);
                    var red = (byte) (oneMinusY*(b1) + fractionY*(b2));

                    originalBitmap.SetPixel(x, y, Color.FromArgb(255, red, green, blue));
                }
            }

            bTemp.Dispose();

            return originalBitmap;
        }

        #endregion

        #region GRAYSCALE FUNCTIONS

        /// <summary>
        ///     Converts an image to use grayscale tones.
        /// </summary>
        /// <param name="originalBitmap">Bitmap object to convert.</param>
        /// <returns>Returns a Bitmap object.</returns>
        /// <remarks>n/a.</remarks>
        public static Bitmap Grayscale(Bitmap originalBitmap)
        {
            if (originalBitmap == null)
            {
                throw new ArgumentNullException("originalBitmap");
            }

            var bmData = originalBitmap.LockBits(new Rectangle(0, 0, originalBitmap.Width, originalBitmap.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);
            var stride = bmData.Stride;
            var scan0 = bmData.Scan0;

            unsafe
            {
                var p = (byte*) (void*) scan0;

                var nOffset = stride - originalBitmap.Width*3;

                for (var y = 0; y < originalBitmap.Height; ++y)
                {
                    for (var x = 0; x < originalBitmap.Width; ++x)
                    {
                        var blue = p[0];
                        var green = p[1];
                        var red = p[2];

                        p[0] = p[1] = p[2] = (byte) (.299*red + .587*green + .114*blue);

                        p += 3;
                    }
                    p += nOffset;
                }
            }

            originalBitmap.UnlockBits(bmData);
            Stream objStream = new MemoryStream();
            originalBitmap.Save(objStream, originalBitmap.RawFormat);

            objStream.Close();

            return originalBitmap;
        }

        #endregion

        #region BRIGHTNESS FUNCTIONS

        /// <summary>
        ///     Alters the brightness of a Bitmap object.
        /// </summary>
        /// <param name="originalBitmap">Bitmap object to alter.</param>
        /// <param name="brightnessFactor">The brightness factor (-255 to 255).</param>
        /// <returns>Returns a Bitmap object.</returns>
        /// <remarks>n/a.</remarks>
        public static Bitmap Brightness(Bitmap originalBitmap, int brightnessFactor)
        {
            if (originalBitmap == null)
            {
                throw new ArgumentNullException("originalBitmap");
            }

            if ((brightnessFactor > -255) || (brightnessFactor < 255))
            {
                var bmData = originalBitmap.LockBits(new Rectangle(0, 0, originalBitmap.Width, originalBitmap.Height),
                    ImageLockMode.ReadWrite,
                    PixelFormat.Format24bppRgb);

                var stride = bmData.Stride;
                var scan0 = bmData.Scan0;

                unsafe
                {
                    var p = (byte*) (void*) scan0;

                    var nOffset = stride - originalBitmap.Width*3;
                    var nWidth = originalBitmap.Width*3;

                    for (var y = 0; y < originalBitmap.Height; ++y)
                    {
                        for (var x = 0; x < nWidth; ++x)
                        {
                            var nVal = (p[0] + brightnessFactor);

                            if (nVal < 0)
                            {
                                nVal = 0;
                            }
                            if (nVal > 255)
                            {
                                nVal = 255;
                            }

                            p[0] = (byte) nVal;

                            ++p;
                        }
                        p += nOffset;
                    }
                }

                originalBitmap.UnlockBits(bmData);
            }

            return originalBitmap;
        }

        #endregion

        #region COMPRESS FUNCTIONS

        /// <summary>
        ///     Applies JPEG compression to the specified Image object.
        /// </summary>
        /// <param name="originalBitmap">Image object to compress.</param>
        /// <param name="compressionFactor">The amount of compression (0 = low quality...100 = high quality).</param>
        /// <returns>Returns an Image object.</returns>
        public static Image Compress(Bitmap originalBitmap, long compressionFactor)
        {
            if (originalBitmap == null)
            {
                throw new ArgumentNullException("originalBitmap");
            }

            if (compressionFactor < 0)
            {
                compressionFactor = 0;
            }
            if (compressionFactor > 100)
            {
                compressionFactor = 100;
            }
            var objCodecInfo = GetImageCodecInfo("image/jpeg");

            var objCompressionRatio = new EncoderParameter(Encoder.Quality, compressionFactor);
            var objEncoderParams = new EncoderParameters(1);
            objEncoderParams.Param[0] = objCompressionRatio;

            Stream objStream = new MemoryStream();
            originalBitmap.Save(objStream, objCodecInfo, objEncoderParams);


            return Image.FromStream(objStream); //originalImage;
        }

        #endregion

        #region FLIP FUNCTIONS

        /// <summary>
        ///     Flips a Bitmap horizontally.
        /// </summary>
        /// <param name="originalBitmap">Bitmap to flip.</param>
        /// <returns>Returns the flipped Bitmap.</returns>
        /// <remarks>n/a.</remarks>
        public static Bitmap FlipHorizontal(Bitmap originalBitmap)
        {
            return Flip(originalBitmap, true, false);
        }


        /// <summary>
        ///     Flips a Bitmap vertically.
        /// </summary>
        /// <param name="originalBitmap">Bitmap to flip.</param>
        /// <returns>Returns the flipped Bitmap.</returns>
        /// <remarks>n/a.</remarks>
        public static Bitmap FlipVertical(Bitmap originalBitmap)
        {
            return Flip(originalBitmap, false, true);
        }

        #endregion

        #region ADDTEXTTOIMAGE FUNCTION

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
        public static Image AddTextToImage(Bitmap originalBitmap,
            string text,
            Color textBgColour,
            Brush textColour,
            bool addDateTime,
            bool placeTop)
        {
            Rectangle objRectangle;
            var intX = 0;
            int intY;
            int intHeight;


            if (originalBitmap == null)
            {
                throw new ArgumentNullException("originalBitmap");
            }

            var objSb = new StringBuilder();
            objSb.Append(text);

            var objGraphics = Graphics.FromImage(originalBitmap);
            var objOriginalSize = originalBitmap.Size;

            var objFont = new Font("Courier New", 12, FontStyle.Regular, GraphicsUnit.Pixel);
            var intFontHeight = Convert.ToInt32(objFont.GetHeight(objGraphics));
            var intWidth = objOriginalSize.Width;

            if (placeTop)
            {
                intY = intFontHeight + 2;
                intHeight = intY;
                objRectangle = new Rectangle(intX, 0, intWidth, intHeight);
            }
            else
            {
                intY = (originalBitmap.Height - (intFontHeight + 2));
                intHeight = originalBitmap.Height - intY;
                objRectangle = new Rectangle(intX, intY, intWidth, intHeight);
            }

            if (addDateTime)
            {
                objSb.Append(DateTime.Now.ToString(CultureInfo.CurrentCulture));
            }

            objGraphics.DrawRectangle(new Pen(textBgColour), objRectangle);
            objGraphics.FillRectangle(new SolidBrush(textBgColour), objRectangle);

            objGraphics.DrawString(objSb.ToString(), objFont, textColour,
                placeTop ? new PointF(2, 0) : new PointF(2, (objOriginalSize.Height - intHeight)));

            objGraphics.Save();
            Stream objStream = new MemoryStream();
            originalBitmap.Save(objStream, originalBitmap.RawFormat);


            objStream.Close();


            objGraphics.Dispose();


            objFont.Dispose();

            return originalBitmap;
        }


        /// <summary>
        ///     Adds either header or footer text to an image.
        /// </summary>
        /// <param name="originalBitmap">Bitmap object to add text to.</param>
        /// <param name="text">Text string to add.</param>
        /// <returns>Returns an Image object.</returns>
        /// <remarks>This will add the specified text (white on black) to the bottom of the image.</remarks>
        public static Image AddTextToImage(Bitmap originalBitmap, string text)
        {
            return AddTextToImage(originalBitmap, text, Color.Black, Brushes.White, false, false);
        }

        #endregion

        #region PRIVATE FUNCTIONS

        /// <summary>
        ///     Flips an image either horizontally, verticall or both.
        /// </summary>
        /// <param name="originalBitmap">Bitmap to flip.</param>
        /// <param name="flipHorizontal">True to flip horizontally.</param>
        /// <param name="flipVertical">True to flip vertically.</param>
        /// <returns>Returns altered Bitmap.</returns>
        /// <remarks>n/a.</remarks>
        private static Bitmap Flip(Bitmap originalBitmap, bool flipHorizontal, bool flipVertical)
        {
            if (originalBitmap == null)
            {
                throw new ArgumentNullException("originalBitmap");
            }

            var nWidth = originalBitmap.Width;
            var nHeight = originalBitmap.Height;
            var ptFlip = new Point[originalBitmap.Width, originalBitmap.Height];

            for (var intX = 0; intX < nWidth; ++intX)
            {
                for (var intY = 0; intY < nHeight; ++intY)
                {
                    ptFlip[intX, intY].X = (flipHorizontal) ? nWidth - (intX + 1) : intX;
                    ptFlip[intX, intY].Y = (flipVertical) ? nHeight - (intY + 1) : intY;
                }
            }

            var objReturn = OffsetFilterAbs(originalBitmap, ptFlip);


            return objReturn;
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
                    .FirstOrDefault(
                        t =>
                            t.MimeType.ToUpper(CultureInfo.CurrentCulture)
                                .Equals(mimeType.ToUpper(CultureInfo.CurrentCulture)));
        }


        /// <summary>
        /// </summary>
        /// <param name="originalBitmap"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static Bitmap OffsetFilterAbs(Bitmap originalBitmap, Point[,] offset)
        {
            if (originalBitmap == null)
            {
                throw new ArgumentNullException("originalBitmap");
            }

            var bSrc = (Bitmap) originalBitmap.Clone();

            var bmData = originalBitmap.LockBits(new Rectangle(0, 0, originalBitmap.Width, originalBitmap.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);
            var bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);

            var scanline = bmData.Stride;
            var scan0 = bmData.Scan0;
            var srcScan0 = bmSrc.Scan0;

            unsafe
            {
                var p = (byte*) (void*) scan0;
                var pSrc = (byte*) (void*) srcScan0;

                var nOffset = bmData.Stride - originalBitmap.Width*3;
                var nWidth = originalBitmap.Width;
                var nHeight = originalBitmap.Height;

                for (var y = 0; y < nHeight; ++y)
                {
                    for (var x = 0; x < nWidth; ++x)
                    {
                        var xOffset = offset[x, y].X;
                        var yOffset = offset[x, y].Y;

                        if (yOffset >= 0 && yOffset < nHeight && xOffset >= 0 && xOffset < nWidth)
                        {
                            p[0] = pSrc[(yOffset*scanline) + (xOffset*3)];
                            p[1] = pSrc[(yOffset*scanline) + (xOffset*3) + 1];
                            p[2] = pSrc[(yOffset*scanline) + (xOffset*3) + 2];
                        }

                        p += 3;
                    }
                    p += nOffset;
                }
            }

            originalBitmap.UnlockBits(bmData);
            bSrc.UnlockBits(bmSrc);

            bSrc.Dispose();

            return originalBitmap;
        }


        /// <summary>
        ///     Converts pixels
        /// </summary>
        /// <param name="originalBitmap"></param>
        /// <param name="matrix"></param>
        /// <returns>Bitmap</returns>
        private static Bitmap ConvertThreeByThree(Bitmap originalBitmap, ConvertMatrix matrix)
        {
            Bitmap bSrc = null;

            if (originalBitmap == null)
            {
                throw new ArgumentNullException("originalBitmap");
            }

            if (matrix.IntFactor != 0)
            {
                bSrc = (Bitmap) originalBitmap.Clone();
                var bmData = originalBitmap.LockBits(new Rectangle(0, 0, originalBitmap.Width, originalBitmap.Height),
                    ImageLockMode.ReadWrite,
                    PixelFormat.Format24bppRgb);
                var bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height),
                    ImageLockMode.ReadWrite,
                    PixelFormat.Format24bppRgb);
                var stride = bmData.Stride;
                var stride2 = stride*2;
                var scan0 = bmData.Scan0;
                var srcScan0 = bmSrc.Scan0;

                unsafe
                {
                    var p = (byte*) (void*) scan0;
                    var pSrc = (byte*) (void*) srcScan0;

                    var nOffset = stride - originalBitmap.Width*3;
                    var nWidth = originalBitmap.Width - 2;
                    var nHeight = originalBitmap.Height - 2;

                    for (var y = 0; y < nHeight; ++y)
                    {
                        for (var x = 0; x < nWidth; ++x)
                        {
                            var nPixel = ((((pSrc[2]*matrix.IntTopLeft) + (pSrc[5]*matrix.IntTopMid) +
                                            (pSrc[8]*matrix.IntTopRight) + (pSrc[2 + stride]*matrix.IntMidLeft) +
                                            (pSrc[5 + stride]*matrix.IntPixel) + (pSrc[8 + stride]*matrix.IntMidRight) +
                                            (pSrc[2 + stride2]*matrix.IntBottomLeft) +
                                            (pSrc[5 + stride2]*matrix.IntBottomMid) +
                                            (pSrc[8 + stride2]*matrix.IntBottomRight))/matrix.IntFactor) +
                                          matrix.IntOffset);

                            if (nPixel < 0)
                            {
                                nPixel = 0;
                            }
                            if (nPixel > 255)
                            {
                                nPixel = 255;
                            }

                            p[5 + stride] = (byte) nPixel;

                            nPixel = ((((pSrc[1]*matrix.IntTopLeft) + (pSrc[4]*matrix.IntTopMid) +
                                        (pSrc[7]*matrix.IntTopRight) + (pSrc[1 + stride]*matrix.IntMidLeft) +
                                        (pSrc[4 + stride]*matrix.IntPixel) + (pSrc[7 + stride]*matrix.IntMidRight) +
                                        (pSrc[1 + stride2]*matrix.IntBottomLeft) +
                                        (pSrc[4 + stride2]*matrix.IntBottomMid) +
                                        (pSrc[7 + stride2]*matrix.IntBottomRight))/matrix.IntFactor) +
                                      matrix.IntOffset);

                            if (nPixel < 0)
                            {
                                nPixel = 0;
                            }
                            if (nPixel > 255)
                            {
                                nPixel = 255;
                            }

                            p[4 + stride] = (byte) nPixel;

                            nPixel = ((((pSrc[0]*matrix.IntTopLeft) + (pSrc[3]*matrix.IntTopMid) +
                                        (pSrc[6]*matrix.IntTopRight) + (pSrc[0 + stride]*matrix.IntMidLeft) +
                                        (pSrc[3 + stride]*matrix.IntPixel) + (pSrc[6 + stride]*matrix.IntMidRight) +
                                        (pSrc[0 + stride2]*matrix.IntBottomLeft) +
                                        (pSrc[3 + stride2]*matrix.IntBottomMid) +
                                        (pSrc[6 + stride2]*matrix.IntBottomRight))/matrix.IntFactor) +
                                      matrix.IntOffset);

                            if (nPixel < 0)
                            {
                                nPixel = 0;
                            }
                            if (nPixel > 255)
                            {
                                nPixel = 255;
                            }

                            p[3 + stride] = (byte) nPixel;

                            p += 3;
                            pSrc += 3;
                        }
                        p += nOffset;
                        pSrc += nOffset;
                    }
                }

                originalBitmap.UnlockBits(bmData);
                bSrc.UnlockBits(bmSrc);
            }

            if (bSrc != null)
            {
                bSrc.Dispose();
            }

            return originalBitmap;
        }

        #endregion

        #region IMAGE CLARITY FUNCTIONS

        /// <summary>
        ///     Sharpens the appearance of the specified Bitmp.
        /// </summary>
        /// <param name="originalBitmap">Bitmap to be sharpened.</param>
        /// <param name="sharpenFactor">Value between 1 and 10 (10 being highest sharpen level.</param>
        /// <returns>Returns sharpened Bitmap.</returns>
        /// <remarks>n/a.</remarks>
        public static Bitmap Sharpen(Bitmap originalBitmap, int sharpenFactor)
        {
            if (originalBitmap == null)
            {
                throw new ArgumentNullException("originalBitmap");
            }

            if (sharpenFactor < 0)
            {
                sharpenFactor = 1;
            }

            if (sharpenFactor > 10)
            {
                sharpenFactor = 10;
            }

            var matrix = new ConvertMatrix();

            matrix.SetAll(0);
            matrix.IntPixel = sharpenFactor;
            matrix.IntTopMid = matrix.IntMidLeft = matrix.IntMidRight = matrix.IntBottomMid = -2;
            matrix.IntFactor = sharpenFactor - 8;


            return ConvertThreeByThree(originalBitmap, matrix);
        }


        /// <summary>
        ///     Smooths (softens) the appearance of the specified Bitmp.
        /// </summary>
        /// <param name="originalBitmap">Bitmap to be smoothed.</param>
        /// <param name="smoothFactor">Value between 1 and 10 (1 being highest smoothing level.</param>
        /// <returns>Returns smoothed Bitmap.</returns>
        /// <remarks>n/a.</remarks>
        public static Bitmap Smooth(Bitmap originalBitmap, int smoothFactor)
        {
            if (originalBitmap == null)
            {
                throw new ArgumentNullException("originalBitmap");
            }

            if (smoothFactor < 0)
            {
                smoothFactor = 1;
            }

            if (smoothFactor > 10)
            {
                smoothFactor = 10;
            }

            var matrix = new ConvertMatrix();

            matrix.SetAll(1);
            matrix.IntPixel = smoothFactor;
            matrix.IntFactor = smoothFactor + 8;

            return ConvertThreeByThree(originalBitmap, matrix);
        }


        /// <summary>
        ///     Blurs the appearance of the specified Bitmp.
        /// </summary>
        /// <param name="originalBitmap">Bitmap to be smoothed.</param>
        /// <param name="blurFactor">Value between 1 and 10 (1 being highest blur level.</param>
        /// <returns>Returns blurred Bitmap.</returns>
        /// <remarks>n/a.</remarks>
        public static Bitmap GaussianBlur(Bitmap originalBitmap, int blurFactor)
        {
            if (originalBitmap == null)
            {
                throw new ArgumentNullException("originalBitmap");
            }

            if (blurFactor < 0)
            {
                blurFactor = 1;
            }

            if (blurFactor > 10)
            {
                blurFactor = 10;
            }

            var matrix = new ConvertMatrix();

            matrix.SetAll(1);
            matrix.IntPixel = blurFactor;
            matrix.IntTopMid = 2;
            matrix.IntMidLeft = 2;
            matrix.IntMidRight = 2;
            matrix.IntBottomMid = 2;
            matrix.IntFactor = blurFactor + 12;


            return ConvertThreeByThree(originalBitmap, matrix);
        }


        /// <summary>
        ///     Embosses a Bitmap.
        /// </summary>
        /// <param name="originalBitmap">Bitmap to be embossed.</param>
        /// <returns>Returns embossed Bitmap</returns>
        /// <remarks>n/a.</remarks>
        public static Bitmap Emboss(Bitmap originalBitmap)
        {
            if (originalBitmap == null)
            {
                throw new ArgumentNullException("originalBitmap");
            }

            var matrix = new ConvertMatrix();

            matrix.SetAll(-1);
            matrix.IntTopMid = 0;
            matrix.IntMidLeft = 0;
            matrix.IntMidRight = 0;
            matrix.IntBottomMid = 0;
            matrix.IntPixel = 4;
            matrix.IntOffset = 100;


            return ConvertThreeByThree(originalBitmap, matrix);
        }

        #endregion
    }
}