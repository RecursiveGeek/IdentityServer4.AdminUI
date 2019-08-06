using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.AdminUI.Models;
using Microsoft.AspNetCore.Http;

namespace IdentityServer4.AdminUI.Controllers
{
    public class ClientPropertiesController : Controller
    {
        #region Fields
        private readonly IdentityServer4AdminUIContext _context;
        public string name = " default";
        #endregion

        #region Constructors
        public ClientPropertiesController(IdentityServer4AdminUIContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        // GET: ClientProperties
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
            var client = from m in _context.ClientProperties
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

        // GET: ClientProperties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientProperties = await _context.ClientProperties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clientProperties == null)
            {
                return NotFound();
            }

            return View(clientProperties);
        }

        // GET: ClientProperties/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32(Helpers.VarHelper.ClientId) == 0 || HttpContext.Session.GetInt32(Helpers.VarHelper.ClientId) == null)
            {
                return View(Helpers.VarHelper.error404);
            }
            return View();
        }

        // POST: ClientProperties/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientId,Key,Value")] ClientProperties clientProperties)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clientProperties);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clientProperties);
        }

        // GET: ClientProperties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientProperties = await _context.ClientProperties.FindAsync(id);
            if (clientProperties == null)
            {
                return NotFound();
            }
            return View(clientProperties);
        }

        // POST: ClientProperties/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClientId,Key,Value")] ClientProperties clientProperties)
        {
            if (id != clientProperties.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clientProperties);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientPropertiesExists(clientProperties.Id))
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
            return View(clientProperties);
        }

        // GET: ClientProperties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientProperties = await _context.ClientProperties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clientProperties == null)
            {
                return NotFound();
            }

            return View(clientProperties);
        }

        // POST: ClientProperties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clientProperties = await _context.ClientProperties.FindAsync(id);
            _context.ClientProperties.Remove(clientProperties);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// Checks that there is an object at location "#" on the table. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>boolean</returns>
        private bool ClientPropertiesExists(int id)
        {
            return _context.ClientProperties.Any(e => e.Id == id);
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
