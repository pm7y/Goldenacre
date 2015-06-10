using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
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
            const int MAX_COLOR_LEVEL = 255;
            const int MIN_COLOR_LEVEL = -255;

            BitmapData bmData = null;
            var stride = 0;
            IntPtr Scan0;

            if (originalBitmap == null)
            {
                throw new ArgumentNullException("originalBitmap");
            }

            bmData = originalBitmap.LockBits(new Rectangle(0, 0, originalBitmap.Width, originalBitmap.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);

            stride = bmData.Stride;
            Scan0 = bmData.Scan0;

            unsafe
            {
                var p = (byte*) (void*) Scan0;

                var nOffset = stride - originalBitmap.Width*3;
                int nPixel;

                for (var y = 0; y < originalBitmap.Height; ++y)
                {
                    for (var x = 0; x < originalBitmap.Width; ++x)
                    {
                        nPixel = p[2] + redLevel;
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
            BitmapData bmData = null;
            var stride = 0;
            IntPtr Scan0;

            if (originalBitmap == null)
            {
                throw new ArgumentNullException("originalBitmap");
            }

            bmData = originalBitmap.LockBits(new Rectangle(0, 0, originalBitmap.Width, originalBitmap.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);

            stride = bmData.Stride;
            Scan0 = bmData.Scan0;

            unsafe
            {
                var p = (byte*) (void*) Scan0;

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

            if (bmData != null)
            {
                bmData = null;
            }

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
            Bitmap bTemp = null;
            double nXFactor = 0;
            double nYFactor = 0;

            if (originalBitmap == null)
            {
                throw new ArgumentNullException("originalBitmap");
            }

            bTemp = (Bitmap) originalBitmap.Clone();
            originalBitmap = new Bitmap(width, height, bTemp.PixelFormat);
            nXFactor = bTemp.Width/(double) width;
            nYFactor = bTemp.Height/(double) height;

            double fraction_x, fraction_y, one_minus_x, one_minus_y;
            int ceil_x, ceil_y, floor_x, floor_y;
            var c1 = new Color();
            var c2 = new Color();
            var c3 = new Color();
            var c4 = new Color();
            byte red, green, blue;

            byte b1, b2;

            for (var x = 0; x < originalBitmap.Width; ++x)
            {
                for (var y = 0; y < originalBitmap.Height; ++y)
                {
                    floor_x = (int) Math.Floor(x*nXFactor);
                    floor_y = (int) Math.Floor(y*nYFactor);
                    ceil_x = floor_x + 1;

                    if (ceil_x >= bTemp.Width)
                    {
                        ceil_x = floor_x;
                    }

                    ceil_y = floor_y + 1;

                    if (ceil_y >= bTemp.Height)
                    {
                        ceil_y = floor_y;
                    }

                    fraction_x = x*nXFactor - floor_x;
                    fraction_y = y*nYFactor - floor_y;
                    one_minus_x = 1.0 - fraction_x;
                    one_minus_y = 1.0 - fraction_y;

                    c1 = bTemp.GetPixel(floor_x, floor_y);
                    c2 = bTemp.GetPixel(ceil_x, floor_y);
                    c3 = bTemp.GetPixel(floor_x, ceil_y);
                    c4 = bTemp.GetPixel(ceil_x, ceil_y);

                    // Blue
                    b1 = (byte) (one_minus_x*c1.B + fraction_x*c2.B);
                    b2 = (byte) (one_minus_x*c3.B + fraction_x*c4.B);
                    blue = (byte) (one_minus_y*(b1) + fraction_y*(b2));

                    // Green
                    b1 = (byte) (one_minus_x*c1.G + fraction_x*c2.G);
                    b2 = (byte) (one_minus_x*c3.G + fraction_x*c4.G);
                    green = (byte) (one_minus_y*(b1) + fraction_y*(b2));

                    // Red
                    b1 = (byte) (one_minus_x*c1.R + fraction_x*c2.R);
                    b2 = (byte) (one_minus_x*c3.R + fraction_x*c4.R);
                    red = (byte) (one_minus_y*(b1) + fraction_y*(b2));

                    originalBitmap.SetPixel(x, y, Color.FromArgb(255, red, green, blue));
                }
            }

            if (bTemp != null)
            {
                bTemp.Dispose();
                bTemp = null;
            }


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
            BitmapData bmData = null;
            var stride = 0;
            IntPtr Scan0;
            Stream objStream = null;


            if (originalBitmap == null)
            {
                throw new ArgumentNullException("originalBitmap");
            }

            bmData = originalBitmap.LockBits(new Rectangle(0, 0, originalBitmap.Width, originalBitmap.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);
            stride = bmData.Stride;
            Scan0 = bmData.Scan0;

            unsafe
            {
                var p = (byte*) (void*) Scan0;

                var nOffset = stride - originalBitmap.Width*3;

                byte red, green, blue;

                for (var y = 0; y < originalBitmap.Height; ++y)
                {
                    for (var x = 0; x < originalBitmap.Width; ++x)
                    {
                        blue = p[0];
                        green = p[1];
                        red = p[2];

                        p[0] = p[1] = p[2] = (byte) (.299*red + .587*green + .114*blue);

                        p += 3;
                    }
                    p += nOffset;
                }
            }

            originalBitmap.UnlockBits(bmData);
            //				objStream = new MemoryStream();
            //				originalBitmap.Save(objStream, originalBitmap.RawFormat);

            if (objStream != null)
            {
                objStream.Close();
                objStream = null;
            }
            if (bmData != null)
            {
                bmData = null;
            }


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
            BitmapData bmData = null;
            var stride = 0;
            IntPtr Scan0;
            var nVal = 0;


            if (originalBitmap == null)
            {
                throw new ArgumentNullException("originalBitmap");
            }

            if ((brightnessFactor > -255) || (brightnessFactor < 255))
            {
                bmData = originalBitmap.LockBits(new Rectangle(0, 0, originalBitmap.Width, originalBitmap.Height),
                    ImageLockMode.ReadWrite,
                    PixelFormat.Format24bppRgb);

                stride = bmData.Stride;
                Scan0 = bmData.Scan0;
                nVal = 0;

                unsafe
                {
                    var p = (byte*) (void*) Scan0;

                    var nOffset = stride - originalBitmap.Width*3;
                    var nWidth = originalBitmap.Width*3;

                    for (var y = 0; y < originalBitmap.Height; ++y)
                    {
                        for (var x = 0; x < nWidth; ++x)
                        {
                            nVal = (p[0] + brightnessFactor);

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

            if (bmData != null)
            {
                bmData = null;
            }


            return originalBitmap;
        }

        #endregion

        #region COMPRESS FUNCTIONS

        /// <summary>
        ///     Applies JPEG compression to the specified Image object.
        /// </summary>
        /// <param name="originalImage">Image object to compress.</param>
        /// <param name="compressionFactor">The amount of compression (0 = low quality...100 = high quality).</param>
        /// <returns>Returns an Image object.</returns>
        public static Image Compress(Bitmap originalBitmap, long compressionFactor)
        {
            ImageCodecInfo objCodecInfo = null;
            EncoderParameters objEncoderParams = null;
            EncoderParameter objCompressionRatio = null;
            Stream objStream = null;


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
            objCodecInfo = GetImageCodecInfo("image/jpeg");

            objCompressionRatio = new EncoderParameter(Encoder.Quality, compressionFactor);
            objEncoderParams = new EncoderParameters(1);
            objEncoderParams.Param[0] = objCompressionRatio;

            objStream = new MemoryStream();
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
        /// <param name="textBGColour">The background colour of the text.</param>
        /// <param name="textColour">The colour of the text.</param>
        /// <param name="addDateTime">Indicates whether a time stamp should be added.</param>
        /// <param name="placeTop">Indicates whether the text should be a header (true) or a footer (false).</param>
        /// <returns>Returns an Image object.</returns>
        /// <remarks>n/a.</remarks>
        public static Image AddTextToImage(Bitmap originalBitmap,
            string text,
            Color textBGColour,
            Brush textColour,
            bool addDateTime,
            bool placeTop)
        {
            Graphics objGraphics = null;
            Size objOriginalSize;
            Font objFont = null;
            Rectangle objRectangle;
            var intX = 0;
            var intY = 0;
            var intFontHeight = 0;
            var intWidth = 0;
            var intHeight = 0;
            Stream objStream = null;
            StringBuilder objSB = null;


            if (originalBitmap == null)
            {
                throw new ArgumentNullException("originalBitmap");
            }

            objSB = new StringBuilder();
            objSB.Append(text);

            objGraphics = Graphics.FromImage(originalBitmap);
            objOriginalSize = originalBitmap.Size;

            objFont = new Font("Courier New", 12, FontStyle.Regular, GraphicsUnit.Pixel);
            intFontHeight = Convert.ToInt32(objFont.GetHeight(objGraphics));
            intWidth = objOriginalSize.Width;

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
                objSB.Append(DateTime.Now.ToString(CultureInfo.InvariantCulture));
            }

            objGraphics.DrawRectangle(new Pen(textBGColour), objRectangle);
            objGraphics.FillRectangle(new SolidBrush(textBGColour), objRectangle);

            if (placeTop)
            {
                objGraphics.DrawString(objSB.ToString(), objFont, textColour, new PointF(2, 0));
            }
            else
            {
                objGraphics.DrawString(objSB.ToString(),
                    objFont,
                    textColour,
                    new PointF(2, (objOriginalSize.Height - intHeight)));
            }

            objGraphics.Save();
            //objStream = new MemoryStream();
            //originalBitmap.Save(objStream, originalImage.RawFormat);

            if (objStream != null)
            {
                objStream.Close();
                objStream = null;
            }
            if (objGraphics != null)
            {
                objGraphics.Dispose();
                objGraphics = null;
            }
            if (objFont != null)
            {
                objFont.Dispose();
                objFont = null;
            }
            if (objSB != null)
            {
                objSB = null;
            }


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
            Point[,] ptFlip = null;
            var nWidth = 0;
            var nHeight = 0;
            Bitmap objReturn = null;


            if (originalBitmap == null)
            {
                throw new ArgumentNullException("originalBitmap");
            }

            nWidth = originalBitmap.Width;
            nHeight = originalBitmap.Height;
            ptFlip = new Point[originalBitmap.Width, originalBitmap.Height];

            for (var intX = 0; intX < nWidth; ++intX)
            {
                for (var intY = 0; intY < nHeight; ++intY)
                {
                    ptFlip[intX, intY].X = (flipHorizontal) ? nWidth - (intX + 1) : intX;
                    ptFlip[intX, intY].Y = (flipVertical) ? nHeight - (intY + 1) : intY;
                }
            }

            objReturn = OffsetFilterAbs(originalBitmap, ptFlip);


            return objReturn;
        }


        /// <summary>
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        /// <remarks>n/a.</remarks>
        private static ImageCodecInfo GetImageCodecInfo(string mimeType)
        {
            ImageCodecInfo[] objEncoders = null;
            ImageCodecInfo objReturn = null;


            objEncoders = ImageCodecInfo.GetImageEncoders();

            for (var intCount = 0; intCount < objEncoders.Length; intCount++)
            {
                if (
                    objEncoders[intCount].MimeType.ToUpper(CultureInfo.InvariantCulture).Equals(
                        mimeType.ToUpper(CultureInfo.InvariantCulture)))
                {
                    objReturn = objEncoders[intCount];
                    break;
                }
                objReturn = null;
            }

            if (objEncoders != null)
            {
                objEncoders = null;
            }


            return objReturn;
        }


        /// <summary>
        /// </summary>
        /// <param name="originalBitmap"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static Bitmap OffsetFilterAbs(Bitmap originalBitmap, Point[,] offset)
        {
            Bitmap bSrc = null;
            BitmapData bmData = null;
            BitmapData bmSrc = null;
            var scanline = 0;
            IntPtr Scan0;
            IntPtr SrcScan0;

            if (originalBitmap == null)
            {
                throw new ArgumentNullException("originalBitmap");
            }

            bSrc = (Bitmap) originalBitmap.Clone();

            bmData = originalBitmap.LockBits(new Rectangle(0, 0, originalBitmap.Width, originalBitmap.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);
            bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);

            scanline = bmData.Stride;
            Scan0 = bmData.Scan0;
            SrcScan0 = bmSrc.Scan0;

            unsafe
            {
                var p = (byte*) (void*) Scan0;
                var pSrc = (byte*) (void*) SrcScan0;

                var nOffset = bmData.Stride - originalBitmap.Width*3;
                var nWidth = originalBitmap.Width;
                var nHeight = originalBitmap.Height;

                int xOffset, yOffset;

                for (var y = 0; y < nHeight; ++y)
                {
                    for (var x = 0; x < nWidth; ++x)
                    {
                        xOffset = offset[x, y].X;
                        yOffset = offset[x, y].Y;

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

            if (bSrc != null)
            {
                bSrc.Dispose();
                bSrc = null;
            }
            if (bmData != null)
            {
                bmData = null;
            }
            if (bmSrc != null)
            {
                bmSrc = null;
            }


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
            BitmapData bmData = null;
            BitmapData bmSrc = null;
            var stride = 0;
            var stride2 = 0;
            IntPtr Scan0;
            IntPtr SrcScan0;

            if (originalBitmap == null)
            {
                throw new ArgumentNullException("originalBitmap");
            }

            if (matrix.intFactor != 0)
            {
                bSrc = (Bitmap) originalBitmap.Clone();
                bmData = originalBitmap.LockBits(new Rectangle(0, 0, originalBitmap.Width, originalBitmap.Height),
                    ImageLockMode.ReadWrite,
                    PixelFormat.Format24bppRgb);
                bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height),
                    ImageLockMode.ReadWrite,
                    PixelFormat.Format24bppRgb);
                stride = bmData.Stride;
                stride2 = stride*2;
                Scan0 = bmData.Scan0;
                SrcScan0 = bmSrc.Scan0;

                unsafe
                {
                    var p = (byte*) (void*) Scan0;
                    var pSrc = (byte*) (void*) SrcScan0;

                    var nOffset = stride - originalBitmap.Width*3;
                    var nWidth = originalBitmap.Width - 2;
                    var nHeight = originalBitmap.Height - 2;

                    int nPixel;

                    for (var y = 0; y < nHeight; ++y)
                    {
                        for (var x = 0; x < nWidth; ++x)
                        {
                            nPixel = ((((pSrc[2]*matrix.intTopLeft) + (pSrc[5]*matrix.intTopMid) +
                                        (pSrc[8]*matrix.intTopRight) + (pSrc[2 + stride]*matrix.intMidLeft) +
                                        (pSrc[5 + stride]*matrix.intPixel) + (pSrc[8 + stride]*matrix.intMidRight) +
                                        (pSrc[2 + stride2]*matrix.intBottomLeft) +
                                        (pSrc[5 + stride2]*matrix.intBottomMid) +
                                        (pSrc[8 + stride2]*matrix.intBottomRight))/matrix.intFactor) +
                                      matrix.intOffset);

                            if (nPixel < 0)
                            {
                                nPixel = 0;
                            }
                            if (nPixel > 255)
                            {
                                nPixel = 255;
                            }

                            p[5 + stride] = (byte) nPixel;

                            nPixel = ((((pSrc[1]*matrix.intTopLeft) + (pSrc[4]*matrix.intTopMid) +
                                        (pSrc[7]*matrix.intTopRight) + (pSrc[1 + stride]*matrix.intMidLeft) +
                                        (pSrc[4 + stride]*matrix.intPixel) + (pSrc[7 + stride]*matrix.intMidRight) +
                                        (pSrc[1 + stride2]*matrix.intBottomLeft) +
                                        (pSrc[4 + stride2]*matrix.intBottomMid) +
                                        (pSrc[7 + stride2]*matrix.intBottomRight))/matrix.intFactor) +
                                      matrix.intOffset);

                            if (nPixel < 0)
                            {
                                nPixel = 0;
                            }
                            if (nPixel > 255)
                            {
                                nPixel = 255;
                            }

                            p[4 + stride] = (byte) nPixel;

                            nPixel = ((((pSrc[0]*matrix.intTopLeft) + (pSrc[3]*matrix.intTopMid) +
                                        (pSrc[6]*matrix.intTopRight) + (pSrc[0 + stride]*matrix.intMidLeft) +
                                        (pSrc[3 + stride]*matrix.intPixel) + (pSrc[6 + stride]*matrix.intMidRight) +
                                        (pSrc[0 + stride2]*matrix.intBottomLeft) +
                                        (pSrc[3 + stride2]*matrix.intBottomMid) +
                                        (pSrc[6 + stride2]*matrix.intBottomRight))/matrix.intFactor) +
                                      matrix.intOffset);

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
                bSrc = null;
            }
            if (bmData != null)
            {
                bmData = null;
            }
            if (bmSrc != null)
            {
                bmSrc = null;
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
            ConvertMatrix matrix = null;

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

            matrix = new ConvertMatrix();

            matrix.SetAll(0);
            matrix.intPixel = sharpenFactor;
            matrix.intTopMid = matrix.intMidLeft = matrix.intMidRight = matrix.intBottomMid = -2;
            matrix.intFactor = sharpenFactor - 8;


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
            ConvertMatrix matrix = null;


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

            matrix = new ConvertMatrix();

            matrix.SetAll(1);
            matrix.intPixel = smoothFactor;
            matrix.intFactor = smoothFactor + 8;

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
            ConvertMatrix matrix = null;


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

            matrix = new ConvertMatrix();

            matrix.SetAll(1);
            matrix.intPixel = blurFactor;
            matrix.intTopMid = 2;
            matrix.intMidLeft = 2;
            matrix.intMidRight = 2;
            matrix.intBottomMid = 2;
            matrix.intFactor = blurFactor + 12;


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
            ConvertMatrix matrix = null;


            if (originalBitmap == null)
            {
                throw new ArgumentNullException("originalBitmap");
            }

            matrix = new ConvertMatrix();

            matrix.SetAll(-1);
            matrix.intTopMid = 0;
            matrix.intMidLeft = 0;
            matrix.intMidRight = 0;
            matrix.intBottomMid = 0;
            matrix.intPixel = 4;
            matrix.intOffset = 100;


            return ConvertThreeByThree(originalBitmap, matrix);
        }

        #endregion
    }
}