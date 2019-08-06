using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.AdminUI.Models;
using Microsoft.AspNetCore.Http;

namespace IdentityServer4.AdminUI.Controllers
{
    public class ClientClaimsController : Controller
    {
        #region Fields
        private readonly IdentityServer4AdminUIContext _context;
        public string name = " default";
        #endregion

        #region Constructors
        public ClientClaimsController(IdentityServer4AdminUIContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Cliend claims index page, the session states are used for tracking the name and id across pages
        /// the paramaters passed to this function assist in filtering the page to only display relevant claims
        /// relevant = claims associated with the client the user was just selected.
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="name"></param>
        /// <returns>This returns a filtered index page based upon the client id.</returns>
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
            var client = from m in _context.ClientClaims
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                // converts teh search string to an int to perform the search function. 
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

        // GET: ClientClaims/Details/5
        /// <summary>
        /// This presents the details page bassed upon the id number passed to it. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>returns the details view for the client claims passed to it. </returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientClaims = await _context.ClientClaims
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clientClaims == null)
            {
                return NotFound();
            }

            return View(clientClaims);
        }

        // GET: ClientClaims/Create
        /// <summary>
        /// create view
        /// </summary>
        /// <returns>clientclaims/create</returns>
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32(Helpers.VarHelper.ClientId) == 0 || HttpContext.Session.GetInt32(Helpers.VarHelper.ClientId) == null)
            {
                return View(Helpers.VarHelper.error404);
            }
            return View();
        }
        /// <summary>
        /// This is for the clients form. it binds the attributes to the client claims object, 
        /// it uses that bind to confirm that the input is valid, it then saves it to the table and returns the view
        /// </summary>
        /// <param name="clientClaims"></param>
        /// <returns></returns>
        // POST: ClientClaims/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientId,Type,Value")] ClientClaims clientClaims)
        {
           
            if (ModelState.IsValid)
            {
                _context.Add(clientClaims);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clientClaims);
        }
        /// <summary>
        /// Opens the edit page of of the client claims id that was passed
        /// </summary>
        /// <param name="id"></param>
        /// <returns>returns us to the index page when complete, with the edits applied </returns>
        // GET: ClientClaims/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientClaims = await _context.ClientClaims.FindAsync(id);
            if (clientClaims == null)
            {
                return NotFound();
            }
            return View(clientClaims);
        }
        /// <summary>
        /// this binds the form values of the client claims. 
        /// this brings up the form page for the client claims in the location of id passed through/
        /// </summary>
        /// <param name="id"></param>
        /// <param name="clientClaims"></param>
        /// <returns>edit page of the clients claims id passed to it. </returns>
        // POST: ClientClaims/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClientId,Type,Value")] ClientClaims clientClaims)
        {
            if (id != clientClaims.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clientClaims);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientClaimsExists(clientClaims.Id))
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
            return View(clientClaims);
        }
        /// <summary>
        /// delete page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: ClientClaims/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientClaims = await _context.ClientClaims
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clientClaims == null)
            {
                return NotFound();
            }

            return View(clientClaims);
        }
        /// <summary>
        /// the action to perform the delete, 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST: ClientClaims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clientClaims = await _context.ClientClaims.FindAsync(id);
            _context.ClientClaims.Remove(clientClaims);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientClaimsExists(int id)
        {
            return _context.ClientClaims.Any(e => e.Id == id);
        }
        /// <summary>
        /// This is a function that returns the current session id
        /// this is used in the main index function to fetch the id incase it's not passed through.
        /// </summary>
        /// <returns>currentl id stored in the session state</returns>
        public int GetSessionId()
        {
            return HttpContext.Session.GetInt32(Helpers.VarHelper.ClientId) ?? default;
        }
        #endregion
    }
}
