using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Drawing;
using System.Drawing.Imaging;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Color = System.Drawing.Color;



namespace ProArtist.Presentation.Theme.Helps
{
    public class BitmapHelper
    {
        /// <summary>
        ///     Takes a bitmap and converts it to an image that can be handled by WPF ImageBrush
        /// </summary>
        /// <param name="bitmap">A bitmap image</param>
        /// <returns>The image as a BitmapImage for WPF</returns>
        public static ImageSource ConvertToBitmapImage(Bitmap bitmap)
        {

            //bitmap = DeepClone(bitmap);
            //     bitmap = ToGray(bitmap);
            IntPtr hBitmap = bitmap.GetHbitmap();
            ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            if (!DeleteObject(hBitmap))
            {
                throw new System.ComponentModel.Win32Exception();
            }
            bitmap.Dispose();
            return wpfBitmap;

            //var ms = new MemoryStream();
            //bitmap.Save(ms, ImageFormat.Bmp);
            //var image = new BitmapImage();
            //image.BeginInit();
            //ms.Seek(0, SeekOrigin.Begin);
            //image.StreamSource = ms;
            //image.EndInit();
            //return image;
        }


        public static Bitmap BitmapSourceToBitmap(BitmapSource source)
        {


            Bitmap bmp = null;
            try
            {
               PixelFormat format = PixelFormat.Format24bppRgb;
                /*set the translate type according to the in param(source)*/
                switch (source.Format.ToString())
                {
                    case "Rgb24":
                    case "Bgr24": format = PixelFormat.Format24bppRgb; break;
                    case "Bgra32": format = PixelFormat.Format32bppPArgb; break;
                    case "Bgr32": format = PixelFormat.Format32bppRgb; break;
                    case "Pbgra32": format = PixelFormat.Format32bppArgb; break;
                }
                bmp = new Bitmap((int)source.Width, (int)source.Height, format);
                BitmapData data = bmp.LockBits(new System.Drawing.Rectangle(System.Drawing.Point.Empty, bmp.Size),
                    ImageLockMode.WriteOnly,
                    format);
                source.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
                bmp.UnlockBits(data);
            }
            catch
            {
                if (bmp != null)
                {
                    bmp.Dispose();
                    bmp = null;
                }
            }

            return bmp;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        /// <example>
        ///     Bitmap source = new Bitmap(@"c:\example.jpg");
        ///     Rectangle section = new Rectangle(new Point(12, 50), new Size(150, 150));
        ///     Bitmap CroppedImage = CropImage(source, section);
        /// </example>
        public static Bitmap CropImage(Bitmap source, Rectangle section)
        {
            // An empty bitmap which will hold the cropped image
            Bitmap bmp = new Bitmap(section.Width, section.Height);

            Graphics g = Graphics.FromImage(bmp);

            // Draw the given area (section) of the source image
            // at location 0,0 on the empty bitmap (bmp)
            g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);

            return bmp;
        }
        /// <summary>
        ///     Bitmap 转换为 WriteableBitmap(此方法有可能产生bug:转化后的图片尺寸与原bitmap不一样，dpi不一样，建议使用ConvertToBitmapImage)
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static WriteableBitmap ConvertToWriteableBitmap(Bitmap bitmap, WriteableBitmap writableBitmap = null)
        {
            var bytes = ConvertToBytes(bitmap);
            if (writableBitmap == null)
                writableBitmap = new WriteableBitmap(bitmap.Width, bitmap.Height, bitmap.HorizontalResolution, bitmap.VerticalResolution, PixelFormats.Indexed8, BitmapPalettes.Gray256);
            writableBitmap.Lock();
            try
            {
                Marshal.Copy(bytes, 0, writableBitmap.BackBuffer, bytes.Length);
                writableBitmap.AddDirtyRect(new Int32Rect(0, 0, bitmap.Width, bitmap.Height));
                return writableBitmap;
            }
            finally
            {
                writableBitmap.Unlock();
            }
        }
        /// <summary>
        ///     Bitmap 转换为字节数组
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static byte[] ConvertToBytes(Bitmap bitmap)
        {
            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            try
            {
                var multiple = bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed ? 1 : 4;
                var picSize = bitmap.Width * bitmap.Height * multiple;
                var bytes = new byte[picSize];
                Marshal.Copy(bmpData.Scan0, bytes, 0, picSize);
                return bytes;
            }
            finally
            {
                bitmap.UnlockBits(bmpData);
            }
        }
        /// <summary>
        /// 保存图片到硬盘中，bmp格式
        /// </summary>
        /// <param name="bitmapImage">图片</param>
        /// <param name="path">文件路径</param>
        /// <param name="fileName">不包含扩展名的文件名</param>
        public static void SaveBitmapImageIntoFile(BitmapImage bitmapImage, string path, string fileName)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            if (!fileName.EndsWith(".bmp"))
            {
                fileName += ".bmp";
            }
            using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }
        /// <summary>
        /// 保存图片到硬盘中，bmp格式
        /// </summary>
        /// <param name="bitmap">图片</param>
        /// <param name="path">文件路径</param>
        /// <param name="fileName">文件名</param>
        public static void SaveBitmapImageIntoFile(Bitmap bitmap, string path, string fileName)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!fileName.EndsWith(".bmp"))
            {
                fileName += ".bmp";
            }
            bitmap.Save(Path.Combine(path, fileName), ImageFormat.Bmp);
        }
        /// <summary>
        /// 将Bitmap转成灰图，根据图片大小会耗时几秒，要转8位图请使用ConvertTo8bit
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap ConvertToGrayImage(Bitmap bitmap)
        {
            int Height = bitmap.Height;
            int Width = bitmap.Width;
            Bitmap result = new Bitmap(Width, Height);
            Color pixel;
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    pixel = bitmap.GetPixel(x, y);
                    int r, g, b, Result = 0;
                    r = pixel.R;
                    g = pixel.G;
                    b = pixel.B;
                    //实例程序以加权平均值法产生黑白图像  
                    int iType = 2;
                    switch (iType)
                    {
                        case 0://平均值法  
                            Result = ((r + g + b) / 3);
                            break;
                        case 1://最大值法  
                            Result = r > g ? r : g;
                            Result = Result > b ? Result : b;
                            break;
                        case 2://加权平均值法  
                            Result = ((int)(0.7 * r) + (int)(0.2 * g) + (int)(0.1 * b));
                            break;
                    }
                    result.SetPixel(x, y, Color.FromArgb(Result, Result, Result));
                }
            return result;
        }
        /// <summary>
        /// 转化为8位灰度图，说是24位转8位，但亲测32位也可以转8位
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static Bitmap ConvertTo8bit(Bitmap bmp)
        {

            System.Drawing.Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = bmpData.Stride * bmpData.Height;
            byte[] rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            Rectangle rect2 = new Rectangle(0, 0, bmp.Width, bmp.Height);
            Bitmap bit = new Bitmap(bmp.Width, bmp.Height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            System.Drawing.Imaging.BitmapData bmpData2 = bit.LockBits(rect2, System.Drawing.Imaging.ImageLockMode.ReadWrite, bit.PixelFormat);
            IntPtr ptr2 = bmpData2.Scan0;
            int bytes2 = bmpData2.Stride * bmpData2.Height;
            byte[] rgbValues2 = new byte[bytes2];
            System.Runtime.InteropServices.Marshal.Copy(ptr2, rgbValues2, 0, bytes2);
            double colorTemp = 0;
            for (int i = 0; i < bmpData.Height; i++)
            {
                for (int j = 0; j < bmpData.Width * 3; j += 3)
                {
                    colorTemp = rgbValues[i * bmpData.Stride + j + 2] * 0.299 + rgbValues[i * bmpData.Stride + j + 1] * 0.578 + rgbValues[i * bmpData.Stride + j] * 0.114;
                    rgbValues2[i * bmpData2.Stride + j / 3] = (byte)colorTemp;
                }
            }
            Marshal.Copy(rgbValues2, 0, ptr2, bytes2);
            bmp.UnlockBits(bmpData);
            ColorPalette tempPalette;
            {
                using (Bitmap tempBmp = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format8bppIndexed))
                {
                    tempPalette = tempBmp.Palette;
                }
                for (int i = 0; i < 256; i++)
                {
                    tempPalette.Entries[i] = System.Drawing.Color.FromArgb(i, i, i);
                }
                bit.Palette = tempPalette;
            }
            bit.UnlockBits(bmpData2);
            return bit;

        }
        public static int GetBit(Bitmap bmp)
        {
            if (bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
            {
                return 8;
            }
            else if (bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format16bppGrayScale || bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format16bppArgb1555 || bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format16bppRgb555 || bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format16bppRgb565)
            {
                return 16;
            }
            else if (bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
            {
                return 24;
            }
            else if (bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppArgb || bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppPArgb || bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppRgb)
            {
                return 32;
            }
            else if (bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format48bppRgb)
            {
                return 48;
            }
            else if (bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format64bppArgb || bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format64bppPArgb)
            {
                return 64;
            }
            else
                return -1;
        }

        //public static Bitmap DeepClone(Bitmap bitmap)
        //{
        //    //   return  bitmap.Clone(new Rectangle(0, 0, bitmap.Width, bitmap.Height), bitmap.PixelFormat);
        //    Bitmap dstBitmap = null;
        //    using (MemoryStream mStream = new MemoryStream())
        //    {
        //        BinaryFormatter bf = new BinaryFormatter();
        //        bf.Serialize(mStream, bitmap);
        //        mStream.Seek(0, SeekOrigin.Begin);//指定当前流的位置为流的开头。
        //        dstBitmap = (Bitmap)bf.Deserialize(mStream);
        //        mStream.Close();
        //        if (bitmap.Palette.Entries.Length > 0)
        //            dstBitmap.Palette = bitmap.Palette;
        //    }
        //    return dstBitmap;
        //}


        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool DeleteObject([In] IntPtr hObject);
    }
}
