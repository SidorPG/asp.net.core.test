using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations;

namespace Api.Models.UserJournal;

public class UserJournalGetSingleQueryStringParameters
{
    public UserJournalGetSingleQueryStringParameters(long id)
    {
        this.id = id;

    }
    [Required]
    public long id { get; set; }

}