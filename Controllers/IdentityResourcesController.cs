﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.AdminUI.Models;
using Microsoft.AspNetCore.Http;

namespace IdentityServer4.AdminUI.Controllers
{
    public class IdentityResourcesController : Controller
    {
        #region fields
        private readonly IdentityServer4AdminUIContext _context;
        const string SessionKey = "FirstSeen";
        public int Sessionid;
        #endregion
        #region constructor
        public IdentityResourcesController(IdentityServer4AdminUIContext context)
        {
            _context = context;
        }
        #endregion
        #region methods
        // GET: IdentityResources
        public async Task<IActionResult> Index(string searchString)
        {
            // sets the variable value to the current session key.
            var value = HttpContext.Session.GetString(SessionKey);
            // if there is not value currently set, it sets to the id passed to the index as a string.
            if (string.IsNullOrEmpty(value))
            {
                if (string.IsNullOrEmpty(searchString))
                {
                    searchString = " ";
                }
                HttpContext.Session.SetString(SessionKey, searchString);
            }
            // this sets up our variable client to get the instances of our clients
            var client = from m in _context.IdentityResources
                         select m;
            // 
            if (!string.IsNullOrEmpty(searchString))
            {
                client = client.Where(s => s.DisplayName.Contains(searchString));
            }

            // returns an update with our clints that we searched. 
            return View(await client.ToListAsync());

        }

        // GET: IdentityResources/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            Sessionid = id ?? default(int);
            string retrievedName = FetchName(Sessionid);
            RecordNameInSession(retrievedName);
            RecordIdInSession(Sessionid);
            HttpContext.Session.SetString(SessionKey, retrievedName);


            var identityResources = await _context.IdentityResources
                .FirstOrDefaultAsync(m => m.Id == id);
            if (identityResources == null)
            {
                return NotFound();
            }

            return View(identityResources);
        }

        // GET: IdentityResources/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: IdentityResources/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,DisplayName,Emphasize,Enabled,Name,Required,ShowInDiscoveryDocument")] IdentityResources identityResources)
        {
            if (ModelState.IsValid)
            {
                _context.Add(identityResources);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(identityResources);
        }

        // GET: IdentityResources/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Sessionid = id ?? default(int);
            string retrievedName = FetchName(Sessionid);

            RecordNameInSession(retrievedName);
            RecordIdInSession(Sessionid);
            var identityResources = await _context.IdentityResources.FindAsync(id);
            if (identityResources == null)
            {
                return NotFound();
            }
            return View(identityResources);
        }

        // POST: IdentityResources/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,DisplayName,Emphasize,Enabled,Name,Required,ShowInDiscoveryDocument")] IdentityResources identityResources)
        {
            if (id != identityResources.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(identityResources);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IdentityResourcesExists(identityResources.Id))
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
            return View(identityResources);
        }

        // GET: IdentityResources/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Sessionid = id ?? default(int);
            string retrievedName = FetchName(Sessionid);

            RecordNameInSession(retrievedName);
            RecordIdInSession(Sessionid);
            if (id == null)
            {
                return NotFound();
            }

            var identityResources = await _context.IdentityResources
                .FirstOrDefaultAsync(m => m.Id == id);
            if (identityResources == null)
            {
                return NotFound();
            }

            return View(identityResources);
        }

        // POST: IdentityResources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var identityResources = await _context.IdentityResources.FindAsync(id);
            _context.IdentityResources.Remove(identityResources);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// Checks that there is an object at location "#" on the table. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>boolean</returns>
        private bool IdentityResourcesExists(int id)
        {
            return _context.IdentityResources.Any(e => e.Id == id);
        }
        /// <summary>
        /// this retrieves the displayname of the identity resource at id "#"
        /// </summary>
        /// <param name="id"></param>
        /// <returns>"#".displayname</returns>
        public string FetchName(int id)
        {
            IdentityResources Name = GetClients(id);
            return Name.DisplayName;
        }
        /// <summary>
        /// this returns the identity resource that is at location "#" on the table. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>identity resource # "#"</returns>
        public IdentityResources GetClients(int id)
        {
            return _context.IdentityResources.Find(id);
        }
        // attempt: set the client name
        /// <summary>
        /// This sets the display name into the identity session state. 
        /// </summary>
        /// <param name="action"></param>
        private void RecordNameInSession(string action)
        {
            HttpContext.Session.SetString(Helpers.VarHelper.IdentityName, action);
        }
        /// <summary>
        /// this sets the id into the session state for identity resources. 
        /// </summary>
        /// <param name="action"></param>
        // this records the last used id
        private void RecordIdInSession(int action)
        {
            HttpContext.Session.SetInt32(Helpers.VarHelper.IdentityId, action);
        }
        #endregion

    }
}