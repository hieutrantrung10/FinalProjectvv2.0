using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using PagedList;

namespace Model.Dao
{
    public class FeedbackDao
    {
        OnlineShopDbContext db = null;

        public FeedbackDao()
        {
            db = new OnlineShopDbContext();
        }

        public IEnumerable<Feedback> ListAllPaging(int page, int pageSize)
        {
            IQueryable<Feedback> model = db.Feedbacks;
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        public bool Delete(long id)
        {
            try
            {
                var feedback = db.Feedbacks.Find(id);
                db.Feedbacks.Remove(feedback);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Feedback GetById(long id)
        {
            return db.Feedbacks.SingleOrDefault(x => x.ID == id);
        }
    }
}
