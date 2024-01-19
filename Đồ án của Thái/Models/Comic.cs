﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace Đồ_án_của_Thái.Models
{
    public class Comic
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string NameComic { get; set; }
        [Required]
        [StringLength(255)]
        public string Author { get; set; }
        [Required]
        [StringLength(1000)]
        public string Title { get; set; }
        public Category Category { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        [StringLength(255)]
        public string Picture { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        
        public bool isShowFollow = false;
    }
}