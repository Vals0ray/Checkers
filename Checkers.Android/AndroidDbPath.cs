using System;
using Checkers.Droid;
using System.IO;
using Xamarin.Forms;
using Checkers.Models;

[assembly: Dependency(typeof(AndroidDbPath))]
namespace Checkers.Droid
{
    public class AndroidDbPath : IPath
    {
        public string GetDatabasePath(string filename)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), filename);
        }
    }
}