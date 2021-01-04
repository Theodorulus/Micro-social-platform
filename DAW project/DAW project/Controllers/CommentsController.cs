using DAW_project.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAW_project.Controllers 
{
    public class CommentsController : Controller
    {
        private Models.ApplicationDbContext db = new Models.ApplicationDbContext();

        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }

        /*[Authorize]
        [HttpPost]
        public ActionResult New(Comment comm)
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
                    return View(comm);
                }
            }

            catch (Exception e)
            {
                return Redirect("/Posts/Show/" + comm.PostId);
            }

        }*/

        [Authorize]
        public ActionResult Edit(int id)
        {
            Comment comm = db.Comments.Find(id);
            if (comm.UserId == User.Identity.GetUserId()) //|| User.IsInRole("Administrator"))
            {
                ViewBag.Comment = comm;
                return View(comm);
            }
            else
            {
                TempData["message_comm"] = "Nu puteti sa editati comentariul.";
                return Redirect("/Posts/Show/" + comm.PostId);
            }
        }

        [Authorize]
        [HttpPut]
        public ActionResult Edit(int id, Comment requestComment)
        {
            try
            {
                Comment comm = db.Comments.Find(id);
                if (comm.UserId == User.Identity.GetUserId()) //|| User.IsInRole("Administrator"))
                {
                    if (TryUpdateModel(comm))
                    {
                        comm.Text = requestComment.Text;
                        db.SaveChanges();
                        TempData["message_comm"] = "Comentariul a fost modificat!";
                    }
                    return Redirect("/Posts/Show/" + comm.PostId);
                }
                else
                {
                    TempData["message_comm"] = "Nu puteti sa editati comentariul.";
                    return Redirect("/Posts/Show/" + comm.PostId);
                }
            }
            catch (Exception e)
            {
                return View(requestComment);
            }

        }
        [Authorize]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Comment comm = db.Comments.Find(id);
            Post post = db.Posts.Find(comm.PostId);
            if (comm.UserId == User.Identity.GetUserId())
            {
                db.Comments.Remove(comm);
                db.SaveChanges();
                TempData["message_comm"] = "Comentariul a fost sters!";
                return Redirect("/Posts/Show/" + post.PostId);
            }
            else if (post.UserId == User.Identity.GetUserId())
            {
                db.Comments.Remove(comm);
                db.SaveChanges();
                TempData["message_comm"] = "Comentariul a fost sters!";
                return Redirect("/Posts/Show/" + post.PostId);
            }
            else if (User.IsInRole("Administrator"))
            {
                string iduser = comm.UserId;
                db.Comments.Remove(comm);
                db.SaveChanges();
                TempData["message"] = "Comentariul a fost sters!";
                return Redirect("/Notifications/NewComment/" + iduser);
            }
            else
            {
                TempData["message_comm"] = "Nu puteti sa stergeti comentariul.";
                return Redirect("/Posts/Show/" + comm.PostId);
            }
        }
    }
}