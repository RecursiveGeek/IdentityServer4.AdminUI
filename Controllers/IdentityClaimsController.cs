using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.AdminUI.Models;
using Microsoft.AspNetCore.Http;

namespace IdentityServer4.AdminUI.Controllers
{
    public class IdentityClaimsController : Controller
    {
        #region Fields
        private readonly IdentityServer4AdminUIContext _context;
        public string name = " default";
        #endregion

        #region Constructors
        public IdentityClaimsController(IdentityServer4AdminUIContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        // GET: IdentityClaims
        public async Task<IActionResult> Index(string searchString, string name)
        {
            if (string.IsNullOrEmpty(searchString) && string.IsNullOrEmpty(name) && string.IsNullOrEmpty(HttpContext.Session.GetString(Helpers.VarHelper.IdentityName)))
            {
                return View(Helpers.VarHelper.error404);
            }
            
            this.name = name;

            if (!string.IsNullOrEmpty(name))
            {
                HttpContext.Session.SetString(Helpers.VarHelper.IdentityName, this.name);
            }
            else if (string.IsNullOrEmpty(HttpContext.Session.GetString(Helpers.VarHelper.IdentityName)))
            {
                this.name = "null/empty";
                HttpContext.Session.SetString(Helpers.VarHelper.IdentityName, this.name);
            }

            var claim = from m in _context.IdentityClaims
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                int id = int.Parse(searchString);
                claim = claim.Where(s => s.IdentityResourceId.Equals(id));
                HttpContext.Session.SetInt32(Helpers.VarHelper.IdentityId, id);
            }
            else
            {
                claim = claim.Where(s => s.IdentityResourceId.Equals(GetSessionId()));
            }

            // returns an update with our clints that we searched. 
            return View(await claim.ToListAsync());
        }
        // GET: IdentityClaims/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var identityClaims = await _context.IdentityClaims
                .FirstOrDefaultAsync(m => m.Id == id);
            if (identityClaims == null)
            {
                return NotFound();
            }

            return View(identityClaims);
        }

        // GET: IdentityClaims/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32(Helpers.VarHelper.IdentityId) == 0 || HttpContext.Session.GetInt32(Helpers.VarHelper.IdentityId) == null)
            {
                return View(Helpers.VarHelper.error404);
            }
            return View();
        }

        // POST: IdentityClaims/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdentityResourceId,Type")] IdentityClaims identityClaims)
        {
            if (ModelState.IsValid)
            {
                _context.Add(identityClaims);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(identityClaims);
        }

        // GET: IdentityClaims/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var identityClaims = await _context.IdentityClaims.FindAsync(id);
            if (identityClaims == null)
            {
                return NotFound();
            }
            return View(identityClaims);
        }

        // POST: IdentityClaims/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdentityResourceId,Type")] IdentityClaims identityClaims)
        {
            if (id != identityClaims.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(identityClaims);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IdentityClaimsExists(identityClaims.Id))
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
            return View(identityClaims);
        }

        // GET: IdentityClaims/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var identityClaims = await _context.IdentityClaims
                .FirstOrDefaultAsync(m => m.Id == id);
            if (identityClaims == null)
            {
                return NotFound();
            }

            return View(identityClaims);
        }

        // POST: IdentityClaims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var identityClaims = await _context.IdentityClaims.FindAsync(id);
            _context.IdentityClaims.Remove(identityClaims);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// Checks that there is an object at location "#" on the table. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>boolean</returns>
        private bool IdentityClaimsExists(int id)
        {
            return _context.IdentityClaims.Any(e => e.Id == id);
        }
        /// <summary>
        /// returns the id stored in the session state, this is used for setting up the filter applied in the index
        /// </summary>
        /// <returns>int of the current id</returns>
        public int GetSessionId()
        {
            return HttpContext.Session.GetInt32(Helpers.VarHelper.IdentityId) ?? default;
        }
        #endregion
    }
}
