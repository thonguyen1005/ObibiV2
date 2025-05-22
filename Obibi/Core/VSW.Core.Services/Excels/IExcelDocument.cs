using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VSW.Core.Services.Excels
{
    public interface IExcelDocument: IDisposable
    {
        IExcelSheet this[string name] { get; }

        IExcelSheet this[int index] { get; }

        List<string> GetSheetNames();

        int SheetCount { get; }

        bool HasSheet(string name);

        void Load(string filePath);

        void Load(Stream stream);

        void Load(byte[] data);
    }
}
