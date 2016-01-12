using System;
using System.Text.RegularExpressions;

namespace Jane.API.Helpers
{
    public static class YouTubeVideoHelper
    {
        public static string TransformYouTubeVideo(string url)
        {
            if (url == null)
                return null;

            url = url.Trim();

            if (String.IsNullOrEmpty(url))
                return url;

            if (Regex.IsMatch(url, @"https://www.youtube.com/embed/([A-Za-z0-9-_+=]+)$"))
            {
                return url;
            }
            else
            {
                Match m = Regex.Match(url, @"https://youtu.be/([A-Za-z0-9-_+=]+)$");

                if (m.Success && m.Groups.Count == 2)
                    return "https://www.youtube.com/embed/" + m.Groups[1];
            }

            return null;
        }
    }
}