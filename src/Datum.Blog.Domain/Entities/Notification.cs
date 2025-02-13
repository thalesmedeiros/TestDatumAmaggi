using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datum.Blog.Domain.Entities;

[Table("Notification")]
public class Notification
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Mensagem { get; set; } = string.Empty;
    public DateTime DataEnvio { get; set; } = DateTime.UtcNow;
}