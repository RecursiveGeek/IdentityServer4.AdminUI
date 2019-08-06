using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.AdminUI.Models;
using Microsoft.AspNetCore.Http;

namespace IdentityServer4.AdminUI.Controllers
{
    public class ClientGrantTypesController : Controller
    {
        #region fields
        private readonly IdentityServer4AdminUIContext _context;
        public string name = " default";
        #endregion
        #region constructor
        public ClientGrantTypesController(IdentityServer4AdminUIContext context)
        {
            _context = context;
        }
        #endregion
        #region methods
        // GET: ClientGrantTypes
        public async Task<IActionResult> Index(string searchString, string name)
        {
            // If checks if there is no variables passed and nothing in the session state. 
            // if all of those are empty, it returns a 404 message and redirects the user bacl to the main clients page. 
            if (string.IsNullOrEmpty(searchString) && string.IsNullOrEmpty(name) && string.IsNullOrEmpty(HttpContext.Session.GetString(Helpers.VarHelper.ClientName)))
            {
                return View(Helpers.VarHelper.error404);
            }
            // assigns the name variable to the name passed through
            this.name = name;
            //if  both the name passed through is empty, and the session state is emtpy, sets the name to null/empty. 
            // this is to prevent errors that sometimes occured. 
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
            var client = from m in _context.ClientGrantTypes
                         select m;
            // this is where we apply the search string filter to the view. 
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


        // GET: ClientGrantTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientGrantTypes = await _context.ClientGrantTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clientGrantTypes == null)
            {
                return NotFound();
            }

            return View(clientGrantTypes);
        }

        // GET: ClientGrantTypes/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32(Helpers.VarHelper.ClientId) == 0 || HttpContext.Session.GetInt32(Helpers.VarHelper.ClientId) == null)
            {
                return View(Helpers.VarHelper.error404);
            }
            return View();
        }

        // POST: ClientGrantTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientId,GrantType")] ClientGrantTypes clientGrantTypes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clientGrantTypes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clientGrantTypes);
        }

        // GET: ClientGrantTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientGrantTypes = await _context.ClientGrantTypes.FindAsync(id);
            if (clientGrantTypes == null)
            {
                return NotFound();
            }
            return View(clientGrantTypes);
        }

        // POST: ClientGrantTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClientId,GrantType")] ClientGrantTypes clientGrantTypes)
        {
            if (id != clientGrantTypes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clientGrantTypes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientGrantTypesExists(clientGrantTypes.Id))
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
            return View(clientGrantTypes);
        }

        // GET: ClientGrantTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientGrantTypes = await _context.ClientGrantTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clientGrantTypes == null)
            {
                return NotFound();
            }

            return View(clientGrantTypes);
        }

        // POST: ClientGrantTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clientGrantTypes = await _context.ClientGrantTypes.FindAsync(id);
            _context.ClientGrantTypes.Remove(clientGrantTypes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// Checks that there is an object at location "#" on the table. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>boolean</returns>
        private bool ClientGrantTypesExists(int id)
        {
            return _context.ClientGrantTypes.Any(e => e.Id == id);
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
