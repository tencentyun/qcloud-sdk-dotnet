using System.Collections.Generic;
using System.Xml.Serialization;

namespace COSXML.Model.Tag
{
    [XmlRoot("Response")]
    public sealed class MediaInfoResult
    {
        [XmlElement]
        public MediaInfoDetail MediaInfo;
        public sealed class MediaInfoDetail
        {
            [XmlElement]
            public FormatDetail Format;

            [XmlElement]
            public StreamDetail Stream;

            public sealed class StreamDetail 
            {
                [XmlElement]
                public VideoDetail Video;

                [XmlElement]
                public AudioDetail Audio;

                [XmlElement]
                public SubtitleDetail Subtitle;

                public sealed class VideoDetail
                {
                    [XmlElement]
                    public string Index;

                    [XmlElement]
                    public string CodecName;

                    [XmlElement]
                    public string CodecLongName;

                    [XmlElement]
                    public string CodecTimeBase;

                    [XmlElement]
                    public string CodecTagString;

                    [XmlElement]
                    public string CodecTag;

                    [XmlElement]
                    public string Profile;

                    [XmlElement]
                    public string Height;

                    [XmlElement]
                    public string Width;

                    [XmlElement]
                    public string HasBFrame;

                    [XmlElement]
                    public string RefFrames;

                    [XmlElement]
                    public string Sar;

                    [XmlElement]
                    public string Dar;

                    [XmlElement]
                    public string PixFormat;

                    [XmlElement]
                    public string FieldOrder;

                    [XmlElement]
                    public string Level;

                    [XmlElement]
                    public string Fps;

                    [XmlElement]
                    public string AvgFps;

                    [XmlElement]
                    public string Timebase;

                    [XmlElement]
                    public string StartTime;

                    [XmlElement]
                    public string Duration;

                    [XmlElement]
                    public string Bitrate;
                    
                    [XmlElement]
                    public string NumFrames;
                    
                    [XmlElement]
                    public string Language;
                    
                }

                public sealed class AudioDetail
                {
                    [XmlElement]
                    public string Index;

                    [XmlElement("CodecName")]
                    public string CodecName;

                    [XmlElement("CodecLongName")]
                    public string CodecLongName;

                    [XmlElement("CodecTimeBase")]
                    public string CodecTimeBase;

                    [XmlElement("CodecTagString")]
                    public string CodecTagString;

                    [XmlElement("CodecTag")]
                    public string CodecTag;
                    
                    [XmlElement("SampleFmt")]
                    public string SampleFmt;
                    
                    [XmlElement("SampleRate")]
                    public string SampleRate;

                    [XmlElement("Channel")]
                    public string Channel;

                    [XmlElement("ChannelLayout")]
                    public string ChannelLayout;

                    [XmlElement("Timebase")]
                    public string Timebase;

                    [XmlElement("StartTime")]
                    public string StartTime;

                    [XmlElement("Duration")]
                    public string Duration;

                    [XmlElement("Bitrate")]
                    public string Bitrate;
                    
                    [XmlElement("Language")]
                    public string Language;
                }

                public sealed class SubtitleDetail
                {
                    [XmlElement("Index")]
                    public string Index;

                    [XmlElement("Language")]
                    public string Language;
                }
            }

            public sealed class FormatDetail
            {
                [XmlElement("Bitrate")]
                public string Bitrate;

                [XmlElement("NumStream")]
                public string NumStream;

                [XmlElement("NumProgram")]
                public string NumProgram;

                [XmlElement("FormatName")]
                public string FormatName;

                [XmlElement("FormatLongName")]
                public string FormatLongName;

                [XmlElement("StartTime")]
                public string StartTime;

                [XmlElement("Duration")]
                public string Duration;
                
                [XmlElement("Size")]
                public string Size;

            }
        }

    }
}