using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Database;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using MediaPlayer.Droid;
using MediaPlayer.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(DirectoryPath))]
namespace MediaPlayer.Droid
{
    public class DirectoryPath : IDirectoryPath
    {
        private Context _Context => Android.App.Application.Context;
        public DirectoryPath()
        {
            Files = new List<FileDetail>();
            extensions = new string[]
            {
                "WAV",
                "MP3"
            };
            exludeDirs = new[]
            {
                "android",
                "android/data",
                "android/framework",
                "dcim"
            };
        }

        private File[] GetPath() => new File[] 
        { 
            new File(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyMusic)), 
            new File(Android.OS.Environment.DirectoryMusic),
            Android.OS.Environment.ExternalStorageDirectory,
            new File(Android.OS.Environment.DirectoryDownloads),
            Android.OS.Environment.GetExternalStoragePublicDirectory(""),
            Android.OS.Environment.DataDirectory,
            new File(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)), 
        };

        public async Task<List<FileDetail>> GetFiles()
        {

            var cacheDir = FileSystem.CacheDirectory;
            var mainDir = FileSystem.AppDataDirectory;


            //Retrieve a list of Music files currently listed in the Media store DB via URI.

            //MediaStore..query(
            //android.provider.MediaStore.Audio.Media.EXTERNAL_CONTENT_URI,
            //new String[] { "_id", "_data" },
            //"is_music == 1",
            //null, null);

            //Some audio may be explicitly marked as not being music
            String selection = MediaStore.Audio.Media.ContentType.Contains("is_music") + " == 1";

String[] projection = {
	MediaStore.Audio.Media._ID,
	MediaStore.Audio.Media.ARTIST,
	MediaStore.Audio.Media.TITLE,
	MediaStore.Audio.Media.DATA,
	MediaStore.Audio.Media.DISPLAY_NAME,
	MediaStore.Audio.Media.DURATION,
    MediaStore.Audio.Media.ContentType,
    
};


            ContentResolver contentResolver = _Context.ContentResolver;
            ICursor cursor=contentResolver.Query(MediaStore.Audio.Media.ExternalContentUri,null,null,null);

            if(cursor!=null && cursor.MoveToFirst())
            {
                int songTitle = cursor.GetColumnIndex("Title");
                
            }

//ICursor cursor = this.ManagedQuery(
//	MediaStore.Audio.Media.ExternalContentUri,
//	projection,
//	selection,
//	null,
//	null);

private List<String> songs = new ArrayList<String>();
while(cursor.moveToNext()){
	songs.add(cursor.getString(0) + "||" + cursor.getString(1) + "||" +   cursor.getString(2) + "||" +   cursor.getString(3) + "||" +  cursor.getString(4) + "||" +  cursor.getString(5));
}















            Files = new List<FileDetail>();
            Messages = new List<string>();
            Messages.Add("###Playlist");
            foreach (var home in GetPath())
            {
                Messages.Add("#MAIN " + home.AbsolutePath);
                var tempFiles = (await home.ListFilesAsync());
                Messages.Add("#DIR-COUNT " + tempFiles?.Length);
                if (tempFiles != null && tempFiles?.Length > 0)
                {
                    foreach (var file in tempFiles)
                    {
                        if (file.AbsolutePath == null || IsExists(file.AbsolutePath))
                        {
                        }
                        else if (file.IsDirectory)
                            await SubDirectories(file);
                        else if (extensions.Any(e=>file.Name.Contains(e)))
                            AddFile(file);
                        else
                            Messages.Add("#FILE " + file.AbsolutePath);
                    }
                }
            }

            return Files;
        }

        //private void SubDirectories(File file, string[] extensions)
        //{
        //    var tempFiles = file.ListFiles();
        //    if (tempFiles != null && tempFiles?.Length > 0)
        //    {
        //        foreach (var f in tempFiles)
        //        {
        //            if (file.IsDirectory)
        //                SubDirectories(file, extensions);
        //            else if (f.CanRead() && extensions.Any(e => f.AbsolutePath.Contains(e)))
        //                lists.Add(f.AbsolutePath);
        //        }
        //    }
        //}


        private List<string> songs = new List<string>();
        public List<FileDetail> Files { get; set; }
        //private Context Context => (Forms.Context as MainActivity).Window.Context;
        public List<string> Messages { get; set; }
        private string[] extensions, exludeDirs;


        private bool IsExists(string filePath)
        {
            foreach (var dir in exludeDirs)
            {
                if (filePath.ToLower().Trim().Contains(dir.ToLower().Trim()))
                    return true;
            }

            return false;
        }

        private async Task SubDirectories(File home)
        {
            Messages.Add("#DIR " + home?.AbsolutePath);
            try
            {
                var tempFiles = (await home.ListFilesAsync());
                Messages.Add("#DIR-COUNT " + tempFiles?.Length);
                if (tempFiles != null && tempFiles?.Length > 0)
                {
                    foreach (var file in tempFiles)
                    {
                        if (file.AbsolutePath == null || IsExists(file.AbsolutePath))
                        {
                        }
                        else if (file.IsDirectory)
                            await SubDirectories(file);
                        else if (extensions.Any(e=>file.Name.Contains(e)))
                            AddFile(file);
                        else
                            Messages.Add("#FILE " + file.AbsolutePath);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Messages.Add("E. " + ex.Message);
            }
        }

        private void AddFile(File file)
        {
            Messages.Add("#FILE " + file?.AbsolutePath);
            if (file != null)
                Files.Add(new FileDetail()
                {
                    Name = file.Name,
                    Path = file.AbsolutePath,
                    IsDirectory = file.IsDirectory,
                    IsFile = file.IsFile,
                    Parent = file.Parent
                });
        }
    }
}