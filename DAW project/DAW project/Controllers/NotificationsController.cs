using DAW_project.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAW_project.Controllers
{
    public class NotificationsController : Controller
    {
        public ApplicationDbContext db = ApplicationDbContext.Create();
        // GET: Notification
        [Authorize]
        public ActionResult Index()
        {
            string id = User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(id);
            var notifs = user.UserNotifications;
            ViewBag.Notifications = notifs;
            List<Friendship> requests= db.Friendships.Where(i => i.User2.Id == id && i.Accepted == false).ToList();
            ViewBag.Requests = requests;
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult NewPost(string id)
        {
            Notification notif = new Notification();
            notif.User_Id = id;
            notif.Admin_Id = User.Identity.GetUserId();
            notif.Type = "Post";
            return View(notif);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult NewPost(Notification notif)
        {
            notif.RequestTime = DateTime.Now;
            
            try
            {
                if (ModelState.IsValid)
                {
                    db.Notifications.Add(notif);
                    db.SaveChanges();
                    return Redirect("/User/Show/" + notif.User_Id);
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

        [Authorize(Roles = "Administrator")]
        public ActionResult NewComment(string id)
        {
            Notification notif = new Notification();
            notif.User_Id = id;
            notif.Admin_Id = User.Identity.GetUserId();
            notif.Type = "Comment";
            return View(notif);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult NewComment(Notification notif)
        {
            notif.RequestTime = DateTime.Now;
            
            try
            {
                if (ModelState.IsValid)
                {
                    db.Notifications.Add(notif);
                    db.SaveChanges();
                    return Redirect("/User/Show/" + notif.User_Id);
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

        [Authorize(Roles = "Administrator")]
        public ActionResult NewGroup(string id)
        {
            Notification notif = new Notification();
            notif.User_Id = id;
            notif.Admin_Id = User.Identity.GetUserId();
            notif.Type = "Group";
            return View(notif);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult NewGroup(Notification notif)
        {
            notif.RequestTime = DateTime.Now;

            try
            {
                if (ModelState.IsValid)
                {
                    db.Notifications.Add(notif);
                    db.SaveChanges();
                    return Redirect("/User/Show/" + notif.User_Id);
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