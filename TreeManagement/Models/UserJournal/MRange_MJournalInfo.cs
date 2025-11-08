using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public record MRange_MJournalInfo
{
    [Required]
    public int skip { get; private set; }
    [Required]
    public int count { get; private set; }
    [Required]
    public IEnumerable<MJournalInfo> items { get; private set; }
    public MRange_MJournalInfo(List<MJournalInfo> items, int count, int skip)
    {
        this.count = count;
        this.skip = skip;
        this.items = items;
    }

    public static MRange_MJournalInfo ToPagedList(IQueryable<MJournalInfo> source, int skip, int take)
    {
        var count = source.Count();
        IQueryable<MJournalInfo> items = source;
        if (skip > 0)
            items = items.Skip(skip);
        if (take > 0)
            items = items.Take(take);
        return new MRange_MJournalInfo(items.ToList(), count, skip);
    }
}
