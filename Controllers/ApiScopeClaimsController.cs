using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.AdminUI.Models;
using Microsoft.AspNetCore.Http;

namespace IdentityServer4.AdminUI.Controllers
{
    public class ApiScopeClaimsController : Controller
    {
        #region Fields
        private readonly IdentityServer4AdminUIContext _context;
        string name = "default";
        #endregion

        #region Constructors
        public ApiScopeClaimsController(IdentityServer4AdminUIContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        // GET: ApiScopeClaims
        /// <summary>
        /// paramaters are used to filter the relevant scope claims
        /// These are used with the session states for displaying names carried between the pages
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="name"></param>
        /// <returns> ScopeClaims/Index </returns>
        public async Task<IActionResult> Index(string searchString, string name)
        {
            if (string.IsNullOrEmpty(searchString) && string.IsNullOrEmpty(name) && string.IsNullOrEmpty(HttpContext.Session.GetString(Helpers.VarHelper.ApiScopeName)))
            {
                return View(Helpers.VarHelper.error404);
            }

            this.name = name;

            if (!string.IsNullOrEmpty(name))
            {
                HttpContext.Session.SetString(Helpers.VarHelper.ApiScopeName, this.name);
            }
            else if (string.IsNullOrEmpty(HttpContext.Session.GetString(Helpers.VarHelper.ApiScopeName)))
            {
                this.name = "null/empty";
                HttpContext.Session.SetString(Helpers.VarHelper.ApiScopeName, this.name);
            }

            // this sets up our variable client to get the instances of our clients
            var client = from m in _context.ApiScopeClaims
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                // converts teh search string to an int to perform the search function. 
                int id = int.Parse(searchString);
                client = client.Where(s => s.ApiScopeId.Equals(id));
                HttpContext.Session.SetInt32(Helpers.VarHelper.ApiScopeId, id);
            }
            else
            {
                client = client.Where(s => s.ApiScopeId.Equals(GetSessionId()));
            }

            // returns an update with our clints that we searched. 
            return View(await client.ToListAsync());
        }


        // GET: ApiScopeClaims/Details/5
        /// <summary>
        /// returns the details associated with the passed scopes claim id. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>/apiscopeclaims/details/"#"</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apiScopeClaims = await _context.ApiScopeClaims
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apiScopeClaims == null)
            {
                return NotFound();
            }

            return View(apiScopeClaims);
        }

        /// <summary>
        /// the create view
        /// </summary>
        /// <returns>/apiscopeclaims/create</returns>
        // GET: ApiScopeClaims/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32(Helpers.VarHelper.ApiResourceId) == 0 || HttpContext.Session.GetInt32(Helpers.VarHelper.ApiResourceId) == null)
            {
                return View(Helpers.VarHelper.error404);
            }
            return View();
        }

        // POST: ApiScopeClaims/Create
        /// <summary>
        /// this is the action to create, saves it to the table.
        /// </summary>
        /// <param name="apiScopeClaims"></param>
        /// <returns>returns to the apiscopeclaims index view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ApiScopeId,Type")] ApiScopeClaims apiScopeClaims)
        {
            if (ModelState.IsValid)
            {
                _context.Add(apiScopeClaims);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(apiScopeClaims);
        }

        // GET: ApiScopeClaims/Edit/5
        /// <summary>
        /// edit view
        /// </summary>
        /// <param name="id"></param>
        /// <returns>/apiscopeclaims/edit/"#"</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apiScopeClaims = await _context.ApiScopeClaims.FindAsync(id);
            if (apiScopeClaims == null)
            {
                return NotFound();
            }
            return View(apiScopeClaims);
        }

        // POST: ApiScopeClaims/Edit/5
        /// <summary>
        /// edit action, saves edits from form to table. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apiScopeClaims"></param>
        /// <returns>apiScopeClaim index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ApiScopeId,Type")] ApiScopeClaims apiScopeClaims)
        {
            if (id != apiScopeClaims.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(apiScopeClaims);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApiScopeClaimsExists(apiScopeClaims.Id))
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
            return View(apiScopeClaims);
        }

        // GET: ApiScopeClaims/Delete/5
        /// <summary>
        /// opens delete view
        /// </summary>
        /// <param name="id"></param>
        /// <returns>delete view for id "#"</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apiScopeClaims = await _context.ApiScopeClaims
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apiScopeClaims == null)
            {
                return NotFound();
            }

            return View(apiScopeClaims);
        }

        // POST: ApiScopeClaims/Delete/5
        /// <summary>
        /// delete action
        /// </summary>
        /// <param name="id"></param>
        /// <returns>api scope claim index</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var apiScopeClaims = await _context.ApiScopeClaims.FindAsync(id);
            _context.ApiScopeClaims.Remove(apiScopeClaims);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// checks if the scope claim at id "#" exists on the table. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true/false</returns>
        private bool ApiScopeClaimsExists(int id)
        {
            return _context.ApiScopeClaims.Any(e => e.Id == id);
        }
        /// <summary>
        /// returns the session id 
        /// </summary>
        /// <returns>the id from the session state. </returns>
        public int GetSessionId()
        {
            return HttpContext.Session.GetInt32(Helpers.VarHelper.ApiScopeId) ?? default;
        }
        #endregion

    }
}
