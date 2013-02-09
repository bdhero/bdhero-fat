using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using DotNetUtils.Annotations;

namespace BDAutoMuxerCore.Services
{
    public class HttpImageCache
    {
        private static HttpImageCache _instance;
        public static HttpImageCache Instance
        {
            get { return _instance ?? (_instance = new HttpImageCache()); }
        }

        private readonly MemoryCache _cache = new MemoryCache("bdam_http_image_cache");

        [CanBeNull]
        public Image GetImage([NotNull] string url)
        {
            return _cache.Contains(url) ? _cache[url] as Image : Fetch(url);
        }

        [CanBeNull]
        private Image Fetch([NotNull] string url)
        {
            var image = HttpRequest.GetImage(url);
            _cache.Set(url, image, new CacheItemPolicy());
            return image;
        }
    }
}
