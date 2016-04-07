using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class OrderViewModel
    {
        public long OrderID { set; get; }
        public long ProductID { set; get; }
        public int Quantity { set; get; }
        public string ProductName { set; get; }
        public decimal Price { set; get; }
        public string ShipEmail { set; get; }
        public string ShipName { set; get; }
        public int Status { set; get; }
        
    }
}
