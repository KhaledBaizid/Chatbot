using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared;

public class Admin
{  [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Mail { get; set; }
    public string Password { get; set; }
    public List<PDF>? PDFs { get; set; }
}