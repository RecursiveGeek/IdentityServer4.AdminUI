using IdentityServer4.AdminUI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;


namespace IdentityServer4.AdminUI.Controllers
{
    public class ApiScopesController : Controller
    {
        #region Fields
        private readonly IdentityServer4AdminUIContext _context;
        string name = "default";
        #endregion

        #region Constuctors
        public ApiScopesController(IdentityServer4AdminUIContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        /// <summary>
        /// This is the main index page 
        /// This runs a filter based upon the paramaters to only show the relevant objects
        /// </summary>
        /// <param name="searchString"> The search string inputs the ID of the ApiResources linked to this </param>
        /// <param name="name"> name will take the name of the ApiResource linked to this </param>
        /// <returns>index page</returns>
        /// <example>GET: ApiScopes</example>
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

            var scope = from m in _context.ApiScopes
                        select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                // converts the search string to an int to perform the search function. 
                var id = int.Parse(searchString);
                scope = scope.Where(s => s.ApiResourceId.Equals(id));
                HttpContext.Session.SetInt32(Helpers.VarHelper.ApiResourceId, id);
            }
            else
            {
                scope = scope.Where(s => s.ApiResourceId.Equals(GetSessionId()));
            }

            // returns an update with our clints that we searched. 
            return View(await scope.ToListAsync());
        }

        /// <summary>
        /// Displays the view for the Details page 
        /// </summary>
        /// <param name="id"> This is the table ID </param>
        /// <returns>View Details</returns>
        /// <example>GET: ApiScopes/Details/5</example>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // below sets up the session states for navigating to the scopes claims pages. 
            var ScopeId = id ?? default;
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

        /// <summary>
        /// Displays the Create page 
        /// </summary>
        /// <returns>View Create</returns>
        /// <example>GET: ApiScopes/Create</example>
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32(Helpers.VarHelper.ApiResourceId) == 0 || HttpContext.Session.GetInt32(Helpers.VarHelper.ApiResourceId) == null)
            {
                return View(Helpers.VarHelper.error404);
            }
            return View();
        }

        /// <summary>
        /// Displays the Create page 
        /// </summary>
        /// <returns>View Create</returns>
        /// <example>GET: ApiScopes/Create</example>
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

        /// <summary>
        /// displays the edit view 
        /// </summary>
        /// <param name="id">Item on table to edit</param>
        /// <returns>Edit view</returns>
        /// <example>ET: ApiScopes/Edit/5</example>
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

        /// <summary>
        /// Edit task, saves edits. 
        /// </summary>
        /// <param name="id">Location of item on table</param>
        /// <param name="apiScopes">updated object</param>
        /// <returns>index page after updating table</returns>
        /// <example> POST: Apiscopes/Edit/5  </example>
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
        /// <summary>
        /// Displays the Delete Page
        /// </summary>
        /// <param name="id">Location of item on table</param>
        /// <returns>Delete View</returns>
        /// <example>  GET: ApiScopes/Delete/5 </example>
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

        /// <summary>
        /// action to delete from table. 
        /// </summary>
        /// <param name="id"> Location of item on table </param>
        /// <returns>Index with item deleted</returns>
        /// <example>  POST: ApiScopes/Delete/5</example>
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
        /// validates if claim exists = true
        /// </summary>
        /// <param name="id">item on table</param>
        /// <returns>boolean</returns>
        private bool ApiScopesExists(int id)
        {
            return _context.ApiScopes.Any(e => e.Id == id);
        }
        /// <summary>
        /// retrieves the relevant id
        /// </summary>
        /// <returns>the id stored in the session state</returns>
        public int GetSessionId()
        {
            return HttpContext.Session.GetInt32(Helpers.VarHelper.ApiResourceId) ?? default; ;
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
