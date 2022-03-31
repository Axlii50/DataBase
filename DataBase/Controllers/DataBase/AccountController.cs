using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataBase_Website.Data;
using DataBase_Website.Models.DataBaseModels;

namespace DataBase_Website.Controllers.DataBase
{
    public class AccountController : Controller
    {
        private readonly DataBase_WebsiteContext _context;

        public AccountController(DataBase_WebsiteContext context)
        {
            _context = context;
        }

        // GET: AccountModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.AccountModel.ToListAsync());
        }

        // GET: AccountModels/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountModel = await _context.AccountModel
                .FirstOrDefaultAsync(m => m.PrivateAccountKey == id);
            if (accountModel == null)
            {
                return NotFound();
            }

            return View(accountModel);
        }

        // GET: AccountModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AccountModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Login,Password,AccountName,Permission,PrivateAccountKey")] AccountModel accountModel)
        {
            if (ModelState.IsValid)
            {
                _context.AccountModel.Add(accountModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(accountModel);
        }

        // GET: AccountModels/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountModel = await _context.AccountModel.FindAsync(id);
            if (accountModel == null)
            {
                return NotFound();
            }


            System.Diagnostics.Debug.WriteLine(accountModel.Password);
            return View(accountModel);
        }

        // POST: AccountModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Login,Password,AccountName,Permission,PrivateAccountKey")] AccountModel accountModel)
        {
            if (id != accountModel.PrivateAccountKey)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accountModel);
                    await _context.SaveChangesAsync();   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountModelExists(accountModel.PrivateAccountKey))
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
            return View(accountModel);
        }

        // GET: AccountModels/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountModel = await _context.AccountModel
                .FirstOrDefaultAsync(m => m.PrivateAccountKey == id);
            if (accountModel == null)
            {
                return NotFound();
            }

            return View(accountModel);
        }

        // POST: AccountModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var accountModel = await _context.AccountModel.FindAsync(id);
            _context.AccountModel.Remove(accountModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountModelExists(string id)
        {
            return _context.AccountModel.Any(e => e.PrivateAccountKey == id);
        }
    }
}
