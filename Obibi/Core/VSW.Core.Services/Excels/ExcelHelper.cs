using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VSW.Core.Services.Excels
{
    public static class ExcelHelper
    {
        public static void Init()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public static IExcelDocument LoadDocument(string fileName)
        {
            var document = CoreService.ServiceProvider.GetService<IExcelDocument>();
            if (document == null)
            {
                return document;
            }

            document.Load(fileName);
            return document;
        }

        public static IExcelDocument LoadDocument(Stream stream)
        {
            var document = CoreService.ServiceProvider.GetService<IExcelDocument>();
            if (document == null)
            {
                return document;
            }

            document.Load(stream);
            return document;
        }

        public static IExcelDocument LoadDocument(byte[] data)
        {
            var document = CoreService.ServiceProvider.GetService<IExcelDocument>();
            if (document == null)
            {
                return document;
            }

            document.Load(data);
            return document;
        }

        public static IExcelReader CreateReader(this IExcelDocument document, string sheetName, ExcelDimension dimension = null)
        {
            var sheet = document[sheetName];
            return sheet.CreateReader(dimension);
        }

        public static IExcelReader CreateReader(this IExcelDocument document, int sheetIndex = 0, ExcelDimension dimension = null)
        {
            var sheet = document[sheetIndex];
            return sheet.CreateReader(dimension);
        }

        public static IExcelReader CreateReader(this IExcelSheet sheet, ExcelDimension dimension = null)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException("sheet");
            }

            if (dimension == null)
            {
                dimension = sheet.Dimension;
            }

            return new ExcelReader(sheet, dimension);
        }      
    }
}
