using System;
using System.IO;
using badmintoncourtmanager.DS;
using badmintoncourtmanager.iOS.DS;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(DSLoadImageIOS))]
namespace badmintoncourtmanager.iOS.DS
{
    public class DSLoadImageIOS : ILoadImage
    {
		public Stream FromUrl(string uri)
		{
            using (var url = new NSUrl(uri))
            {
                using (var data = NSData.FromUrl(url))
                {
                    var s = UIImage.LoadFromData(data).AsPNG().AsStream();
                    return s;
                }
            }
		}
    }
}
