using System.ComponentModel.DataAnnotations;

namespace Api.Models.UserJournal;

public class UserJournalGetSingleQueryStringParameters
{
    [Required]
    public UserJournalGetSingleQueryStringParameters(long id)
    {
        this.id = id;

    }
    public long id { get; set; }

}