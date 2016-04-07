namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("Credential")]
    [Serializable]
    public class Credential
    {
        [Key]
        [StringLength(20)]
        public string UserGroupID { get; set; }
        
        [StringLength(50)]
        public string RoleID { get; set; }
    }
}
