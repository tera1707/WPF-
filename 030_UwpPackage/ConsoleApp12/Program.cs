using ClassLibrary1;
using System;
using System.Diagnostics;

namespace ConsoleApp12
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = string.Empty;

            try
            {
                var localDir = Windows.Storage.ApplicationData.Current.LocalFolder;
                path = localDir.Path;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            }

            Debug.WriteLine("target path is : " + path);
        }
    }
}
