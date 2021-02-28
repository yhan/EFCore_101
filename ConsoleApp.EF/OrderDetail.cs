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
        public Order Order { get; set; }
    }

    public class Order
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime CreatedAt { get; set; }

        public Order(int customerId, int employeeId, DateTime createdAt)
        {
            CustomerID = customerId;
            EmployeeID = employeeId;
            CreatedAt = createdAt;
        }
        public Order()
        {
            
        }

        public List<OrderDetail> OrderDetails { get; set; }
    }

}