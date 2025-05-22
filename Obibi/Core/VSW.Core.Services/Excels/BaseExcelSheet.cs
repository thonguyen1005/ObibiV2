using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Services.Excels
{
    public abstract class BaseExcelSheet : IExcelSheet
    {
        internal BaseExcelSheet(IExcelDocument document)
        {
            Document = document;
        }

        public virtual object this[int rowIndex, int columnIndex] => GetValue<object>(rowIndex, columnIndex);

        public int Index { get; protected set; }

        public string Name { get; protected set; }

        public IExcelDocument Document { get; protected set; }

        public ExcelDimension Dimension { get; protected set; }

        public virtual T GetValue<T>(int rowIndex, int columnIndex, CultureCode? culture = null, params string[] formats)
        {
            return (T)GetValue(typeof(T), rowIndex, columnIndex, culture, formats);
        }

        public virtual object GetValue(Type valueType, int rowIndex, int columnIndex, CultureCode? culture = null, params string[] formats)
        {
            throw new NotImplementedException();
        }
    }
}
