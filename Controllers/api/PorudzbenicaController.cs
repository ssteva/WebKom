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
    //[Authorize("Bearer")]
    public class PorudzbenicaController : Controller
    {
        private readonly ISession _session;
        private readonly ILogger _logger;

        public PorudzbenicaController(ISession session, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<KorisnikController>();
            _session = session;
        }

        //[HttpGet("{id}")]
        //[Route("[Action]")]
        [HttpGet]
        public ActionResult Get(int id, string vrsta)
        {
            Porudzbenica porudzbenica = null;
            try
            {
                if (id == 0)
                {
                    //porudzbenica = new Porudzbenica();
                    var query = _session.CreateSQLQuery("exec defPorudzbenica :vrsta");
                    query.SetParameter("vrsta", vrsta);
                    query.AddEntity(typeof(Porudzbenica));
                    porudzbenica = query.List<Porudzbenica>()[0];
                    _logger.LogInformation(porudzbenica.Valuta.Naziv);
                }
                else
                {
                    porudzbenica = _session.Get<Porudzbenica>(id);
                }
                return Ok(porudzbenica);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.InnerException.Message);
                return BadRequest();
            }
        }
              
        [HttpGet]
        [Route("[Action]")]
        public ActionResult PorudzbenicaStavka(int id, string vrsta)
        {
            PorudzbenicaStavka stavka = null;
            try
            {
                if (id == 0)
                {
                    //porudzbenica = new Porudzbenica();
                    var query = _session.CreateSQLQuery("exec defPorudzbenicaStavka :vrsta");
                    query.SetParameter("vrsta", vrsta);
                    query.AddEntity(typeof(PorudzbenicaStavka));
                    stavka = query.List<PorudzbenicaStavka>()[0];
                }
                else
                {
                    stavka = _session.Get<PorudzbenicaStavka>(id);
                }
                return Ok(stavka);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.InnerException.Message);
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("[Action]")]
        public ActionResult New()
        {

            var porudzbenica = new Porudzbenica();
            var props = porudzbenica.GetType().GetProperties();
            foreach (var item in props)
            {
                _logger.LogInformation($"Name: {item.Name}, tip: {item.GetType()}");
            }
            return Ok(porudzbenica);
        }
    }
}
