using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace VSW.Core.Services.Excels
{
    public abstract class BaseExcelDocument : IExcelDocument
    {
        protected List<IExcelSheet> Sheets = new List<IExcelSheet>();

        public IExcelSheet this[string name] => Sheets.FirstOrDefault(x => x.Name.EqualsIgnoreCase(name));

        public IExcelSheet this[int index] => (index < 0 || index >= Sheets.Count) ? null : Sheets[index];

        public int SheetCount => Sheets.Count;

        public virtual void Dispose()
        {
            Sheets.Clear();
        }

        public List<string> GetSheetNames()
        {
            return Sheets.Select(x => x.Name).ToList();
        }

        public bool HasSheet(string name)
        {
            return this[name] != null;
        }

        public BaseExcelDocument()
        {
           
        }    

        public virtual void Load(string filePath)
        {
            throw new NotImplementedException();
        }


        public virtual void Load(Stream stream)
        {
            throw new NotImplementedException();
        }


        public virtual void Load(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
