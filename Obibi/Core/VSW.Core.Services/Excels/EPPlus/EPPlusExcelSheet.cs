using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Services.Excels
{
    public class EPPlusExcelSheet : BaseExcelSheet
    {
        private OfficeOpenXml.ExcelWorksheet _innerSheet;

        internal EPPlusExcelSheet(IExcelDocument document, OfficeOpenXml.ExcelWorksheet sheet) : base(document)
        {
            _innerSheet = sheet;
            Index = _innerSheet.Index;
            Name = _innerSheet.Name;
            var innerDimension = _innerSheet.Dimension;
            Dimension = new ExcelDimension(innerDimension.End.Row, innerDimension.End.Column, innerDimension.Start.Row, innerDimension.Start.Column);
        }

        public override object GetValue(Type valueType, int rowIndex, int columnIndex, CultureCode? culture = null, params string[] formats)
        {
            var value = _innerSheet.GetValue(rowIndex, columnIndex);

            if (value == null || value.ToString().IsEmpty())
            {
                return TypeManager.DefaultValue(valueType);
            }

            if (valueType == typeof(object))
            {
                return value;
            }

            if (culture != null && valueType.IsNumeric())
            {
                var cul = CultureHelper.Get(culture.Value);
                return Convert.ChangeType(value, valueType, cul);
            }

            if (valueType == typeof(DateTime))
            {
                if (value is DateTime)
                {
                    return value;
                }
                else if (value is string)
                {
                    if (formats.IsEmpty())
                    {
                        formats = new string[] { DateTimeHelper.DD_MM_YYYY_VN };
                    }

                    var s = (value as string).TrimToEmpty();
                    var v = DateTimeHelper.Parse(s, formats);
                    return v.HasValue ? v.Value : DateTimeHelper.MIN_LOCAL;
                }
            }

            var rs = value.To(valueType);

            if (valueType == typeof(string))
            {
                return (rs as string).TrimToEmpty();
            }

            return rs;
        }
    }
}
