namespace DataBase_Website.Models.DataBaseModels
{
    public class AccountModel
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public string AccountName { get; set; }

        public string PrivateAccountKey { get; set; }

        public MobileApp.Permission Permission { get; set; }
    }
}
