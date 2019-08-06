using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.AdminUI.Models;
using Microsoft.AspNetCore.Http;


namespace IdentityServer4.AdminUI.Controllers
{
    public class ApiScopesController : Controller
    {
        #region fields
        private readonly IdentityServer4AdminUIContext _context;
        string name = "default";
        #endregion

        #region constuctor
        public ApiScopesController(IdentityServer4AdminUIContext context)
        {
            _context = context;
        }
        #endregion
        #region methods
        // GET: ApiScopes
        /// <summary>
        /// paramaters apply the filter for the page, sets up the session states. 
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="name"></param>
        /// <returns>/apiscopes/index</returns>
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

            // this sets up our variable client to get the instances of our clients
            var client = from m in _context.ApiScopes
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                // converts teh search string to an int to perform the search function. 
                int id = int.Parse(searchString);
                client = client.Where(s => s.ApiResourceId.Equals(id));
                HttpContext.Session.SetInt32(Helpers.VarHelper.ApiResourceId, id);
            }
            else
            {
                client = client.Where(s => s.ApiResourceId.Equals(GetSessionId()));
            }

            // returns an update with our clints that we searched. 
            return View(await client.ToListAsync());
        }

        // GET: ApiScopes/Details/5
        /// <summary>
        /// details for the apiscopes at id "#"
        /// </summary>
        /// <param name="id"></param>
        /// <returns>apiScopes/details/"#"</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // below sets up the session states for navigating to the scopes claims pages. 
            var ScopeId = id ?? default(int);
            var ScopeName = FetchName(ScopeId);
            HttpContext.Session.SetString(Helpers.VarHelper.ApiScopeName, ScopeName);
            HttpContext.Session.SetInt32(Helpers.VarHelper.ApiScopeId, ScopeId);

            var apiScopes = await _context.ApiScopes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apiScopes == null)
            {
                return NotFound();
            }

            return View(apiScopes);
        }

        // GET: ApiScopes/Create
        /// <summary>
        /// create view
        /// </summary>
        /// <returns>apiScopes/create</returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApiScopes/Create
        /// <summary>
        /// create action, saves the create form.
        /// </summary>
        /// <param name="apiScopes"></param>
        /// <returns>api scopes index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ApiResourceId,Description,DisplayName,Emphasize,Name,Required,ShowInDiscoveryDocument")] ApiScopes apiScopes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(apiScopes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(apiScopes);
        }

        // GET: ApiScopes/Edit/5
        /// <summary>
        /// edit page.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>apiScopes/Edit/"#"</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apiScopes = await _context.ApiScopes.FindAsync(id);
            if (apiScopes == null)
            {
                return NotFound();
            }
            return View(apiScopes);
        }

        // POST: ApiScopes/Edit/5
        /// <summary>
        /// edit action, saves edit to table. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apiScopes"></param>
        /// <returns>apiScopes/index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ApiResourceId,Description,DisplayName,Emphasize,Name,Required,ShowInDiscoveryDocument")] ApiScopes apiScopes)
        {
            if (id != apiScopes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(apiScopes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApiScopesExists(apiScopes.Id))
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
            return View(apiScopes);
        }

        // GET: ApiScopes/Delete/5
        /// <summary>
        /// Delete view
        /// </summary>
        /// <param name="id"></param>
        /// <returns>apiScopes/Delete/"#"</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apiScopes = await _context.ApiScopes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apiScopes == null)
            {
                return NotFound();
            }

            return View(apiScopes);
        }

        // POST: ApiScopes/Delete/5
        /// <summary>
        /// delete action., deleted the item at id"#" 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>index for apiscopes</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var apiScopes = await _context.ApiScopes.FindAsync(id);
            _context.ApiScopes.Remove(apiScopes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// checks if the apiscope exists on the table. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>boolean</returns>
        private bool ApiScopesExists(int id)
        {
            return _context.ApiScopes.Any(e => e.Id == id);
        }
        /// <summary>
        /// retrieves the id from the session state
        /// </summary>
        /// <returns>session id int. </returns>
        public int GetSessionId()
        {
            int x = HttpContext.Session.GetInt32(Helpers.VarHelper.ApiResourceId) ?? default;
            return x;
        }
        /// <summary>
        /// retrieves the name from the session
        /// </summary>
        /// <param name="id"></param>
        /// <returns>string name of item @ id"#"</returns>
        public string FetchName(int id)
        {
            ApiScopes Name = _context.ApiScopes.Find(id);

            return Name.DisplayName;
        }
        #endregion
    }
}
