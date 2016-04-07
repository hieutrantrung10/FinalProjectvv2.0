using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

namespace Model.Dao
{
    public class OrderDetailDao
    {
        OnlineShopDbContext db = null;

        public OrderDetailDao()
        {
            db = new OnlineShopDbContext();
        }

        public bool Insert(OrderDetail details)
        {
            try
            {
                db.OrderDetails.Add(details);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            
        }
    }
}
