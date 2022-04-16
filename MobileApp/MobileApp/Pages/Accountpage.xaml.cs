using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

            GuidLabel.Text = App.Account.PrivateAccountKey;

            SetUp();

        }
        private async void SetUp()
        {
            var values = new Dictionary<string, string>
             {
                { "GUID", App.Guid },
                { "AccountID", App.Account.PrivateAccountKey }
             };

            HttpClient client = new HttpClient();
            var content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = null;
            try
            {
                Uri Url = new Uri("https://testowanazwa.somee.com/MobileApp/GetJob");

                response = await client.PostAsync(Url, content);
                System.Diagnostics.Debug.Write(response);
            }
            catch (Exception exc)
            {

            };
        }
    }
}