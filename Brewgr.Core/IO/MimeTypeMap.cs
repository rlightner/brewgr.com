using System;
using System.Collections.Generic;
using System.Linq;

namespace ctorx.Core.IO
{
    public static class MimeTypeMap
    {
        static Dictionary<string, string> ExtensionMimeMap;
 
        static MimeTypeMap()
        {
            ExtensionMimeMap = new Dictionary<string, string>();

            ExtensionMimeMap.Add("avi", "video/x-msvideo");
            ExtensionMimeMap.Add("bmp", "image/bmp");
            ExtensionMimeMap.Add("doc", "application/msword");
            ExtensionMimeMap.Add("docx", "application/msword");
            ExtensionMimeMap.Add("gif", "image/gif");
            ExtensionMimeMap.Add("gtar", "application/x-gtar");
            ExtensionMimeMap.Add("gz", "application/x-gzip");
            ExtensionMimeMap.Add("htm", "text/html");
            ExtensionMimeMap.Add("html", "text/html");
            ExtensionMimeMap.Add("jpe", "image/jpeg");
            ExtensionMimeMap.Add("jpeg", "image/jpeg");
            ExtensionMimeMap.Add("jpg", "image/jpeg");
            ExtensionMimeMap.Add("mov", "video/quicktime");
            ExtensionMimeMap.Add("mp2", "video/mpeg");
            ExtensionMimeMap.Add("mpa", "video/mpeg");
            ExtensionMimeMap.Add("mpe", "video/mpeg");
            ExtensionMimeMap.Add("mpeg", "video/mpeg");
            ExtensionMimeMap.Add("mpg", "video/mpeg");
            ExtensionMimeMap.Add("mpv2", "video/mpeg");
            ExtensionMimeMap.Add("pbm", "image/x-portable-bitmap");
            ExtensionMimeMap.Add("pdf", "application/pdf");
            ExtensionMimeMap.Add("png", "image/png");
            ExtensionMimeMap.Add("pps", "application/vnd.ms-powerpoint");
            ExtensionMimeMap.Add("ppt", "application/vnd.ms-powerpoint");
            ExtensionMimeMap.Add("rtf", "application/rtf");
            ExtensionMimeMap.Add("rtx", "text/richtext");
            ExtensionMimeMap.Add("tar", "application/x-tar");
            ExtensionMimeMap.Add("tgz", "application/x-compressed");
            ExtensionMimeMap.Add("tif", "image/tiff");
            ExtensionMimeMap.Add("tiff", "image/tiff");
            ExtensionMimeMap.Add("txt", "text/plain");
            ExtensionMimeMap.Add("xls", "application/vnd.ms-excel");
            ExtensionMimeMap.Add("xlsx", "application/vnd.ms-excel");
            ExtensionMimeMap.Add("zip", "application/zip");
        }

        /// <summary>
        /// Gets a common mime type for a given extension
        /// </summary>
        public static string GetMimeTypeFromExtension(string extension)
        {
	        return
		        ExtensionMimeMap.Where(x => x.Key.ToLower() == extension.ToLower()).Select(x => x.Value).FirstOrDefault() ??
			        "application/octet-stream";
        }
    }
}