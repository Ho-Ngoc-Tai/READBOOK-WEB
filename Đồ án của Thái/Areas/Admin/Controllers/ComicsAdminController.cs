using Đồ_án_của_Thái.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Đồ_án_của_Thái.Areas.Admin.ViewModelsAdmin;
using PagedList;
using System.Data.Entity;


namespace Đồ_án_của_Thái.Areas.Admin.Controllers
{

    public class ComicsAdminController : Controller
    {
        private readonly ApplicationDbContext context;
        public ComicsAdminController()
        {
            context = new ApplicationDbContext();
        }
        public string ProcessUpload(HttpPostedFileBase file)
        {
            var categorys = context.Categories.ToList();
            ViewBag.Categories = categorys;
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
            return "/Content/images/" + file.FileName;
        }
        [Authorize]
        public ActionResult CreateAdmin()
        {
            var categorys = context.Categories.ToList();
            ViewBag.Categories = categorys;
            var viewModel = new ComicViewModelsAdmin
            {
                Categories = context.Categories.ToList()
            };
            return View(viewModel);
        }
        [Authorize]
        [HttpPost]
        public ActionResult CreateAdmin(ComicViewModelsAdmin comics)
        {
            var comic = new Comic
            {
                NameComic = comics.NameComic,
                Author = comics.Author,
                CategoryId = comics.Category,
                Title = comics.Title,
                Picture = comics.Picture,
            };
            context.Comics.Add(comic);
            context.SaveChanges();

            return RedirectToAction("IndexAdmin", "HomeAdmin");
        }
        public ActionResult EditAdmin(int id)
        {
            var categories = context.Categories.ToList();
            ViewBag.Categories = categories;
            ViewBag.Categori = new SelectList(categories, "Id", "Name");

            var comic = context.Comics.SingleOrDefault(c => c.Id == id);
            ViewBag.SelectedCategory = comic.CategoryId;

            return View(comic);
        }
        [Authorize]
        [HttpPost]
        public ActionResult EditAdmin(Comic comics)
        {
            if (!ModelState.IsValid)
            {
                return View(comics);
            }
            var comic = context.Comics.SingleOrDefault(c => c.Id == comics.Id);
            comic.NameComic = comics.NameComic;
            comic.Author = comics.Author;
            comic.CategoryId = comics.CategoryId;
            comic.Title = comics.Title;
            comic.Picture = comics.Picture;
            context.SaveChanges();
            return RedirectToAction("IndexAdmin", "HomeAdmin");
        }
        [Authorize]
        public ActionResult DeleteAdmin(int id)
        {
            var categorys = context.Categories.ToList();
            ViewBag.Categories = categorys;
            Comic comics = context.Comics.Find(id);
            if (comics == null)
            {
                return HttpNotFound();
            }
            return View(comics);
        }


        [HttpPost, ActionName("DeleteAdmin")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int id)
        {
            var categorys = context.Categories.ToList();
            ViewBag.Categories = categorys;
            using (var db = new ApplicationDbContext())
            {
                Comic comics = context.Comics.Find(id);
                context.Comics.Remove(comics);
                context.SaveChanges();
            }
            return RedirectToAction("IndexAdmin", "HomeAdmin");
        }
        public ActionResult DetailAdmin(int id)
        {
            var categorys = context.Categories.ToList();
            ViewBag.Categories = categorys;
            var comic = context.Comics.SingleOrDefault(c => c.Id == id);
            if (comic == null)
            {
                return HttpNotFound();
            }
            var chapters = context.Chapters.Where(c => c.ComicId == id).ToList();
            var comments = context.Comments.Where(c => c.ComicId == id).ToList();
            var viewModelAD = new ComicDetailViewModelsAdmin
            {
                Comic = comic,
                Chapters = chapters,
                Comments = comments,
                Url = Url.Action("ReadAdmin", new { id = comic.Id })
            };
            return View(viewModelAD);
        }
        public ActionResult ReadAdmin(int id, int? page)
        {
            var categorys = context.Categories.ToList();
            ViewBag.Categories = categorys;
            int pageSize = 1;
            int pageNumber = (page ?? 1);
            var read = context.Chapters.Where(c => c.ComicId == id).ToList();
            var pagedChaps = read.ToPagedList(pageNumber, pageSize);
            return View(pagedChaps);
        }
        [Authorize]
        public ActionResult CreateChapterAdmin()
        {
            var categorys = context.Categories.ToList();
            ViewBag.Categories = categorys;
            var viewModel = new ComicViewModelsAdmin()
            {
                Comics = context.Comics.ToList(),
            };
            return View(viewModel);
        }
        [Authorize]
        [HttpPost]
        public ActionResult CreateChapterAdmin(ComicViewModelsAdmin viewChap)
        {
            var categorys = context.Categories.ToList();
            ViewBag.Categories = categorys;
            var chapter = new Chapter
            {
                ComicId = viewChap.Comic,
                Trang = viewChap.Trang,
                PictureChap = viewChap.PictureChap,
            };
            context.Chapters.Add(chapter);
            context.SaveChanges();

            return RedirectToAction("IndexAdmin", "HomeAdmin");
        }
        [Authorize]
        public ActionResult CategoryAdmin()
        {
            var categorys = context.Categories.ToList();
            ViewBag.Categories = categorys;
            return View();
        }
        [HttpPost]
        [Authorize]
        public ActionResult CategoryAdmin(Category model)
        {
            var category = new Category
            {
                Name = model.Name,
            };
            context.Categories.Add(category);
            context.SaveChanges();
            return RedirectToAction("CategoryAdmin", "ComicsAdmin");
        }
        public ActionResult TheLoaiAdmin(int? id)
        {
            var category = context.Categories.ToList();
            ViewBag.Categories = category;
            var comic = context.Comics.Include(c => c.Category).Where(c => c.CategoryId == id).ToList();

            return View(comic);
        }
    }
}