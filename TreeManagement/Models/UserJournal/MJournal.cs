using System.ComponentModel.DataAnnotations;

namespace Api.Models.UserJournal;

public record MJournal
{
    [Required]
    public string text { get; set; }
    [Required]
    public long id { get; set; }
    [Required]
    public long eventId { get; set; }
    [Required]
    [property: Example("2025-05-23T12:18:16.9222634Z")]
    public DateTime createdAt { get; set; }
}