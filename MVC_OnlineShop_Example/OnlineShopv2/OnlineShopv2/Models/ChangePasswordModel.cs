using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShopv2.Models
{
    public class ChangePasswordModel
    {
        [Key]
        public long ID { set; get; }

        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Bạn không được để trống tên đăng nhập")]
        public string UserName { set; get; }

        [Display(Name = "Mật khẩu")]
        [StringLength(500, MinimumLength = 6, ErrorMessage = "Bạn cần nhập mật khẩu có độ dài lớn hơn 6 ký tự")]
        [Required(ErrorMessage = "Bạn không được để trống mật khẩu")]
        public string Password { set; get; }

        [Display(Name = "Mật khẩu mới")]
        [StringLength(500, MinimumLength = 6, ErrorMessage = "Bạn cần nhập mật khẩu có độ dài lớn hơn 6 ký tự")]
        [Required(ErrorMessage = "Bạn hãy nhập mật khẩu mới")]
        public string NewPassword { set; get; }

        [Display(Name = "Nhập lại mật khẩu mới")]
        [Required(ErrorMessage = "Bạn hãy nhập lại mật khẩu mới")]
        public string ConfirmPassword { set; get; }

        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessage = "Hãy nhập lại địa chỉ Email cho đúng định dạng")]
        public string Email { get; set; }
    }
}