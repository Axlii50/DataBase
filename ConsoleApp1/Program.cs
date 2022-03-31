using System;

namespace ConsoleApp1
{
    class LoginModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {

            LoginModel LoginData = new LoginModel()
            {
                Login = "Admin",
                Password = "admin"
            };


            CryptoGraphy.EncryptLoginModel(ref LoginData);

            Console.WriteLine(LoginData.Login);
            Console.WriteLine(LoginData.Password);

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}
