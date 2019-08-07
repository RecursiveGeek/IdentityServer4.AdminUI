using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.AdminUI.Models;
using Microsoft.AspNetCore.Http;

namespace IdentityServer4.AdminUI.Controllers
{
    public class ApiClaimsController : Controller
    {
        #region Fields
        private readonly IdentityServer4AdminUIContext _context;
        string name = "default";
        #endregion

        #region Constructors
        public ApiClaimsController(IdentityServer4AdminUIContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        // GET: ApiClaims
        /// <summary>
        /// the search string is set for the id, the name is the name.
        /// It will check that atleast one of those two fields, or the session state, has something. 
        /// Then it will filter out and only display the claims associated with the api information passed. 
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="name"></param>
        /// <returns>api claims index page</returns>
        public async Task<IActionResult> Index(string searchString, string name)
        {
            if (string.IsNullOrEmpty(searchString) && string.IsNullOrEmpty(name) && string.IsNullOrEmpty(HttpContext.Session.GetString(Helpers.VarHelper.ApiResourceName)))
            {
                return View(Helpers.VarHelper.error404);
            }

            this.name = name;

            if (!string.IsNullOrEmpty(name))
            {
                HttpContext.Session.SetString(Helpers.VarHelper.ApiResourceName, this.name);
            }
            else if (string.IsNullOrEmpty(HttpContext.Session.GetString(Helpers.VarHelper.ApiResourceName)))
            {
                this.name = "null/empty";
                HttpContext.Session.SetString(Helpers.VarHelper.ApiResourceName, this.name);
            }

            var claim = from m in _context.ApiClaims
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                // converts teh search string to an int to perform the search function. 
                var id = int.Parse(searchString);
                claim = claim.Where(s => s.ApiResourceId.Equals(id));
                HttpContext.Session.SetInt32(Helpers.VarHelper.ApiResourceId, id);
            }
            else
            {
                claim = claim.Where(s => s.ApiResourceId.Equals(GetSessionId()));
            }

            // returns an update with our clints that we searched. 
            return View(await claim.ToListAsync());
        }
        // GET: ApiClaims/Details/5
        /// <summary>
        /// takes in the id and displays the details view for that claim
        /// </summary>
        /// <param name="id"></param>
        /// <returns>details page for the api claim</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apiClaims = await _context.ApiClaims
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apiClaims == null)
            {
                return NotFound();
            }

            return View(apiClaims);
        }

        // GET: ApiClaims/Create
        /// <summary>
        /// create page
        /// </summary>
        /// <returns>/apiclaims/create</returns>
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32(Helpers.VarHelper.ApiResourceId) == 0 || HttpContext.Session.GetInt32(Helpers.VarHelper.ApiResourceId) == null)
            {
                return View(Helpers.VarHelper.error404);
            }
            return View();
        }

        // POST: ApiClaims/Create
        /// <summary>
        /// create task - saves the create page form information to the table.
        /// </summary>
        /// <param name="apiClaims"></param>
        /// <returns>apiclaims index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ApiResourceId,Type")] ApiClaims apiClaims)
        {
            if (ModelState.IsValid)
            {
                _context.Add(apiClaims);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(apiClaims);
        }

        // GET: ApiClaims/Edit/5
        /// <summary>
        /// displays the edit view for the api claim id passed in.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>/apiclaims/edit/#</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apiClaims = await _context.ApiClaims.FindAsync(id);
            if (apiClaims == null)
            {
                return NotFound();
            }
            return View(apiClaims);
        }

        // POST: ApiClaims/Edit/5
        /// <summary>
        /// Edit task, saves edits. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apiClaims"></param>
        /// <returns>/apiclaims/index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ApiResourceId,Type")] ApiClaims apiClaims)
        {
            if (id != apiClaims.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(apiClaims);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApiClaimsExists(apiClaims.Id))
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
            return View(apiClaims);
        }

        // GET: ApiClaims/Delete/5
        /// <summary>
        /// Delete page
        /// </summary>
        /// <param name="id"></param>
        /// <returns>/apiclaims/delete/"#"</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apiClaims = await _context.ApiClaims
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apiClaims == null)
            {
                return NotFound();
            }

            return View(apiClaims);
        }

        // POST: ApiClaims/Delete/5
        /// <summary>
        /// action to delete from table. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>/apiclaims/index</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var apiClaims = await _context.ApiClaims.FindAsync(id);
            _context.ApiClaims.Remove(apiClaims);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// validates if claim exists = true
        /// </summary>
        /// <param name="id"></param>
        /// <returns>boolean</returns>
        private bool ApiClaimsExists(int id)
        {
            return _context.ApiClaims.Any(e => e.Id == id);
        }
        /// <summary>
        /// retrieves the relevant apiresource id
        /// </summary>
        /// <returns>the id stored in the session state</returns>
        public int GetSessionId()
        {
            return HttpContext.Session.GetInt32(Helpers.VarHelper.ApiResourceId) ?? default;
        }
        #endregion
    }
}
