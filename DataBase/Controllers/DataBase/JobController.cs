using DataBase_Website.Data;
using DataBase_Website.Models.DataBaseModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DataBase_Website.Controllers.DataBase
{
    public class JobController : Controller
    {
        private readonly DataBase_WebsiteContext _context;
        private IHostingEnvironment Environment;

        public JobController(DataBase_WebsiteContext context, IHostingEnvironment _env)
        {
            _context = context;
            Environment = _env;
        }

        // GET: JobModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.JobModel.ToListAsync());
        }

        // GET: Job/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Job/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JobId,AssignedImages,AssignedAccounts")] JobModel accountModel)
        {
            if (ModelState.IsValid)
            {
                _context.JobModel.Add(accountModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(accountModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddFileAsync()
        {
            //var c = Request.Form.Files;

            foreach(IFormFile x in Request.Form.Files)
            {
                System.Diagnostics.Debug.WriteLine(x.FileName);

                string filename = ContentDispositionHeaderValue.Parse(x.ContentDisposition).FileName.Trim('"');

                filename = this.EnsureCorrectFilename(filename);

                using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filename)))
                    await x.CopyToAsync(output); 

                return PartialView("ItemPartial", x.FileName);
            }

            return PartialView("ItemPartial", "test");
        }

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }

        private string GetPathAndFilename(string filename)
        {
            return Environment.ContentRootPath + "\\Images\\" + filename;
             
        }

        [HttpGet]
        public IActionResult ReplaceItem()
        {
            return PartialView("ItemAccountsPartial",_context.AccountModel.ToListAsync().Result);
        }

        [HttpPost]
        public IActionResult DeleteImage(string FileName)
        {

            System.IO.File.Delete(GetPathAndFilename(FileName));

            return Ok();
        }
    }
}
