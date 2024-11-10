using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace wowberry_crud.Models
{
    public class Audit
    {
        [Key]
        public int AuditID { get; set; }

        [Required]
        [StringLength(50)]
        public string TableName { get; set; }

        [Required]
        [StringLength(50)]
        public string FieldName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        [Required]
        public int ModifiedBy { get; set; }

        [ForeignKey("ModifiedBy")]
        public Users User { get; set; } 

        [Required]
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    }
}
