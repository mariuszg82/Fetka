using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fetka.DAL;
using Fetka.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Fetka.Controllers
{
    public class HomeController : Controller
    {
        public void Init()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            Fetka.DAL.MyDbContextInitializer initializer = new Fetka.DAL.MyDbContextInitializer();
            initializer.InitializeDatabase(new CompoundContext());
        }

        public void OrInt()
        {            
            using (CompoundContext c = new CompoundContext())
            {
                c.Database.Create();
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Opis aplikacji";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Kontakt z autorem";

            return View();
        }
    }
}