using DAW_project.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DAW_project.Controllers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NoDirectAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.UrlReferrer == null ||
                        filterContext.HttpContext.Request.Url.Host != filterContext.HttpContext.Request.UrlReferrer.Host)
            {
                filterContext.Result = new RedirectToRouteResult(new
                               RouteValueDictionary(new { controller = "Home", action = "Index", area = "" }));
            }
        }
    }

    public class UserController : Controller
    {
        private readonly Models.ApplicationDbContext db = new Models.ApplicationDbContext();

        // GET: User
        public ActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
        }

        public ActionResult Show(string id = "0")
        {
            ViewBag.ButtonId = id;
            if (id == "0") { id = User.Identity.GetUserId(); }
            string myId = User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(id);
            ViewBag.AlreadySent = db.Friendships.Where(i => i.User1.Id ==myId && i.User2.Id == id).Count();
            if (user.Privacy == 0 || id == User.Identity.GetUserId() || User.IsInRole("Administrator"))
            {
                ViewBag.Posts = user.UserPosts;
                return View(user);
            }
            else
            {
                TempData["message"] = "Profilul este privat";
                return RedirectToAction("Index");
            }
        }
        //[NoDirectAccess]
        public ActionResult ReqFriend(string id)
        {
            var myId = User.Identity.GetUserId();
            if (id == myId)
            {
                TempData["message"] = "Nu va puteti adauga ca prieten";
                return RedirectToAction("Index");
            }
            if (db.Friendships.Where(i => i.User1.Id == id && i.User2.Id== myId || i.User2.Id == id && i.User1.Id == myId).Count() != 0)
            {
                TempData["message"] = "O cerere este deja in asteptare sau sunteti deja prieteni";
                return RedirectToAction("Index");
            }
            Friendship friendship = new Friendship
            {
                User1 = db.Users.Find(myId),
                User2 = db.Users.Find(id),
                RequestTime = DateTime.Now,
                Accepted = false
            };

            db.Friendships.Add(friendship);
            db.SaveChanges();

            return RedirectToAction("Show", new { id });
        }
        public ActionResult AddFriend(string id)
        {
            var myId = User.Identity.GetUserId();

            Friendship request = db.Friendships.Where(i => i.User1.Id == id && i.User2.Id == myId).First();
            request.Accepted = true;

            db.SaveChanges();

            return RedirectToAction("Index", "Notifications");
        }
        public ActionResult DelFriend(string id)
        {
            var myId = User.Identity.GetUserId();

            Friendship request = db.Friendships.Where(i => i.User1.Id == myId && i.User2.Id == id).First();
            db.Friendships.Remove(request);
            
            db.SaveChanges();

            return RedirectToAction("Index", "Notifications");
        }
        /*
        public ActionResult New()
        {
            User user = new User();
            return View(user);
        }
       
        [HttpPost]
        public ActionResult New(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    TempData["message"] = "Contul a fost creat cu succes";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(user);
                }
            }
            catch (Exception)
            {
                return View(user);
            }

        }
        
        public ActionResult Edit(int id)
        {
            User user = db.Users.Find(id);
            return View(user);
        }
        
        [HttpPut]
        public ActionResult Edit(int id, User requestUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = db.Users.Find(id);

                    if (TryUpdateModel(user))
                    {
                        user.Email = requestUser.Email;
                        if (requestUser.Password != "")
                        {
                            user.Password = requestUser.Password;
                        }
                        user.FirstName = requestUser.FirstName;
                        user.LastName = requestUser.LastName;
                        user.PhoneNumber = requestUser.PhoneNumber;
                        if (requestUser.BirthDate != user.BirthDate)
                        {
                            user.BirthDate = requestUser.BirthDate;
                        }
                        user.Privacy = requestUser.Privacy;
                        db.SaveChanges();
                        TempData["message"] = "Modificarile au fost facute cu succes!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(requestUser);
                    }
                }
                else
                {
                    return View(requestUser);
                }
            }
            catch (Exception e)
            {
                return View(requestUser);
            }
        }
        
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            TempData["message"] = "Profil sters cu succes";
            return RedirectToAction("Index");
        }
        */
    }
}