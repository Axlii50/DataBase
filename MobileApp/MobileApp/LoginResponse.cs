using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp
{
    struct LoginResponse
    {
        public int Status { get; set; } // 1 ok 2 bad
        public string guid { get; set; }
    }
}
