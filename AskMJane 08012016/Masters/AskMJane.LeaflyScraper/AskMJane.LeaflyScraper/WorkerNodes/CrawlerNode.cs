using System;
using System.IO;
using System.Net;
using AskMJane.LeaflyScraper.Logging;

namespace AskMJane.LeaflyScraper.WorkerNodes
{
    public class CrawlerNode
    {
        private readonly Logger _logger;

        public CrawlerNode()
        {
            _logger = new Logger();
        }
        public string RequestPage(string url)
        {
            var webClient = new WebClient();
            try
			{
				HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
				req.Method = "GET";
                req.UserAgent = "Mozilla/5.0 (compatible; bingbot/2.0 +http://www.bing.com/bingbot.htm)";

				var resp = (HttpWebResponse)req.GetResponse();
			    string htmlString = "";
                if (resp.StatusCode == HttpStatusCode.OK)
			    {
			        var stream = resp.GetResponseStream();
			        StreamReader sr = new StreamReader(stream);
			        htmlString = sr.ReadToEnd();
                 //   _logger.DebugFormat("{0} responded with {1}",url,htmlString);
			    }
			    else
			    {
			        _logger.ErrorFormat("{0} responded with code {1}", url, resp.StatusCode);
			        
			    }
                resp.Close();
                return htmlString;
			}
			catch(Exception e)
			{
                _logger.Error("Error crawling page",e);
			    return "";
			}
        }

    }
}
