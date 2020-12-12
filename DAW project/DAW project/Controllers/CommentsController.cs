using DAW_project.Models;
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

        [HttpPost]
        public ActionResult New(Comment comm)
        {
            comm.Date = DateTime.Now;
            try
            {
                if (ModelState.IsValid)
                {
                    comm.UserId = 1;
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

        }

        public ActionResult Edit(int id)
        {
            Comment comm = db.Comments.Find(id);
            ViewBag.Comment = comm;
            return View(comm);
        }

        [HttpPut]
        public ActionResult Edit(int id, Comment requestComment)
        {
            try
            {
                Comment comm = db.Comments.Find(id);
                if (TryUpdateModel(comm))
                {
                    comm.Text = requestComment.Text;
                    db.SaveChanges();
                    TempData["message_comm"] = "Comentariul a fost modificat!";
                }
                return Redirect("/Posts/Show/" + comm.PostId);
            }
            catch (Exception e)
            {
                return View(requestComment);
            }

        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Comment comm = db.Comments.Find(id);
            db.Comments.Remove(comm);
            db.SaveChanges();
            TempData["message_comm"] = "Comentariul a fost sters!";
            return Redirect("/Posts/Show/" + comm.PostId);
        }
    }
}