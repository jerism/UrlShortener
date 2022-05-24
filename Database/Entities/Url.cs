using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    public class Url
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string OriginalUrl { get; set; }
        [MaxLength(6)]
        public string UniqueIdentifier { get; set; }
        public DateTime? LastAccessed { get; set; }
    }
}
