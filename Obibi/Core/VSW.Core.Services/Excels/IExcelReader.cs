using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Services.Excels
{
    public interface IExcelReader
    {
        IExcelSheet Sheet { get; }

        ExcelDimension Dimension { get; }

        int CurrentRow { get; }

        int CurrentColumn { get; }

        bool Read();

        T GetNext<T>(CultureCode? culture = null, params string[] formats);

        T GetValue<T>(int columnIndex, CultureCode? culture = null, params string[] formats);

        void SetCurrentColumn(int currentCol);

        T GetNext<T>(int nextNumber, CultureCode? culture = null, params string[] formats);

        object GetNext(Type valueType, CultureCode? culture = null, params string[] formats);

        object GetValue(Type valueType, int columnIndex, CultureCode? culture = null, params string[] formats);

        object GetNext(Type valueType, int nextNumber, CultureCode? culture = null, params string[] formats);
    }
}
