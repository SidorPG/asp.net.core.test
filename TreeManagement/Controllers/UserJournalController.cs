namespace Data.Controllers
{
    /// <summary>
    /// Represents journal API
    /// </summary>
    [ApiController]
    [Route("api.user.journal")]
    public class UserJournalController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public UserJournalController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary></summary>
        /// <remarks> >- Provides the pagination API. Skip means the number of items should be
        /// skipped by server. Take means the maximum number items should be
        /// returned by server. All fields of the filter are optional.
        /// </remarks>
        [Route("/api.user.journal.getRange")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successful response", typeof(MRange_MJournalInfo))]
        [HttpPost]
        public MRange_MJournalInfo GetRange([FromQuery] UserJournalQueryStringParameters qargs)
        {
            if (qargs.skip < 0)
                throw new SecureException(UserJournalErrors.SkipOnlyPositive);
            if (qargs.take < 0)
                throw new SecureException(UserJournalErrors.TakeOnlyPositive);

            var journalsQuery = _dbContext.JournalMessages.Include(x => x.JournalEvent).AsNoTracking().AsQueryable();

            if (qargs.filter.from != DateTime.MinValue)
                journalsQuery = journalsQuery.Where(x => x.JournalEvent.Created > qargs.filter.from);
            if (qargs.filter.to != DateTime.MinValue)
                journalsQuery = journalsQuery.Where(x => x.JournalEvent.Created <= qargs.filter.to);
            IQueryable<MJournalInfo> journalsFiltered = journalsQuery
                .Select(x => new MJournalInfo()
                {
                    id = x.Id,
                    createdAt = x.JournalEvent.Created,
                    eventId = x.EventId,
                })
                .OrderByDescending(x => x.createdAt);

            MRange_MJournalInfo journalsPage = MRange_MJournalInfo.ToPagedList(journalsFiltered, qargs.skip, qargs.take);

            return journalsPage;
        }

        /// <summary></summary>
        /// <remarks>
        /// Returns the information about an particular event by ID.
        /// </remarks>
        [HttpPost("/api.user.journal.getSingle")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successful response", typeof(MJournal))]
        public async Task<MJournal> getSingle([FromQuery] UserJournalGetSingleQueryStringParameters qargs)
        {
            var journalMessage = await _dbContext.JournalMessages.Include(x => x.JournalEvent).FirstOrDefaultAsync(x => x.Id == qargs.id);
            if (journalMessage == null) throw new SecureException(UserJournalErrors.MessageNotFound);

            var data = new List<string>
            {
                $"Request ID = {journalMessage.EventId}",
                $"Path = {journalMessage.JournalEvent.Path}",
                $"{journalMessage.JournalEvent.RequestQuery}",
                $"{journalMessage.JournalEvent.RequestBody}",
                $"{journalMessage.JournalEvent.Exception}: {journalMessage.JournalEvent.ExceptionMessage}",
                $"{journalMessage.JournalEvent.ExceptionStackTrace}"
            };

            var text = string.Join("\r\n", data);

            return new MJournal()
            {
                id = journalMessage.Id,
                createdAt = journalMessage.JournalEvent.Created,
                eventId = journalMessage.EventId,
                text = text,
            };
        }
    }
}
