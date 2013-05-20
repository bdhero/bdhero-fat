using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDHero.BDROM;
using BDInfo;

namespace BDHero.Plugin.DiscReader.Transformer
{
    static class StreamClipTransformer
    {
        public static List<StreamClip> Transform(IEnumerable<TSStreamClip> tsStreamClips)
        {
            return tsStreamClips.Select(Transform).ToList();
        }

        private static StreamClip Transform(TSStreamClip tsStreamClip, int index)
        {
            return new StreamClip(tsStreamClip.StreamFile.FileInfo, tsStreamClip.Name, tsStreamClip.FileSize, index, tsStreamClip.Length);
        }
    }
}
