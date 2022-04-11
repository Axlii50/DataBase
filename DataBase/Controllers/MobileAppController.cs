using DataBase_Website.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DataBase.Controllers
{
    public class MobileAppController : Controller
    {
        private readonly DataBase_WebsiteContext _context;

        public MobileAppController(DataBase_WebsiteContext context)
        {
            _context = context;
        }

        /// <summary>
        /// method with purpose to authenticate correct password and login 
        /// when both are correct return status = 1 and new guid with 1 hour lifespan 
        /// when incorrect return status = 2 
        /// </summary>
        /// <param name="requestdata"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        public JsonResult Login([Bind("Login,Password")] LoginModel requestdata)
        {
            //Decrypt sended data 
            DataBase.Cryptography.DecryptLoginModel(ref requestdata);

            string guid = Guid.NewGuid().ToString();
            DataBase_Website.Models.DataBaseModels.AccountModel accountModel = _context.AccountModel
                .FirstOrDefault(acc => acc.Login == requestdata.Login && acc.Password == requestdata.Password);
            if (accountModel != null)
            {
                Startup.AuthorizedGuids.Add(new Models.GuidEntity { Guid = guid, Created = DateTime.Now.AddHours(1) });

                foreach (Models.GuidEntity c in Startup.AuthorizedGuids)
                    System.Diagnostics.Debug.WriteLine(c.Guid);
                return Json(new Models.JsonResult
                {
                    guid = guid,
                    Status = 1,
                    Permission =  accountModel.Permission
                });
            }
            return Json(new Models.JsonResult { guid = "null", Status = 2, Permission = Models.Permission.NULL });
        }


        /// <summary>
        /// allows you to download specific image from website without making it public 
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Download([Bind("GUID,fileName")] string GUID, string fileName)
        {
            UpdateGuids();

            if (Startup.AuthorizedGuids.Find(x => x.Guid == GUID) == null) return Unauthorized();

            if (!string.IsNullOrEmpty(fileName))
            {
                string filePath = "/Images/";
                string fullPath = AppDomain.CurrentDomain.BaseDirectory + filePath + "/" + fileName;
                if (System.IO.File.Exists(fullPath))
                {
                    var Image = System.IO.File.OpenRead(fullPath);
                    return File(Image,"image/jpeg");
                }
            }

            return NotFound();
        }

        /// <summary>
        /// remove all expired GUID from list
        /// </summary>
        public void UpdateGuids()
        {
            List<Models.GuidEntity> expired = new List<Models.GuidEntity>();
            foreach (Models.GuidEntity x in Startup.AuthorizedGuids)
                if (IsExpired(x.Created))
                    expired.Add(x);
            foreach (Models.GuidEntity x in expired)
                Startup.AuthorizedGuids.Remove(x);
        }

        /// <summary>
        /// return bool value that represent if given date is expired or not 
        /// </summary>
        /// <param name="Expire"></param>
        /// <returns></returns>
        public bool IsExpired(DateTime Expire)
        {
            return Expire - DateTime.Now > TimeSpan.Zero ? false : true;
        }
    }
}
