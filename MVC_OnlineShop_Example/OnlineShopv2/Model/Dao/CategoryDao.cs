using Common;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Model.Dao
{
    public class CategoryDao
    {
        OnlineShopDbContext db = null;
        public CategoryDao()
        {
            db = new OnlineShopDbContext();
        }
        public List<Category> ListAll()
        {
            return db.Categories.Where(x => x.Status == true).ToList();
        }
        public IEnumerable<Category> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Category> model = db.Categories;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        public long Insert(Category category)
        {
            db.Categories.Add(category);
            db.SaveChanges();

            return category.ID;
        }
        public bool Update(Category category)
        {
            try
            {
                if (string.IsNullOrEmpty(category.MetaTitle))
                {
                    category.MetaTitle = StringHelper.ToUnsignString(category.Name);
                }
                var cate = db.Categories.Find(category.ID);
                cate.Name = category.Name;
                cate.MetaTitle = category.MetaTitle;
                cate.ParentID = category.ParentID;
                cate.DisplayOrder = category.DisplayOrder;
                cate.SeoTitle = category.SeoTitle;                
                cate.ModifiedBy = category.ModifiedBy;
                cate.ModifiedDate = DateTime.Now;
                cate.MetaKeywords = category.MetaKeywords;
                cate.MetaDescriptions = category.MetaDescriptions;
                cate.Status = category.Status;
               
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
        public bool Delete(long id)
        {
            try
            {
                var cate = db.Categories.Find(id);
                db.Categories.Remove(cate);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public ProductCategory ViewDetail(long id)
        {
            return db.ProductCategories.Find(id);
        }

        public Category GetByID(long id)
        {
            return db.Categories.Find(id);
        }

    }
}
