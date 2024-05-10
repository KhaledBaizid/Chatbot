using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Pgvector;

namespace Shared;

public class Chunks
{    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Text { get; set; }
    
    [Column(TypeName = "vector(1536)")]
    public Vector? Embedding { get; set; }
    
    [ForeignKey("PDF")]
    [JsonIgnore]
    public int PDFId { get; set; } 
    
    public PDF? PDF { get; set; } 
    [JsonIgnore]
    public List<Conversation_PDF_Chunks> PDFChunks { get; set; }
    
    
}