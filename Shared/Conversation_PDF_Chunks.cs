using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Shared;

public class Conversation_PDF_Chunks
{  [ForeignKey("Conversation")]
    [JsonIgnore]
    public int? ConversationId { get; set; }
   
    
    [ForeignKey("PDF_Chuncks")]
    [JsonIgnore]
    public int? PDFChuncksId { get; set; }
    
    
    
    public Chunks? PDF_Chuncks { get; set; }
    [JsonIgnore]
    public Conversation? Conversation { get; set; }
    
}