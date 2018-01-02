using System;
using System.IO;
using badmintoncourtmanager.DS;
using badmintoncourtmanager.iOS.DS;
using Xamarin.Forms;

[assembly: Dependency(typeof(DSFileSystem))]
namespace badmintoncourtmanager.iOS.DS
{
    public class DSFileSystem : IFileSystem
    {
        public bool DeleteFile(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                try{
                    File.Delete(source);
                    return true;
                }catch{
                    return false;
                }

            }
            return false;
        }


		//https://stackoverflow.com/questions/39482685/how-to-store-images-in-sqlite-database-in-xamarin-forms-for-cross-platforms-appl
		public string SaveFile(string fileName, byte[] fileStream)
		{
			string path = null;
            string imageFolderPath = System.IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Personal), "PlayerProfileImages");

			//Check if the folder exist or not
			if (!System.IO.Directory.Exists(imageFolderPath))
			{
				System.IO.Directory.CreateDirectory(imageFolderPath);
			}
			string imagefilePath = System.IO.Path.Combine(imageFolderPath, fileName);

			//Try to write the file bytes to the specified location.
			try
			{
				System.IO.File.WriteAllBytes(imagefilePath, fileStream);
				path = imagefilePath;
			}
			catch (System.Exception e)
			{
				throw e;
			}
			return path;
		}

		public void DeleteDirectory()
		{
            string imageFolderPath = System.IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Personal), "PlayerProfileImages");
			if (System.IO.Directory.Exists(imageFolderPath))
			{
				System.IO.Directory.Delete(imageFolderPath, true);
			}
		}
    }
}
