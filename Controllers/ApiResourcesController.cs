using IdentityServer4.AdminUI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.AdminUI.Controllers
{
    public class ApiResourcesController : Controller
    {
        #region Fields
        private readonly IdentityServer4AdminUIContext _context;
        public int Sessionid;
        #endregion

        #region Constructors
        public ApiResourcesController(IdentityServer4AdminUIContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        /// <summary>
        /// This is the main index page 
        /// This runs a filter based upon the paramaters to only show the relevant objects
        /// </summary>
        /// <returns>index page</returns>
        /// <example>GET: ApiResources</example>
        public async Task<IActionResult> Index()
        {
            return View(await _context.ApiResources.ToListAsync());
        }

        /// <summary>
        /// Displays the view for the Details page 
        /// </summary>
        /// <param name="id"> This is the  table ID </param>
        /// <returns>View  Details</returns>
        /// <example>GET: ApiResources/Details/5</example>
        public async Task<IActionResult> Details(int? id)
        {

            Sessionid = id ?? default;
            var retrievedName = FetchName(Sessionid);
            if (!string.IsNullOrEmpty(retrievedName))
            {
                RecordNameInSession(retrievedName);
            }
            RecordIdInSession(Sessionid);

            if (id == null)
            {
                return NotFound();
            }

            var apiResources = await _context.ApiResources
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apiResources == null)
            {
                return NotFound();
            }

            return View(apiResources);
        }

        /// <summary>
        /// Displays the Create page
        /// </summary>
        /// <returns>View  Create</returns>
        /// <example>GET: ApiResources/Create</example>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// create task - saves the create page form information to the table.
        /// </summary>
        /// <param name="apiResources">The object to be saved to the table</param>
        /// <returns>Saves form to table, then returns to index</returns>
        /// <example>POST: ApiResources/Create</example>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,DisplayName,Enabled,Name")] ApiResources apiResources)
        {
            if (ModelState.IsValid)
            {
                _context.Add(apiResources);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(apiResources);
        }

        /// <summary>
        /// Edit task, saves edits. 
        /// </summary>
        /// <param name="id">Location of item on table</param>
        /// <returns>index page after updating table</returns>
        /// <example> POST: ApiResources/Edit/5  </example>
        public async Task<IActionResult> Edit(int? id)
        {
            Sessionid = id ?? default;
            var retrievedName = FetchName(Sessionid);
            if (!string.IsNullOrEmpty(retrievedName))
            {
                RecordNameInSession(retrievedName);
            }
            RecordIdInSession(Sessionid);

            if (id == null)
            {
                return NotFound();
            }

            var apiResources = await _context.ApiResources.FindAsync(id);
            if (apiResources == null)
            {
                return NotFound();
            }
            return View(apiResources);
        }

        /// <summary>
        /// Edit task, saves edits. 
        /// </summary>
        /// <param name="id">Location of item on table</param>
        /// <param name="apiResources">updated object</param>
        /// <returns>index page after updating table</returns>
        /// <example> POST: ApiResources/Edit/5  </example>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,DisplayName,Enabled,Name")] ApiResources apiResources)
        {

            if (id != apiResources.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(apiResources);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApiResourcesExists(apiResources.Id))
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
            return View(apiResources);
        }

        /// <summary>
        /// Displays the Delete Page
        /// </summary>
        /// <param name="id">Location of item on table</param>
        /// <returns>Delete View</returns>
        /// <example>  GET: ApiResources/Delete/5 </example>
        public async Task<IActionResult> Delete(int? id)
        {

            Sessionid = id ?? default;
            var retrievedName = FetchName(Sessionid);
            if (!string.IsNullOrEmpty(retrievedName))
            {
                RecordNameInSession(retrievedName);
            }
            RecordIdInSession(Sessionid);

            if (id == null)
            {
                return NotFound();
            }

            var apiResources = await _context.ApiResources
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apiResources == null)
            {
                return NotFound();
            }

            return View(apiResources);
        }

        /// <summary>
        /// action to delete from table. 
        /// </summary>
        /// <param name="id"> Location of item on table </param>
        /// <returns>Index with item deleted</returns>
        /// <example>  POST: ApiResources/Delete/5</example>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var apiResources = await _context.ApiResources.FindAsync(id);
            _context.ApiResources.Remove(apiResources);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// validates if claim exists = true
        /// </summary>
        /// <param name="id">item on table</param>
        /// <returns>boolean</returns>
        private bool ApiResourcesExists(int id)
        {
            return _context.ApiResources.Any(e => e.Id == id);
        }
        // the below code is how the setup for the sessions
        /// <summary>
        /// retrieves the name
        /// </summary>
        /// <param name="id"></param>
        /// <returns>the display name at id "x"</returns>
        public string FetchName(int id)
        {
            ApiResources Name = _context.ApiResources.Find(id);

            return Name.DisplayName;
        }
        // attempt: records the name on session
        /// <summary>
        /// This is the method to set the sname in the session. 
        /// </summary>
        /// <param name="action"></param>
        private void RecordNameInSession(string action)
        {
            HttpContext.Session.SetString(Helpers.VarHelper.ApiResourceName, action);
        }
        // this records the last used id
        /// <summary>
        /// This is the method to set the id session state.  
        /// </summary>
        /// <param name="action"></param>
        private void RecordIdInSession(int action)
        {
            HttpContext.Session.SetInt32(Helpers.VarHelper.ApiResourceId, action);
        }
        #endregion

    }
}

