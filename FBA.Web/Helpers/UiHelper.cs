using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.Mvc.Html;

using FBA.Models;

namespace FBA.Web.Helpers {
    public static class UiHelper {
        private static bool ValuesEqual(object a, object b) {
            if (a == null && b == null)
                return true;
            if (a == null)
                return false;
            if (b == null)
                return false;

            var aType = a.GetType();
            if (!aType.IsInstanceOfType(b))
                b = Convert.ChangeType(b, aType, CultureInfo.InvariantCulture);

            return Equals(a, b);
        }

        public static HtmlString PrintTree<TModel, TProperty>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> property,
            IEnumerable<TreeViewNode> tree) {
            return PrintTree<TModel, TProperty>(helper, property, tree, null);
        }

        public static HtmlString PrintTree<TModel, TProperty>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> property,
            IEnumerable<TreeViewNode> tree,
            object htmlAttributes) {
            return PrintTree(helper, property, tree, HtmlExtensions.ToDictionary(htmlAttributes));
        }

        public static HtmlString PrintTree<TModel, TProperty>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> property,
            IEnumerable<TreeViewNode> tree, IDictionary<string, object> htmlAttributes) {
            var builder = new StringBuilder();

            if (tree.Count() != 0) {
                builder.AppendLine("<ul>");
                foreach (var node in tree) {
                    builder.Append("<li>");

                    var id = string.Format("{0}_{1}", helper.IdFor(property), node.Id);
                    var value = ModelMetadata.FromLambdaExpression(property, helper.ViewData).Model;

                    var props = new Dictionary<string, object>(htmlAttributes ?? new Dictionary<string, object>());
                    props["id"] = id;

                    if (ValuesEqual(value, node.Id))
                        props["checked"] = "checked";

                    var label = helper.Label(id, node.Name);
                    var radio = helper.RadioButtonFor(property, node.Id, props).ToHtmlString();

                    builder.Append(radio);
                    builder.Append(label);

                    builder.Append(PrintTree(helper, property, node.ChildNodes, htmlAttributes));

                    builder.AppendLine("</li>");

                    if (node == tree.Last())
                        builder.AppendLine("</ul>");
                }
            }

            return new HtmlString(builder.ToString());
        }
    }
}