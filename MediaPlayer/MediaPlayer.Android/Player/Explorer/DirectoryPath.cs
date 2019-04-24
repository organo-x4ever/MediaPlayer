using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using MediaPlayer.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(DirectoryPath))]
namespace MediaPlayer.Droid
{
    public class DirectoryPath : IDirectoryPath
    {
        public DirectoryPath()
        {
            lists = new List<string>();
        }

        private string[] GetPath() => new string[] { System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyMusic), Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryMusic };
        List<string> lists;
        public List<string> GetFiles(string[] extensions)
        {
            foreach (var path in GetPath())
            {
                File file = new File(path);
                SubDirectories(file, extensions);
            }
            return lists;
        }

        private void SubDirectories(File file, string[] extensions)
        {
            var tempFiles = file.ListFiles();
            if (tempFiles != null && tempFiles?.Length > 0)
            {
                foreach (var f in tempFiles)
                {
                    if (file.IsDirectory)
                        SubDirectories(file, extensions);
                    else if (f.CanRead() && extensions.Any(e => f.AbsolutePath.Contains(e)))
                        lists.Add(f.AbsolutePath);
                }
            }
        }
    }
}