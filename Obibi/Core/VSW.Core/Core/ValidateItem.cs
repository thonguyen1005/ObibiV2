using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace VSW.Core
{
    public class ValidateItem
    {
        public string Property { get; set; }

        public string Message { get; set; }
    }

    public static class ValidateItemExtensions
    {
        public static Result<List<ValidateItem>> Error<TModel>(Expression<Func<TModel, object>> exp, string message, string prefix = "")
        {
            var validates = new List<ValidateItem>();
            validates.NewItem(prefix, exp, message);
            return new Result<List<ValidateItem>>("VALIDATE_ITEM", "", validates);
        }

        public static Result<List<ValidateItem>> Error<TModel>(List<ValidateItem> validateItems, string responseMsg = "Lỗi")
        {
            return new Result<List<ValidateItem>>("VALIDATE_ITEM", responseMsg, validateItems);
        }

        public static ValidateItem NewItem<TModel>(this List<ValidateItem> lst, Expression<Func<TModel, object>> exp, string message)
        {
            return lst.NewItem("", exp, message);
        }

        public static string ValidatePropertyFor<TModel>(Expression<Func<TModel, object>> exp, int? index = null)
        {
            var fullName = exp.FullName();
            if (index.HasValue)
            {
                fullName += "[" + index.ToString() + "]";
            }

            return fullName;
        }

        public static ValidateItem NewItem<TModel>(this List<ValidateItem> lst, string prefix, Expression<Func<TModel, object>> exp, string message)
        {
            var name = ValidatePropertyFor(exp);
            if (prefix.IsNotEmpty())
            {
                name = prefix + "." + name;
            }

            return lst.NewItem(name, message);
        }

        public static ValidateItem NewItem(this List<ValidateItem> lst, string prop, string message)
        {
            var item = new ValidateItem
            {
                Property = prop,
                Message = message
            };

            lst.Add(item);
            return item;
        }
    }
}
