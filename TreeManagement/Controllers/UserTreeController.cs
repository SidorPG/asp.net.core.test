using System.ComponentModel.DataAnnotations;
using Api.Models.UserTree;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Data.Controllers
{
    /// <summary>
    /// Represents entire tree API
    /// </summary>
    [ApiController]
    [Route("api.user.tree")]
    public class UserTreeController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public UserTreeController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        /// <summary></summary>
        /// <remarks> >- Returns your entire tree. If your tree doesn't exist it will be created 
        /// automatically.
        /// </remarks>
        [Route("/api.user.tree.get")]
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK, "Successful response", typeof(UserTree))]
        public async Task<UserTree> get([FromQuery] UserTreeQueryStringParameters args)
        {
            var treeNode = _dbContext.TreeNodes.AsNoTracking().FirstOrDefault(x => x.Name == args.treeName);

            if (treeNode == null)
            {
                treeNode = new tree_node() { Name = args.treeName, TreeName = args.treeName };
                await _dbContext.AddAsync(treeNode);
                await _dbContext.SaveChangesAsync();
                return new UserTree { name = args.treeName, id = treeNode.Id, children = new List<UserTree>() };
            }

            var result = new UserTree { name = args.treeName, id = treeNode.Id, children = await getChilder(treeNode.Id) };

            return result;
        }

        private async Task<List<UserTree>> getChilder([Required] int parantId)
        {
            var childNodes = _dbContext.TreeNodes.AsNoTracking().Where(x => x.ParentNodeId == parantId).ToList();
            if (childNodes == null) { return new List<UserTree>(); }
            var result = new List<UserTree>();
            childNodes.ForEach(async treeNode =>
            {
                var resultIterm = new UserTree { name = treeNode.Name, id = treeNode.Id, children = await getChilder(treeNode.Id) };
                result.Add(resultIterm);
            });
            return result;
        }
    }
}
