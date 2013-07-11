﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CsQuery;
using CsQuery.Web;
using DotNetUtils.Extensions;

namespace IsanPlugin
{
    public class IsanMetadataProvider
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public TimeSpan Timeout = ServerConfig.Default.Timeout;

        /// <summary>
        /// Spoof IE6 on XP.
        /// </summary>
        private const string UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.1.4322)";

        private const string IsanLookupUrl = "http://web.isan.org/template/1.2/publicSearch.do?code={0}";

        private static readonly Regex TitleYearLengthRegex = new Regex(@"(.*)\s+\(\s*(\d{4})[\s\-]*(\d+)\s*min\s*\)", RegexOptions.IgnoreCase);

        public void Populate(VIsan vIsan)
        {
            if (!vIsan.IsSearchable)
                return;

            var dom = GetDom(vIsan);

            SetTitle(vIsan, dom);
            SetParent(vIsan, dom);
            SetParentTitle(vIsan);
        }

        private CQ GetDom(Isan isan)
        {
            var numberEscaped = Uri.EscapeUriString(isan.Number);
            var url = string.Format(IsanLookupUrl, numberEscaped);
            CQ dom = CQ.CreateFromUrl(url, new ServerConfig { UserAgent = UserAgent, Timeout = Timeout });
            return dom;
        }

        private static void SetTitle(Isan isan, CQ dom)
        {
            var titles = dom[".title"];
            titles.ForEach(delegate(IDomObject o)
                {
                    var innerText = (o.InnerText ?? "").Trim();
                    var fullText = o.Render().StripHtml().TrimLeadingIsan().Trim();
                    TrySetTitle(isan, innerText);
                    TrySetTitle(isan, fullText);
                });
        }

        private static void TrySetTitle(Isan isan, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;
            if (Isan.IsIsan(text))
                Logger.DebugFormat("Full V-ISAN: {0}", text);
            else if (IsTitleYearLength(text))
                SetTitleYearLength(isan, text);
        }

        private static bool IsTitleYearLength(string text)
        {
            return TitleYearLengthRegex.IsMatch(text);
        }

        private static void SetTitleYearLength(Isan isan, string text)
        {
            var match = TitleYearLengthRegex.Match(text);
            isan.Title = match.Groups[1].Value;
            isan.Year = Int32.Parse(match.Groups[2].Value);
            isan.LengthMin = Int32.Parse(match.Groups[3].Value);
        }

        private static void SetParent(VIsan vIsan, CQ dom)
        {
            var titles = dom["a[href*=\"javascript:publicDisplayWork\"]"];
            titles.ForEach(delegate(IDomObject o)
                {
                    var href = (o.GetAttribute("href") ?? "").Trim();
                    if (Isan.IsIsan(href))
                    {
                        vIsan.Parent = Isan.TryParse(href);
                    }
                });
        }

        private void SetParentTitle(VIsan vIsan)
        {
            if (vIsan.Parent == null)
                return;

            var dom = GetDom(vIsan.Parent);

            SetTitle(vIsan.Parent, dom);
        }
    }

    internal static class StringExtensions
    {
        private static readonly Regex LeadingIsanRegex = new Regex(@"\b(?:(?:V-)?ISAN\s*)?([0-9a-f]{4})-([0-9a-f]{4})-([0-9a-f]{4})-([0-9a-f]{4})-?[a-z]?-?([0-9a-f]{4})-?([0-9a-f]{4})-?[a-z]?\b", RegexOptions.IgnoreCase);

        // TODO: Write unit tests
        public static string TrimLeadingIsan(this string str)
        {
            return LeadingIsanRegex.Replace(str, "");
        }
    }
}
