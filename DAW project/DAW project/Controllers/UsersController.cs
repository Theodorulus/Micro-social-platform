using DAW_project.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
//using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAW_project.Controllers
{
    public class UsersController : Controller
    {
        private Models.ApplicationDbContext db = new Models.ApplicationDbContext();

        // GET: User
        public ActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
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
        */
        public ActionResult Show(string id = "0")
        {
            if (id == "0")
            {
                id = User.Identity.GetUserId();
            }
            ApplicationUser user = db.Users.Find(id);
            ViewBag.ApplicationUser = user;
            return View(user);
        }
        /*
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