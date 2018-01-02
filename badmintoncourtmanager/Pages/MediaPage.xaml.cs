using System;
using System.Collections.Generic;
using badmintoncourtmanager.DS;
using Plugin.Media;
using Xamarin.Forms;

namespace badmintoncourtmanager.Pages
{
    public partial class MediaPage : ContentPage
    {
        public MediaPage()
        {
            InitializeComponent();


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

				lblFilePath.Text = file.AlbumPath;

				image.Source = ImageSource.FromStream(() =>
				{
				  var stream = file.GetStream();
				  file.Dispose();
				  return stream;
				});

                //After return stream. Delete the photo from memory / temp file location, which cannot be deleted or accessed by user. Therefore need to delete by system.
                //var fileSystem = DependencyService.Get<IFileSystem>();
                //var isDeleted = fileSystem.DeleteFile(file.Path);
			};



			btnPickPhoto.Clicked += async (sender, args) =>
			{
				if (!CrossMedia.Current.IsPickPhotoSupported)
				{
					await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
					return;
				}

				var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
				{
					PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
				});


				if (file == null)
					return;

                lblFilePath.Text = file.AlbumPath;

				image.Source = ImageSource.FromStream(() =>
				{
				  var stream = file.GetStream();
				  file.Dispose();
				  return stream;
				});
			};


            /*
			takeVideo.Clicked += async (sender, args) =>
			{
				if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
				{
					DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
					return;
				}

				var file = await CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
				{
					Name = "video.mp4",
					Directory = "DefaultVideos",
				});

				if (file == null)
					return;

				DisplayAlert("Video Recorded", "Location: " + file.Path, "OK");

				file.Dispose();
			};



			pickVideo.Clicked += async (sender, args) =>
			{
				if (!CrossMedia.Current.IsPickVideoSupported)
				{
					DisplayAlert("Videos Not Supported", ":( Permission not granted to videos.", "OK");
					return;
				}
				var file = await CrossMedia.Current.PickVideoAsync();

				if (file == null)
					return;

				DisplayAlert("Video Selected", "Location: " + file.Path, "OK");
				file.Dispose();
			};
			*/
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}
