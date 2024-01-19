using Đồ_án_của_Thái.Models;
using Đồ_án_của_Thái.ViewModels;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Đồ_án_của_Thái.Controllers
{
    public class ComicsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public ComicsController()
        {
            _dbContext = new ApplicationDbContext();
        }
        
        public string ProcessUpload(HttpPostedFileBase file)
        {
            var category = _dbContext.Categories.ToList();
            ViewBag.Categories = category;
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
            return "/Content/images/" + file.FileName;
        }
        public ActionResult Detail(int id)
        {
            var category = _dbContext.Categories.ToList();
            ViewBag.Categories = category;
            var comic = _dbContext.Comics.Include(c => c.Category).SingleOrDefault(c => c.Id == id);
            if (comic == null)
            {
                return HttpNotFound();
            }
            var chapters = _dbContext.Chapters.Where(c => c.ComicId == id).ToList();
            var comments = _dbContext.Comments.Include(c => c.NameUser).Where(c => c.ComicId == id).ToList();
            var viewModel = new ComicDetailViewModel
            {
                Comic = comic,
                Chapters = chapters,
                Comments = comments,
                Url = Url.Action("Read", new { id = comic.Id })
            };
            
            return View(viewModel);
        }
        
        public ActionResult Read(int id, int? page)
        {
            var category = _dbContext.Categories.ToList();
            ViewBag.Categories = category;
            int pageSize = 1;
            int pageNumber = (page ?? 1);
            var read = _dbContext.Chapters.Where(c => c.ComicId == id).ToList();
            var pagedChaps = read.ToPagedList(pageNumber, pageSize);
            var userId = User.Identity.GetUserId();
            ViewBag.LoginUser = userId;
            foreach (Chapter i in read)
            {
                Save find = _dbContext.Saves.FirstOrDefault(s => s.NameUserId == userId && s.ChapterId == i.Id);
                if (find == null)
                {
                    i.isShowSave = true;
                }
            }
            return View(pagedChaps);
        }
        
        [Authorize]
        public ActionResult Following()
        {
            var category = _dbContext.Categories.ToList();
            ViewBag.Categories = category;
            var userId = User.Identity.GetUserId();
            var comics = _dbContext.Follows
                .Where(f => f.FolloweeId == userId)
                .Select(f => f.Comic)
                .Include(f => f.Category)
                .ToList();
            return View(comics);
        }
        [Authorize]
        public ActionResult Saving()
        {
            var category = _dbContext.Categories.ToList();
            ViewBag.Categories = category;
            var userId = User.Identity.GetUserId();
            var saves = _dbContext.Saves
                .Where(s => s.NameUserId == userId)
                .Select(s => s.Chapters)
                .Include(ch => ch.Comic)
                .ToList();
            return View(saves);
        }
        [HttpPost]
        public ActionResult Comment(int id, ComicDetailViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                
                var newComment = new Comment
                {
                    ComicId = id,
                    BinhLuan = model.BinhLuan,
                    NameUserId = User.Identity.GetUserId()
                };
                _dbContext.Comments.Add(newComment);
                _dbContext.SaveChanges();
                return RedirectToAction("Detail", "Comics", new { id = id });
            }
            return View("Index", "Home");
        }
        public ActionResult TheLoai(int? id)
        {
            var category = _dbContext.Categories.ToList();
            ViewBag.Categories = category;
            var comic = _dbContext.Comics.Include(c => c.Category).Where(c => c.CategoryId == id).ToList();
            
            return View(comic);
        }
    }
}