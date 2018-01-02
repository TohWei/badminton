using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using badmintoncourtmanager.DS;
using badmintoncourtmanager.Managers;
using badmintoncourtmanager.Models;
using Plugin.Media;
using PropertyChanged;
using Xamarin.Forms;

namespace badmintoncourtmanager.PageModels
{
    [AddINotifyPropertyChangedInterface]
    public class PlayerPageModel : FreshMvvm.FreshBasePageModel
    {
        //Using iOC to inject.
        IPlayerManager _playerManager;

        IFileSystem _fileSystemHelper;

        List<string> GenderList = new List<string>() {"Please specify", "Male", "Female" };

        public Player Player { get; set; }

        //More on binding to UI elements.
        public string PageTitle { get; set; }

		public int PlayerLevel { get; set;}

		public bool HideBtnDelete { get; set; }

        public DateTime DefaultDOB{
            get{
                if (Player.DOB != null){
                    return Player.DOB;
                }else{
                    return DateTime.Now.AddYears(-80);    
                }
            }
			set
			{
				if (Player.DOB != value)
				{
					Player.DOB = value;
				}
			}
        }

        public DateTime DatePickerMinimumDateDOB{
            get {
                 return DateTime.Now.AddYears(-80);
            }
        }


        public List<string> GenderPickerList
        {
            get
            {
                return GenderList;
            }
        }

        public string SelectedItemGender
        {
            set
            {
                if (Player.Gender != value)
                {
                    Player.Gender = value;
                }
            }
            get
            {
                if (string.IsNullOrEmpty(Player.Gender))
                {
                    return Player.Gender = GenderList[0];
                }else{
                    return Player.Gender;
                }
            }
        }

        //Return Player's level if record found. Else set to 0;
        double currentProgress = 0f;
        public double LevelValueChangedSlider
        {
			get {
                if(Player.Level > -1){
                    return Player.Level;
				}
                return currentProgress; 
            }
			set
			{
				currentProgress = value;
                Player.Level = (int)currentProgress;
                PlayerLevel = Player.Level;
                RaisePropertyChanged("PlayerLevel");
			}
        }


        //Path to file system.
        string _photoFilePathToFileSys;
        public string PhotoFilePathToFileSys
        {
            get { 
                return _photoFilePathToFileSys; 
            }
            set { 
                _photoFilePathToFileSys = value; 
                RaisePropertyChanged("PhotoFilePathToFileSys"); //Notify the changes.
			}
        }

        ImageSource _imageSource;
        public ImageSource ImageSource { 
            get{
                return _imageSource;
            } 
            set {
                _imageSource = value;
                RaisePropertyChanged("ImageSource");
            }
        }


        //Constructor, pass in iOC.
        public PlayerPageModel(IPlayerManager playerManager)
        {
            _playerManager = playerManager;
            _fileSystemHelper = DependencyService.Get<IFileSystem>();
        }

        //Pre load data. Either add new player or edit. 
        public override void Init(object initData)
        {
            base.Init(initData);


            Player = initData as Player;

            if (Player == null)
            {
                Player = new Player();
                PageTitle = "Create New Player";
                HideBtnDelete = false;
            }
            else
            {
                PhotoFilePathToFileSys = Player.ProfilePhotoFilePath;

                if (!string.IsNullOrEmpty(PhotoFilePathToFileSys))
                {
                    ImageSource = ImageSource.FromFile(PhotoFilePathToFileSys);
                }

                PageTitle = "Edit Player";
                HideBtnDelete = true;
            }
        }


        //Binding to Save button.
        public Command SavePlayer
        {
            get
            {
                return new Command(async () =>
                {
                    if (!string.IsNullOrEmpty(PhotoFilePathToFileSys))
                    {
                        Player.ProfilePhotoFilePath = PhotoFilePathToFileSys;
                    }

                    if (!string.IsNullOrEmpty(Player.FirstName) && !string.IsNullOrEmpty(Player.LastName) && Player.Gender != GenderList[0])
                    {
                        await _playerManager.CreateOrUpdatePlayer(Player);
                        await CoreMethods.DisplayAlert("Created User", Player.FirstName + ", " + Player.LastName, "OK", "Cancel");
                        await CoreMethods.PopPageModel(Player);
                    }else{
                        await CoreMethods.DisplayAlert("Unable to save the form", "Please complete the registration form.", "OK");
                        await Task.FromResult(0);
                    }
                });
            }
        }

