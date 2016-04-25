using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShopv2.Models
{
    public class AnswerModel
    {
        [Key]
        public long ID { set; get; }
        [Display(Name = "Câu hỏi bí mật")]
        public string SecurityQuestion { get; set; }

        [Display(Name = "Câu trả lời")]
        public string SecurityAnswer { get; set; }
    }
}