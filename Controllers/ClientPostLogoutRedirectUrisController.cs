using IdentityServer4.AdminUI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.AdminUI.Controllers
{
    public class ClientPostLogoutRedirectUrisController : Controller
    {
        #region Fields
        private readonly IdentityServer4AdminUIContext _context;
        public string name = " default";
        #endregion

        #region Constructors
        public ClientPostLogoutRedirectUrisController(IdentityServer4AdminUIContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods 
        /// <summary>
        /// This is the main index page 
        /// This runs a filter based upon the paramaters to only show the relevant objects
        /// </summary>
        /// <param name="searchString"> The search string inputs the ID of the client linked to this </param>
        /// <param name="name"> name will take the name of the Client linked to this </param>
        /// <returns>index page</returns>
        /// <example>GET: ClientPostLogoutRedirectUris</example>
        public async Task<IActionResult> Index(string searchString, string name)
        {
            if (string.IsNullOrEmpty(searchString) && string.IsNullOrEmpty(name) && string.IsNullOrEmpty(HttpContext.Session.GetString(Helpers.VarHelper.ClientName)))
            {
                return View(Helpers.VarHelper.error404);
            }

            this.name = name;

            if (!string.IsNullOrEmpty(name))
            {
                HttpContext.Session.SetString(Helpers.VarHelper.ClientName, this.name);
            }
            else if (string.IsNullOrEmpty(HttpContext.Session.GetString(Helpers.VarHelper.ClientName)))
            {
                this.name = "null/empty";
                HttpContext.Session.SetString(Helpers.VarHelper.ClientName, this.name);
            }

            // this sets up our variable client to get the instances of our clients
            var client = from m in _context.ClientPostLogoutRedirectUris
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                var id = int.Parse(searchString);
                client = client.Where(s => s.ClientId.Equals(id));
                HttpContext.Session.SetInt32(Helpers.VarHelper.ClientId, id);
            }
            else
            {
                client = client.Where(s => s.ClientId.Equals(GetSessionId()));
            }

            // returns an update with our clints that we searched. 
            return View(await client.ToListAsync());
        }


        /// <summary>
        /// Displays the view for the Details page 
        /// </summary>
        /// <param name="id"> This is the table ID </param>
        /// <returns>View Details</returns>
        /// <example>GET: ClientPostLogoutRedirectUri/Details/5</example>     
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientPostLogoutRedirectUris = await _context.ClientPostLogoutRedirectUris
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clientPostLogoutRedirectUris == null)
            {
                return NotFound();
            }

            return View(clientPostLogoutRedirectUris);
        }
        /// <summary>
        /// Displays the Create page 
        /// </summary>
        /// <returns>View Create</returns>
        /// <example>GET: ClientPostLogoutRedirectUri/Create</example>
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32(Helpers.VarHelper.ClientId) == 0 || HttpContext.Session.GetInt32(Helpers.VarHelper.ClientId) == null)
            {
                return View(Helpers.VarHelper.error404);
            }
            return View();
        }
        /// <summary>
        /// create task - saves the create page form information to the table.
        /// </summary>
        /// <param name="clientPostLogoutRedirectUris">The object to be saved to the table</param>
        /// <returns>Saves form to table, then returns to index</returns>
        /// <example>POST: ClientPostLogoutRedirectUri/Create</example>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientId,PostLogoutRedirectUri")] ClientPostLogoutRedirectUris clientPostLogoutRedirectUris)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clientPostLogoutRedirectUris);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clientPostLogoutRedirectUris);
        }
        /// <summary>
        /// displays the edit view 
        /// </summary>
        /// <param name="id">Item on table to edit</param>
        /// <returns>Edit view</returns>
        /// <example>ET: ClientPostLogoutRedirectUris/Edit/5</example>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientPostLogoutRedirectUris = await _context.ClientPostLogoutRedirectUris.FindAsync(id);
            if (clientPostLogoutRedirectUris == null)
            {
                return NotFound();
            }
            return View(clientPostLogoutRedirectUris);
        }
        /// <summary>
        /// Edit task, saves edits. 
        /// </summary>
        /// <param name="id">Location of item on table</param>
        /// <param name="clientPostLogoutRedirectUris">updated object</param>
        /// <returns>index page after updating table</returns>
        /// <example> POST: ClientPostLogoutRedirectUris/Edit/5  </example>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClientId,PostLogoutRedirectUri")] ClientPostLogoutRedirectUris clientPostLogoutRedirectUris)
        {
            if (id != clientPostLogoutRedirectUris.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clientPostLogoutRedirectUris);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientPostLogoutRedirectUrisExists(clientPostLogoutRedirectUris.Id))
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
            return View(clientPostLogoutRedirectUris);
        }
        /// <summary>
        /// Displays the Delete Page
        /// </summary>
        /// <param name="id">Location of item on table</param>
        /// <returns>Delete View</returns>
        /// <example>  GET: ClientPostLogoutRedirectUri/Delete/5 </example>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientPostLogoutRedirectUris = await _context.ClientPostLogoutRedirectUris
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clientPostLogoutRedirectUris == null)
            {
                return NotFound();
            }

            return View(clientPostLogoutRedirectUris);
        }
        /// <summary>
        /// action to delete from table. 
        /// </summary>
        /// <param name="id"> Location of item on table </param>
        /// <returns>Index with item deleted</returns>
        /// <example>  POST: ClientPostLogoutRedirectUris/Delete/5</example>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clientPostLogoutRedirectUris = await _context.ClientPostLogoutRedirectUris.FindAsync(id);
            _context.ClientPostLogoutRedirectUris.Remove(clientPostLogoutRedirectUris);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// validates if claim exists = true
        /// </summary>
        /// <param name="id">item on table</param>
        /// <returns>boolean</returns>
        private bool ClientPostLogoutRedirectUrisExists(int id)
        {
            return _context.ClientPostLogoutRedirectUris.Any(e => e.Id == id);
        }

        /// <summary>
        /// returns the id stored in the session state, this is used for setting up the filter applied in the index
        /// </summary>
        /// <returns>int of the current clients table id</returns>
        public int GetSessionId()
        {
            return HttpContext.Session.GetInt32(Helpers.VarHelper.ClientId) ?? default;
        }
        #endregion
    }
}
