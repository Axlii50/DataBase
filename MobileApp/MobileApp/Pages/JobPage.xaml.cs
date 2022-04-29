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
            //set request type to GET method
            request.Method = "GET";
            //set what type of content we expect from method 
            request.ContentType = "application/json";
            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    //check if result status code was acceptable 
                    if (response.StatusCode != HttpStatusCode.OK)
                        System.Diagnostics.Debug.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        //read content from stream and check if is empty or null
                        var content = reader.ReadToEnd();
                        if (string.IsNullOrWhiteSpace(content))
                        {
                            System.Diagnostics.Debug.WriteLine("Response contained empty body...");
                        }
                        else
                        {
                            //deserialize content from response 
                            JobModel Job = Newtonsoft.Json.JsonConvert.DeserializeObject<JobModel>(content);

                            //add eache image contained in result 
                            foreach (string x in Job.Images)
                            {
                                var Image = new Image()
                                {
                                    //set source for image 
                                    Source = "https://testowanazwa.somee.com/MobileApp/Download?GUID="+ App.Guid + "&fileName=" + x
                                };
                                this.Images.Children.Add(Image);
                            }
                            this.UpdateChildrenLayout();
                        }
                        reader.Dispose();
                    }
                    response.Dispose();
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
            //when return true you cancle base reaction to back button 
            return true;
        }
    }
}
