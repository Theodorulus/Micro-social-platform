using DAW_project.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAW_project.Controllers
{
    public class GroupController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Group
        public ActionResult Index()
        {
            var groups = db.Groups;
            ViewBag.Groups = groups;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            SetAccessRights();
            return View();
        }

        [Authorize]
        public ActionResult New()
        {
            Group group = new Group();
            group.GroupAdmin_Id = User.Identity.GetUserId();
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult New(Group group)
        {
            group.DateCreated = DateTime.Now;
            group.GroupAdmin_Id = User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(group.GroupAdmin_Id);
            try
            {
                if (ModelState.IsValid)
                {
                    group.GroupUsers = new Collection<ApplicationUser>();
                    group.GroupUsers.Add(user);
                    db.Groups.Add(group);
                    db.SaveChanges();
                    TempData["message"] = "Grupul a fost creat cu succes!";
                    return RedirectToAction("Index", "Group");
                }
                else
                {
                    return View(group);
                }
            }
            catch (Exception)
            {
                return View(group);
            }
        }

        public ActionResult Show(int id)
        {
            Group group = db.Groups.Find(id);
            if (TempData.ContainsKey("group_message"))
            {
                ViewBag.GroupMessage = TempData["group_message"];
            }
            ViewBag.Posts = group.GroupPosts;
            SetAccessRights();
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            if (group.GroupUsers.Contains(user))
            {
                ViewBag.esteinGrup = 1;
            }
            else
            {
                ViewBag.esteinGrup = 0;
            }
            
            return View(group);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Show(int id, Post post)
        {
            post.Date = DateTime.Now;
            post.UserId = User.Identity.GetUserId();
            Group group = db.Groups.Find(id);
            post.Group = group;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Posts.Add(post);
                    db.SaveChanges();
                    return Redirect("/Posts/Show/" + post.PostId);
                }
                else
                {
                    ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                    if (group != null)
                    {
                        if (group.GroupUsers.Contains(user))
                        {
                            ViewBag.esteInGrup = 1;
                        }
                        else
                        {
                            ViewBag.esteInGrup = 0;
                        }
                    }
                    ViewBag.Posts = group.GroupPosts;
                    SetAccessRights();
                    return View(group);
                }
            }

            catch (Exception e)
            {
                ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                if (group != null)
                {
                    if (group.GroupUsers.Contains(user))
                    {
                        ViewBag.esteInGrup = 1;
                    }
                    else
                    {
                        ViewBag.esteInGrup = 0;
                    }
                }
                ViewBag.Posts = group.GroupPosts;
                SetAccessRights();
                return View(group);
            }

        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            Group group = db.Groups.Find(id);
            if (group.GroupAdmin_Id == User.Identity.GetUserId()) //|| User.IsInRole("Administrator"))
            {
                return View(group);
            }
            else
            {
                TempData["group_message"] = "Nu puteti sa modificati grupul.";
                return RedirectToAction("Show/" + id);
            }
        }


        [HttpPut]
        [Authorize]
        public ActionResult Edit(int id, Group requestGroup)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Group group = db.Groups.Find(id);
                    if (group.GroupAdmin_Id == User.Identity.GetUserId())// || User.IsInRole("Administrator"))
                    {
                        if (TryUpdateModel(group))
                        {
                            group.GroupName = requestGroup.GroupName;
                            db.SaveChanges();
                            TempData["group_message"] = "Grupul a fost modificat!";
                            return RedirectToAction("Show/" + id);
                        }
                        else
                        {
                            return View(requestGroup);
                        }
                    }
                    else
                    {
                        TempData["group_message"] = "Nu puteti modifica grupul.";
                        return RedirectToAction("Show/" + id);
                    }

                }
                else
                {
                    return View(requestGroup);
                }
            }
            catch (Exception e)
            {
                return View(requestGroup);
            }
        }

        [Authorize]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Group group = db.Groups.Find(id);
            if(group.GroupAdmin_Id == User.Identity.GetUserId() || User.IsInRole("Administrator"))
            {
                /*foreach (Post post in db.Posts.Where(p => p.Group == group))
                {
                    db.Posts.Remove(post);
                }*/
                db.Posts.RemoveRange(group.GroupPosts);
            }
            if (group.GroupAdmin_Id == User.Identity.GetUserId())
            {
                db.Groups.Remove(group);
                db.SaveChanges();
                TempData["message"] = "Grupul a fost sters!";
                return RedirectToAction("Show", "User");
            }
            else if (User.IsInRole("Administrator"))
            {
                string iduser = group.GroupAdmin_Id;
                db.Groups.Remove(group);
                db.SaveChanges();
                TempData["message"] = "Grupul a fost sters!";
                return Redirect("/Notifications/NewGroup/" + iduser);
            }
            else
            {
                TempData["group_message"] = "Nu puteti sa stergeti grupul.";
                return RedirectToAction("Show/" + id);
            }
        }

        [Authorize]
        public ActionResult Join(int id)
        {
            Group group = db.Groups.Find(id);
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            group.GroupUsers.Add(user);
            db.SaveChanges();
            TempData["group_message"] = "Acum sunteti membru in grup.";
            return RedirectToAction("Show/" + id);
        }

        [Authorize]
        public ActionResult Leave(int id)
        {
            Group group = db.Groups.Find(id);
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            group.GroupUsers.Remove(user);
            db.SaveChanges();
            TempData["group_message"] = "Ati parasit grupul.";
            return RedirectToAction("Show/" + id);
        }

        public ActionResult Members(int id)
        {
            Group group = db.Groups.Find(id);
            ViewBag.Users = group.GroupUsers;
            return View(group);
        }

        private void SetAccessRights()
        {
            ViewBag.esteAdmin = User.IsInRole("Administrator");
            ViewBag.utilizatorCurent = User.Identity.GetUserId();
        }
    }
}