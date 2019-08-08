using IdentityModel;
using IdentityServer4.AdminUI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.AdminUI.Controllers
{
    public class ClientSecretsController : Controller
    {
        #region Fields
        public string name = " default";
        private readonly IdentityServer4AdminUIContext _context;
        #endregion

        #region Constructors
        public ClientSecretsController(IdentityServer4AdminUIContext context)
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
        /// <example>GET: ClientSecrets</example>
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
            var client = from m in _context.ClientSecrets
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                // converts the search string to an int to perform the search function. 
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
        /// <example>GET: ClientSecrets/Details/5</example>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var clientSecrets = await _context.ClientSecrets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clientSecrets == null)
            {
                return NotFound();
            }

            return View(clientSecrets);
        }
        /// <summary>
        /// Displays the Create page 
        /// </summary>
        /// <returns>View Create</returns>
        /// <example>GET: ClientSecrets/Create</example>
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
        /// <param name="clientSecrets">The object to be saved to the table</param>
        /// <returns>Saves form to table, then returns to index</returns>
        /// <example>POST: ClientSecrets/Create</example>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientId,Description,Expiration,Type,Value")] ClientSecrets clientSecrets, string confirmPassword)
        {
            if (string.IsNullOrEmpty(clientSecrets.Value))
            {
                ViewBag.error = "Please enter a Value for: 'Client Secret'";
                return View();
            }
            if (confirmPassword != clientSecrets.Value)
            {
                ViewBag.error = "Secret fields do not match";
                return View();
            }
            clientSecrets.Value = clientSecrets.Value.ToSha256();
            if (ModelState.IsValid)
            {
                _context.Add(clientSecrets);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clientSecrets);
        }
        /// <summary>
        /// displays the edit view 
        /// </summary>
        /// <param name="id">Item on table to edit</param>
        /// <returns>Edit view</returns>
        /// <example>ET: ClientSecrets/Edit/5</example>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientSecrets = await _context.ClientSecrets.FindAsync(id);
            if (clientSecrets == null)
            {
                return NotFound();
            }
            //This passes the old hashed password to the edit view, this is then passed back as the "oldhash" variable. 
            ViewBag.OldPass = clientSecrets.Value;

            return View(clientSecrets);
        }
        /// <summary>
        /// Edit task, saves edits. 
        /// </summary>
        /// <param name="id">Location of item on table</param>
        /// <param name="clientSecrets">updated object</param>
        /// <returns>index page after updating table</returns>
        /// <example> POST: ClientSecrets/Edit/5  </example>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClientId,Description,Expiration,Type,Value")] ClientSecrets clientSecrets, string OldHash, string confirmPassword)
        {
            if (id != clientSecrets.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (confirmPassword != clientSecrets.Value)
                {
                    ViewBag.error = "Secret fields do not match";
                    return View();
                }
                if (string.IsNullOrEmpty(clientSecrets.Value))
                {
                    clientSecrets.Value = OldHash;
                }
                else
                {
                    clientSecrets.Value = clientSecrets.Value.ToSha256();
                }
                try
                {
                    _context.Update(clientSecrets);
                    await _context.SaveChangesAsync(); // saves
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientSecretsExists(clientSecrets.Id))
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

            return View(clientSecrets);
        }

        /// <summary>
        /// Displays the Delete Page
        /// </summary>
        /// <param name="id">Location of item on table</param>
        /// <returns>Delete View</returns>
        /// <example>  GET: ClientSecrets/Delete/5 </example>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientSecrets = await _context.ClientSecrets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clientSecrets == null)
            {
                return NotFound();
            }

            return View(clientSecrets);
        }

        /// <summary>
        /// action to delete from table. 
        /// </summary>
        /// <param name="id"> Location of item on table </param>
        /// <returns>Index with item deleted</returns>
        /// <example>  POST: ClientSecrets/Delete/5</example>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clientSecrets = await _context.ClientSecrets.FindAsync(id);
            _context.ClientSecrets.Remove(clientSecrets);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// Checks that there is an object at location "#" on the table. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>boolean</returns>
        private bool ClientSecretsExists(int id)
        {
            return _context.ClientSecrets.Any(e => e.Id == id);
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
