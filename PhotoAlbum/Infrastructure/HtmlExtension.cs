using PhotoAlbum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoAlbum.Infrastructure
{
    public static class HtmlExtension
    {
        public static MvcHtmlString Button(this HtmlHelper helper, string text, object htmlAttributes)
        {
            var tagBuilder = new TagBuilder("button");
            FillWithAttributes(tagBuilder, htmlAttributes);
            tagBuilder.InnerHtml += text;

            return new MvcHtmlString(tagBuilder.ToString());
        }

        public static MvcHtmlString Button(this HtmlHelper helper, 
            string text, string urlAction, FormMethod method, object htmlAttributes)
        {
            var tagBuilder = new TagBuilder("input");
            FillWithAttributes(tagBuilder, new { type = "submit", formaction = urlAction, formmethod = method.ToString(), value = text });
            FillWithAttributes(tagBuilder, htmlAttributes);

            return new MvcHtmlString(tagBuilder.ToString());
        }

        private static void FillWithAttributes(TagBuilder tagBuilder, object htmlAttributes)
        {
            foreach (var attribute in htmlAttributes.GetType().GetProperties())
            {
                var value = attribute.GetValue(htmlAttributes).ToString();
                tagBuilder.MergeAttribute(attribute.Name, value);
            }
        }
    }
}