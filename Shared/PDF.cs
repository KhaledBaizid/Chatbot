using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Shared;

public class PDF
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Url { get; set; }
    
    [ForeignKey("Admin")]
    [JsonIgnore]
    public int AdminId { get; set; } // Foreign key
    
    public Admin Admin { get; set; } // Navigation property
    public List<Chunks> PDFChuncks { get; set; }
}