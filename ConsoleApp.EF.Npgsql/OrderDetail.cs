using System;
using System.Collections.Generic;

namespace ConsoleApp.EF
{
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        
        public virtual Order Order { get; set; }
    }

    public class Order
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime OrderDate { get; set; }
        
        public virtual List<OrderDetail> OrderDetails { get; set; }
    }

}