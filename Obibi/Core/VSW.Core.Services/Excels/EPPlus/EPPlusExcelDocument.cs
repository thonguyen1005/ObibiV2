using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VSW.Core.Services.Excels
{
    public class EPPlusExcelDocument : BaseExcelDocument
    {
        private ExcelPackage package;

        public EPPlusExcelDocument() : base()
        {
        }


        public override void Dispose()
        {
            base.Dispose();

            if (package != null)
            {
                package.Dispose();
            }
        }

        public override void Load(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                Load(stream);
            }
        }

        public override void Load(Stream stream)
        {
            package = new ExcelPackage(stream);
            InitSheets();
        }

        public override void Load(string filePath)
        {
            package = new ExcelPackage(new FileInfo(filePath));
            InitSheets();
        }

        private void InitSheets()
        {
            foreach (var innerSheet in package.Workbook.Worksheets)
            {
                var sheet = new EPPlusExcelSheet(this, innerSheet);
                Sheets.Add(sheet);
            }
        }
    }
}
