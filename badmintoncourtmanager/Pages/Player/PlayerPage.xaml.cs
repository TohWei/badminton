using System;
using System.Collections.Generic;
using badmintoncourtmanager.PageModels;
using Plugin.Media;
using PropertyChanged;
using Xamarin.Forms;

namespace badmintoncourtmanager.Pages
{
    [AddINotifyPropertyChangedInterface]
    public partial class PlayerPage : ContentPage
    {
        public PlayerPage()
        {
            InitializeComponent();

          
            var tap = new TapGestureRecognizer();
            tap.Tapped += (sender, e) => {
                Navigation.PushAsync(new MediaPage());
            };
            photoHolder.GestureRecognizers.Add(tap);



            /*
			btnTakePhoto.Clicked += async (sender, args) =>
			{

				if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
				{
					await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
					return;
				}


				var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
				{
					PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
					Directory = "Sample",
					Name = "test.jpg",
					AllowCropping = true,
					SaveToAlbum = true
				});

				if (file == null)
					return;

				//DisplayAlert("File Location", file.Path, "OK");

				lblPhotoFilePath.Text = file.AlbumPath;
               

				imagePhoto.Source = ImageSource.FromStream(() =>
				{
					var stream = file.GetStream();
					file.Dispose();
					return stream;
				});

                //Need to use bindingContext first. Then bind the filepath to photoProperty.


				//After return stream. Delete the photo from memory / temp file location, which cannot be deleted or accessed by user. Therefore need to delete by system.
				//var fileSystem = DependencyService.Get<IFileSystem>();
				//var isDeleted = fileSystem.DeleteFile(file.Path);
			};
			*/
        }

    }
}