        public Command DeletePlayer
        {
            get
            {
                return new Command(async () =>
                {
                    var confirmed = await CoreMethods.DisplayAlert("Delete a player", "Are you sure you want to delete this player?", "OK", "Cancel");
                    if (confirmed)
                    {
                        var isDeleted = await _playerManager.DeletePlayer(Player.ID);
                        await CoreMethods.DisplayAlert("Status", isDeleted.ToString(), "OK");
                        if (isDeleted)
                        {
                            _fileSystemHelper.DeleteFile(PhotoFilePathToFileSys);
                            await CoreMethods.DisplayAlert("Status", "Image deleted", "OK");
                        }
                    }
                    await CoreMethods.PopPageModel(Player);
                });
            }
        }


        public Command TapPhotoCommand
        {
            get
            {
                return new Command(async (arg) =>
                {
                    await CoreMethods.DisplayAlert("Take Photo", "TapVia Command", "OK");
                    await CoreMethods.PushPageModel<MediaPageModel>();
                });
            }
        }


        public Command TakePhoto
        {
            get{
                return new Command(async (arg) =>
                {
					if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
					{
                        await CoreMethods.DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
						return;
					}


					var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
					{
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
						Directory = "Sample",
						Name = "temp.jpg",
						AllowCropping = true,
						//SaveToAlbum = true
					});

					if (file == null)
						return;

                    await CoreMethods.DisplayAlert("File Location", file.Path, "OK");


                    //Get the file stream.
					var stream = file.GetStream();
					file.Dispose();

                    //Turn the stream into bytes.
					var memoryStream = new System.IO.MemoryStream();
					stream.CopyTo(memoryStream);
					byte[] myBynary = memoryStream.ToArray();

                    //Save the file as bytes into file system. Keep the file path. 
                    Guid _guid = Guid.NewGuid();
                    var fileName = string.Format("{0}.jpg", _guid.ToString());
                    PhotoFilePathToFileSys = _fileSystemHelper.SaveFile(fileName, myBynary);

                    //Show the saved image in UI. 
                    ImageSource = ImageSource.FromFile(PhotoFilePathToFileSys);



                    /*
					ImageSource = ImageSource.FromStream(() =>
					{
						var stream = file.GetStream();
						file.Dispose();

                        var fileSystemHelper = DependencyService.Get<IFileSystem>();

						var memoryStream = new System.IO.MemoryStream();
                        stream.CopyTo(memoryStream);
						byte[] myBynary = memoryStream.ToArray();
						
                        PhotoAlbumFilePath = fileSystemHelper.SaveFile("test.jpg", myBynary);
                        System.Diagnostics.Debug.WriteLine("PhotoAlbumFilePath : " +PhotoAlbumFilePath);

						return stream;
					});
					*/
                });
				
            }
			
        }

        public Command PickPhoto{
			get
            {
                return new Command(async (arg) =>
                {
					if (!CrossMedia.Current.IsPickPhotoSupported)
					{
						await CoreMethods.DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
						return;
					}

					var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
					{
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small
					});


					if (file == null)
						return;

					//Get the file stream.
					var stream = file.GetStream();
					file.Dispose();

					//Turn the stream into bytes.
					var memoryStream = new System.IO.MemoryStream();
					stream.CopyTo(memoryStream);
					byte[] myBynary = memoryStream.ToArray();

					//Save the file as bytes into file system. Keep the file path. 
					Guid _guid = Guid.NewGuid();
					var fileName = string.Format("{0}.jpg", _guid.ToString());
					PhotoFilePathToFileSys = _fileSystemHelper.SaveFile(fileName, myBynary);

					//Show the saved image in UI. 
					ImageSource = ImageSource.FromFile(PhotoFilePathToFileSys);
					
                    /*
					image.Source = ImageSource.FromStream(() =>
					{
						var stream = file.GetStream();
						file.Dispose();
						return stream;
					});
					*/
                });
            }
        }


    }
}
