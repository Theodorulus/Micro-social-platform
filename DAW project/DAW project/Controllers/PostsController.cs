using DAW_project.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAW_project.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Post
        public ActionResult Index()
        {
            var posts = db.Posts.Where(p => p.Group == null);// && p.User.Privacy == 0);
            ViewBag.Posts = posts;
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
            Post post = new Post();
            post.UserId = User.Identity.GetUserId();
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult New(Post post)
        {
            post.Date = DateTime.Now;
            post.UserId = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Posts.Add(post);
                    db.SaveChanges();
                    TempData["message"] = "Postarea a fost creata cu succes!";
                    return RedirectToAction("Show", "User");
                }
                else
                {
                    return View(post);
                }
            }
            catch (Exception)
            {
                return View(post);
            }
        }

        public ActionResult Show(int id)
        {
            Post post = db.Posts.Find(id);
            //ViewBag.Post = post;
            
            if (TempData.ContainsKey("message_post"))
            {
                ViewBag.PostMessage = TempData["message_post"];
            }
            if (TempData.ContainsKey("message_comm"))
            {
                ViewBag.CommMessage = TempData["message_comm"];
            }
            SetAccessRights();
            if(post.Group != null)
            {
                ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                if (post.Group.GroupUsers.Contains(user))
                {
                    ViewBag.esteInGrup = 1;
                }
                else
                {
                    ViewBag.esteInGrup = 0;
                }
            }
            return View(post);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Show(Comment comm)
        {
            Post p = db.Posts.Find(comm.PostId);
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            if (p.Group == null || p.Group.GroupUsers.Contains(user))
            {
                comm.Date = DateTime.Now;
                comm.UserId = User.Identity.GetUserId();
                try
                {
                    if (ModelState.IsValid)
                    {
                        db.Comments.Add(comm);
                        db.SaveChanges();
                        return Redirect("/Posts/Show/" + comm.PostId);
                    }

                    else
                    {
                        if (p.Group != null)
                        {
                            if (p.Group.GroupUsers.Contains(user))
                            {
                                ViewBag.esteInGrup = 1;
                            }
                            else
                            {
                                ViewBag.esteInGrup = 0;
                            }
                        }
                        SetAccessRights();
                        return View(p);
                    }

                }

                catch (Exception e)
                {
                    if(p.Group!= null)
                    {
                        if (p.Group.GroupUsers.Contains(user))
                        {
                            ViewBag.esteInGrup = 1;
                        }
                        else
                        {
                            ViewBag.esteInGrup = 0;
                        }
                    }
                    
                    SetAccessRights();
                    return View(p);
                }
            }
            else
            {
                SetAccessRights();
                return View(p);
            }

        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            Post post = db.Posts.Find(id);
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            if ((post.UserId == User.Identity.GetUserId() && post.Group == null) || post.UserId == User.Identity.GetUserId() && post.Group.GroupUsers.Contains(user)) //|| User.IsInRole("Administrator"))
            {
                return View(post);
            }
            else
            {
                TempData["message_post"] = "Nu puteti sa editati postarea.";
                return RedirectToAction("Show/" + id);
            }
        }

        
        [HttpPut]
        [Authorize]
        public ActionResult Edit(int id, Post requestPost)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    requestPost.Date = DateTime.Now;
                    Post post = db.Posts.Find(id);
                    if (post.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
                    {
                        if (TryUpdateModel(post))
                        {
                            post.Text = requestPost.Text;
                            post.Date = requestPost.Date;
                            db.SaveChanges();
                            TempData["message_post"] = "Postarea a fost modificata!";
                            return RedirectToAction("Show/" + id);
                        }
                        else
                        {
                            return View(requestPost);
                        }
                    }
                    else
                    {
                        TempData["message_post"] = "Nu puteti sa editati postarea.";
                        return RedirectToAction("Show/" + id);
                    }
                    
                }
                else
                {
                    return View(requestPost);
                }
            }
            catch (Exception e)
            {
                return View(requestPost);
            }
        }

        [Authorize]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Post post = db.Posts.Find(id);
            if (post.UserId == User.Identity.GetUserId())
            {
                db.Posts.Remove(post);
                db.SaveChanges();
                TempData["message"] = "Postarea a fost stearsa!";
                return RedirectToAction("Show", "User");
            }
            else if (User.IsInRole("Administrator"))
            {
                string iduser = post.UserId;
                db.Posts.Remove(post);
                db.SaveChanges();
                TempData["message"] = "Postarea a fost stearsa!";
                return Redirect("/Notifications/NewPost/" + iduser);
            }
            else
            {
                TempData["message_post"] = "Nu puteti sa stergeti postarea.";
                return RedirectToAction("Show/" + id);
            }
        }

        private void SetAccessRights()
        {
            ViewBag.esteAdmin = User.IsInRole("Administrator");
            ViewBag.utilizatorCurent = User.Identity.GetUserId();
        }

    }
}