using System.Collections.Generic;
using System.Xml.Serialization;

namespace COSXML.Model.Tag
{
    [XmlRoot("Response")]
    public sealed class MediaInfoResult
    {
        [XmlElement("MediaInfo")]
        public MediaInfo mediaInfo;
        public sealed class MediaInfo
        {
            [XmlElement("Format")]
            public Format format;

            [XmlElement("Stream")]
            public Stream stream;

            public sealed class Stream 
            {
                [XmlElement("Video")]
                public Video video;

                [XmlElement("Audio")]
                public Audio audio;

                [XmlElement("Subtitle")]
                public Subtitle subtitle;

                public sealed class Video
                {
                    [XmlElement("Index")]
                    public int index;

                    [XmlElement("CodecName")]
                    public string codecName;

                    [XmlElement("CodecLongName")]
                    public string codecLongName;

                    [XmlElement("CodecTimeBase")]
                    public string codecTimeBase;

                    [XmlElement("CodecTagString")]
                    public string codecTagString;

                    [XmlElement("CodecTag")]
                    public string codecTag;

                    [XmlElement("Profile")]
                    public string profile;

                    [XmlElement("Height")]
                    public int height;

                    [XmlElement("Width")]
                    public int width;

                    [XmlElement("HasBFrame")]
                    public int hasBFrame;

                    [XmlElement("RefFrames")]
                    public int refFrames;

                    [XmlElement("Sar")]
                    public string sar;

                    [XmlElement("Dar")]
                    public string dar;

                    [XmlElement("PixFormat")]
                    public string pixformat;

                    [XmlElement("FieldOrder")]
                    public string fieldOrder;

                    [XmlElement("Level")]
                    public int level;

                    [XmlElement("Fps")]
                    public float fps;

                    [XmlElement("AvgFps")]
                    public string avgFps;

                    [XmlElement("Timebase")]
                    public string timebase;

                    [XmlElement("StartTime")]
                    public float startTime;

                    [XmlElement("Duration")]
                    public float duration;

                    [XmlElement("Bitrate")]
                    public float bitrate;
                    
                    [XmlElement("NumFrames")]
                    public int numFrames;
                    
                    [XmlElement("Language")]
                    public string Language;
                    
                }

                public sealed class Audio
                {
                    [XmlElement("Index")]
                    public int index;

                    [XmlElement("CodecName")]
                    public string codecName;

                    [XmlElement("CodecLongName")]
                    public string codecLongName;

                    [XmlElement("CodecTimeBase")]
                    public string codecTimeBase;

                    [XmlElement("CodecTagString")]
                    public string codecTagString;

                    [XmlElement("CodecTag")]
                    public string codecTag;
                    
                    [XmlElement("SampleFmt")]
                    public string sampleFmt;
                    
                    [XmlElement("SampleRate")]
                    public int sampleRate;

                    [XmlElement("Channel")]
                    public int channel;

                    [XmlElement("ChannelLayout")]
                    public string channelLayout;

                    [XmlElement("Timebase")]
                    public string timebase;

                    [XmlElement("StartTime")]
                    public float startTime;

                    [XmlElement("Duration")]
                    public float duration;

                    [XmlElement("Bitrate")]
                    public float bitrate;
                    
                    [XmlElement("Language")]
                    public string language;
                }

                public sealed class Subtitle
                {
                    [XmlElement("Index")]
                    public int index;

                    [XmlElement("Language")]
                    public string language;
                }
            }

            public sealed class Format
            {
                [XmlElement("Bitrate")]
                public float bitrate;

                [XmlElement("NumStream")]
                public int numStream;

                [XmlElement("NumProgram")]
                public int numProgram;

                [XmlElement("FormatName")]
                public string formatName;

                [XmlElement("FormatLongName")]
                public string formatLongName;

                [XmlElement("StartTime")]
                public float startTime;

                [XmlElement("Duration")]
                public float duration;
                
                [XmlElement("Size")]
                public int size;

            }
        }

    }
}