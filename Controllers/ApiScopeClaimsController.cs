using IdentityServer4.AdminUI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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
        /// <summary>
        /// This is the main index page 
        /// This runs a filter based upon the paramaters to only show the relevant objects
        /// </summary>
        /// <param name="searchString"> The search string inputs the ID of the ApiResources linked to this </param>
        /// <param name="name"> name will take the name of the ApiResource linked to this </param>
        /// <returns> index page</returns>
        /// <example>GET: ApiScopeclaims</example>
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

            var claim = from m in _context.ApiScopeClaims
                        select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                // converts the search string to an int to perform the search function. 
                var id = int.Parse(searchString);
                claim = claim.Where(s => s.ApiScopeId.Equals(id));
                HttpContext.Session.SetInt32(Helpers.VarHelper.ApiScopeId, id);
            }
            else
            {
                claim = claim.Where(s => s.ApiScopeId.Equals(GetSessionId()));
            }

            // returns an update with our clints that we searched. 
            return View(await claim.ToListAsync());
        }


        /// <summary>
        /// Displays the view for the Details page 
        /// </summary>
        /// <param name="id"> This is the table ID </param>
        /// <returns>View Details</returns>
        /// <example>GET: ApiScopeClaims/Details/5</example>
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
        /// Displays the Create page 
        /// </summary>
        /// <returns>View Create</returns>
        /// <example>GET: ApiScopeClaims/Create</example>
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32(Helpers.VarHelper.ApiResourceId) == 0 || HttpContext.Session.GetInt32(Helpers.VarHelper.ApiResourceId) == null)
            {
                return View(Helpers.VarHelper.error404);
            }
            return View();
        }

        /// <summary>
        /// create task - saves the create page form information to the table.
        /// </summary>
        /// <param name="apiScopeClaims">The object to be saved to the table</param>
        /// <returns>Saves form to table, then returns to index</returns>
        /// <example>POST: ApiScopeclaims/Create</example>
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

        /// <summary>
        /// displays the edit view.
        /// </summary>
        /// <param name="id">Item on table to edit</param>
        /// <returns>Edit view</returns>
        /// <example>ET: ApiScopeClaims/Edit/5</example>
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

        /// <summary>
        /// Edit task, saves edits. 
        /// </summary>
        /// <param name="id">Location of item on table</param>
        /// <param name="apiScopeClaims">updated object</param>
        /// <returns>index page after updating table</returns>
        /// <example> POST: ApiScopeclaims/Edit/5  </example>
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

        /// <summary>
        /// Displays the Delete Page
        /// </summary>
        /// <param name="id">Location of item on table</param>
        /// <returns>Delete View</returns>
        /// <example>  GET: ApiScopeclaims/Delete/5 </example>
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

        /// <summary>
        /// action to delete from table. 
        /// </summary>
        /// <param name="id"> Location of item on table </param>
        /// <returns>Index with item deleted</returns>
        /// <example>  POST: ApiScopeClaims/Delete/5</example>
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
        /// validates if claim exists = true
        /// </summary>
        /// <param name="id">item on table</param>
        /// <returns>boolean</returns>
        private bool ApiScopeClaimsExists(int id)
        {
            return _context.ApiScopeClaims.Any(e => e.Id == id);
        }
        /// <summary>
        /// retrieves the relevant id
        /// </summary>
        /// <returns>the id stored in the session state</returns>
        public int GetSessionId()
        {
            return HttpContext.Session.GetInt32(Helpers.VarHelper.ApiScopeId) ?? default;
        }
        #endregion

    }
}
