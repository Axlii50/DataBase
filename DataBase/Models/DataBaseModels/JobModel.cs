using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase_Website.Models.DataBaseModels
{
    public class JobModel
    {
        [Key]
        public string JobId { get; set; }

        [DataType(DataType.Text)]
        public string AssignedAccounts { get; set; }

        #region Functions
        public void AddAccount(ref AccountModel Account) => AssignedAccounts += $"{Account.PrivateAccountKey}:";

        public void RemoveAccount(ref AccountModel Account)
        {
            string[] Accounts = AssignedAccounts.Split(':');

            string NewAssignedAccountString = String.Empty;

            foreach (string x in Accounts)
                if (x != Account.PrivateAccountKey)
                    NewAssignedAccountString += $"{x}:";

            AssignedAccounts = NewAssignedAccountString;
        }

        //return all Assigned Accounts to this job
        public List<string> Accounts
        {
            get => AssignedAccounts.Split(':').ToList<string>();
        }
        #endregion

        [DataType(DataType.Text)]
        public string AssignedImages { get; set; }

        #region Functions
        public void AddImage(string ImageName) => AssignedImages += $"{ImageName}:";

        public void RemoveImage(string ImageName)
        {
            string[] Images = AssignedAccounts.Split(':');

            string NewAssignedImagesString = String.Empty;

            foreach (string x in Accounts)
                if (x != ImageName)
                    NewAssignedImagesString += $"{x}:";

            AssignedImages = NewAssignedImagesString;
        }

        //return all Assigned Accounts to this job
        public List<string> Images
        {
            get => AssignedImages.Split(':').ToList<string>();
        }
        #endregion

    }
}
