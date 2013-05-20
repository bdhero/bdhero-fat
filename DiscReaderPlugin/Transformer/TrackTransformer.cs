using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDHero.BDROM;
using BDInfo;

namespace BDHero.Plugin.DiscReader.Transformer
{
    static class TrackTransformer
    {
        // TODO: Make this thread-safe
        private static int _numVideo;
        private static int _numAudio;
        private static int _numSubtitle;

        public static IList<Track> Transform(IEnumerable<TSStream> streams)
        {
            _numVideo = _numAudio = _numSubtitle = 0;
            return streams.Select(Transform).ToList();
        }

        public static Track Transform(TSStream stream, int index)
        {
            var videoStream = stream as TSVideoStream;
            var audioStream = stream as TSAudioStream;
            var subtitleStream = stream as TSGraphicsStream;

            var indexOfType = 0;

            if (videoStream != null) indexOfType = _numVideo++;
            if (audioStream != null) indexOfType = _numAudio++;
            if (subtitleStream != null) indexOfType = _numSubtitle++;

            return new Track
            {
                Index = index,
                PID = stream.PID,
                Language = stream.Language,
                IsHidden = stream.IsHidden,
                Codec = CodecTransformer.CodecFromStream(stream),
                IsVideo = stream.IsVideoStream,
                IsAudio = stream.IsAudioStream,
                IsSubtitle = stream.IsGraphicsStream || stream.IsTextStream,
                ChannelCount = audioStream != null ? audioStream.ChannelCountDouble : 0,
                BitDepth = audioStream != null ? audioStream.BitDepth : 0,
                VideoFormat = videoStream != null ? videoStream.VideoFormat : 0,
                FrameRate = videoStream != null ? videoStream.FrameRate : TSFrameRate.Unknown,
                AspectRatio = videoStream != null ? videoStream.AspectRatio : TSAspectRatio.Unknown,
                IndexOfType = indexOfType
            };
        }
    }
}
