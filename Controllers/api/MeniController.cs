using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using webkom.Models;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;
using ISession = NHibernate.ISession;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace webkom.Controllers.api
{
  [Route("api/[controller]")]
  [Authorize("Bearer")]
  public class MeniController : Controller
  {
    private readonly ISession _session;
    private readonly ILogger _logger;

    public MeniController(ISession session, ILoggerFactory loggerFactory)
    {
      _logger = loggerFactory.CreateLogger<KorisnikController>();
      _session = session;
    }

    [HttpGet]
    public ActionResult Get()
    {
      
      var upit = _session.QueryOver<Meni>();
      upit.OrderBy(m=>m.Rbr);
      var lista = upit.List<Meni>();

      return Ok(lista);
    }
  }
}
