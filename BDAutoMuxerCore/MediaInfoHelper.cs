using BDAutoMuxerCore.BDInfo;
using MediaInfoWrapper;

namespace BDAutoMuxerCore
{
    /// <summary>
    /// Translates between BDInfo and MediaInfo data types and codecs.
    /// </summary>
    public static class MediaInfoHelper
    {
        public static MICodec CodecFromStream(TSStream stream)
        {
            if (stream == null) return MICodec.UnknownCodec;

            var audioStream = stream as TSAudioStream;

            switch (stream.StreamType)
            {
                case TSStreamType.MPEG1_VIDEO:
                    return MICodec.MPEG1Video;
                case TSStreamType.MPEG2_VIDEO:
                    return MICodec.MPEG2Video;
                case TSStreamType.AVC_VIDEO:
                    return MICodec.AVC;
                case TSStreamType.MVC_VIDEO:
                    return MICodec.UnknownVideo;
                case TSStreamType.VC1_VIDEO:
                    return MICodec.VC1;
                case TSStreamType.MPEG1_AUDIO:
                    return MICodec.MP3;
                case TSStreamType.MPEG2_AUDIO:
                    return MICodec.UnknownAudio;
                case TSStreamType.LPCM_AUDIO:
                    return MICodec.LPCM;
                case TSStreamType.AC3_AUDIO:
                    if (audioStream != null && audioStream.AudioMode == TSAudioMode.Extended)
                        return MICodec.AC3EX;
                    if (audioStream != null && audioStream.AudioMode == TSAudioMode.Surround)
                        return MICodec.ProLogic;
                    return MICodec.AC3;
                case TSStreamType.AC3_PLUS_AUDIO:
                case TSStreamType.AC3_PLUS_SECONDARY_AUDIO:
                    return MICodec.EAC3;
                case TSStreamType.AC3_TRUE_HD_AUDIO:
                    return MICodec.TrueHD;
                case TSStreamType.DTS_AUDIO:
                    if (audioStream != null && audioStream.AudioMode == TSAudioMode.Extended)
                        return MICodec.DTSES;
                    return MICodec.DTS;
                case TSStreamType.DTS_HD_AUDIO:
                    return MICodec.DTSHDHRA;
                case TSStreamType.DTS_HD_SECONDARY_AUDIO:
                    return MICodec.DTSExpress;
                case TSStreamType.DTS_HD_MASTER_AUDIO:
                    return MICodec.DTSHDMA;
                case TSStreamType.PRESENTATION_GRAPHICS:
                    return MICodec.PGS;
                case TSStreamType.INTERACTIVE_GRAPHICS:
                    return MICodec.UnknownSubtitle;
                case TSStreamType.SUBTITLE:
                    return MICodec.UnknownSubtitle;
                default:
                    return MICodec.UnknownCodec;
            }
        }
    }
}
