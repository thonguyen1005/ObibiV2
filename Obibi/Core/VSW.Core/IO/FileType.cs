using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace VSW.Core.IO
{
    public enum FileType
    {
        [Description("Images")]
        [Value(".jpg;.jpeg;.png;.tif;.ico;.gif;.bmp")]
        IMAGE,

        [Description("Video")]
        [Value(".mp4")]
        VIDEO,

        [Description("Audio")]
        [Value(".mp3")]
        AUDIO,

        [Description("Pdf document")]
        [Value(".pdf")]
        PDF,

        [Description("Micrososft Word document")]
        [Value(".doc;.docx")]
        DOC,

        [Description("Micrososft Excel document")]
        [Value(".xls;.xlsx")]
        EXCEL,

        [Description("Compressed File")]
        [Value(".zip;.rar;.7z")]
        COMPRESSION,

        [Description("Application File")]
        [Value(".exe;.dll;.bat")]
        APPLICATION,

        [Description("Text File")]
        [Value(".txt;.xml;.json;.html")]
        TEXT,

        [Description("Other File")]
        UNKNOWN
    }
}
