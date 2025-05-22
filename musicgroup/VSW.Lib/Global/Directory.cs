namespace VSW.Lib.Global
{
    public static class Directory
    {
        public static string GetFolder(int gid)
        {
            var strId = gid.ToString();
            var folder = string.Empty;
            folder += strId.Substring(0, 1) + "/";
            return folder;
        }

        public static void Create(string path)
        {
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
        }

        public static void Delete(string path)
        {
            if (System.IO.Directory.Exists(path))
                System.IO.Directory.Delete(path);
        }

        public static void DeleteAll(string path)
        {
            if (!System.IO.Directory.Exists(path)) return;

            foreach (var s in System.IO.Directory.GetDirectories(path))
                DeleteAll(s);

            foreach (var s in System.IO.Directory.GetFiles(path))
                System.IO.File.Delete(s);

            Delete(path);
        }

        public static string[] GetListDir(string path)
        {
            return !System.IO.Directory.Exists(path) ? new string[] { } : System.IO.Directory.GetDirectories(path);
        }
    }
}