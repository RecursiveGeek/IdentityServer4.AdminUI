using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.AdminUI.Models;
using Microsoft.AspNetCore.Http;

namespace IdentityServer4.AdminUI.Controllers
{
    public class ClientCorsOriginsController : Controller
    {
        #region fields
        private readonly IdentityServer4AdminUIContext _context;
        public string name = " default";
        #endregion
        #region constructor
        public ClientCorsOriginsController(IdentityServer4AdminUIContext context)
        {
            _context = context;
        }
        #endregion
        #region methods
        /// <summary>
        /// indexpage for client cors origins
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="name"></param>
        /// <returns>returns a filtered list of the current client id related CorsOrigins. </returns>
        // GET: ClientCorsOrigins
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
            var client = from m in _context.ClientCorsOrigins
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                // converts teh search string to an int to perform the search function. 
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


        // GET: ClientCorsOrigins/Details/5
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

        // GET: ClientCorsOrigins/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("id") == 0 || HttpContext.Session.GetInt32("id") == null)
            {
                return View(Helpers.VarHelper.error404);
            }
            return View();
        }

        // POST: ClientCorsOrigins/Create
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

        // GET: ClientCorsOrigins/Edit/5
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

        // POST: ClientCorsOrigins/Edit/5
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

        // GET: ClientCorsOrigins/Delete/5
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

        // POST: ClientCorsOrigins/Delete/5
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
        /// Checks that there is an object at location "#" on the table. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>boolean</returns>
        private bool ClientCorsOriginsExists(int id)
        {
            return _context.ClientCorsOrigins.Any(e => e.Id == id);
        }
        /// <summary>
        /// returns the id stored in the session state, this is used for setting up the filter applied in the index
        /// </summary>
        /// <returns>int of the current clients table id</returns>
        public int GetSessionId()
        {
            int x = HttpContext.Session.GetInt32("id") ?? default;
            return x;
        }
        #endregion
    }
}
