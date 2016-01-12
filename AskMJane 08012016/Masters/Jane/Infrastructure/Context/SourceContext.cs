using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Jane.Web.Infrastructure.Context
{
    public class SourceContext
    {
        private static readonly SourceContext _instance = new SourceContext();

        private static readonly Dictionary<string, bool> ValidValues = new Dictionary<string, bool>()
        {
            {"utm_source", true},
            {"utm_medium",true},
            {"utm_campaign",true},
            {"utm_term",true},
            {"utm_content",true}
        };
        public static SourceContext Instance
        {
            get
            {
                return _instance;
            }
        }

        private SourceContext()
        {
        }

        public void SetUtmValuesCookie()
        {
            var collection = HttpContext.Current.Request.QueryString;
            var validCollection = new NameValueCollection();
            var current = HttpContext.Current.Request.Cookies.Get("sourceData");
            if (current == null)
            {
                current = new HttpCookie("sourceData");

            }
            foreach (string key in collection.AllKeys)
            {
                if (key != null)
                {
                    if (ValidValues.ContainsKey(key))
                    {
                        validCollection.Add(key, collection[key]);

                    }
                }
            }

            //Check if the valid collection has anything to set, otherwise dont set cookie
            if (validCollection.HasKeys())
            {
                bool hasChanged = false;
                foreach (var key in validCollection.AllKeys)
                {
                    var value = collection[key];
                    if (current.Values.AllKeys.Contains(key))
                    {
                        if (current.Values[key] != collection[key])
                        {
                            current.Values[key] = collection[key];
                            hasChanged = true;
                        }

                    }
                    else
                    {
                        current.Values.Add(key, value);
                        hasChanged = true;
                    }
                }

                if (hasChanged)
                {
                    HttpContext.Current.Response.Cookies.Add(current);
                }
            }

        }

        public NameValueCollection GetUtmValuesFromCookie()
        {
            NameValueCollection collection = new NameValueCollection();
            var current = HttpContext.Current.Request.Cookies.Get("sourceData");
            if (current != null)
            {
                collection = current.Values;
            }
            return collection;
        }

    }
}