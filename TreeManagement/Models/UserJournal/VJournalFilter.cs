using System.ComponentModel.DataAnnotations;

namespace Api.Models.UserJournal;

public class VJournalFilter
{
    [Example("2025-05-23T12:18:16.9223615Z")]
    public DateTime from { get; set; }

    [Example("2025-05-23T12:18:16.9223726Z")]
    public DateTime to { get; set; }

    [Required] public string search { get; set; } = "";
}