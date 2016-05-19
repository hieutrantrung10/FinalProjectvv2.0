using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using Model.EF;

namespace Model.Dao
{
    public class OrderDao
    {
        private OnlineShopDbContext db = null;

        public OrderDao()
        {
            db = new OnlineShopDbContext();
        }
        public long Insert(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
            return order.ID;
        }
        public IEnumerable<Order> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Order> model = db.Orders;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x =>x.ShipName.Contains(searchString));
            }            
            //UPDATE!!!!//Sử dụng thêm câu lệnh if nếu muốn lọc thêm theo các trường khác nếu muốn , làm tương tự như cấu trúc if ở trên...
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public int ChangeStatus(long id)
        {
            var order = db.Orders.Find(id);
            switch (order.Status)
            {
                case 0:
                    order.Status = 1;
                    break;
                case 1:
                    order.Status = 2;
                    break;
                case 2:
                    order.Status = 3;
                    break;
                case 3:
                    order.Status = 0;
                    break;
                default:
                    order.Status = 0;
                    break;
            }
            db.SaveChanges();
            return (int) order.Status;
        }

        public bool Delete(long id)
        {
            try
            {
                var order = db.Orders.Find(id);
                db.Orders.Remove(order);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
