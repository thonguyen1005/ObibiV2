using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Services.Excels
{
    public interface IExcelSheet
    {
        int Index { get; }

        string Name { get; }

        IExcelDocument Document { get; }

        ExcelDimension Dimension { get; }

        object this[int rowIndex, int columnIndex] { get; }

        T GetValue<T>(int rowIndex, int columnIndex, CultureCode? culture = null, params string[] formats);

        object GetValue(Type valueType, int rowIndex, int columnIndex, CultureCode? culture = null, params string[] formats);
    }
}
