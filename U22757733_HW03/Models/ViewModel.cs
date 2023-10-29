using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace U22757733_HW03.Models
{
    public class ViewModel
    {
        public IEnumerable<author> authors { get; set; }

        public IEnumerable<book> books { get; set; }

        public IEnumerable<borrow> borrows { get; set; }

        public IEnumerable<student> students { get; set; }

        public IEnumerable<type> types { get; set; }

           

    }

}
