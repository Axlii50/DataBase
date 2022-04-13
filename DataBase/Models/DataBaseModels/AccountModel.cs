using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase_Website.Models.DataBaseModels
{
    public class AccountModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Text)]
        public string AccountName { get; set; }

        [Key]
        [Required]
        [DataType(DataType.Text)]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string PrivateAccountKey { get; set; }

        [Required]
        public DataBase.Models.Permission Permission { get; set; }
    }
}
