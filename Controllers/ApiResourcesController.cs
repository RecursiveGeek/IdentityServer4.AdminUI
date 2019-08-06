using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.AdminUI.Models;
using Microsoft.AspNetCore.Http;

namespace IdentityServer4.AdminUI.Controllers
{
    public class ApiResourcesController : Controller
    {
        #region fields
        private readonly IdentityServer4AdminUIContext _context;
        public int Sessionid;
        #endregion
        #region constructors
        public ApiResourcesController(IdentityServer4AdminUIContext context)
        {
            _context = context;
        }
        #endregion
        #region methods
        // GET: ApiResources
        /// <summary>
        /// displays the main api resources page.
        /// </summary>
        /// <returns>/apiresources/index (view) </returns>
        public async Task<IActionResult> Index()
        {
            return View(await _context.ApiResources.ToListAsync());
        }

        /// <summary>
        /// checks for existing resource, also sets the session states
        /// </summary>
        /// <param name="id">Identity collumn</param>
        /// <returns>apiresources/details/"id"</returns>
        /// <remarks>
        /// Example GET: ApiResources/Details/5
        /// </remarks>
        public async Task<IActionResult> Details(int? id)
        {

            Sessionid = id ?? default(int);
            var retrievedName = FetchName(Sessionid);
            RecordNameInSession(retrievedName);
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

        // GET: ApiResources/Create
        /// <summary>
        /// opens the create view
        /// </summary>
        /// <returns>apiresources/create</returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApiResources/Create
        /// <summary>
        /// the create action, saves to the table
        /// </summary>
        /// <param name="apiResources"></param>
        /// <returns>apiresources index page</returns>
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

        // GET: ApiResources/Edit/5
        /// <summary>
        /// sets the session information, opens edit view
        /// </summary>
        /// <param name="id"></param>
        /// <returns> /apiresources/edit </returns>
        public async Task<IActionResult> Edit(int? id)
        {
            Sessionid = id ?? default(int);
            string retrievedName = FetchName(Sessionid);
            RecordNameInSession(retrievedName);
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

        // POST: ApiResources/Edit/5
        /// <summary>
        /// the edit action , saves edits to table.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apiResources"></param>
        /// <returns>api resources index page.</returns>
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

        // GET: ApiResources/Delete/5
        /// <summary>
        /// opens the delete view
        /// </summary>
        /// <param name="id"></param>
        /// <returns>/apiresources/delete/"#"</returns>
        public async Task<IActionResult> Delete(int? id)
        {

            Sessionid = id ?? default(int);
            string retrievedName = FetchName(Sessionid);
            RecordNameInSession(retrievedName);
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

        // POST: ApiResources/Delete/5
        /// <summary>
        /// This deletes the resource from the table
        /// </summary>
        /// <param name="id"></param>
        /// <returns>apiresources / index </returns>
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
        /// checks if the api resource exists. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>t/f, is there na api resource at id "#"</returns>
        private bool ApiResourcesExists(int id)
        {
            return _context.ApiResources.Any(e => e.Id == id);
        }
        // the below code is how the setup for the sessions
        /// <summary>
        /// retrieves the name
        /// </summary>
        /// <param name="id"></param>
        /// <returns>the display name of the client at id "x"</returns>
        public string FetchName(int id)
        {
            ApiResources Name = GetClients(id);

            return Name.DisplayName;
        }
        /// <summary>
        /// returns the ApiResource object associated with id "#"
        /// </summary>
        /// <param name="id"></param>
        /// <returns>an apiresources object</returns>
        public ApiResources GetClients(int id)
        {
            return _context.ApiResources.Find(id);
        }
        // attempt: set the client name
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

