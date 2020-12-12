using DAW_project.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAW_project.Controllers
{
    public class NotificationController : Controller
    {
        public ApplicationDbContext db = ApplicationDbContext.Create();
        // GET: Notification
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult New(int id)
        {
            Notification notif = new Notification();
            notif.Post_Id = id;
            notif.Admin_Id = User.Identity.GetUserId();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult New(Notification notif)
        {
            notif.RequestTime = DateTime.Now;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Notifications.Add(notif);
                    db.SaveChanges();
                    return Redirect("/Posts/Index");
                }
                else
                {
                    return View(notif);
                }
            }
            catch (Exception)
            {
                return View(notif);
            }
        }
    }
}