using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Model.EF;
using PagedList;

namespace Model.Dao
{
    public class ProductCategoryDao
    {
        OnlineShopDbContext db = null;

        public ProductCategoryDao()
        {
            db = new OnlineShopDbContext();
        }

        public List<ProductCategory> ListAll()
        {
            return db.ProductCategories.Where(x => x.Status == true).OrderBy(x=>x.DisplayOrder).ToList();
        }

        public IEnumerable<ProductCategory> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<ProductCategory> model = db.ProductCategories;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        public long Insert(ProductCategory pcategory)
        {
            db.ProductCategories.Add(pcategory);
            db.SaveChanges();

            return pcategory.ID;
        }
        public bool Update(ProductCategory pcategory)
        {
            try
            {
                if (string.IsNullOrEmpty(pcategory.MetaTitle))
                {
                    pcategory.MetaTitle = StringHelper.ToUnsignString(pcategory.Name);
                }
                var cate = db.ProductCategories.Find(pcategory.ID);
                cate.Name = pcategory.Name;
                cate.MetaTitle = pcategory.MetaTitle;
                cate.ParentID = pcategory.ParentID;
                cate.DisplayOrder = pcategory.DisplayOrder;
                cate.SeoTitle = pcategory.SeoTitle;
                cate.ModifiedBy = pcategory.ModifiedBy;
                cate.ModifiedDate = DateTime.Now;
                cate.MetaKeywords = pcategory.MetaKeywords;
                cate.MetaDescriptions = pcategory.MetaDescriptions;
                cate.Status = pcategory.Status;

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
                var cate = db.ProductCategories.Find(id);
                db.ProductCategories.Remove(cate);
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

        
    }
}
