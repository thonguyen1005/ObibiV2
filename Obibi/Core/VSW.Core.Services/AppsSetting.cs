namespace VSW.Core.Services
{
    public class AppsSetting
    {
        public string Code { get; set; }
        public bool MultiSite { get; set; }
        public bool Debug { get; set; }
        public string[] StaticFileExtensions { get; set; }
    }
}
