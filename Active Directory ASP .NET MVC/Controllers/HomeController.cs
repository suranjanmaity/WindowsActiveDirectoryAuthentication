using ActiveDirectory.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;

namespace ActiveDirectory.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public ActionResult GetAllUsers()
        {
            List<User> ADUsers = GetallAdUsers();
            return View(ADUsers);
        }

        public ActionResult GetAllGroups()
        {
            List<Group> ADGroups = GetallGroups();
            return View(ADGroups);
        }

        //if you want to get Groups of Specific OU you have to add OU Name in Context        
        public static List<User> GetallAdUsers()
        {
            List<User> AdUsers = new List<User>();
            //MBS.com My Domain Controller which i created 
            //OU=DevOU --Organizational Unit which i created 
            //and create users and groups inside it 
            var ctx = new PrincipalContext(ContextType.Domain, "MBS", "OU=DevOU,DC=MBS,DC=com");
            UserPrincipal userPrin = new UserPrincipal(ctx);
            userPrin.Name = "*";
            var searcher = new System.DirectoryServices.AccountManagement.PrincipalSearcher();
            searcher.QueryFilter = userPrin;
            var results = searcher.FindAll();
            foreach (Principal p in results)
            {
                AdUsers.Add(new User
                {
                    DisplayName = p.DisplayName,
                    Samaccountname = p.SamAccountName
                });
            }
            return AdUsers;
        }

        public ActionResult ResetPassword(string Samaccountname)
        {
            //i get the user by its SamaccountName to change his password
            PrincipalContext context = new PrincipalContext
                                       (ContextType.Domain, "MBS", "OU=DevOU,DC=MBS,DC=com");
            UserPrincipal user = UserPrincipal.FindByIdentity
                                 (context, IdentityType.SamAccountName, Samaccountname);
            //Enable Account if it is disabled
            user.Enabled = true;
            //Reset User Password
            string newPassword = "P@ssw0rd";
            user.SetPassword(newPassword);
            //Force user to change password at next logon dh optional
            user.ExpirePasswordNow();
            user.Save();
            TempData["msg"] = "<script>alert('Password Changed Successfully');</script>";
            return RedirectToAction("GetAllUsers");
        }

        //if you want to get all Groups of Specific OU you have to add OU Name in Context 
        public static List<Group> GetallGroups()
        {
            List<Group> AdGroups = new List<Group>();
            var ctx = new PrincipalContext(ContextType.Domain, "MBS", "OU=DevOU,DC=MBS,DC=com");
            GroupPrincipal _groupPrincipal = new GroupPrincipal(ctx);

            PrincipalSearcher srch = new PrincipalSearcher(_groupPrincipal);

            foreach (var found in srch.FindAll())
            {
                AdGroups.Add(new Group { GroupName = found.ToString() });

            }
            return AdGroups;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}