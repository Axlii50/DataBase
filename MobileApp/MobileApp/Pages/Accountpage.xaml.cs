﻿using DataBase_Website.Models.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountPage : ContentPage
    {
        public AccountPage()
        {
            InitializeComponent();

            GuidLabel.Text = App.Account.PrivateAccountKey;

            SetUp();
        }
        private void SetUp()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

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
                            List<string> Jobs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(content);

                            foreach (string x in Jobs)
                            {
                                var button = new Button()
                                {
                                    Text = x,

                                };
                                button.Clicked += button_Clicked;
                                //To test
                                this.ListView.Children.Add(button);
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
            stopwatch.Stop();
            System.Diagnostics.Debug.WriteLine(stopwatch.Elapsed);
        }

        private void button_Clicked(object sender, EventArgs e)
        {
            Button _button = (Button)sender;
            Application.Current.MainPage = new Pages.JobPage(_button.Text);
        }
    }
}