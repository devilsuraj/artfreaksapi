﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace artfriks.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ParentId { get; set; }
    }
    public class CategoryView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ParentId { get; set; }
    }
}
