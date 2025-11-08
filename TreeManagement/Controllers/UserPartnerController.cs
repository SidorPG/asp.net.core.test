using System.ComponentModel.DataAnnotations;

namespace Data.Controllers
{
    /// <summary>
    /// Represents auth API
    /// </summary>
    [ApiController]
    [Route("api.user.partner")]
    public class UserPartnerController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public UserPartnerController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary></summary>
        /// <remarks> >- (Optional) Saves user by unique code and returns auth token required on all other requests, if implemented.
        /// </remarks>
        [Route("/api.user.partner.rememberMe")]
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK, "Successful response", typeof(TokenInfo))]
        public ActionResult rememberMe([FromQuery][Required] string code)
        {
            throw new Exception("strange ");
            return Ok(new TokenInfo());
        }
    }
}
