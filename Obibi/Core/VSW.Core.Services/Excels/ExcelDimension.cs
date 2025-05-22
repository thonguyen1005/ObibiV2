using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Services.Excels
{
    public class ExcelDimension
    {
        public int MinRowIndex { get; private set; }

        public int MaxRowIndex { get; private set; }

        public int MinColumnIndex { get; private set; }

        public int MaxColumnIndex { get; private set; }

        public int RowCount { get; private set; }

        public int ColumnCount { get; private set; }

        public ExcelDimension(int maxRow = int.MaxValue, int maxColumn = int.MaxValue, int minRow = 1, int minColumn = 1)
        {
            MinRowIndex = minRow;
            MaxRowIndex = maxRow;
            MinColumnIndex = minColumn;
            MaxColumnIndex = maxColumn;

            RowCount = (MaxRowIndex - MinRowIndex) + 1;
            ColumnCount = (MaxColumnIndex - MinColumnIndex) + 1;
        }

        public void Combine(ExcelDimension dimension)
        {
            MinRowIndex = MinRowIndex < dimension.MinRowIndex ? dimension.MinRowIndex : MinRowIndex;
            MinColumnIndex = MinColumnIndex < dimension.MinColumnIndex ? dimension.MinColumnIndex : MinColumnIndex;

            MaxRowIndex = MaxRowIndex > dimension.MaxRowIndex ? dimension.MaxRowIndex : MaxRowIndex;
            if (MaxRowIndex < MinRowIndex)
            {
                MaxRowIndex = MinRowIndex;
            }

            MaxColumnIndex = MaxColumnIndex > dimension.MaxColumnIndex ? dimension.MaxColumnIndex : MaxColumnIndex;
            if (MaxColumnIndex < MinColumnIndex)
            {
                MaxColumnIndex = MinColumnIndex;
            }

            RowCount = (MaxRowIndex - MinRowIndex) + 1;
            ColumnCount = (MaxColumnIndex - MinColumnIndex) + 1;
        }
    }
}
