//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace U22757733_HW03.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class type
    {
        public type()
        {
            this.books = new HashSet<book>();
        }
    
        public int typeId { get; set; }
        public string name { get; set; }
    
        public virtual ICollection<book> books { get; set; }
    }
}
