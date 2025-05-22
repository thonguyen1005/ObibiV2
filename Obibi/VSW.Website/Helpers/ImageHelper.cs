using SixLabors.ImageSharp.Formats.Webp;
using VSW.Core;

namespace VSW.Website.Helpers
{
    public static class ImageHelper
    {
        public static string GetUrlView(string filePath)
        {
            if (filePath.IsEmpty()) return "";
            if (filePath.StartsWith("http")) return filePath;
            if (filePath.StartsWith("~")) filePath = filePath.Substring(1);
            return filePath;
        }
        public static string ResizeToWebp(string filePath, int width = 0, int height = 0, int quality = 100)
        {
            if (filePath.IsEmpty()) return "";
            if (filePath.StartsWith("http")) return filePath;
            if (filePath.StartsWith("~")) filePath = filePath.Substring(1);
            if (filePath.StartsWith("/")) filePath = filePath.Substring(1);

            var inputPath = Path.Combine(WebAppExtensions.GetContentPath(), filePath);
            if (!File.Exists(inputPath))
            {
                return filePath;
            }

            var uploadRoot = Path.Combine(WebAppExtensions.GetContentPath(), "Data", "upload");
            var relativeUploadPath = Path.GetRelativePath(uploadRoot, inputPath);
            var outputPath = Path.Combine(WebAppExtensions.GetContentPath(), "Data", "ResizeImage", relativeUploadPath);
            outputPath = Path.ChangeExtension(outputPath, ".webp");

            if (!Directory.Exists(Path.GetDirectoryName(outputPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

            if (!File.Exists(outputPath))
            {
                using var image = Image.Load(inputPath);

                // Resize nếu có thông số cụ thể
                if (width > 0 || height > 0)
                {
                    int finalWidth = width;
                    int finalHeight = height;

                    if (width == 0)
                    {
                        finalWidth = (int)((double)height / image.Height * image.Width);
                    }
                    else if (height == 0)
                    {
                        finalHeight = (int)((double)width / image.Width * image.Height);
                    }

                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(finalWidth, finalHeight)
                    }));
                }

                var encoder = new WebpEncoder
                {
                    Quality = quality,
                    FileFormat = WebpFileFormatType.Lossy
                };

                image.Save(outputPath, encoder);
            }

            var webOutputPath = "/Data/ResizeImage/" + relativeUploadPath.Replace("\\", "/");
            webOutputPath = Path.ChangeExtension(webOutputPath, ".webp");
            return webOutputPath;
        }

        public static async Task<string> ResizeToWebpAsync(string filePath, int width = 0, int height = 0, int quality = 100)
        {
            if (filePath.IsEmpty()) return "";
            if (filePath.StartsWith("http")) return filePath;
            if (filePath.StartsWith("~")) filePath = filePath.Substring(1);
            if (filePath.StartsWith("/")) filePath = filePath.Substring(1);

            var inputPath = Path.Combine(WebAppExtensions.GetContentPath(), filePath);
            if (!File.Exists(inputPath))
            {
                return filePath;
            }

            var uploadRoot = Path.Combine(WebAppExtensions.GetContentPath(), "Data", "upload");
            var relativeUploadPath = Path.GetRelativePath(uploadRoot, inputPath);
            var outputPath = Path.Combine(WebAppExtensions.GetContentPath(), "Data", "ResizeImage", relativeUploadPath);
            outputPath = Path.ChangeExtension(outputPath, ".webp");

            if (!Directory.Exists(Path.GetDirectoryName(outputPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

            if (!File.Exists(outputPath))
            {
                using var image = await Image.LoadAsync(inputPath);

                // Resize nếu có thông số cụ thể
                if (width > 0 || height > 0)
                {
                    int finalWidth = width;
                    int finalHeight = height;

                    if (width == 0)
                    {
                        finalWidth = (int)((double)height / image.Height * image.Width);
                    }
                    else if (height == 0)
                    {
                        finalHeight = (int)((double)width / image.Width * image.Height);
                    }

                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(finalWidth, finalHeight)
                    }));
                }

                var encoder = new WebpEncoder
                {
                    Quality = quality,
                    FileFormat = WebpFileFormatType.Lossy
                };

                await image.SaveAsync(outputPath, encoder);
            }

            var webOutputPath = "/Data/ResizeImage/" + relativeUploadPath.Replace("\\", "/");
            webOutputPath = Path.ChangeExtension(webOutputPath, ".webp");
            return webOutputPath;
        }
    }
}
