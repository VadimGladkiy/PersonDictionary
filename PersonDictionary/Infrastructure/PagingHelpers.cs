using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PersonDictionary.Infrastructure
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html,
        PageInfo pageInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= pageInfo.TotalPages; i++)
            {
                
                TagBuilder tag_a = new TagBuilder("a");
                tag_a.MergeAttribute("href", pageUrl(i));
                tag_a.InnerHtml = i.ToString();
              
                if (i == pageInfo.PageNumber)
                {
                    tag_a.AddCssClass("btn-info");                    
                }
                tag_a.AddCssClass("btn btn-default");
                result.Append(tag_a.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}