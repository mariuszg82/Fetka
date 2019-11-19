using Fetka.Models;
using Fetka.DAL;
using Fetka.Commons;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Security;
using PagedList;

namespace Fetka.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private CompoundContext db = new CompoundContext();
        private ApplicationDbContext adb = new ApplicationDbContext();
        private ApplicationUserManager aum = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        public AdminController()
        {
        }

        private List<Compound> FilterDeleted(List<Compound> compounds)
        {
            Task<List<Compound>> task = Task<List<Compound>>.Factory.StartNew(() =>
            {
                var res = compounds.Where(r => (!r.IsDeleted));
                return res.ToList();
            });
            return task.Result;
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Compound compound)
        {
            using (CompoundContext db = new CompoundContext())
            {
                if (ModelState.IsValid)
                {
                    HttpPostedFileBase file = Request.Files["imageFile"];
                    if (file != null && file.ContentLength > 0)
                    {
                        compound.Image = file.FileName;
                        file.SaveAs(HttpContext.Server.MapPath("~/Resources/Pics/") + compound.Image);
                    }
                    compound.IsDeleted = false;
                    db.Compounds.Add(compound);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(compound);
            }
        }

        private List<ApplicationUser> FilterLogged(List<ApplicationUser> users)
        {
            Task<List<ApplicationUser>> task = Task<List<ApplicationUser>>.Factory.StartNew(() =>
            {
                string userId = HttpContext.User.Identity.GetUserId();
                var res = users.Where(r => (!r.Id.Equals(userId)));
                return res.ToList();
            });
            return task.Result;
        }

        public ActionResult ManageUsers()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                List<ApplicationUser> users = FilterLogged(context.Users.ToList<ApplicationUser>());
                return View(users);
            }
        }

        public ActionResult DeleteUser(string id)
        {
            using (ApplicationDbContext adb = new ApplicationDbContext())
            {
                ApplicationUser au = adb.Users.Find(id);
                adb.Users.Remove(au);
                adb.SaveChanges();

                return RedirectToAction("ManageUsers", "Admin");
            }
        }

        public ActionResult EditUser(string id)
        {
            using (ApplicationDbContext adb = new ApplicationDbContext())
            {
                SimpleUserViewModel user = new SimpleUserViewModel();

                if (id.Count() == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ApplicationUser au = adb.Users.Find(id);
                string role = aum.GetRoles(au.Id).FirstOrDefault();

                if (au == null)
                {
                    return HttpNotFound();
                }

                user.Id = au.Id;
                user.FirstName = au.FirstName;
                user.LastName = au.LastName;
                user.Login = au.Login;
                user.Role = (Fetka.Commons.Roles)Enum.Parse(typeof(Fetka.Commons.Roles), role);
                user.Blocked = au.Blocked;

                ViewBag.Roles = GetAvailableRoles();

                return View(user);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser([Bind(Include = "Id,FirstName,LastName,Login,Role,Blocked")] SimpleUserViewModel suvm)
        {
            using (ApplicationDbContext adb = new ApplicationDbContext())
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser au = adb.Users.Find(suvm.Id);
                    string role = aum.GetRoles(au.Id).FirstOrDefault();

                    if (!au.FirstName.Equals(suvm.FirstName))
                        au.FirstName = suvm.FirstName;
                    if (!au.LastName.Equals(suvm.LastName))
                        au.LastName = suvm.LastName;
                    if (!au.Login.Equals(suvm.Login))
                    {
                        au.Login = suvm.Login;
                        au.UserName = suvm.Login;
                    }

                    if (!role.Equals(suvm.Role.ToString()))
                    {
                        aum.RemoveFromRole(au.Id, role);
                        aum.AddToRole(au.Id, suvm.Role.ToString());
                    }
                    au.FirstName = suvm.FirstName;
                    if (!au.Blocked.Equals(suvm.Blocked))
                        au.Blocked = suvm.Blocked;


                    adb.Entry(au).State = EntityState.Modified;
                    adb.SaveChanges();
                    return RedirectToAction("ManageUsers");
                }
                return View(suvm);
            }
        }

        private IEnumerable<string> GetAvailableRoles()
        {
            var roleStore = new RoleStore<IdentityRole>(adb);
            var roleMngr = new RoleManager<IdentityRole>(roleStore);

            var roles = roleMngr.Roles.Select(x => x.Name).ToList();

            return roles;
        }

        public ActionResult ManageBase(int? page, string sortOrder)
        {
            var compounds = db.Compounds.ToList();

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Nazwa" : "";
            switch (sortOrder)
            {
                case "Nazwa":
                    compounds = compounds.OrderByDescending(s => s.Name).ToList();
                    break;
                default:
                    compounds = compounds.OrderBy(s => s.Name).ToList();
                    break;
            }

            var pageNumber = page ?? 1;
            var onePageOfProducts = compounds.ToPagedList(pageNumber, 10);
            ViewBag.OnePageOfProducts = onePageOfProducts;
            return View();
        }

        public ActionResult DeleteCompound(int id)
        {
            using (CompoundContext db = new CompoundContext())
            {
                Compound compound = db.Compounds.Find(id);
                compound.IsDeleted = true;
                db.Entry(compound).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("ManageBase");
            }
        }

        public ActionResult EditCompound(int id)
        {
            using (CompoundContext db = new CompoundContext())
            {
                return View(db.Compounds.Find(id));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCompound(Compound compound)
        {
            if (ModelState.IsValid)
            {
                Compound newCompound = db.Compounds.Find(compound.Id);
                HttpPostedFileBase file = Request.Files["imageFile"];
                if (file != null && file.ContentLength > 0)
                {
                    if (newCompound.Image == null || !newCompound.Image.Equals(file.FileName))
                    {
                        newCompound.Image = file.FileName;
                        file.SaveAs(HttpContext.Server.MapPath("~/Resources/Pics/") + newCompound.Image);
                    }
                }
                newCompound.CAS = compound.CAS;
                newCompound.Certificate = compound.Certificate;
                newCompound.Description = compound.Description;
                newCompound.IsDeleted = compound.IsDeleted;
                newCompound.Name = compound.Name;
                newCompound.Orders = compound.Orders;
                newCompound.Purity = compound.Purity;

                db.SaveChanges();
                return RedirectToAction("ManageBase");
            }
            return View(compound);
        }

        public ActionResult ManageOrders()
        {
            return View(db.YearOrders.ToList());
        }

        private List<YearOrder> YearFilter(List<YearOrder> orders, int year)
        {
            Task<List<YearOrder>> task = Task<List<YearOrder>>.Factory.StartNew(() =>
            {
                var res = orders.Where(r => (r.Year.Equals(year)));
                return res.ToList();
            });
            return task.Result;
        }

        [HttpPost]
        public ActionResult ManageOrders(string year)
        {
            int y;
            if (Int32.TryParse(year, out y))
            {
                return View(YearFilter(db.YearOrders.ToList(), y));
            }
            else
            {
                return View(db.YearOrders.ToList());
            }
        }

        public ActionResult DeleteOrder(int id)
        {
            using (CompoundContext db = new CompoundContext())
            {
                YearOrder order = db.YearOrders.Find(id);
                db.YearOrders.Remove(order);
                db.SaveChanges();

                return RedirectToAction("ManageOrders");
            }
        }

        public ActionResult EditOrder(int id)
        {
            YearOrder order = db.YearOrders.Find(id);
            return View(order);
        }

        [HttpPost]
        public ActionResult EditOrder(YearOrder order)
        {
            if (ModelState.IsValid)
            {
                order.CompoundId = order.Compound.Id;
                db.Entry(order).State = EntityState.Modified;
                if (order.Quantity == 0)
                    order.IsRealized = true;
                db.SaveChanges();
                return RedirectToAction("ManageOrders");
            }
            return RedirectToAction("ManageOrders");
        }
    }
}
