using VSW.Core.Crypto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VSW.Core.IO
{
    public static class FileHelper
    {

        private static string _pathSeparator;
        public static string PathSeparator
        {
            get
            {
                if (_pathSeparator.IsEmpty())
                    _pathSeparator = Convert.ToString(Path.DirectorySeparatorChar);

                return _pathSeparator;
            }
        }

        public static string ReadText(string filename, Encoding encoding = null)
        {
            if (!FileExists(filename))
            {
                return null;
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            string s = File.ReadAllText(filename, encoding);
            return s;
        }

        public static string[] ReadAllLines(string filename, Encoding encoding = null)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException(filename);
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            var result = File.ReadAllLines(filename, encoding);

            return result;
        }

        public static bool SaveTextFile(string content, string filePath, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var outfile = new StreamWriter(fs, encoding))
                {
                    outfile.Write(content);
                }
            }
            return true;
        }

        public static bool SaveFile(byte[] content, string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                fs.Write(content, 0, content.Length);
            }

            return true;
        }

        public static bool SaveFile(byte[] marker, byte[] content, string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                fs.Write(marker, 0, marker.Length);
                fs.Write(content, 0, content.Length);
            }

            return true;
        }

        public static byte[] LoadFile(string filePath)
        {
            byte[] buffer = File.ReadAllBytes(filePath);
            return buffer;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static string GetTempPath()
        {
            return Path.GetTempPath();
        }

        public static string GetTempFileName()
        {
            return Path.GetRandomFileName();
        }

        /// <summary>
        /// Chỉ trả về tên file, không tạo file như Path.GetTempFileName
        /// </summary>
        /// <returns></returns>
        public static string GetFullTempFileName()
        {
            return Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        }

        /// <summary>
        /// Bao gồm cả dấu \ cuối đường dẫn
        /// </summary>
        /// <returns></returns>
        private static string _executingFolder;
        public static string GetExecutingFolder()
        {
            if (_executingFolder == null)
            {
                _executingFolder = AppDomain.CurrentDomain.DynamicDirectory;
                if (_executingFolder == null)
                {
                    _executingFolder = AppDomain.CurrentDomain.BaseDirectory;
                }
            }
            return _executingFolder;
        }

        public static string GetAbsolutePath(string path)
        {
            if (Path.IsPathRooted(path))
            {
                return path;
            }
            var p = Path.Combine(GetExecutingFolder(), path);
            return p;
        }

        public static string GetUpFolderPath(string folderPath, int level = 1)
        {
            var p = folderPath;
            for (int i = 0; i < level; i++)
            {
                p = Path.Combine(p, @".." + PathSeparator);
            }
            string r = Path.GetFullPath(p);
            return r;
        }

        /// <summary>
        /// Trả về đường dẫn của thư mục chứa file/folder trong tham số
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string GetFolderPath(string fullPath)
        {
            return Path.GetDirectoryName(fullPath);
        }

        /// <summary>
        /// Chỉ lấy tên, không phải đường dẫn
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string GetFileFolderName(string fullPath)
        {
            if (fullPath.EndsWith(PathSeparator))
            {
                fullPath = fullPath.Substring(0, fullPath.Length - 1);
            }
            return Path.GetFileName(fullPath);
        }

        public static string GetFileNameWithoutExtension(string fullPath)
        {
            return Path.GetFileNameWithoutExtension(fullPath);
        }

        /// <summary>
        /// Get Extionsion of File (include "." charactor)
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string GetExtension(string fullPath)
        {
            return Path.GetExtension(fullPath);
        }

        public static List<string> GetExtensions(this FileType type)
        {
            var arr = type.GetValueAttr().Split(";").ToList();
            return arr;
        }

        public static string GetFileFilter(this FileType type)
        {
            var desc = type.GetDescription();
            var exts = GetExtensions(type);
            var extValue = exts.Select(x => "*" + x).Join(";");
            return "{0} ({1})|{1}".Format(desc, extValue);
        }

        public static string GetFileFilters(params FileType[] types)
        {
            var lst = types.Select(x => x.GetFileFilter()).ToList();
            return lst.Join("|");
        }

        public static bool FolderExists(string path)
        {
            return Directory.Exists(path);
        }

        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public static bool IsFolder(string path)
        {
            var attr = File.GetAttributes(path);
            bool isFolder = attr.HasFlag(FileAttributes.Directory);
            return isFolder;
        }

        /// <summary>
        /// Có kiểm tra sự tồn tại của folder trước khi tạo
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return true;
        }

        public static string[] GetAllFilesInDir(string path)
        {
            string[] files = GetAllFilesInDir(path, "*", SearchOption.AllDirectories);
            return files;
        }

        /// <summary>
        /// Get File Size in byte
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static long GetFileSize(string path)
        {
            var fi = new FileInfo(path);
            return fi.Length;
        }

        public static FileInfo GetFileInfo(string path)
        {
            return new FileInfo(path);
        }

        public static DateTime GetCreationTime(string path)
        {
            DateTime t = File.GetCreationTime(path);
            return t;
        }

        public static string[] GetAllFilesInDir(string path, string pattern, SearchOption option = SearchOption.AllDirectories)
        {
            if (!FolderExists(path))
            {
                return new string[0];
            }
            string[] files = Directory.GetFiles(path, pattern, option);
            return files;
        }

        public static bool Copy(string srcFileName, string destFileName)
        {
            File.Copy(srcFileName, destFileName, true);
            return true;
        }

        public static bool Delete(string fileName)
        {
            if (FileExists(fileName))
                File.Delete(fileName);

            return true;
        }

        public static bool Move(string srcFileName, string destFileName)
        {
            File.Copy(srcFileName, destFileName, true);
            File.Delete(srcFileName);
            return true;
        }

        public static bool IsEmptyFolder(string path)
        {
            string[] files = GetAllFilesInDir(path);
            return (files == null) || (files.Length == 0);
        }

        public static List<string> GetChildrenFolders(string path, int level)
        {
            if (level == 1)
            {
                return GetChildrenFolders(path);
            }
            else if (level > 1)
            {
                var temp = GetChildrenFolders(path);
                if (temp.IsNotEmpty())
                {
                    foreach (string key in temp)
                    {
                        var tempChild = GetChildrenFolders(key, level - 1);
                        if (tempChild.IsNotEmpty())
                        {
                            temp.AddRange(tempChild);
                        }
                    }
                }

                return temp;
            }

            return new List<string>();
        }

        private static List<string> GetChildrenFolders(string path, SearchOption options = SearchOption.TopDirectoryOnly)
        {
            var dir = new DirectoryInfo(path);
            var subDirs = dir.GetDirectories(String.Empty, options);
            return subDirs.Select(d => d.FullName).ToList();
        }

        public static bool MoveFolder(string src, string dest)
        {
            Directory.Move(src, dest);
            return true;
        }

        public static bool DeleteFolder(string folder)
        {
            if (FolderExists(folder))
            {
                Directory.Delete(folder, true);
            }

            return true;
        }

        public static bool CopyFolder(string src, string dest)
        {
            if (Directory.Exists(src) && Directory.Exists(dest))
            {
                string[] files = Directory.GetFiles(src);

                // Copy the files and overwrite destination files if they already exist.
                foreach (string s in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    string fileName = Path.GetFileName(s);
                    if (fileName != null)
                    {
                        string destFile = Path.Combine(dest, fileName);
                        File.Copy(s, destFile, true);
                    }
                }
                return true;
            }

            return false;
        }

        public static List<string> GetFiles(string folder, SearchOption opt = SearchOption.TopDirectoryOnly, string pattern = "*", string excludePattern = null)
        {
            var arr = pattern.Split('|');
            var files = new List<string>();
            foreach (var p in arr)
            {
                files.AddRange(GetAllFilesInDir(folder, p, opt));
            }
            if (excludePattern != null)
            {
                var exclFiles = new List<string>();
                arr = excludePattern.Split('|');
                foreach (var p in arr)
                {
                    exclFiles.AddRange(GetAllFilesInDir(folder, p, opt));
                }
                foreach (var f in exclFiles)
                {
                    var s = files.Find(x => x.Equals(f));
                    if (s != null)
                    {
                        files.Remove(s);
                    }
                }
            }
            return files;
        }

        public static string GetCurrentDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public static List<string> GetFilesByPattern(string pathPattern, string basePath = "")
        {
            pathPattern = GetFullPath(pathPattern, basePath);

            if (!pathPattern.Contains("*"))
            {
                return new List<string> { pathPattern };
            }

            var folder = GetFolderPath(pathPattern);
            if (folder.IsEmpty())
            {
                folder = GetCurrentDirectory();
            }

            var pattern = GetFileFolderName(pathPattern);
            return GetFiles(folder, pattern: pattern);
        }

        private static Dictionary<string, FileType> _dicFileType = new Dictionary<string, FileType>();
        private static void InitFileType()
        {
            if (_dicFileType.Count == 0)
            {
                var values = EnumExtensions.GetValues<FileType>();
                if (values.IsNotEmpty())
                {
                    foreach (var e in values)
                    {
                        var attValue = e.GetAttribute<ValueAttribute>();
                        if (attValue != null && attValue.Value.IsNotEmpty())
                        {
                            var arr = attValue.Value.SplitWithTrim(";");
                            foreach (var a in arr)
                            {
                                _dicFileType.Add(a, e);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get File Type by Extension
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static FileType GetFileType(string path)
        {
            InitFileType();
            var ext = GetExtension(path);
            if (ext.IsEmpty())
            {
                return FileType.UNKNOWN;
            }

            ext = ext.ToLower();
            if (_dicFileType.ContainsKey(ext))
            {
                return _dicFileType[ext];
            }

            return FileType.UNKNOWN;
        }

        public static string GetFullPath(string path)
        {
            string fullpath = Path.GetFullPath(path);
            fullpath = fullpath.Replace(@"~\", "");
            return fullpath;
        }
        public static string GetFullPath(string path, string basePath = "")
        {
            if (!Path.IsPathRooted(path))
            {
                if (basePath.IsEmpty())
                {
                    basePath = GetCurrentDirectory();
                }

                path = Path.Combine(basePath, path);
            }

            //Cần phải xử lý trường hợp này do chạy trên .net 4.8 bị lỗi không lấy FullPath của Path có chứa dấu * được
            var name = GetFileFolderName(path);
            if (name.IsNotEmpty() && name.Contains("*"))
            {
                path = GetFolderPath(path);
                var rs = Path.GetFullPath(path);
                return Path.Combine(rs, name);
            }
            //---------------------------------------------------

            return Path.GetFullPath(path);
        }

        /// <summary>
        /// Chuyển file theo mẫu, có loại trừ từ thư mục này sang thư mục khác, có giữ nguyên cấu trúc thư mục bên trong
        /// </summary>
        /// <param name="srcFolder"></param>
        /// <param name="destFolder"></param>
        /// <param name="pattern"></param>
        /// <param name="excludePattern"></param>
        /// <returns></returns>
        public static bool MoveFiles(string srcFolder, string destFolder, string pattern = "*", string excludePattern = null)
        {
            if (!FolderExists(srcFolder))
            {
                return false;
            }
            if (!FolderExists(destFolder))
            {
                CreateFolder(destFolder);
            }

            var arr = pattern.Split('|');
            var files = GetFiles(srcFolder, SearchOption.TopDirectoryOnly, pattern, excludePattern);
            //
            foreach (string s in files)
            {
                var fileName = Path.GetFileName(s);
                var destFile = Path.Combine(destFolder, fileName);
                Move(s, destFile);
            }
            // đệ quy
            string[] folders = Directory.GetDirectories(srcFolder);
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                MoveFiles(folder, dest, pattern, excludePattern);
            }
            return true;
        }

        public static bool CopyFiles(string srcFolder, string destFolder, string pattern = "*", string excludePattern = null)
        {
            if (!FolderExists(srcFolder))
            {
                return false;
            }
            if (!FolderExists(destFolder))
            {
                CreateFolder(destFolder);
            }

            var files = GetFiles(srcFolder, SearchOption.TopDirectoryOnly, pattern, excludePattern);
            //
            foreach (string s in files)
            {
                var fileName = Path.GetFileName(s);
                var destFile = Path.Combine(destFolder, fileName);
                Copy(s, destFile);
            }
            // đệ quy
            string[] folders = Directory.GetDirectories(srcFolder);
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                CopyFiles(folder, dest, pattern, excludePattern);
            }
            return true;
        }

    }
}
