using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.AdminUI.Models;
using Microsoft.AspNetCore.Http;

namespace IdentityServer4.AdminUI.Controllers
{
    public class ClientScopesController : Controller
    {
        #region Fields
        private readonly IdentityServer4AdminUIContext _context;
        public string name = " default";
        #endregion

        #region Construcors
        public ClientScopesController(IdentityServer4AdminUIContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        // GET: ClientScopes
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
            var client = from m in _context.ClientScopes
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
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

        // GET: ClientScopes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientScopes = await _context.ClientScopes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clientScopes == null)
            {
                return NotFound();
            }

            return View(clientScopes);
        }

        // GET: ClientScopes/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32(Helpers.VarHelper.ClientId) == 0 || HttpContext.Session.GetInt32(Helpers.VarHelper.ClientId) == null)
            {
                return View(Helpers.VarHelper.error404);
            }
            return View();
        }

        // POST: ClientScopes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientId,Scope")] ClientScopes clientScopes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clientScopes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clientScopes);
        }

        // GET: ClientScopes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientScopes = await _context.ClientScopes.FindAsync(id);
            if (clientScopes == null)
            {
                return NotFound();
            }
            return View(clientScopes);
        }

        // POST: ClientScopes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClientId,Scope")] ClientScopes clientScopes)
        {
            if (id != clientScopes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clientScopes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientScopesExists(clientScopes.Id))
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
            return View(clientScopes);
        }

        // GET: ClientScopes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientScopes = await _context.ClientScopes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clientScopes == null)
            {
                return NotFound();
            }

            return View(clientScopes);
        }

        // POST: ClientScopes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clientScopes = await _context.ClientScopes.FindAsync(id);
            _context.ClientScopes.Remove(clientScopes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// Checks that there is an object at location "#" on the table. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>boolean</returns>
        private bool ClientScopesExists(int id)
        {
            return _context.ClientScopes.Any(e => e.Id == id);
        }
        /// <summary>
        /// returns the id stored in the session state, this is used for setting up the filter applied in the index
        /// </summary>
        /// <returns>int of the current clients table id</returns>
        public int GetSessionId()
        {
            int x = HttpContext.Session.GetInt32(Helpers.VarHelper.ClientId) ?? default;
            return x;
        }
        #endregion
    }
}
