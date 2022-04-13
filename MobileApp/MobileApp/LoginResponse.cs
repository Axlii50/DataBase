using DataBase_Website.Models.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp
{
    struct LoginResponse
    {
        public int Status { get; set; } // 1 ok 2 bad
        public string guid { get; set; }
        public AccountModel Account { get; set; }
    }

    public enum Permission
    {
        Admin,
        Regular,
        NULL
    }

}
