using System;
using System.Collections.Generic;
using AskMJane.LeaflyScraper.Logging;
using AskMJane.LeaflyScraper.Models;
using HtmlAgilityPack;

namespace AskMJane.LeaflyScraper.WorkerNodes
{
    public class ReviewParserNode
    {
        private readonly Logger _logger;

        public ReviewParserNode()
        {
            _logger = new Logger();
        }

        public LeaflyReview ParseLeaflyReview(int id, string content)
        {
            var reviewModel = new LeaflyReview();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);
            var root = doc.DocumentNode;
            var nodes = root.Descendants();

            reviewModel.Id = id;
            reviewModel.Review = GetReview(doc);
            reviewModel.Rating = GetRating(doc);
            reviewModel.Url = GetStrainUrl(doc);
            reviewModel.Category = GetCategory(doc);
            reviewModel.StrainDisplayName = GetStrainTitle(doc);
            reviewModel.LeaflySlug = GetStrainUrl(doc);
            reviewModel.Flavors = GetFlavors(doc);
            reviewModel.Effects = GetEffects(doc);
            reviewModel.Forms = GetForms(doc);
            reviewModel.AcquiredFrom = GetAcquiredFrom(doc);
            reviewModel.AcquiredFromUrl = GetAcquiredFromUrl(doc);
            return reviewModel;
        }


        public decimal GetRating(HtmlDocument doc)
        {
            try
            {

                HtmlNode node = doc.QuerySelector("span[star-rating]");
                var rating = node.Attributes["star-rating"];
                return Decimal.Parse(rating.Value);
            }
            catch (Exception e)
            {
                _logger.Error("Get Rating",e);
                return -1;
            }
        }

        public string GetStrainUrl(HtmlDocument doc)
        {
            try
            {

                HtmlNode node = doc.QuerySelector("a.strain-link");
                var url = node.Attributes["href"];
                return url.Value;
            }
            catch (Exception e)
            {
                _logger.Error("Get Strain URL", e);
                return "";
            }
        }

        public string GetCategory(HtmlDocument doc)
        {
            try
            {

                HtmlNode node = doc.QuerySelector("[data-ng-bind~=category]");
                return node.InnerText;
            }
            catch (Exception e)
            {
                _logger.Error("Get Category",e);
                return "";
            }
        }
        public string GetStrainTitle(HtmlDocument doc)
        {
            try{
                HtmlNode node = doc.QuerySelector("[data-ng-bind-html~=name]");
                return node.InnerText;  
            }
            catch (Exception e)
            {
                _logger.Error("Get Strain Title",e);
                return "";
            }
        }
        public string GetReview(HtmlDocument doc)
        {
            try
            {

                HtmlNode node = doc.QuerySelector("div.m-review > div.padding-rowItem--xl .notranslate");
                return node.InnerText;
            }
            catch (Exception e)
            {
                _logger.Error("Get Review", e);
                return "";
            }
        }

        public List<string> GetForms(HtmlDocument doc)
        {
            try
            {
                var nodes = doc.QuerySelectorAll("div.divider .bottom > h2.heading--lg");
                var forms = new List<string>();
                foreach (var node in nodes)
                {
                    if (node.InnerText.Contains("Form &amp; Method"))
                    {
                        var formnode = node.NextSibling.NextSibling;
                        foreach (var innernode in formnode.ChildNodes)
                        {
                            if (innernode.NodeType == HtmlNodeType.Element)
                            {
                                forms.Add(innernode.InnerText);
                            }
                        }
                    }
                }
                return forms;
            }
            catch (Exception e)
            {
                _logger.Error("Get Forms and Methods",e);
                return null;
            }
        }
        public List<string> GetEffects(HtmlDocument doc)
        {
            try
            {
                var nodes = doc.QuerySelectorAll("div.divider .bottom > h2.heading--lg");
                var effects = new List<string>();
                foreach (var node in nodes)
                {
                    if (node.InnerText.Contains("Effects"))
                    {
                        var formnode = node.NextSibling.NextSibling;
                        foreach (var innernode in formnode.ChildNodes)
                        {
                            if (innernode.NodeType == HtmlNodeType.Element)
                            {
                                effects.Add(innernode.InnerText);
                            }
                        }
                    }
                }
                return effects;
            }
            catch (Exception e)
            {
                _logger.Error("Get Effects",e);
                return null;
            }
        }
        public List<string> GetFlavors(HtmlDocument doc)
        {
            try
            {
                var nodes = doc.QuerySelectorAll("div.divider .bottom > h2.heading--lg");
                var flavors = new List<string>();
                foreach (var node in nodes)
                {
                    if (node.InnerText.Contains("Flavor Profile"))
                    {
                        var formnode = node.NextSibling.NextSibling;
                        foreach (var innernode in formnode.ChildNodes)
                        {
                            if (innernode.NodeType == HtmlNodeType.Element)
                            {
                                flavors.Add(innernode.InnerText);
                            }
                        }
                    }
                }
                return flavors;
            }
            catch (Exception e)
            {
                _logger.Error("Get Flavors", e);
                return null;
            }
        }

        public string GetAcquiredFrom(HtmlDocument doc)
        {
            try
            {

                var nodes = doc.QuerySelectorAll("div.padding-listItem > h2.heading--lg");
                foreach (var node in nodes)
                {
                    if (node.InnerText.Contains("Acquired From"))
                    {
                        var thenode = node.NextSibling.NextSibling;
                        return thenode.InnerText;
                    }
                }
                return "";
            }
            catch (Exception e)
            {
                _logger.ErrorFormat("Error getting Acquired from",e);
                return "";
            }
        }
        public string GetAcquiredFromUrl(HtmlDocument doc)
        {
            try
            {

                var nodes = doc.QuerySelectorAll("div.padding-listItem > h2.heading--lg");
                foreach (var node in nodes)
                {
                    if (node.InnerText.Contains("Acquired From"))
                    {
                        var thenode = node.NextSibling.NextSibling;
                        if (thenode.Attributes["href"] != null)
                        {
                            return thenode.Attributes["href"].Value;
                        }
                    }
                }
                return "";
            }
            catch (Exception e)
            {
                _logger.ErrorFormat("Error getting Acquired from", e);
                return "";
            }
        }
    }
}
