using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Security;
using System.Globalization;

namespace Startup
{
    /// <summary>
    /// Wrapper for recent file info.
    /// </summary>
    public class RecentItem
    {
        private FileInfo _path;

        public RecentItem(FileInfo path)
        {
            _path = path;
        }

        public FileInfo File { get { return _path; } }

        public override string ToString()
        {
            string filename = "";
            int index = _path.Name.IndexOf(".db", StringComparison.CurrentCultureIgnoreCase);
            if (index != -1)
            {
                filename = _path.Name.Remove(index);
            }

            return filename;
        }
    }

    /// <summary>
    /// Class for managing recent files of the application.
    /// Loads the list of recent files on first use.
    /// </summary>
    static class RecentFiles
    {
        /// <summary>
        /// Recent files. Newest first.
        /// </summary>
        private static List<FileInfo> _recentFiles = LoadRecentFiles();
        private const int MAX_FILES = 10;

        private static List<FileInfo> LoadRecentFiles()
        {
            CreateApplicationDirectory();

            List<FileInfo> recentFiles = new List<FileInfo>();
            try
            {
                string fileName = String.Format(CultureInfo.CurrentCulture,
                                                "{0}{2}{1}{2}recent_files.txt",
                                                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                                Properties.Resources.AppId,
                                                Path.DirectorySeparatorChar);

                string[] paths = File.ReadAllLines(fileName);
                recentFiles.Capacity = paths.Length;
                foreach (string path in paths)
                {
                    if (File.Exists(path))
                        recentFiles.Add(new FileInfo(path));
                }
            }
            catch (IOException ioe)
            {
                Debug.Write(ioe);
            }
            catch (SecurityException se)
            {
                Debug.Write(se);
            }

            return recentFiles;
        }

        private static void SaveRecentFiles()
        {
            try
            {
                string fileName = String.Format(CultureInfo.CurrentCulture,
                                                "{0}{2}{1}{2}recent_files.txt",
                                                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                                Properties.Resources.AppId,
                                                Path.DirectorySeparatorChar);
                File.WriteAllLines(fileName, _recentFiles.Select(fileInfo => fileInfo.FullName).ToArray());
            }
            catch (IOException ioe)
            {
                Debug.Write(ioe);
            }
            catch (SecurityException se)
            {
                Debug.Write(se);
            }
        }

        private static void CreateApplicationDirectory()
        {
            try
            {
                string path = String.Format(CultureInfo.CurrentCulture,
                                            "{0}{1}{2}",
                                            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                            Path.DirectorySeparatorChar,
                                            Properties.Resources.AppId);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (IOException ioe)
            {
                Debug.Write(ioe);
            }
            catch (SecurityException se)
            {
                Debug.Write(se);
            }
        }

        /// <summary>
        /// Add a recently used path in front of the list. Duplicates will be removed.
        /// </summary>
        /// <param name="path"></param>
        public static void AddNewFile(string path)
        {
            _recentFiles.Insert(0, new FileInfo(path));

            _recentFiles = new List<FileInfo>(_recentFiles.Select(fileInfo => fileInfo.FullName).
                                            Distinct().Select(x => new FileInfo(x)).Take(MAX_FILES));
            SaveRecentFiles();
        }

        /// <summary>
        /// Get list of recently opened files. Latest first.
        /// </summary>
        /// <returns></returns>
        public static List<FileInfo> GetRecentFiles()
        {
            return new List<FileInfo>(_recentFiles);
        }
    }
}
