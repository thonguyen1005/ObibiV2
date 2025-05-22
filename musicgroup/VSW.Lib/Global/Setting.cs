using VSW.Core.Global;

namespace VSW.Lib.Global
{
    public class Setting : Core.Web.Setting
    {
        public static string EXTENSION_IMAGES_ALLOW = "gif,jpg,png,jpeg,pjpeg,swf"; //cac duoi img hop le duoc dang len web
        public static int filesize = 10; //10MB 
        public static string keymap = "AIzaSyBk34Dz4fna4SoMs1DjRUxx8l0EZwEOMn0";
        public static bool HasTraGopBaoKim = VSW.Core.Global.Convert.ToBool(VSW.Core.Global.Config.GetValue("Mod.TraGopBaoKim"));
        public static string bk_Success = VSW.Core.Global.Config.GetValue("bk_Success").ToString();
        public static string bk_Error = VSW.Core.Global.Config.GetValue("bk_Error").ToString();
        public static string bk_Webhook = VSW.Core.Global.Config.GetValue("bk_Webhook").ToString();
        public static string bk_Key = VSW.Core.Global.Config.GetValue("bk_Key").ToString();
        public static string bk_Secret = VSW.Core.Global.Config.GetValue("bk_Secret").ToString();
        public static long bk_MeChanID = VSW.Core.Global.Config.GetValue("bk_MeChanID").ToLong();
    }
}