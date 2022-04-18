using DataBase_Website.Models.DataBaseModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
        private void SetUp()
        {
            var request = System.Net.HttpWebRequest.Create($"https://testowanazwa.somee.com/MobileApp/GetJobs?GUID={App.Guid}&AccountID={App.Account.PrivateAccountKey}");
            request.Method = "GET";
            request.ContentType = "application/json";
            try
            {
                using(HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if(response.StatusCode != HttpStatusCode.OK)
                        System.Diagnostics.Debug.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        var content = reader.ReadToEnd();
                        if (string.IsNullOrWhiteSpace(content))
                        {
                            System.Diagnostics.Debug.WriteLine("Response contained empty body...");
                        }
                        else
                        {
                            List<string> Jobs =  Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(content);

                            foreach (string x in Jobs)
                            {
                                var label = new Label()
                                {
                                    Text = x
                                };
                                //To test
                                //label.GestureRecognizers.Add(new TapGestureRecognizer() { Command = ILabelClick});
                                this.ListView.Children.Add(label);
                            }
                            this.UpdateChildrenLayout();
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw;
            };
        }

        public ICommand ILabelClick => new Command(OnLableClick);
        private void OnLableClick()
        {
            throw new NotImplementedException();
        }
    }
}