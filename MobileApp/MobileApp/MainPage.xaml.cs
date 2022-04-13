using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MobileApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        HttpClient client;
        private async void  Button_Clicked(object sender, EventArgs e)
        {
            client = new HttpClient();

            Droid.LoginModel LoginData = new Droid.LoginModel() 
            { 
                Login = Login.Text, 
                Password = Password.Text
            };

            //check if any of neccessary field is empty 
            //if one of this field is empty just give warning "Password or login are empty"
            //if both are not empty continue 
            //TODO TEST
            if (LoginData.Login == string.Empty || LoginData.Password == string.Empty /*|| (LoginData.Login == string.Empty && LoginData.Password == string.Empty)*/)
            {
                NotificifationLabel.Text = "Password or login are empty";
                NotificifationLabel.TextColor = Color.Red;
                return;
            }
            else
            {
                NotificifationLabel.Text = "Logging in";
                NotificifationLabel.TextColor = Color.Green;
            }

            Droid.Cryptography.EncryptLoginModel(ref LoginData);

            var values = new Dictionary<string, string>
             {
                { "Login", LoginData.Login },
                { "Password", LoginData.Password }
             };

            var content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = null;
            try
            {
                Uri Url = new Uri("https://testowanazwa.somee.com/MobileApp/Login");

                response = await client.PostAsync(Url, content);
            }
            catch (Exception exc)
            {
                
            };
                //Thread.Sleep(500);

            if(response == null)
            {
                NotificifationLabel.Text = "Error while connecting to service";
                NotificifationLabel.TextColor = Color.Red;
                return;
            }
            if (!response.IsSuccessStatusCode)
            {
                NotificifationLabel.Text = "Failed to Login";
                NotificifationLabel.TextColor = Color.Red;
                return;
            }

            string result = await response.Content.ReadAsStringAsync();

            LoginResponse LoginResult_Converted = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginResponse>(result);
            System.Diagnostics.Debug.WriteLine(result);
            if(LoginResult_Converted.Status == 2)
            {
                NotificifationLabel.Text = "Logging in failed";
                NotificifationLabel.TextColor = Color.Red;
                Login.Text = string.Empty;
                Password.Text = string.Empty;
            }
            else
            {
                //await Navigation.PushAsync(new Page1());
                App.Guid = LoginResult_Converted.guid;
                App.Account = LoginResult_Converted.Account;
                Application.Current.MainPage = new Page1();
            }
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            Password.IsPassword = !CheckBox.IsChecked;
        }
    }
}
