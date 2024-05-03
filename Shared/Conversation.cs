using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Shared;

public class Conversation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public string Feedback { get; set; }
    public DateTime ConversationTime { get; set; }
    [ForeignKey("Chat_session")]
    [JsonIgnore]
    public int ChatSessionId { get; set; } // Foreign key
    
    public Chat_session? ChatSession { get; set; } // Navigation property
    [JsonIgnore]
    public List<Conversation_PDF_Chunks> ConversationPDFChunks { get; set; }
}