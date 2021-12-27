using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
{
    public class TodoItem
    {
        [Key]   //assume identity
        [Column(TypeName = "int")]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }
        
        [Required]
        [Column(TypeName = "bit")]
        public bool IsComplete { get; set; }
    }
}
