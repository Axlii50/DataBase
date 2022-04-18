using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase_Website.Models.DataBaseModels
{
    public class JobModel
    {
        public string JobId { get; set; }

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
            get => AssignedAccounts?.Split(':').ToList<string>();
        }
        #endregion

        public string AssignedImages { get; set; }

        #region Functions
        public void AddImage(string ImageName) => AssignedImages += $"{ImageName}:";

        public void RemoveImage(string ImageName)
        {
            string[] Images = AssignedAccounts.Split(':');

            string NewAssignedImagesString = String.Empty;

            foreach (string x in Images)
                if (x != ImageName)
                    NewAssignedImagesString += $"{x}:";

            AssignedImages = NewAssignedImagesString;
        }

        //return all Assigned Accounts to this job
        public List<string> Images
        {
            get => AssignedImages?.Split(':').ToList<string>();
        }
        #endregion

    }
}
