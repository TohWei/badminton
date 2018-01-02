using System;
using System.IO;
using Xamarin.Forms;

namespace badmintoncourtmanager.DS
{
    public interface ILoadImage
    {
        Stream FromUrl(string uri);
	}
}
