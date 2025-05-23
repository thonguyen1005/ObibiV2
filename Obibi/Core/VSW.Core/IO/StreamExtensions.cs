﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VSW.Core.IO
{
    public static class StreamExtensions
    {
        public static byte[] ReadAll(this Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
