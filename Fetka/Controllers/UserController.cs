using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Fetka.DAL;
using Fetka.Models;
using System.Threading.Tasks;
using PagedList;

namespace Fetka.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private CompoundContext db;

        public UserController()
        {
            db = new CompoundContext();
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

        public ActionResult ViewBase(int? page, string sortOrder)
        {
            var compounds = FilterDeleted(db.Compounds.ToList());

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
        
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Compound compound = db.Compounds.Find(id);
            if (compound == null)
            {
                return HttpNotFound();
            }
            return View(compound);
        }

        public ActionResult Order(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            YearOrder order = new YearOrder();
            Compound compound = db.Compounds.Find(id);
            if (compound == null)
            {
                return HttpNotFound();
            }
            order.Compound = compound;
            return View(order);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Order(YearOrder newOrder, int id)
        {
            newOrder.CompoundId = id;
            newOrder.Compound = null;
            YearOrder oldOrder = db.YearOrders.FirstOrDefault(yo => yo.CompoundId == newOrder.CompoundId && yo.Year == newOrder.Year);
            if (ModelState.IsValid)
            {
                if (oldOrder == null)
                {
                    db.YearOrders.Add(newOrder);
                    db.SaveChanges();

                }
                else
                {
                    oldOrder.Quantity += newOrder.Quantity;
                    db.SaveChanges();
                }

                return RedirectToAction("ViewBase");
            }
            else
                return View(newOrder);
        }

        private List<YearOrder> FilterOrder(List<YearOrder> orders)
        {
            Task<List<YearOrder>> task = Task<List<YearOrder>>.Factory.StartNew(() =>
            {
                List<YearOrder> res = new List<YearOrder>();

                foreach (YearOrder o in orders)
                {
                    if ((o.Year.Equals(DateTime.Now.Year) && (o.IsRealized == false) && (o.IsDeleted == false)))
                    {
                        res.Add(o);
                    }
                }
                return res.ToList();
            });
            return task.Result;
        }

        public ActionResult RealizeOrder()
        {
            List<YearOrder> list = db.YearOrders.ToList();
            return View(FilterOrder(list));
        }

        public ActionResult Realize(int? id)
        {
            return View(db.YearOrders.Find(id));
        }

        [HttpPost]
        public ActionResult Realize(YearOrder yearOrder)
        {
            if (ModelState.IsValid)
            {
                YearOrder order = db.YearOrders.Find(yearOrder.Id);
                order.Quantity -= yearOrder.Quantity;
                if (order.Quantity == 0)
                    order.IsRealized = true;

                db.SaveChanges();
                return RedirectToAction("ViewBase");
            }
            else
            {
                return View(yearOrder);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
