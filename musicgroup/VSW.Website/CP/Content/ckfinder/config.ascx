<%@ Control Language="C#" EnableViewState="false" AutoEventWireup="false" Inherits="CKFinder.Settings.ConfigFile" %>
<%@ Import Namespace="CKFinder.Settings" %>
<script runat="server">

    private Permissions _Permissions = null;
    public Permissions Permissions
    {
        get
        {
            if (_Permissions != null)
                return _Permissions;

            if (CPLogin.CurrentUser != null)
                return _Permissions = CPLogin.CurrentUser.GetPermissionsByModule("ModFile");

            try
            {
                var suser = Request.QueryString[VSW.Core.Web.Setting.Sys_SiteID + "_CP.UserID"].Replace(" ", "+");
                var IP = VSW.Core.Web.HttpRequest.IP;
                var suserid = VSW.Core.Global.CryptoString.Decrypt(suser).Replace(IP + "_VSW_" + VSW.Core.Web.Setting.Sys_SiteID + "_CP.UserID", string.Empty);
                var _UserID = VSW.Core.Global.Convert.ToInt(suserid);

                if (_UserID < 1)
                    return _Permissions = new Permissions(0);

                var _User = CPUserService.Instance.GetByID(_UserID);

                if (_User == null)
                    return _Permissions = new Permissions(0);

                return _Permissions = _User.GetPermissionsByModule("ModFile");
            }
            catch
            {
                return _Permissions = new Permissions(0);
            }
        }
    }

    public override bool CheckAuthentication()
    {
        return Permissions.Any;
    }

    public override void SetConfig()
    {
        // The base URL used to reach files in CKFinder through the browser.
        BaseUrl = "/Data/upload/";

        // The phisical directory in the server where the file will end up. If
        // blank, CKFinder attempts to resolve BaseUrl.
        BaseDir = "";
        LicenseName = "localhost";
        LicenseKey = "5E774QBSDPQE2KS5SJHFU2J118JQ1LFV";

        // Optional: enable extra plugins (remember to copy .dll files first).
        Plugins = new string[] {
             "CKFinder.Plugins.ImageResize, CKFinder_ImageResize"
        };

        // Settings for extra plugins.
        PluginSettings = new Hashtable();
        PluginSettings.Add("ImageResize_smallThumb", "90x90");
        PluginSettings.Add("ImageResize_mediumThumb", "120x120");
        PluginSettings.Add("ImageResize_largeThumb", "180x180");
        // Name of the watermark image in plugins/watermark folder
        PluginSettings.Add("Watermark_source", "logo.gif");
        PluginSettings.Add("Watermark_marginRight", "5");
        PluginSettings.Add("Watermark_marginBottom", "5");
        PluginSettings.Add("Watermark_quality", "90");
        PluginSettings.Add("Watermark_transparency", "80");

        // Thumbnail settings.
        // "Url" is used to reach the thumbnails with the browser, while "Dir"
        // points to the physical location of the thumbnail files in the server.
        Thumbnails.Url = BaseUrl + "_thumbs/";
        if (BaseDir != "")
        {
            Thumbnails.Dir = BaseDir + "_thumbs/";
        }
        Thumbnails.Enabled = true;
        Thumbnails.DirectAccess = false;
        Thumbnails.MaxWidth = 100;
        Thumbnails.MaxHeight = 100;
        Thumbnails.Quality = 80;

        // Set the maximum size of uploaded images. If an uploaded image is
        // larger, it gets scaled down proportionally. Set to 0 to disable this
        // feature.
        Images.MaxWidth = 1600;
        Images.MaxHeight = 1200;
        Images.Quality = 80;

        // Indicates that the file size (MaxSize) for images must be checked only
        // after scaling them. Otherwise, it is checked right after uploading.
        CheckSizeAfterScaling = true;

        // Increases the security on an IIS web server.
        // If enabled, CKFinder will disallow creating folders and uploading files whose names contain characters
        // that are not safe under an IIS 6.0 web server.
        DisallowUnsafeCharacters = true;

        // Due to security issues with Apache modules, it is recommended to leave the
        // following setting enabled. It can be safely disabled on IIS.
        ForceSingleExtension = true;

        // For security, HTML is allowed in the first Kb of data for files having the
        // following extensions only.
        HtmlExtensions = new string[] { "html", "htm", "xml", "js" };

        // Folders to not display in CKFinder, no matter their location. No
        // paths are accepted, only the folder name.
        // The * and ? wildcards are accepted.
        HideFolders = new string[] { ".svn", "CVS" };

        // Files to not display in CKFinder, no matter their location. No
        // paths are accepted, only the file name, including extension.
        // The * and ? wildcards are accepted.
        HideFiles = new string[] { ".*" };

        // Perform additional checks for image files.
        SecureImageUploads = true;

        // The session variable name that CKFinder must use to retrieve the
        // "role" of the current user. The "role" is optional and can be used
        // in the "AccessControl" settings (bellow in this file).
        RoleSessionVar = "CKFinder_UserRole";

        // ACL (Access Control) settings. Used to restrict access or features
        // to specific folders.
        // Several "AccessControl.Add()" calls can be made, which return a
        // single ACL setting object to be configured. All properties settings
        // are optional in that object.
        // Subfolders inherit their default settings from their parents' definitions.
        //
        //	- The "Role" property accepts the special "*" value, which means
        //	  "everybody".
        //	- The "ResourceType" attribute accepts the special value "*", which
        //	  means "all resource types".
        var acl = AccessControl.Add();
        acl.Role = "*";
        acl.ResourceType = "*";
        acl.Folder = "/";

        acl.FolderView = Permissions.Any;
        acl.FolderCreate = Permissions.Add;
        acl.FolderRename = Permissions.Edit;
        acl.FolderDelete = Permissions.Delete;

        acl.FileView = Permissions.Any;
        acl.FileUpload = Permissions.Add;
        acl.FileRename = Permissions.Edit;
        acl.FileDelete = Permissions.Delete;

        // Resource Type settings.
        // A resource type is nothing more than a way to group files under
        // different paths, each one having different configuration settings.
        // Each resource type name must be unique.
        // When loading CKFinder, the "type" querystring parameter can be used
        // to display a specific type only. If "type" is omitted in the URL,
        // the "DefaultResourceTypes" settings is used (may contain the
        // resource type names separated by a comma). If left empty, all types
        // are loaded.

        DefaultResourceTypes = "";

        ResourceType type;

        type = ResourceType.Add("Files");
        type.Url = BaseUrl + "files/";
        type.Dir = BaseDir == "" ? "" : BaseDir + "files/";
        type.MaxSize = 0;
        type.AllowedExtensions = new string[] { "7z", "aiff", "asf", "avi", "bmp", "csv", "doc", "docx", "fla", "flv", "gif", "gz", "gzip", "jpeg", "ico", "jpg", "mid", "mov", "mp3", "mp4", "mpc", "mpeg", "mpg", "ods", "odt", "pdf", "png", "ppt", "pptx", "pxd", "qt", "ram", "rar", "rm", "rmi", "rmvb", "rtf", "sdc", "sitd", "swf", "sxc", "sxw", "tar", "tgz", "tif", "tiff", "txt", "vsd", "wav", "wma", "wmv", "xls", "xlsx", "zip" };
        type.DeniedExtensions = new string[] { };

        type = ResourceType.Add("Images");
        type.Url = BaseUrl + "images/";
        type.Dir = BaseDir == "" ? "" : BaseDir + "images/";
        type.MaxSize = 0;
        type.AllowedExtensions = new string[] { "bmp", "gif", "jpeg", "jpg", "png", "ico", "webp" };
        type.DeniedExtensions = new string[] { };

        type = ResourceType.Add("Flash");
        type.Url = BaseUrl + "flash/";
        type.Dir = BaseDir == "" ? "" : BaseDir + "flash/";
        type.MaxSize = 0;
        type.AllowedExtensions = new string[] { "swf", "flv" };
        type.DeniedExtensions = new string[] { };
    }

</script>
