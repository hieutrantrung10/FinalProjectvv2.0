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
    public class ContentDao
    {
        OnlineShopDbContext db = null;
        public ContentDao()
        {
            db = new OnlineShopDbContext();
        }
        public Content GetByID(long id)
        {
            return db.Contents.Find(id);
        }
        //test trả về danh sách các bài viết
        public IEnumerable<Content> ListAllPaging(string searchString,int page, int pageSize)
        {
            IQueryable<Content> model = db.Contents;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.CreatedBy.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        //test thêm mới một tin tức
        public long Insert(Content content)
        {
            db.Contents.Add(content);
            db.SaveChanges();

            return content.ID; 
        }

        public bool Update(Content cont)
        {
            try
            {
                var content = db.Contents.Find(cont.ID);
                content.Name = cont.Name;
                content.MetaTitle = cont.MetaTitle;
                content.Description = cont.Description;
                content.Image = cont.Image;
                content.CategoryID = cont.CategoryID;
                content.Detail = cont.Detail;
                content.Warranty = cont.Warranty;
                content.ModifiedBy = cont.ModifiedBy;
                content.ModifiedDate = DateTime.Now;
                content.MetaKeywords = cont.MetaKeywords;
                content.MetaDescriptions = cont.MetaDescriptions;
                content.Status = cont.Status;
                content.Tags = cont.Tags;
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
                var content = db.Contents.Find(id);
                db.Contents.Remove(content);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public long Create(Content content)
        {
            //Xử lý alias
            if (string.IsNullOrEmpty(content.MetaTitle))
            {
                content.MetaTitle = StringHelper.ToUnsignString(content.Name);
            }
            content.CreatedDate = DateTime.Now;
            content.ViewCount = 0;
            db.Contents.Add(content);
            db.SaveChanges();

            //Xử lý tag
            if (!string.IsNullOrEmpty(content.Tags))
            {
                string[] tags = content.Tags.Split(',');
                foreach (var tag in tags)
                {
                    var tagId = StringHelper.ToUnsignString(tag);
                    var existedTag = this.CheckTag(tagId);

                    //insert to to tag table
                    if (!existedTag)
                    {
                        this.InsertTag(tagId, tag);
                    }

                    //insert to content tag
                    this.InsertContentTag(content.ID, tagId);

                }
            }

            return content.ID;
        }
        public bool CheckTag(string id)
        {
            return db.Tags.Count(x => x.ID == id) > 0;
        }
        public void InsertTag(string id, string name)
        {
            var tag = new Tag();
            tag.ID = id;
            tag.Name = name;
            db.Tags.Add(tag);
            db.SaveChanges();
        }
        public void InsertContentTag(long contentId, string tagId)
        {
            var contentTag = new ContentTag();
            contentTag.ContentID = contentId;
            contentTag.TagID = tagId;
            db.ContentTags.Add(contentTag);
            db.SaveChanges();
        }
        public long Edit(Content content)
        {
            if (string.IsNullOrEmpty(content.MetaTitle))
            {
                content.MetaTitle = StringHelper.ToUnsignString(content.Name);
            }
            content.CreatedDate = DateTime.Now;
            //cause the failed may in here
            var entity = db.Contents.Find(content.ID);
            entity.Name = content.Name;
            entity.MetaTitle = content.MetaTitle;
            entity.Description = content.Description;
            entity.Image = content.Image;
            entity.CategoryID = content.CategoryID;
            entity.Detail = content.Detail;
            entity.Warranty = content.Warranty;
            entity.ModifiedBy = content.ModifiedBy;
            entity.ModifiedDate = DateTime.Now;
            entity.MetaKeywords = content.MetaKeywords;
            entity.MetaDescriptions = content.MetaDescriptions;
            entity.Status = content.Status;
            entity.Tags = content.Tags;
            db.SaveChanges();
            //
            //Kiểm tra xử lý tag
            if (!string.IsNullOrEmpty(content.Tags))
            {
                this.RemoveAllContentTag(content.ID);
                string[] tags = content.Tags.Split(',');
                foreach (var tag in tags)
                {
                    var tagId = StringHelper.ToUnsignString(tag);
                    var existedTag = this.CheckTag(tagId);
                    //thêm bản ghi mới vào bảng Tags
                    if (!existedTag)
                    {
                        this.InsertTag(tagId, tag);
                    }
                    //thêm mới bản ghi vào bảng contentTag
                    this.InsertContentTag(content.ID, tagId);
                }
            }
            return content.ID;
        }
        public void RemoveAllContentTag(long contentId)
        {
            db.ContentTags.RemoveRange(db.ContentTags.Where(x => x.ContentID == contentId));
            db.SaveChanges();
        }
        public IEnumerable<Content> ListAllPagingClient(int page, int pageSize)
        {
            IQueryable<Content> model = db.Contents;
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        public List<Tag> ListTag(long contentId)
        {
            var model = (from a in db.Tags
                         join b in db.ContentTags
                         on a.ID equals b.TagID
                         where b.ContentID == contentId
                         select new
                         {
                             ID = b.TagID,
                             Name = a.Name
                         }).AsEnumerable().Select(x => new Tag()
                         {
                             ID = x.ID,
                             Name = x.Name
                         });
            return model.ToList();
        }
        public IEnumerable<Content> ListAllByTag(string tag, int page, int pageSize)
        {
            var model = (from a in db.Contents
                         join b in db.ContentTags
                         on a.ID equals b.ContentID
                         where b.TagID == tag
                         select new
                         {
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Image = a.Image,
                             Description = a.Description,
                             CreatedDate = a.CreatedDate,
                             CreatedBy = a.CreatedBy,
                             ID = a.ID

                         }).AsEnumerable().Select(x => new Content()
                         {
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Image = x.Image,
                             Description = x.Description,
                             CreatedDate = x.CreatedDate,
                             CreatedBy = x.CreatedBy,
                             ID = x.ID
                         });
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        public Tag GetTag(string id)
        {
            return db.Tags.Find(id);
        }
    }
}
