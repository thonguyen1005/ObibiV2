using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace VSW.Lib.Global
{
    internal class CompareFileByDate : IComparer
    {
        int IComparer.Compare(object a, object b)
        {
            var fia = new FileInfo((string)a ?? throw new InvalidOperationException());
            var fib = new FileInfo((string)b ?? throw new InvalidOperationException());

            var cta = fia.LastWriteTime;
            var ctb = fib.LastWriteTime;

            return DateTime.Compare(ctb, cta);
        }
    }

    public static class File
    {
        public static string GetFileName(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            var arrPath = path.Split('/');
            if (arrPath.Length > 0)
                return arrPath[arrPath.Length - 1];

            return string.Empty;
        }

        public static void Rename(string sourceFileName, string desFileName)
        {
            sourceFileName = sourceFileName.Replace("%20", " ");

            if (sourceFileName == desFileName) return;

            if (!Exists(sourceFileName)) return;

            if (Exists(desFileName)) Delete(desFileName);
            System.IO.File.Move(sourceFileName, desFileName);
        }

        public static bool Exists(string path)
        {
            return System.IO.File.Exists(path);
        }

        public static void Delete(string path)
        {
            if (Exists(path)) System.IO.File.Delete(path);
        }

        public static string FormatFileName(string fileName)
        {
            return Regex.Replace(fileName, @"[^a-zA-Z_0-9\.]", "-").Replace("--", "-");
        }

        public static string GetFile(string path)
        {
            var index = 0;
            while (true)
            {
                index++;

                var file = path;

                if (index > 1)
                    file = Path.Combine(Path.GetDirectoryName(path) ?? throw new InvalidOperationException(), Path.GetFileNameWithoutExtension(path) + "(" + index + ")." + Path.GetExtension(path)?.Replace(".", ""));

                if (!System.IO.File.Exists(file))
                    return file;
            }
        }

        public static string ReadText(string path)
        {
            var s = string.Empty;
            if (!System.IO.File.Exists(path)) return s;

            var streamReader = new StreamReader(path);
            s = streamReader.ReadToEnd();
            streamReader.Close();
            return s;
        }

        //public static void Delete(string path)
        //{
        //   if (System.IO.File.Exists(path))
        //       System.IO.File.Delete(path);
        //}

        public static void WriteText(string path)
        {
            WriteText(path, string.Empty, false);
        }

        public static void WriteText(string path, string content)
        {
            WriteText(path, content, true);
        }

        public static void WriteText(string path, string content, bool isNew)
        {
            var streamWriter = new StreamWriter(path, isNew);
            streamWriter.WriteLine(content);
            streamWriter.Close();
        }

        public static void WriteTextUnicode(string path)
        {
            WriteTextUnicode(path, string.Empty, false);
        }

        public static void WriteTextUnicode(string path, string content)
        {
            WriteTextUnicode(path, content, true);
        }

        public static void WriteTextUnicode(string path, string content, bool isNew)
        {
            var streamWriter = new StreamWriter(path, isNew, Encoding.Unicode);
            streamWriter.WriteLine(content);
            streamWriter.Close();
        }

        public static DataTable GetListFile(int pageIndex, int pageSize, ref int totalRecord, string path, string searchPattern)
        {
            IComparer fileComparer = new CompareFileByDate();

            return GetListFile(pageIndex,
                pageSize,
                ref totalRecord,
                path,
                searchPattern,
                fileComparer);
        }

        public static DataTable GetListFile(int pageIndex, int pageSize, ref int totalRecord, string path, string searchPattern, IComparer fileComparer)
        {
            if (!System.IO.Directory.Exists(path))
                return null;

            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("FullName", typeof(string)));
            dt.Columns.Add(new DataColumn("Size", typeof(float)));
            dt.Columns.Add(new DataColumn("Path", typeof(string)));
            dt.Columns.Add(new DataColumn("Date", typeof(DateTime)));

            var arrFiles = System.IO.Directory.GetFiles(path, searchPattern);

            totalRecord = arrFiles.Length;

            Array.Sort(arrFiles, fileComparer);

            for (var i = pageIndex * pageSize; i < pageSize * (pageIndex + 1) && i < arrFiles.Length; i++)
            {
                var pathFile = arrFiles[i];

                var fileInfo = new FileInfo(pathFile);

                var fileName = Path.GetFileName(pathFile);
                var dirName = Path.GetFileName(Path.GetDirectoryName(pathFile));

                var dr = dt.NewRow();

                dr["Name"] = fileName;
                dr["FullName"] = path + dirName + "/" + fileName;
                dr["Size"] = fileInfo.Length / (float)1024;
                dr["Date"] = fileInfo.LastWriteTime;
                dr["Path"] = pathFile;

                dt.Rows.Add(dr);
            }

            return dt;
        }

        public static string Download(MVC.ViewPage viewPage, string filePath)
        {
            if (!Exists(filePath)) return "File is not exists.";

            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var response = viewPage.Response;
                var request = viewPage.Request;

                var fileLength = file.Length;
                if (fileLength > int.MaxValue) return "The file length is over value.";

                var lastUpdateTiemStr = System.IO.File.GetLastWriteTimeUtc(filePath).ToString("r");
                var fileName = Path.GetFileName(filePath);
                var fileNameUrlEncoded = HttpUtility.UrlEncode(fileName, Encoding.UTF8);
                var eTag = fileNameUrlEncoded + lastUpdateTiemStr;

                var ifRange = request.Headers["If-Range"];
                if (ifRange != null && ifRange.Replace("\"", "") != eTag) return "Error in Headers If-Range.";

                long startBytes = 0;

                var rangeHeader = request.Headers["Range"];
                if (rangeHeader != null)
                {
                    response.StatusCode = 206;
                    var range = rangeHeader.Split(new[] { '=', '-' });
                    startBytes = Convert.ToInt64(range[1]);
                    if (startBytes < 0 || startBytes >= fileLength) return "Error in Headers Range.";
                }

                response.Clear();
                response.Buffer = false;
                response.AddHeader("Content-MD5", Security.Md5(filePath));
                response.AddHeader("Accept-Ranges", "bytes");
                response.AppendHeader("ETag", $"\"{eTag}\"");
                response.AppendHeader("Last-Modified", lastUpdateTiemStr);
                response.ContentType = "application/octet-stream";
                response.AddHeader("Content-Disposition", "attachment;filename=" + fileNameUrlEncoded.Replace("+", "%20").Replace(",", ";"));

                var remaining = fileLength - startBytes;
                response.AddHeader("Content-Length", remaining.ToString());
                response.AddHeader("Connection", "Keep-Alive");
                response.ContentEncoding = Encoding.UTF8;

                if (startBytes > 0)
                {
                    response.AddHeader("Content-Range", $" bytes {startBytes}-{fileLength - 1}/{fileLength}");
                }

                //BinaryReader implements IDisposable so should be in a using block
                using (var br = new BinaryReader(file))
                {
                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);

                    const int packSize = 1024 * 10; //read in block，every block 10K bytes
                    var maxCount = (int)Math.Ceiling((remaining + 0.0) / packSize); //download in block
                    for (var i = 0; i < maxCount && response.IsClientConnected; i++)
                    {
                        response.BinaryWrite(br.ReadBytes(packSize));
                        response.Flush();
                    }
                }
            }

            return string.Empty;
        }

        public static long GetFileSize(string path)
        {
            var fi = new FileInfo(path);
            return fi.Length;
        }
        public static FileInfo GetFileInfo(string path)
        {
            return new FileInfo(path);
        }
    }
}