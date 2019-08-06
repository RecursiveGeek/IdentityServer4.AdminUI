using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.AdminUI.Models;
using Microsoft.AspNetCore.Http;


namespace IdentityServer4.AdminUI.Controllers
{
    
    public class ClientsController : Controller
    {
        #region Fields
        private readonly IdentityServer4AdminUIContext _context;
        public int Sessionid;
        #endregion

        #region Constructors
        public ClientsController(IdentityServer4AdminUIContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        /// <summary>
        /// will filter clients by search string. 
        /// sets up base session state. 
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Main Clients page</returns>
        public async Task<IActionResult> Index(string searchString)
        {
            // this sets up our variable client to get the instances of our clients
            var client = from m in _context.Clients
                         select m;
            // 
            if (!string.IsNullOrEmpty(searchString))
            {
                client = client.Where(s => s.ClientName.Contains(searchString));
            }
            // returns an update with our clints that we searched. 
            return View(await client.ToListAsync());
        }
        /// <summary>
        /// Occurs when traversing to an invalid url
        /// </summary>
        /// <returns> 404 page not found view </returns>
        public IActionResult PageNotFound() {

            return View(Helpers.VarHelper.error404);

        }
        // GET: Clients/Details/5
        /// <summary>
        /// Displays all the information for the selected client. 
        /// Also sets up the session state name and id to be used when traversing among other page details for said client (such as client secrets)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Details page for the client at id "x" on the clients table</returns>
        public async Task<IActionResult> Details(int? id)
        {
            var clients = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);

            if (id == null||clients == null)
            {
                return PageNotFound();
            }
            Sessionid = id ?? default;
            string retrievedName = FetchName(Sessionid);
            if (string.IsNullOrEmpty(retrievedName))
            {
                retrievedName = " ";
            }
            RecordIdInSession(Sessionid);
            RecordNameInSession(retrievedName);

            return View(clients);
        }

        // GET: Clients/Create
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Create a client page</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// This is the action to create a new client. 
        /// This does the adding the new client to the table after checking validations
        /// </summary>
        /// <param name="clients"></param>
        /// <returns>Main index page</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AbsoluteRefreshTokenLifetime,AccessTokenLifetime,AccessTokenType,AllowAccessTokensViaBrowser,AllowOfflineAccess,AllowPlainTextPkce,AllowRememberConsent,AlwaysIncludeUserClaimsInIdToken,AlwaysSendClientClaims,AuthorizationCodeLifetime,BackChannelLogoutSessionRequired,BackChannelLogoutUri,ClientClaimsPrefix,ClientId,ClientName,ClientUri,ConsentLifetime,Description,EnableLocalLogin,Enabled,FrontChannelLogoutSessionRequired,FrontChannelLogoutUri,IdentityTokenLifetime,IncludeJwtId,LogoUri,PairWiseSubjectSalt,ProtocolType,RefreshTokenExpiration,RefreshTokenUsage,RequireClientSecret,RequireConsent,RequirePkce,SlidingRefreshTokenLifetime,UpdateAccessTokenClaimsOnRefresh")] Clients clients)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clients);
               await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clients);
        }

        /// <summary>
        /// This sets up the session states as well for further page navigation. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Edit page for client at id "x"</returns>
        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Sessionid = id ?? default;
            string retrievedName = FetchName(Sessionid);

            if (!string.IsNullOrEmpty(retrievedName))
            {
                RecordNameInSession(retrievedName);
            }
            RecordIdInSession(Sessionid);
            if (id == null)
            {
                return NotFound();
            }

            var clients = await _context.Clients.FindAsync(id);
            if (clients == null)
            {
                return NotFound();
            }
            return View(clients);
        }

        /// <summary>
        /// saves the edits to the database. returns to main clients page. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="clients"></param>
        /// <returns>main client index page.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AbsoluteRefreshTokenLifetime,AccessTokenLifetime,AccessTokenType,AllowAccessTokensViaBrowser,AllowOfflineAccess,AllowPlainTextPkce,AllowRememberConsent,AlwaysIncludeUserClaimsInIdToken,AlwaysSendClientClaims,AuthorizationCodeLifetime,BackChannelLogoutSessionRequired,BackChannelLogoutUri,ClientClaimsPrefix,ClientId,ClientName,ClientUri,ConsentLifetime,Description,EnableLocalLogin,Enabled,FrontChannelLogoutSessionRequired,FrontChannelLogoutUri,IdentityTokenLifetime,IncludeJwtId,LogoUri,PairWiseSubjectSalt,ProtocolType,RefreshTokenExpiration,RefreshTokenUsage,RequireClientSecret,RequireConsent,RequirePkce,SlidingRefreshTokenLifetime,UpdateAccessTokenClaimsOnRefresh")] Clients clients)
        {
            
            if (id != clients.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clients);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientsExists(clients.Id))
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
            return View(clients);
        }

        // GET: Clients/Delete/5
        /// <summary>
        /// This will display the clients information on the confirm delete page. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>confirm delete page</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            Sessionid = id ?? default;
            string retrievedName = FetchName(Sessionid);

            if (!string.IsNullOrEmpty(retrievedName))
            {
                RecordNameInSession(retrievedName);
            }
            RecordIdInSession(Sessionid);

            if (id == null)
            {
                return NotFound();
            }

            var clients = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clients == null)
            {
                return NotFound();
            }

            return View(clients);
        }

        // POST: Clients/Delete/5
        /// <summary>
        /// This is the action that deletes the client from the table. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>main client index page</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clients = await _context.Clients.FindAsync(id);
            _context.Clients.Remove(clients);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// This confirms that the client trying to be loaded exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns>boolean</returns>
        private bool ClientsExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }

        // attempt: set the client name
        /// <summary>
        /// This is used to set the client name to the session state(which is how the other pages will gather this information)
        /// </summary>
        /// <param name="action"></param>
        private void RecordNameInSession(string action)
        {
            HttpContext.Session.SetString(Helpers.VarHelper.ClientName, action);
        }
        // this records the last used id
        /// <summary>
        /// This records the client id in the session state for the other pages to retrieve
        /// </summary>
        /// <param name="action"></param>
        private void RecordIdInSession(int action)
        {
            HttpContext.Session.SetInt32(Helpers.VarHelper.ClientId, action);
        }


        /// <summary>
        ///This returns the name of the client at id "x"
        /// </summary>
        /// <param name="id"></param>
        /// <returns> client name </returns>

        public string FetchName(int id)
        {
            Clients Name = getClients(id);

            return Name.ClientName;
        }
        /// <summary>
        ///  This returns a client object when you pass the id
        ///  input id for client => get the client object
        /// </summary>
        /// <param name="id"></param>
        /// <returns> client@(id) </returns>
        public Clients getClients(int id)
        {
            return _context.Clients.Find(id);
        }
        #endregion

    }
}
