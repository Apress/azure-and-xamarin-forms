using BookLists.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BookLists
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Microsoft : ContentPage
    {
        public Microsoft()
        {
            InitializeComponent();
            BindingContext = new MicrosoftBooksViewModel();
               //         Items = new ObservableCollection<string>
               //         {
               //             "Item 1",
               //             "Item 2",
               //             "Item 3",
               //             "Item 4",
               //             "Item 5"
               //         };

               //MyListView.ItemsSource = Items;
        }

     void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
           // await DisplayAlert("Selected", e.SelectedItem.ToString(), "OK");
           // navigate to the URL with the native browser
            Device.OpenUri(new Uri(e.SelectedItem.ToString()));
           //Deselect Item
           ((ListView)sender).SelectedItem = null;
        }
    }
}
// At the time this code sample was created, the UWP ListView has an error on clicking and getting the correct selected item in the list
// when using group by
// For UWP Bug status see this post: https://forums.xamarin.com/discussion/108761/uwp-listview-returning-wrong-itemselected , 
// Please test in Android or iOS for now, or add UWP Plaform specifc logic here
// use commented XAML to correct, setting ItemsSource to Items and IsGroupingEnabled="false"
