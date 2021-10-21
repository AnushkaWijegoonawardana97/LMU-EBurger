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
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.OrderDeliveries = new HashSet<OrderDelivery>();
            this.OrderItems = new HashSet<OrderItem>();
        }
    
        public int OrderID { get; set; }
        public string OrderType { get; set; }
        public string CreateAt { get; set; }
        public string SubTotal { get; set; }
        public string DeliveryFee { get; set; }
        public string Total { get; set; }
        public string OrderStatus { get; set; }
        public int CustomerID { get; set; }
        public string MenuItems { get; set; }

        public string FullAddress { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderDelivery> OrderDeliveries { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
