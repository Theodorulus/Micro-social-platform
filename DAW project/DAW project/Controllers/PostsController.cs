using DAW_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAW_project.Controllers
{
    public class PostsController : Controller
    {
        private Models.ApplicationDbContext db = new Models.ApplicationDbContext();

        // GET: Post
        public ActionResult Index()
        {
            var posts = db.Posts;
            ViewBag.Posts = posts;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
        }

        public ActionResult New()
        {
            Post post = new Post();
            return View();
        }

        [HttpPost]
        public ActionResult New(Post post)
        {
            post.Date = DateTime.Now;
            try
            {
                if (ModelState.IsValid)
                {
                    post.UserId = 1;
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
            return View(post);

        }

        public ActionResult Edit(int id)
        {

            Post post = db.Posts.Find(id);

            return View(post);
        }

        [HttpPut]
        public ActionResult Edit(int id, Post requestPost)
        {
            try
            {
                if (ModelState.IsValid)
                {
                requestPost.Date = DateTime.Now;
                    Post post = db.Posts.Find(id);
                    requestPost.UserId = 5;
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
                    return View(requestPost);
                }
            }
            catch (Exception e)
            {
                return View(requestPost);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            TempData["message"] = "Postarea a fost stearsa!";
            return RedirectToAction("Index");
        }

    }
}