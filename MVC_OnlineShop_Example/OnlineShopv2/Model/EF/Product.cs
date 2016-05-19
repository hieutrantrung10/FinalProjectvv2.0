namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        public long ID { get; set; }
        [Display(Name = "Tên sản phẩm")]
        [StringLength(250)]
        public string Name { get; set; }
        [Display(Name = "Mã sản phẩm")]
        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(250)]
        public string MetaTitle { get; set; }
        [Display(Name = "Mô tả sản phẩm")]
        [StringLength(500)]
        public string Description { get; set; }
        [Display(Name = "Hình ảnh")]
        [StringLength(250)]
        public string Image { get; set; }

        [Column(TypeName = "xml")]
        public string MoreImages { get; set; }
        [Display(Name = "Đơn giá")]
        public decimal? Price { get; set; }
        [Display(Name = "Giá khuyến mãi")]
        public decimal? PromotionPrice { get; set; }
        [Display(Name = "Bao gồm VAT")]
        public bool? IncludeVAT { get; set; }
        [Display(Name = "Số lượng")]
        public int? Quality { get; set; }
        [Display(Name = "Danh mục sản phẩm")]
        public long? CategoryID { get; set; }
        [Display(Name = "Chi tiết")]
        [Column(TypeName = "ntext")]
        public string Detail { get; set; }
        [Display(Name = "Bảo hành")]
        public int? Warranty { get; set; }
        [Display(Name = "Ngày tạo")]
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
        public bool? Status { get; set; }

        public DateTime? TopHot { get; set; }

        [StringLength(10)]
        public string ViewCount { get; set; }
    }
}
