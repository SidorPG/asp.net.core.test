using System.ComponentModel.DataAnnotations;

namespace Api.Models.UserJournal;

public record MJournalInfo
{
    [Required]
    public int id { get; set; }
    [Required]
    public int eventId { get; set; }
    [Example("2025-05-23T12:18:16.922346Z")]
    [Required]
    public DateTime createdAt { get; set; }
}