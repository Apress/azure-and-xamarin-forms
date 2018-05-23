using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BookLists
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
            if (Device.Idiom == TargetIdiom.Phone)
            {
                TabletView.IsVisible = false;
                PhoneView.IsVisible = true;
            }
            else
            {
                TabletView.IsVisible = true;
                PhoneView.IsVisible = false;
            }
     

        }



        private void MicrosoftBooks_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Microsoft());
        }
        private void Programming_Clicked(object sender, EventArgs e)
        {
           
        }
        private void Mobile_Clicked(object sender, EventArgs e)
        {

        }
        private void MachineLearning_Clicked(object sender, EventArgs e)
        {

        }
    }
}
