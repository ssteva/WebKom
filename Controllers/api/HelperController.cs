using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NHibernate;
using webkom.Models;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;
using ISession = NHibernate.ISession;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace webkom.Controllers.api
{
    [Route("api/[controller]")]
    [Authorize("Bearer")]
    public class HelperController : Controller
    {
        private readonly ISession _session;
        private readonly ILogger _logger;

        public HelperController(ISession session, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HelperController>();
            _session = session;
        }

        [HttpGet]
        public ActionResult Get(string vrsta)
        {

            var upit = _session.QueryOver<Status>()
              .Where(x => x.Vrsta == vrsta);
            upit.OrderBy(m => m.Id);
            var lista = upit.List<Status>();

            return Ok(lista);
        }


        [HttpGet]
        [Route("[Action]")]
        public ActionResult NacinPlacanja() //Get([FromUri] FilterContainer filter, int take, int skip, int page, int pageSize)
        {
            var upit = _session.QueryOver<NacinPlacanja>();
            upit.OrderBy(m => m.Id);
            var lista = upit.List<NacinPlacanja>();

            return Ok(lista);
        }
        [HttpGet]
        [Route("[Action]")]
        public ActionResult NacinDostave() //Get([FromUri] FilterContainer filter, int take, int skip, int page, int pageSize)
        {
            var upit = _session.QueryOver<NacinDostave>();
            upit.OrderBy(m => m.Id);
            var lista = upit.List<NacinDostave>();

            return Ok(lista);
        }
    }
}
