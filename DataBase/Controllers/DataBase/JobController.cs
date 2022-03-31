using DataBase_Website.Data;
using DataBase_Website.Models.DataBaseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataBase_Website.Controllers.DataBase
{
    public class JobController : Controller
    {
        private readonly DataBase_WebsiteContext _context;

        public JobController(DataBase_WebsiteContext context)
        {
            _context = context;
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
        public IActionResult AddFile()
        {
            //var c = Request.Form.Files;

            return PartialView("ItemPartial", "QWER");
        }

        [HttpGet]
        public IActionResult ReplaceItem()
        {
            return PartialView("ItemAccountsPartial",_context.AccountModel.ToListAsync().Result);
        }
    }
}
