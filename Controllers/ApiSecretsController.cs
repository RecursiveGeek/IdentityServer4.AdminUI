using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.AdminUI.Models;
using Microsoft.AspNetCore.Http;
using IdentityModel;

namespace IdentityServer4.AdminUI.Controllers
{
    public class ApiSecretsController : Controller
    {
        #region Fields
        private readonly IdentityServer4AdminUIContext _context;
        string name = "default";
        #endregion
        #region Constructors
        public ApiSecretsController(IdentityServer4AdminUIContext context)
        {
            _context = context;
        }
        #endregion
        #region Methods
        // GET: ApiSecrets
        /// <summary>
        /// parameters filter the secrets accordingly.
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="name"></param>
        /// <returns>apisecrets/index</returns>
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

            // this sets up our variable client to get the instances of our clients
            var client = from m in _context.ApiSecrets
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                // converts the search string to an int to perform the search function. 
                int id = int.Parse(searchString);
                client = client.Where(s => s.ApiResourceId.Equals(id));
                HttpContext.Session.SetInt32(Helpers.VarHelper.ApiResourceId, id);
            }
            else
            {
                client = client.Where(s => s.ApiResourceId.Equals(GetSessionId()));
            }
            // returns an update with our clints that we searched. 
            return View(await client.ToListAsync());
        }
        // GET: ApiSecrets/Details/5
        /// <summary>
        /// secret details view
        /// </summary>
        /// <param name="id"></param>
        /// <returns>/apisecrets/details/"#"</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apiSecrets = await _context.ApiSecrets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apiSecrets == null)
            {
                return NotFound();
            }

            return View(apiSecrets);
        }

        // GET: ApiSecrets/Create
        /// <summary>
        /// the create page for secrets, has a check on the sessions to prevent errors incase of a bookmark. 
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32(Helpers.VarHelper.ApiResourceId) == 0 || HttpContext.Session.GetInt32(Helpers.VarHelper.ApiResourceId) == null)
            {
                return View(Helpers.VarHelper.error404);
            }
            return View();
        }

        // POST: ApiSecrets/Create
        /// <summary>
        /// The create action. 
        /// The additional confirmpassowrd paramater will compare the two password fields to make sure that the secret and confirm secret values match. 
        /// otherwise it returns an error message and the create view again
        /// </summary>
        /// <param name="apiSecrets"></param>
        /// <param name="confirmPassword"></param>
        /// <returns>create view / secret index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ApiResourceId,Description,Expiration,Type,Value")] ApiSecrets apiSecrets, string confirmPassword)
        {
            if (string.IsNullOrEmpty(apiSecrets.Value))
            {
                ViewBag.error = "Please enter a Value for: 'Api Secret'";
                return View();
            }
            if (confirmPassword!=apiSecrets.Value)
            {
                ViewBag.error = "Secret fields do not match";
                return View();
            }
            apiSecrets.Value = apiSecrets.Value.ToSha256();
            if (ModelState.IsValid)
            {
                _context.Add(apiSecrets);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(apiSecrets);
        }

        // GET: ApiSecrets/Edit/5
        /// <summary>
        /// edit view
        /// this passes the value of old hash to the page as a viewbag to be compared to for confirmation.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>apisecrets/edit/"#"</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apiSecrets = await _context.ApiSecrets.FindAsync(id);
            if (apiSecrets == null)
            {
                return NotFound();
            }
            ViewBag.OldPass = apiSecrets.Value;
            return View(apiSecrets);
        }

        // POST: ApiSecrets/Edit/5
        /// <summary>
        /// the edit action, saves to the table. 
        /// The oldhash and confirmpassword paramaters are used to compare.
        /// the confirm password makes sure that both password fields match or returns an error,
        /// the oldhash is used to compare the new password to the old one, if blank do not update the password
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apiSecrets"></param>
        /// <param name="OldHash"></param>
        /// <param name="confirmPassword"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ApiResourceId,Description,Expiration,Type,Value")] ApiSecrets apiSecrets, string OldHash,string confirmPassword)
        {
            if (id != apiSecrets.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (confirmPassword!= apiSecrets.Value)
                {
                    ViewBag.error = "secret values do not match";
                        return View();
                }
                if (string.IsNullOrEmpty(apiSecrets.Value))
                {
                    apiSecrets.Value = OldHash;
                }
                else
                {
                    apiSecrets.Value = apiSecrets.Value.ToSha256();
                }
                try
                {
                    _context.Update(apiSecrets);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApiSecretsExists(apiSecrets.Id))
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
            return View(apiSecrets);
        }

        // GET: ApiSecrets/Delete/5
        /// <summary>
        /// delete view
        /// </summary>
        /// <param name="id"></param>
        /// <returns>apisecrets/delete/"#"</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var apiSecrets = await _context.ApiSecrets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apiSecrets == null)
            {
                return NotFound();
            }
            return View(apiSecrets);
        }

        // POST: ApiSecrets/Delete/5
        /// <summary>
        /// delete action, removes from table
        /// </summary>
        /// <param name="id"></param>
        /// <returns>apisecrets index</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var apiSecrets = await _context.ApiSecrets.FindAsync(id);
            _context.ApiSecrets.Remove(apiSecrets);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// confirms that the secret exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns>boolean</returns>
        private bool ApiSecretsExists(int id)
        {
            return _context.ApiSecrets.Any(e => e.Id == id);
        }
        /// <summary>
        /// retrieves the id from the session state
        /// </summary>
        /// <returns>int </returns>
        public int GetSessionId()
        {
            return HttpContext.Session.GetInt32(Helpers.VarHelper.ApiResourceId) ?? default;
        }
        #endregion
    }
}
