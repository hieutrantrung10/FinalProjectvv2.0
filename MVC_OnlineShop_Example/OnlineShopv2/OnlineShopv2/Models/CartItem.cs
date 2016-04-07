using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.EF;

namespace OnlineShopv2.Models
{
    [Serializable]
    public class CartItem
    {
        
        public Product Product { set; get; }
        public int Quantity { set; get; }
        
    }
}