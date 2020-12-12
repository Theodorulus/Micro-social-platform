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
            var posts = db.Posts;
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
                    return RedirectToAction("Index");
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
            ViewBag.Post = post;
            
            if (TempData.ContainsKey("message_post"))
            {
                ViewBag.PostMessage = TempData["message_post"];
            }
            if (TempData.ContainsKey("message_comm"))
            {
                ViewBag.CommMessage = TempData["message_comm"];
            }
            SetAccessRights();
            return View(post);

        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            Post post = db.Posts.Find(id);
            if (post.UserId == User.Identity.GetUserId()) //|| User.IsInRole("Administrator"))
            {
                return View(post);
            }
            else
            {
                TempData["message_post"] = "Nu puteti sa editati postarea.";
                return RedirectToAction("Show/" + id);
            }
        }

        [Authorize]
        [HttpPut]
        public ActionResult Edit(int id, Post requestPost)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    requestPost.Date = DateTime.Now;
                    Post post = db.Posts.Find(id);
                    if (post.UserId == User.Identity.GetUserId()) //|| User.IsInRole("Administrator"))
                    {
                        if (TryUpdateModel(post))
                        {
                            post.Text = requestPost.Text;
                            post.Date = requestPost.Date;
                            post.UserId = requestPost.UserId;
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
            if (post.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
            {
                db.Posts.Remove(post);
                db.SaveChanges();
                TempData["message"] = "Postarea a fost stearsa!";
                return RedirectToAction("Index");
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