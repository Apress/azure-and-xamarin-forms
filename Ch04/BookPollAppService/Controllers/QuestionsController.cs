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
    public class QuestionsController : TableController<PollQuestion>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            BookPollAppContext context = new BookPollAppContext();
            DomainManager = new EntityDomainManager<PollQuestion>(context, Request);
        }

        // GET tables/Questions
        public IQueryable<PollQuestion> GetAllPollQuestion()
        {
            return Query();
        }

        // GET tables/Questions/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<PollQuestion> GetPollQuestion(string id)
        {
            return Lookup(id);
        }


    }
}
