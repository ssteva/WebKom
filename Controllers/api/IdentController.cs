using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        public ActionResult ListaCombo([FromBody] KendoRequest kr)
        {
            var textInfo = CultureInfo.InvariantCulture.TextInfo;
            var upit = _session.QueryOver<Ident>()
              .Where(x => !x.Obrisan);
            upit.Take(20);
            try
            {
                if (kr.Filter != null && kr.Filter.Filters.Any())
                {
                    foreach (FilterDescription filter in kr.Filter.Filters)
                    {
                        if (!string.IsNullOrEmpty(filter.Value))
                        {
                            var prop = textInfo.ToTitleCase(filter.Field);
                            upit.And(Restrictions.InsensitiveLike(prop, filter.Value, MatchMode.Anywhere));
                        }
                    }
                }
                upit.OrderBy(x => x.Naziv);
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

    }
}
