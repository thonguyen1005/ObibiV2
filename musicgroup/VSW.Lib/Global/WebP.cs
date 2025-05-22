using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace VSW.Lib.Global
{
    public static class WebP
    {
        public static void Save(Bitmap bmp, string path, int quality)
        {
            try
            {
                var data = EncodeLossly(bmp, quality);

                if (data != null)
                    System.IO.File.WriteAllBytes(path, data);
            }
            catch (Exception ex)
            {
                Error.Write(ex.Message);
            }
        }

        public static Bitmap Read(string path)
        {
            try
            {
                return Decode(System.IO.File.ReadAllBytes(path));
            }
            catch (Exception ex)
            {
                Error.Write(ex.Message);
                return null;
            }
        }

        private static Bitmap Decode(byte[] data)
        {
            try
            {
                var pinnedWebP = GCHandle.Alloc(data, GCHandleType.Pinned);
                var ptrData = pinnedWebP.AddrOfPinnedObject();
                var dataSize = (uint)data.Length;
                if (WebPGetInfo(ptrData, dataSize, out int imgWidth, out int imgHeight) != 1)
                    return null;

                var bmp = new Bitmap(imgWidth, imgHeight, PixelFormat.Format24bppRgb);
                var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);

                var outputBufferSize = bmpData.Stride * imgHeight;
                var outputBuffer = Marshal.AllocHGlobal(outputBufferSize);

                outputBuffer = WebPDecodeBGRInto(ptrData, dataSize, outputBuffer, outputBufferSize, bmpData.Stride);
                
                CopyMemory(bmpData.Scan0, outputBuffer, (uint)outputBufferSize);

                bmp.UnlockBits(bmpData);

                pinnedWebP.Free();
                Marshal.FreeHGlobal(outputBuffer);

                return bmp;
            }
            catch (Exception ex)
            {
                Error.Write(ex.Message);
                return null;
            }
        }

        private static byte[] Encode(Bitmap bmp, int quality)
        {
            try
            {
                var lossly = EncodeLossly(bmp, quality);
                var lossless = EncodeLossless(bmp);

                if (lossly == null || lossless == null)
                    return null;

                byte[] data;
                if (lossly.Length >= lossless.Length)
                    data = lossless;
                else
                    data = lossly;

                return data;
            }
            catch (Exception ex)
            {
                Error.Write(ex.Message);
                return null;
            }
        }

        private static byte[] EncodeLossly(Bitmap bmp, int quality)
        {
            try
            {
                var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                var size = WebPEncodeBGR(bmpData.Scan0, bmp.Width, bmp.Height, bmpData.Stride, quality, out IntPtr unmanagedData);

                var data = new byte[size];
                Marshal.Copy(unmanagedData, data, 0, size);

                bmp.UnlockBits(bmpData);

                WebPFree(unmanagedData);

                return data;
            }
            catch (Exception ex)
            {
                Error.Write(ex.Message);
                return null;
            }
        }

        private static byte[] EncodeLossless(Bitmap bmp)
        {
            try
            {
                var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                var size = WebPEncodeLosslessBGR(bmpData.Scan0, bmp.Width, bmp.Height, bmpData.Stride, out IntPtr unmanagedData);

                var data = new byte[size];
                Marshal.Copy(unmanagedData, data, 0, size);

                bmp.UnlockBits(bmpData);

                WebPFree(unmanagedData);

                return data;
            }
            catch (Exception ex)
            {
                Error.Write(ex.Message);
                return null;
            }
        }

        [DllImport("libwebp.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int WebPGetInfo(IntPtr data, uint data_size, out int width, out int height);

        [DllImport("libwebp.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr WebPDecodeBGR(IntPtr data, uint data_size, ref int width, ref int height);

        [DllImport("libwebp.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr WebPDecodeBGRInto(IntPtr data, uint data_size, IntPtr output_buffer, int output_buffer_size, int output_stride);

        [DllImport("libwebp.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int WebPEncodeBGR(IntPtr rgb, int width, int height, int stride, float quality_factor, out IntPtr output);

        [DllImport("libwebp.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int WebPEncodeLosslessBGR(IntPtr rgb, int width, int height, int stride, out IntPtr output);

        [DllImport("libwebp.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int WebPFree(IntPtr p);

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory")]
        private static extern void CopyMemory(IntPtr Destination, IntPtr Source, uint Length);
    }
}