using Đồ_án_của_Thái.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Đồ_án_của_Thái.Areas.Admin.ViewModelsAdmin
{
    public class ComicViewModelsAdmin
    {
        public int Comic { get; set; }
        public IEnumerable<Comic> Comics { get; set; }
        public int CategoryId { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public string NameComic { get; set; }
        public string Author { get; set; }
        public int Category { get; set; }
        public string Title { get; set; }
        public string Picture { get; set; }
        public Comic Comicss { get; set; }
        public string CategoryName { get; set; }
        public int Trang { get; set; }
        public string PictureChap { get; set; }
    }
}