using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Criterion;
using webkom.Helper.Kendo;
using webkom.Models;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;
using ISession = NHibernate.ISession;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace webkom.Controllers.api
{
    [Route("api/[controller]")]
    //[Authorize("Bearer")]
    public class IdentController : Controller
    {
        private readonly ISession _session;
        private readonly ILogger _logger;

        public IdentController(ISession session, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<KorisnikController>();
            _session = session;
        }

        [HttpGet("{id}")]
        //[Route("[Action]")]
        [HttpGet]
        public ActionResult Get(int id)
        {
            var ident = _session.Get<Ident>(id);
            return Ok(ident);
        }

        [Route("[Action]")]
        [HttpGet]
        public ActionResult ListaCombo(string filter)
        {
            try
            {
                var upit = _session.CreateSQLQuery("exec filIdent :filter");
                upit.SetParameter("filter", filter, NHibernateUtil.String);
                upit.AddEntity(typeof(Ident));
                var res = upit.List<Ident>();
                return Ok(res);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.InnerException.Message);
                return BadRequest();
            }
        }

        [Route("[Action]")]
        [HttpGet]
        public ActionResult getPrice(int id)
        {
            try
            {
                var upit = _session.CreateSQLQuery("exec getPrice :id");
                upit.SetParameter("id", id, NHibernateUtil.String);
                upit.AddScalar("cena", NHibernateUtil.Decimal);
                var res = upit.List<decimal>().Single();
                return Ok(res);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.InnerException.Message);
                return BadRequest();
            }
        }
    }
}
