using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using AdventureLandCore.Properties;

namespace AdventureLandCore.Services.Helpers
{
    public static class FileHelper
    {
        public static bool IsSubPathOf(this string path, string baseDirPath)
        {
            string normalizedPath = Path.GetFullPath(path.Replace('/', '\\')
                .WithEnding("\\"));

            string normalizedBaseDirPath = Path.GetFullPath(baseDirPath.Replace('/', '\\')
                .WithEnding("\\"));

            return normalizedPath.StartsWith(normalizedBaseDirPath, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns <paramref name="str"/> with the minimal concatenation of <paramref name="ending"/> (starting from end) that
        /// results in satisfying .EndsWith(ending).
        /// </summary>
        /// <example>"hel".WithEnding("llo") returns "hello", which is the result of "hel" + "lo".</example>
        public static string WithEnding([CanBeNull] this string str, string ending)
        {
            if (str == null)
                return ending;

            string result = str;

            // Right() is 1-indexed, so include these cases
            // * Append no characters
            // * Append up to N characters, where N is ending length
            for (int i = 0; i <= ending.Length; i++)
            {
                string tmp = result + ending.Right(i);
                if (tmp.EndsWith(ending))
                    return tmp;
            }

            return result;
        }

        public static string Right([NotNull] this string value, int length)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), length, "Length is less than zero");
            }

            return (length < value.Length) ? value.Substring(value.Length - length) : value;
        }

        public static BitmapImage GetBitmapImage(string filepath)
        {
            if (string.IsNullOrWhiteSpace(filepath) || !File.Exists(filepath))
            {
                return null;
            }

            var img = new BitmapImage();
            var buffer = File.ReadAllBytes(filepath);
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.StreamSource = stream;
                img.EndInit();
            }

            return img;
        }

        public static void CopyFolder(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(sourceFolder))
            {
                Directory.CreateDirectory(sourceFolder);
            }

            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }

            var files = Directory.GetFiles(sourceFolder);

            foreach (var file in files)
            {
                var name = Path.GetFileName(file);
                var dest = Path.Combine(destFolder, name);
                File.Copy(file, dest);
            }

            var folders = Directory.GetDirectories(sourceFolder);
            foreach (var folder in folders)
            {
                var name = Path.GetFileName(folder);
                var dest = Path.Combine(destFolder, name);
                CopyFolder(folder, dest);
            }
        }

        public static void EnsureDirectories(params string[] directories)
        {
            foreach (var directory in directories)
            {
                if (Directory.Exists(directory)) Directory.Delete(directory, true);

                Directory.CreateDirectory(directory);
            }
        }

        private static bool ThumbnailCallback()
        {
            return false;
        }

        public static string SaveObjectBitmapAsThumbnail(string originalFilePath, string directory, string controlId, bool skipSaveFile)
        {
            Bitmap bitmap = null;
            Image thumbNail = null;
            Bitmap newBitmap = null;

            try
            {
                if (string.IsNullOrWhiteSpace(originalFilePath) || !File.Exists(originalFilePath))
                {
                    return string.Empty;
                }

                var myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);

                bitmap = new Bitmap(originalFilePath);

                thumbNail = bitmap.GetThumbnailImage(Math.Min(160,bitmap.Width), Math.Min(160, bitmap.Height), null, IntPtr.Zero);

                var imageDirectoryPath = GetThumbnailImageName(directory, controlId);

                Directory.CreateDirectory(Path.GetDirectoryName(imageDirectoryPath));

                newBitmap = new Bitmap(thumbNail);

                if (!skipSaveFile)
                {
                    if (File.Exists(imageDirectoryPath))
                    {
                        File.Delete(imageDirectoryPath);
                    }

                    newBitmap.Save(imageDirectoryPath, ImageFormat.Png);
                }

                return imageDirectoryPath;
            }
            finally
            {
                bitmap?.Dispose();
                thumbNail?.Dispose();
                newBitmap?.Dispose();
            }
        }

        public static string GetThumbnailImageName(string imageDirectory, string controlId)
        {
            return Path.Combine(imageDirectory, $"{controlId}.png");
        }

        public static void SaveDataFile(string fullFilePath, string content)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullFilePath));

            using (var writer = File.CreateText(fullFilePath))
            {
                writer.WriteLine(content);
            }
        }

        public static string LoadDataFile(string fullFilePath)
        {
            using (var reader = File.OpenText(fullFilePath))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Deletes a directory and all of its sub directories.
        /// </summary>
        /// <param name="directoryFullName">Name of directory to delete.</param>
        public static void DeleteDirectoryRecursive(string directoryFullName)
        {
            Directory.Delete(directoryFullName, true);
        }
    }
}