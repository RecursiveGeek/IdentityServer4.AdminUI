using IdentityServer4.AdminUI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.AdminUI.Controllers
{
    public class ClientCorsOriginsController : Controller
    {
        #region Fields
        private readonly IdentityServer4AdminUIContext _context;
        public string name = " default";
        #endregion

        #region Constructors
        public ClientCorsOriginsController(IdentityServer4AdminUIContext context)
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
        /// <param name="name"> name will take the name of the Client linked to this </param>
        /// <returns>index page</returns>
        /// <example>GET: ClientCorsOrigins</example>
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
            var client = from m in _context.ClientCorsOrigins
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                // converts the search string to an int to perform the search function. 
                int id = int.Parse(searchString);
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
        /// <example>GET: ClientCorsOrigins/Details/5</example>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ClientCorsOrigins = await _context.ClientCorsOrigins
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ClientCorsOrigins == null)
            {
                return NotFound();
            }

            return View(ClientCorsOrigins);
        }
        /// <summary>
        /// Displays the Create page 
        /// </summary>
        /// <returns>View Create</returns>
        /// <example>GET: ClientCorsOrigins/Create</example>
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
        /// <param name="ClientCorsOrigins">The object to be saved to the table</param>
        /// <returns>Saves form to table, then returns to index</returns>
        /// <example>POST: ClientCorsOrigins/Create</example>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientId,Origin")] ClientCorsOrigins ClientCorsOrigins)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ClientCorsOrigins);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ClientCorsOrigins);
        }
        /// <summary>
        /// displays the edit view 
        /// </summary>
        /// <param name="id">Item on table to edit</param>
        /// <returns>Edit view</returns>
        /// <example>ET: ClientcorsOrigins/Edit/5</example>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ClientCorsOrigins = await _context.ClientCorsOrigins.FindAsync(id);
            if (ClientCorsOrigins == null)
            {
                return NotFound();
            }
            return View(ClientCorsOrigins);
        }
        /// <summary>
        /// Edit task, saves edits. 
        /// </summary>
        /// <param name="id">Location of item on table</param>
        /// <param name="ClientCorsOrigins">updated object</param>
        /// <returns>index page after updating table</returns>
        /// <example> POST: ClientcorsOrigins/Edit/5  </example>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClientId,Origin")] ClientCorsOrigins ClientCorsOrigins)
        {
            if (id != ClientCorsOrigins.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ClientCorsOrigins);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientCorsOriginsExists(ClientCorsOrigins.Id))
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
            return View(ClientCorsOrigins);
        }
        /// <summary>
        /// Displays the Delete Page
        /// </summary>
        /// <param name="id">Location of item on table</param>
        /// <returns>Delete View</returns>
        /// <example>  GET: clientcorsorigins/Delete/5 </example>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ClientCorsOrigins = await _context.ClientCorsOrigins
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ClientCorsOrigins == null)
            {
                return NotFound();
            }

            return View(ClientCorsOrigins);
        }
        /// <summary>
        /// action to delete from table. 
        /// </summary>
        /// <param name="id"> Location of item on table </param>
        /// <returns>Index with item deleted</returns>
        /// <example>  POST: ClientCorsOrigins/Delete/5</example>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ClientCorsOrigins = await _context.ClientCorsOrigins.FindAsync(id);
            _context.ClientCorsOrigins.Remove(ClientCorsOrigins);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// validates if claim exists = true
        /// </summary>
        /// <param name="id">item on table</param>
        /// <returns>boolean</returns>
        private bool ClientCorsOriginsExists(int id)
        {
            return _context.ClientCorsOrigins.Any(e => e.Id == id);
        }
        /// <summary>
        /// retrieves the relevant id
        /// </summary>
        /// <returns>the id stored in the session state</returns>
        public int GetSessionId()
        {
            return HttpContext.Session.GetInt32(Helpers.VarHelper.ClientId) ?? default;
        }
        #endregion
    }
}
