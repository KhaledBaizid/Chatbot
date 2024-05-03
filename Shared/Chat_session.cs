using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared;

public class Chat_session
{  [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public DateTime ChatTime { get; set; }
    public List<Conversation> Conversations { get; set; }
}