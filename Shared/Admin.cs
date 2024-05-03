using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Shared;

public class Admin
{  [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Mail { get; set; }
    public string? Password { get; set; }
    [JsonIgnore]
    public List<PDF>? PDFs { get; set; }
}