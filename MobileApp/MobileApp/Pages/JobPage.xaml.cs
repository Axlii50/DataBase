using DataBase_Website.Models.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JobPage : ContentPage
    {
        private string JobId;

        public JobPage(string JobId)
        {
            InitializeComponent();

            this.JobId = JobId;
            this.JobIDLabel.Text = JobId;
            SetUp();
        }

        private void SetUp()
        {
            var request = System.Net.HttpWebRequest.Create($"https://testowanazwa.somee.com/MobileApp/GetJob?GUID={App.Guid}&JobId={this.JobId}");
            request.Method = "GET";
            request.ContentType = "application/json";
            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
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
                            JobModel Job = Newtonsoft.Json.JsonConvert.DeserializeObject<JobModel>(content);

                            foreach (string x in Job.Images)
                            {
                                var Image = new Image()
                                {
                                    Source = "https://testowanazwa.somee.com/MobileApp/Download?GUID="+ App.Guid + "&fileName=" + /*"alena-aenami-fade-1k.jpg"*/ x
                                };
                                //To test
                                this.Images.Children.Add(Image);
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

        protected override bool OnBackButtonPressed()
        {
            Application.Current.MainPage = App.accPage;
            return true;
        }
    }
}
