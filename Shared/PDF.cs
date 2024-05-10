using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Shared;

public class PDF
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public string Url { get; set; }
    
    public List<Chunks>? PDFChuncks { get; set; }
    
    
    [ForeignKey("Admin")]
    [JsonIgnore]
    public int AdminId { get; set; } 
    
    public Admin? Admin { get; set; } 
   
}