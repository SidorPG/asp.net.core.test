using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Api.Models.UserJournal;

public class UserJournalQueryStringParameters
{
    [Required]
    public int skip { get; set; }
    [Required]
    public int take { get; set; }

    [FromBody]
    public VJournalFilter filter { get; set; }
}