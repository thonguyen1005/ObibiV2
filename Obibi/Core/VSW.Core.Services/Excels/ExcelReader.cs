using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Services.Excels
{
    public class ExcelReader : IExcelReader
    {
        public IExcelSheet Sheet { get; private set; }

        public ExcelDimension Dimension { get; private set; }

        public int CurrentRow { get; private set; }

        public int CurrentColumn { get; private set; }


        public ExcelReader(IExcelSheet sheet, ExcelDimension dimension)
        {
            Sheet = sheet;
            Dimension = dimension;

            if(Dimension == null)
            {
                Dimension = sheet.Dimension;
            }
            else
            {
                Dimension.Combine(sheet.Dimension);
            }            

            CurrentRow = Dimension.MinRowIndex - 1;
            CurrentColumn = Dimension.MinColumnIndex - 1;
        }

        private bool IsValidRow()
        {
            return IsValidRow(CurrentRow);
        }

        private bool IsValidColumn()
        {
            return IsValidColumn(CurrentColumn);
        }

        private bool IsValidRow(int rowIndex)
        {
            return rowIndex >= 0 && rowIndex <= Dimension.MaxRowIndex;
        }

        private bool IsValidColumn(int colIndex)
        {
            return colIndex >= 0 && colIndex <= Dimension.MaxColumnIndex;
        }

        public T GetNext<T>(CultureCode? culture = null, params string[] formats)
        {
            return GetNext<T>(1, culture, formats);
        }

        public T GetNext<T>(int nextNumber, CultureCode? culture = null, params string[] formats)
        {
            return (T)GetNext(typeof(T), nextNumber, culture, formats);
        }

        public T GetValue<T>(int columnIndex, CultureCode? culture = null, params string[] formats)
        {
            return (T)GetValue(typeof(T), columnIndex, culture, formats);
        }

        public bool Read()
        {
            CurrentRow++;
            CurrentColumn = Dimension.MinColumnIndex - 1;
            return IsValidRow();
        }
                
        public void SetCurrentColumn(int currentCol)
        {
            CurrentColumn = currentCol;
        }

        public object GetNext(Type valueType, CultureCode? culture = null, params string[] formats)
        {
            return GetNext(valueType, 1, culture, formats);
        }

        public object GetValue(Type valueType, int columnIndex, CultureCode? culture = null, params string[] formats)
        {
            if (!IsValidRow())
            {
                throw new Exception("ExcelReader is not valid position: {0}".Format(CurrentRow));
            }

            if (!IsValidColumn(columnIndex))
            {
                throw new Exception("ExcelReader Column is not valid position: {0}".Format(columnIndex));
            }

            return Sheet.GetValue(valueType, CurrentRow, columnIndex, culture, formats);
        }

        public object GetNext(Type valueType, int nextNumber, CultureCode? culture = null, params string[] formats)
        {
            if (!IsValidRow())
            {
                throw new Exception("ExcelReader is not valid position{0}".Format(CurrentRow));
            }

            if (nextNumber < 1)
            {
                throw new Exception("nextNumber is not valid: {0}".Format(nextNumber));
            }

            CurrentColumn += nextNumber;
            if (!IsValidColumn())
            {
                throw new Exception("ExcelReader Column is not valid position: {0}".Format(CurrentColumn));
            }

            return GetValue(valueType, CurrentColumn, culture, formats);
        }
    }
}
