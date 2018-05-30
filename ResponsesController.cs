using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using BookPollAppService.DataObjects;
using BookPollAppService.Models;

namespace BookPollAppService.Controllers
{
    public class ResponsesController : TableController<PollResponse>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            BookPollAppContext context = new BookPollAppContext();
            DomainManager = new EntityDomainManager<PollResponse>(context, Request);
        }

        // GET tables/Responses
        public IQueryable<PollResponse> GetAllPollResponse()
        {
            return Query();
        }

        // GET tables/Responses/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<PollResponse> GetPollResponse(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Responses/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [HttpPatch]
        public Task<PollResponse> UpdatePollResponse(string id, Delta<PollResponse> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Responses
        [HttpPost]
        public async Task<IHttpActionResult> InsertPollResponse(PollResponse item)
        {
            PollResponse current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Responses/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeletePollResponse(string id)
        {
             return DeleteAsync(id);
        }
    }
}
