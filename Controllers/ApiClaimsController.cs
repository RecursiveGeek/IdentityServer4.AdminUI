using IdentityServer4.AdminUI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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

        /// <summary>
        /// This is the main index page for ApiClaims
        /// This runs a filter based upon the paramaters to only show the relevant objects
        /// </summary>
        /// <param name="searchString"> The search string inputs the ID of the ApiResources linked to this </param>
        /// <param name="name"> name will take the name of the ApiResource linked to this </param>
        /// <returns>api claims index page</returns>
        /// <example>GET: ApiClaims</example>
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
                // converts the search string to an int to perform the search function. 
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
        /// <summary>
        /// Displays the view for the Details page for ApiClaims
        /// </summary>
        /// <param name="id"> This is the ApiClaim table ID </param>
        /// <returns>View ApiClaims Details</returns>
        /// <example>GET: ApiClaims/Details/5</example>
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
        /// <summary>
        /// Displays the Create page for ApiClaims
        /// </summary>
        /// <returns>View ApiClaims Create</returns>
        /// <example>GET: ApiClaims/Create</example>
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
        /// <param name="apiClaims">The ApiClaims object to be saved to the table</param>
        /// <returns>Saves form to table, then returns to index</returns>
        /// <example>POST: ApiClaims/Create</example>
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

        /// <summary>
        /// displays the edit view for the api claim id passed in.
        /// </summary>
        /// <param name="id">Item on table to edit</param>
        /// <returns>Edit view</returns>
        /// <example>ET: ApiClaims/Edit/5</example>
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

        /// <summary>
        /// Edit task, saves edits. 
        /// </summary>
        /// <param name="id">Location of item on table</param>
        /// <param name="apiClaims">updated object</param>
        /// <returns>index page after updating table</returns>
        /// <example> POST: ApiClaims/Edit/5  </example>
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

        /// <summary>
        /// Displays the Delete Page
        /// </summary>
        /// <param name="id">Location of item on table</param>
        /// <returns>Delete View</returns>
        /// <example>  GET: ApiClaims/Delete/5 </example>
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

        /// <summary>
        /// action to delete from table. 
        /// </summary>
        /// <param name="id"> Location of item on table </param>
        /// <returns>Index with item deleted</returns>
        /// <example>  POST: ApiClaims/Delete/5</example>
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
        /// <param name="id">item on table</param>
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
