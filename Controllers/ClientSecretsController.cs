using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.AdminUI.Models;
using Microsoft.AspNetCore.Http;
using IdentityModel;

namespace IdentityServer4.AdminUI.Controllers
{
    public class ClientSecretsController : Controller
    {
        #region fields
        public string name = " default";
        const string SessionKey = "FirstSeen";
        private readonly IdentityServer4AdminUIContext _context;
        #endregion
        #region constructor
        public ClientSecretsController(IdentityServer4AdminUIContext context)
        {
            _context = context;
        }
        #endregion
        #region methods
        // GET: ClientSecrets
        public async Task<IActionResult> Index(string searchString, string name)
        {
            if (string.IsNullOrEmpty(searchString) && string.IsNullOrEmpty(name) && string.IsNullOrEmpty(HttpContext.Session.GetString("name")))
            {
                return View(Helpers.VarHelper.error404);
            }

            this.name = name;

            if (!string.IsNullOrEmpty(name))
            {
                HttpContext.Session.SetString("name", this.name);
            }
            else if (string.IsNullOrEmpty(HttpContext.Session.GetString("name")))
            {
                this.name = "null/empty";
                HttpContext.Session.SetString("name", this.name);
            }

            // this sets up our variable client to get the instances of our clients
            var client = from m in _context.ClientSecrets
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                // converts the search string to an int to perform the search function. 
                int id = int.Parse(searchString);
                client = client.Where(s => s.ClientId.Equals(id));
                HttpContext.Session.SetInt32("id", id);
            }
            else
            {
                client = client.Where(s => s.ClientId.Equals(GetSessionId()));
            }

            // returns an update with our clints that we searched. 
            return View(await client.ToListAsync());
        }

        // GET: ClientSecrets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            name = HttpContext.Session.GetString(SessionKey);
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

        // GET: ClientSecrets/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("id") == 0 || HttpContext.Session.GetInt32("id") == null)
            {
                return View(Helpers.VarHelper.error404);
            }
            return View();
        }

        // POST: ClientSecrets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientId,Description,Expiration,Type,Value")] ClientSecrets clientSecrets,string confirmPassword)
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

        // GET: ClientSecrets/Edit/5
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

        // POST: ClientSecrets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClientId,Description,Expiration,Type,Value")] ClientSecrets clientSecrets,string OldHash,string confirmPassword)
        {
            if (id != clientSecrets.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (confirmPassword!= clientSecrets.Value)
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
        // GET: ClientSecrets/Delete/5
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
        // POST: ClientSecrets/Delete/5
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
        public int GetSessionId() {
            int x = HttpContext.Session.GetInt32("id") ?? default;
            return x;
        }
        #endregion
    }
}
