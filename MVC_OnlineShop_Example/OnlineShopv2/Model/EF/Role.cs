using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("Role")]
    public class Role
    {
        [Key]
        [StringLength(50)]
        public string ID { get; set; }
        
        [StringLength(50)]
        public string Name { get; set; }
    }
}
