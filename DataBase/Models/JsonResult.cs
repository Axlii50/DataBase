using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Models
{

    public enum Permission
    {
        Admin,
        Regular,
        NULL
    }

    public class EnumTypes
    {
        public static SelectList GetPermissionsTypes()
        {
            return new SelectList(Enum.GetValues(typeof(Permission)));
        }
    }

    public class JsonResult
    {
        public int Status { get; set; } // 1 ok 2 bad
        public string guid { get; set; }
        public Permission Permission { get; set; }
    }
}
