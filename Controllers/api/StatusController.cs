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
  public class StatusController : Controller
  {
    private readonly ISession _session;
    private readonly ILogger _logger;

    public StatusController(ISession session, ILoggerFactory loggerFactory)
    {
      _logger = loggerFactory.CreateLogger<StatusController>();
      _session = session;
    }

    [HttpGet]
    public ActionResult Get(string vrsta)
    {
      
      var upit = _session.QueryOver<Status>()
        .Where(x=>x.Vrsta == vrsta);
      upit.OrderBy(m=>m.Id);
      var lista = upit.List<Status>();

      return Ok(lista);
    }

           
        [HttpGet]
        [Route("[Action]")]
        public ActionResult ListaStatusaComboSp(string vrsta, string filter) //Get([FromUri] FilterContainer filter, int take, int skip, int page, int pageSize)
        {
            try
            {
                var upit = _session.CreateSQLQuery("exec filStatus :vrsta :filter");
                upit.SetParameter("filter", filter, NHibernateUtil.String);
                upit.AddEntity(typeof(Status));
                var res = upit.List<Status>();
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
