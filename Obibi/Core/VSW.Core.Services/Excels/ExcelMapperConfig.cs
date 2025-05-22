using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;
namespace VSW.Core.Services.Excels
{
    public delegate bool ExcelFilterDelegate(IExcelReader reader, int rowIndex);

    public abstract class ExcelMapperItem<T>
    {
        public ExcelMapperItem(string propName, int columnIndex)
        {
            Property = TypeManager.GetProperty(typeof(T), propName);
            ColumnIndex = columnIndex;
        }

        public ITypeProperty Property { get; private set; }

        public int ColumnIndex { get; private set; }

        public bool ByColumnIndex
        {
            get
            {
                return ColumnIndex > 0;
            }
        }

        public virtual object GetValue(IExcelReader reader, T instance)
        {
            return reader.GetValue(Property.GetPropertyType(), ColumnIndex);
        }
    }

    public class ExcelMapperItem<T, TProperty> : ExcelMapperItem<T>
    {
        public ExcelMapperItem(string propName, int columnIndex) : base(propName, columnIndex)
        {
        }

        public ExcelMapperItem(Expression<Func<T, TProperty>> prop, int columnIndex, Func<IExcelReader, T, int, TProperty> funcMap = null) : this(prop.Name(), columnIndex)
        {
            PropertyExpression = prop;
            MapFunction = funcMap;
        }


        public ExcelMapperItem(Expression<Func<T, TProperty>> prop, Func<IExcelReader, T, int, TProperty> func) : this(prop, -1)
        {
            MapFunction = func;
        }

        public Expression<Func<T, TProperty>> PropertyExpression { get; private set; }

        public Func<IExcelReader, T, int, TProperty> MapFunction { get; private set; }

        public override object GetValue(IExcelReader reader, T instance)
        {
            TProperty value = default;

            if (MapFunction != null)
            {
                value = MapFunction(reader, instance, ColumnIndex);
            }
            else
            {
                value = (TProperty)base.GetValue(reader, instance);
            }

            return value;
        }
    }

    public class ExcelMapperConfig<T> where T : class, new()
    {
        public ExcelDimension Dimension { get; private set; }

        public string TemplatePath { get; set; }

        public ExcelMapperConfig()
        {
            MapItems = new List<ExcelMapperItem<T>>();
        }

        public ExcelMapperConfig(ExcelDimension dimension) : this()
        {
            Dimension = dimension;
        }


        public ExcelFilterDelegate Filter { get; private set; }

        public List<ExcelMapperItem<T>> MapItems { get; private set; }

        public Action<IExcelReader, T> AfterMapRowAction { get; private set; }

        public ExcelMapperConfig<T> WithFilter(ExcelFilterDelegate filter)
        {
            Filter = filter;
            return this;
        }

        public ExcelMapperConfig<T> WithAfterMapRow(Action<IExcelReader, T> action)
        {
            AfterMapRowAction = action;
            return this;
        }

        public ExcelMapperConfig<T> Map<TProperty>(Expression<Func<T, TProperty>> prop, int colIndex, Func<IExcelReader, T, int, TProperty> funcMap = null)
        {
            var map = new ExcelMapperItem<T, TProperty>(prop, colIndex, funcMap);
            MapItems.Add(map);
            return this;
        }

        public ExcelMapperConfig<T> MapNext<TProperty>(Expression<Func<T, TProperty>> prop, int nextStep = 1, Func<IExcelReader, T, int, TProperty> funcMap = null)
        {
            if (MapItems.Count <= 0 || !MapItems.Last().ByColumnIndex)
            {
                throw new Exception("Chỉ được phép cấu hình MapNext sau 1 Mapping theo ColumnIndex");
            }
            var index = MapItems.Last().ColumnIndex + nextStep;
            return Map(prop, index, funcMap);
        }

        public ExcelMapperConfig<T> Map<TProperty>(Expression<Func<T, TProperty>> prop, Func<IExcelReader, T, int, TProperty> funcMap)
        {
            var map = new ExcelMapperItem<T, TProperty>(prop, funcMap);
            MapItems.Add(map);
            return this;
        }

        public List<T> ToList(IExcelSheet sheet, out Result error, ExcelDimension dimension = null)
        {
            error = Result.Ok();
            var reader = sheet.CreateReader(dimension);

            var rs = new List<T>();
            List<string> msgs = new List<string>();
            while (reader.Read())
            {
                try
                {
                    var obj = MapToObject(reader);
                    if (obj != null)
                    {
                        rs.Add(obj);
                    }
                }
                catch (Exception ex)
                {
                    string msg = $"Dòng {reader.CurrentRow} cột {NumToLetters(reader.CurrentColumn - 1)}: ";
                    if (ex.Message.Contains("format"))
                    {
                        msg += "Dữ liệu không đúng định dạng.";
                    }
                    else if (ex.Message.Contains("DateTime")) // double can not cast to DateTime
                    {
                        msg += "Dữ liệu không phải là kiểu ngày tháng.";
                    }
                    else if (ex.Message.Contains("string")) // input string was not in a correct format"
                    {
                        msg += "Dữ liệu không phải là kiểu chuỗi.";
                    }
                    else
                    {
                        msg += "Xuất hiện lỗi";
                    }
                    msgs.Add(msg);
                }
            }
            if (msgs.IsNotEmpty())
            {
                error = Result.Error(string.Join(Environment.NewLine, msgs));
            }

            return rs;

        }

        public T FirstOrDefault(IExcelSheet sheet, ExcelDimension dimension = null)
        {
            var reader = sheet.CreateReader(dimension);
            while (reader.Read())
            {
                var obj = MapToObject(reader);
                if (obj != null)
                {
                    return obj;
                }
            }

            return default;
        }

        private T MapToObject(IExcelReader reader)
        {
            if (Filter != null)
            {
                if (!Filter(reader, reader.CurrentRow))
                    return default;
            }

            var obj = new T();
            foreach (var map in MapItems)
            {
                reader.SetCurrentColumn(map.ColumnIndex);
                var v = map.GetValue(reader, obj);
                obj.SetPropValue(map.Property.Name, v);
            }

            if (AfterMapRowAction != null)
            {
                AfterMapRowAction(reader, obj);
            }

            return obj;
        }

        /// <summary>
        /// https://stackoverflow.com/questions/29004792/logic-to-generate-an-alphabetical-sequence-in-c-sharp
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private string NumToLetters(int num)
        {
            string str = string.Empty;

            // We need to do at least a "round" of division
            // to handle num == 0
            do
            {
                // We have to "prepend" the new digit
                str = (char)('A' + (num % 26)) + str;
                num /= 26;
            }
            while (num != 0);

            return str;
        }
    }
}
