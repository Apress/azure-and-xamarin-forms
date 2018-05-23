using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;


namespace BookLists.ViewModels
{
    public class MicrosoftBooksViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Item> Items { get; }

        public ObservableCollection<Grouping<string, Item>> ItemsGrouped { get; }

        public MicrosoftBooksViewModel()
        {
            Items = new ObservableCollection<Item>(new[]
            {

                new Item { Text = "Beginning Entity Framework Core 2.0",
                    URL ="https://www.apress.com/us/book/9781484233740",
                    Detail = "Use the valuable Entity Framework Core 2.0 tool in ASP.NET and the .NET Framework to eliminate the tedium around accessing databases and the data they contain. Entity Framework Core 2.0 greatly simplifies access to relational databases such as SQL Server that are commonly deployed in corporate settings. By eliminating tedious data access code that developers are otherwise forced to use, Entity Framework Core 2.0 enables you to work directly with the data in a database through domain-specific objects and methods." },

                new Item { Text = "Beginning Windows Mixed Reality Programming, For HoloLens and Mixed Reality Headsets",
                    URL ="https://www.apress.com/us/book/9781484227688",
                    Detail = "Develop applications and experiences for Microsoft’s HoloLens and other Windows mixed reality devices. This easy-to-follow guide removes the mystery behind creating amazing augmented reality experiences. Mixed reality development tools and resources are provided. Beginning Windows Mixed Reality Programming clearly explains all the nuances of mixed reality software development. You'll learn how to create 3D objects and holograms, interact with holograms using voice commands and hand gestures, use spatial mapping and 3D spatial sound, build with Microsoft's HoloToolkit, create intuitive user interfaces, and make truly awe-inspiring mixed reality experiences. Start building the holographic future today!" },

                new Item { Text = "Business in Real-Time, Using Azure IoT and Cortana Intelligence Suite Driving Your Digital Transformation",
                    URL ="https://www.apress.com/us/book/9781484226490",
                    Detail = "Learn how today’s businesses can transform themselves by leveraging real-time data and advanced machine learning analytics. This book provides prescriptive guidance for architects and developers on the design and development of modern Internet of Things(IoT) and Advanced Analytics solutions.In addition, Business in Real - Time Using Azure IoT and Cortana Intelligence Suite offers patterns and practices for those looking to engage their customers and partners through Software -as- a - Service solutions that work on any device. Whether you're working in Health & Life Sciences, Manufacturing, Retail, Smart Cities and Buildings or Process Control, there exists a common platform from which you can create your targeted vertical solutions. Business in Real-Time Using Azure IoT and Cortana Intelligence Suite uses a reference architecture as a road map. Building on Azure’s PaaS services, you'll see how a solution architecture unfolds that demonstrates a complete end - to - end IoT and Advanced Analytics scenario." },

                new Item { Text = "Cyber Security on Azure, An IT Professional’s Guide to Microsoft Azure Security Center",
                    URL ="https://www.apress.com/us/book/9781484227398",
                    Detail = "Prevent destructive attacks to your Azure public cloud infrastructure, remove vulnerabilities, and instantly report cloud security readiness. This book provides comprehensive guidance from a security insider's perspective. Cyber Security on Azure explains how this 'security as a service' (SECaaS) business solution can help you better manage security risk and enable data security control using encryption options such as Advanced Encryption Standard(AES) cryptography.Discover best practices to support network security groups, web application firewalls, and database auditing for threat protection. Configure custom security notifications of potential cyberattack vectors to prevent unauthorized access by hackers, hacktivists, and industrial spies." },

                new Item { Text = "Essential Angular for ASP.NET Core MVC",
                    URL ="https://www.apress.com/us/book/9781484229156",
                    Detail = "Angular 5 and .NET Core 2 updates for this book are now available. Follow the Download Source Code link for this book on the Apress website. Discover Angular, the leading client-side web framework, from the point of view of an ASP.NET Core MVC developer. Best-selling author Adam Freeman brings these two key technologies together and explains how to use ASP.NET Core MVC to provide back-end services for Angular applications. This fast - paced, practical guide starts from the nuts and bolt and gives you the knowledge you need to combine Angular(from version 2.0 up) and ASP.NET Core MVC in your projects. " },



             });
   
            var sorted = from item in Items
                         orderby item.Text
                         group item by item.Text[0].ToString() into itemGroup
                         select new Grouping<string, Item>(itemGroup.Key, itemGroup);


            ItemsGrouped = new ObservableCollection<Grouping<string, Item>>(sorted);



            RefreshDataCommand = new Command(
                async () => await RefreshData());
        }

        public ICommand RefreshDataCommand { get; }

        async Task RefreshData()
        {
            IsBusy = true;
            //Load Data Here
            await Task.Delay(2000);

            IsBusy = false;
        }

        bool busy;
        public bool IsBusy
        {
            get { return busy; }
            set
            {
                busy = value;
                OnPropertyChanged();
                ((Command)RefreshDataCommand).ChangeCanExecute();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public class Item
        {
            public string Text { get; set; }
            public string Detail { get; set; }
            public string URL { get; set; }
            public override string ToString() => URL;
        }

        public class Grouping<K, T> : ObservableCollection<T>
        {
            public K Key { get; private set; }

            public Grouping(K key, IEnumerable<T> items)
            {
                Key = key;
                foreach (var item in items)
                    this.Items.Add(item);
            }
        }
    }
}
