using System.Web.Mvc;

namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Content")]
    public partial class Content
    {
        public long ID { get; set; }
        [Display(Name = "Tên bài viết")]
        [StringLength(250)]
        public string Name { get; set; }

        
        [StringLength(250)]
        public string MetaTitle { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(500)]
        public string Description { get; set; }

        [Display(Name = "Hình ảnh")]
        [StringLength(250)]
        public string Image { get; set; }

        [Display(Name = "Danh mục")]
        public long? CategoryID { get; set; }

        [Display(Name = "Nội dung chi tiết")]
        [Column(TypeName = "ntext")]
        [AllowHtml]
        public string Detail { get; set; }

        public int? Warranty { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        [StringLength(250)]
        public string MetaKeywords { get; set; }

        [StringLength(250)]
        public string MetaDescriptions { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }

        public DateTime? TopHot { get; set; }

        public int? ViewCount { get; set; }

        [StringLength(500)]
        public string Tags { get; set; }
    }
}
