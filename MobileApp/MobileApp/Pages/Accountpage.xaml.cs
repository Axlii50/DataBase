using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        public Page1()
        {
            InitializeComponent();

            GuidLabel.Text = App.Guid;

            Image1.Source = $"https://testowanazwa.somee.com/MobileApp/Download?fileName=asteroidbelt.jpg&GUID={App.Guid}";
            
        }
    }
}