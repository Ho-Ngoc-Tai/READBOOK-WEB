using Đồ_án_của_Thái.DTOs;
using Đồ_án_của_Thái.Models;
using Đồ_án_của_Thái.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Đồ_án_của_Thái.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _dbContext;
        public HomeController()
        {
            _dbContext = new ApplicationDbContext();
        }
        
        public ActionResult Index(string searchString)
        {
            var comic = _dbContext.Comics.Include(c => c.Category).ToList();
            var category = _dbContext.Categories.ToList();
            ViewBag.Categories = category;
            var loginUser = User.Identity.GetUserId();
            ViewBag.LoginUser = loginUser;
            foreach (Comic i in comic)
            {
                Follow find = _dbContext.Follows.FirstOrDefault(f => f.FolloweeId == loginUser && f.ComicId == i.Id);
                if (find == null)
                {
                    i.isShowFollow = true;
                }
            }
            var comics = from l in _dbContext.Comics
                         select l;
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                comics = comics.Where(c => c.NameComic.ToLower().Contains(searchString) || c.Author.ToLower().Contains(searchString));
            }
            
            return View(comics);
        }
        
        public ActionResult About()
        {
            var category = _dbContext.Categories.ToList();
            ViewBag.Categories = category;
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            var category = _dbContext.Categories.ToList();
            ViewBag.Categories = category;
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}