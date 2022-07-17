using DataBase_Website.Data;
using DataBase_Website.Models.DataBaseModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DataBase_Website.Controllers.DataBase
{
    public class JobController : Controller
    {
        private readonly DataBase_WebsiteContext _context;
        private IWebHostEnvironment Environment;

        public JobController(DataBase_WebsiteContext context, IWebHostEnvironment _env)
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
        public async Task<IActionResult> Create([Bind("JobId,AssignedImages,AssignedAccounts")] JobModel JobModel)
        {
            if (ModelState.IsValid)
            {
                //prepare all data to be uploaded to DATABASE and make sure is valid
                JobModel.AssignedAccounts = JobModel.AssignedAccounts.Remove(0, 1);
                if (JobModel.AssignedAccounts[JobModel.AssignedAccounts.Length-1] == ':')
                    JobModel.AssignedAccounts = JobModel.AssignedAccounts.Remove(JobModel.AssignedAccounts.Length - 1, 1);
                JobModel.AssignedImages = JobModel.AssignedImages.Remove(0, 1);
                JobModel.AssignedImages = JobModel.AssignedImages.Remove(JobModel.AssignedImages.Length - 1, 1);
                //i dont know if this gonna prevent all bugs with File assiging but for now small bug fix
                //there is needed to do more testing and see if this really fix this problem 
                JobModel.AssignedImages = JobModel.AssignedImages.Replace("::", ":");
                _context.JobModel.Add(JobModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(JobModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddFileAsync()
        {
            //TODO change name of file to make sure that there will be no overwrites of files 
            List<string> Files = new List<string>();
            foreach(IFormFile x in Request.Form.Files)
            {
                //get name of file to upload from IFORMFILE 
                string filename = ContentDispositionHeaderValue.Parse(x.ContentDisposition).FileName.Trim('"');
                
                //make sure file is in correct format
                filename = this.EnsureCorrectFilename(filename);

                //copy file to specyfic location on server
                using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filename)))
                    await x.CopyToAsync(output);

                //add file to list of files
                Files.Add(x.FileName);
            }
            return PartialView("ItemPartial", Files.ToArray());
        }

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }

        /// <summary>
        /// return path to the file of name filename
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private string GetPathAndFilename(string filename)
        {
            //return path to Images folder and add filename to it 
            return Environment.ContentRootPath + "\\Images\\" + filename;
        }

        /// <summary>
        /// get html result with all accounts from database for replacing 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ReplaceItem()
        {
            return PartialView("ItemAccountsPartial",_context.AccountModel.ToListAsync().Result);
        }

        [HttpPost]
        public IActionResult DeleteImage(string FileName)
        {
            //deleting file with specific path
            System.IO.File.Delete(GetPathAndFilename(FileName));

            return Ok();
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobModel = await _context.JobModel
                .FirstOrDefaultAsync(m => m.JobId == id);
            if (jobModel == null)
            {
                return NotFound();
            }

            return View(jobModel);
        }

        /// <summary>
        /// return all accounts from given string that is from job model 
        /// </summary>
        /// <param name="accounts"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllAccounts([Bind("accounts")]string accounts)
        {
            return PartialView("AccountDetailsPartialView", accounts.Split(':'));
        }

        /// <summary>
        /// return all images from given string that is from job model 
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllImages([Bind("images")] string images)
        {
            var imag = images.Split(':');
            return PartialView("ImagePartialView", imag );
        }

        /// <summary>
        /// send file of given name 
        /// </summary>
        /// <param name="Imagename"></param>
        /// <returns></returns>
        public IActionResult GetImage(string Imagename)
        {
            byte[] imageByteData = System.IO.File.ReadAllBytes(GetPathAndFilename(Imagename));
            return File(imageByteData, "image/png");
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobModel = await _context.JobModel.FindAsync(id);
            if (jobModel == null)
            {
                return NotFound();
            }
            return View(jobModel);
        }

        // POST: AccountModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("JobId,AssignedAccounts,AssignedImages")] JobModel jobModel)
        {
            if (id != jobModel.JobId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobModelExists(jobModel.JobId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(jobModel);
        }

        private bool JobModelExists(string id)
        {
            return _context.JobModel.Any(e => e.JobId == id);
        }

        // GET: AccountModels/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobModel = await _context.JobModel
                .FirstOrDefaultAsync(m => m.JobId == id);

            if (jobModel == null)
            {
                return NotFound();
            }

            return View(jobModel);
        }

        // POST: AccountModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var jobModel = await _context.JobModel.FindAsync(id);
            _context.JobModel.Remove(jobModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
