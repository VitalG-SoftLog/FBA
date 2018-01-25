using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace FBA.Web.Helpers
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString RadioButtonForSelectList<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> listOfValues)
        {
            return RadioButtonForSelectList(htmlHelper, expression, listOfValues, null);
        }

        public static MvcHtmlString RadioButtonForSelectList<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> listOfValues,
            object htmlAttributes)
        {
            return RadioButtonForSelectList(htmlHelper, expression, listOfValues, htmlAttributes != null ? htmlAttributes.ToDictionary() : null);
        }

        public static MvcHtmlString RadioButtonForSelectList<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> listOfValues, IDictionary<string, object> htmlAttributes)
        {
            var sb = new StringBuilder();

            if (listOfValues != null)
            {
                sb.Append("<fieldset data-role=\"controlgroup\">");

                foreach (SelectListItem item in listOfValues)
                {
                    var props = new Dictionary<string, object>();
                    if (htmlAttributes != null)
                    {
                        foreach (var attribute in htmlAttributes)
                        {
                            props.Add(attribute.Key, attribute.Value);
                        }
                    }

                    var id = string.Format("{0}_{1}", htmlHelper.IdFor(expression), item.Value);

                    props["id"] = id;

                    var label = htmlHelper.Label(id, item.Text);
                    var radio = htmlHelper.RadioButtonFor(expression, item.Value, props).ToHtmlString();

                    sb.Append(radio);
                    sb.AppendLine();
                    sb.Append(label);
                    sb.AppendLine();
                }

                sb.Append("</fieldset>");
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        public static IDictionary<string, object> ToDictionary(this object data)
        {
            const BindingFlags publicAttributes = BindingFlags.Public | BindingFlags.Instance;
            var dictionary = new Dictionary<string, object>();

            foreach (PropertyInfo property in data.GetType().GetProperties(publicAttributes))
            {
                if (property.CanRead)
                    dictionary.Add(property.Name, property.GetValue(data, null));
            }

            return dictionary;
        }
    }
}