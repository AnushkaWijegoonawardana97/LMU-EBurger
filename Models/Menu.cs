//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LMU_EBurger.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    public partial class Menu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Menu()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }
    
        public int MenuID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Images { get; set; }
        public string Price { get; set; }
        public Nullable<bool> Availability { get; set; }
        public string PrepTime { get; set; }
        public int CategoryID { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }

        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderItem> OrderItems { get; set; }

    }

    public enum AvailabilityOptions
    {
        Available,
        UnAvailable
    }
}
