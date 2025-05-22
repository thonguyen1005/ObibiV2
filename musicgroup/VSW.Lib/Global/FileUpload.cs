using System.Web;
using HttpRequest = VSW.Core.Web.HttpRequest;

namespace VSW.Lib.Global
{
    public class FileUpload
    {
        public string UploadPage { get; set; }
        public string QueryParameters { get; set; }
        public string OnUploadComplete { get; set; }
        public decimal UploadFileSizeLimit { get; set; }
        public decimal TotalUploadSizeLimit { get; set; }
        public string FileTypeDescription { get; set; }
        public string FileTypes { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        private string _htmlUpload = string.Empty;

        public string HtmlUpload => _htmlUpload;

        public void Update()
        {
            var url = "/" + HttpRequest.ApplicationPath + "Data/flash/Upload.swf";

            _htmlUpload = string.Format("<object classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\"" +
                        "codebase=\"http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,0,0\"" +
                        "width=\"{8}\" height=\"{9}\" id=\"fileUpload\" align=\"middle\">" +
                        "<param name=\"allowScriptAccess\" value=\"sameDomain\" />" +
                        "<param name=\"movie\" value=\"{0}\" />" +
                        "<param name=\"quality\" value=\"high\" />" +
                        "<param name=\"wmode\" value=\"transparent\">" +
                        "<PARAM NAME=FlashVars VALUE='{3}{4}{5}{6}{7}&uploadPage={1}?{2}'>" +
                        "<embed src=\"{0}\"" +
                        "FlashVars='{3}{4}{5}{6}{7}&uploadPage={1}?{2}'" +
                        "quality=\"high\" wmode=\"transparent\" width=\"{8}\" height=\"{9}\" " +
                        "name=\"fileUpload\" align=\"middle\" allowScriptAccess=\"sameDomain\" " +
                        "type=\"application/x-shockwave-flash\" " +
                        "pluginspage=\"http://www.macromedia.com/go/getflashplayer\" />" +
                        "</object>",
                        url,
                        UploadPage,
                        HttpContext.Current.Server.UrlEncode(QueryParameters),
                        string.IsNullOrEmpty(OnUploadComplete) ? string.Empty : "&completeFunction=" + OnUploadComplete,
                        string.IsNullOrEmpty(FileTypes) ? string.Empty : "&fileTypes=" + HttpContext.Current.Server.UrlEncode(FileTypes),
                        string.IsNullOrEmpty(FileTypeDescription) ? "" : "&fileTypeDescription=" + HttpContext.Current.Server.UrlEncode(FileTypeDescription),
                        TotalUploadSizeLimit > 0 ? "&totalUploadSize=" + TotalUploadSizeLimit : "",
                        UploadFileSizeLimit > 0 ? "&fileSizeLimit=" + UploadFileSizeLimit : "",
                        Width,
                        Height
                        );
        }
    }
}