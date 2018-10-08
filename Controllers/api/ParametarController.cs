using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webkom.Models;
using Microsoft.Extensions.Logging;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;
using ISession = NHibernate.ISession;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace webkom.Controllers.api
{
    [Route("api/[controller]")]
    [Authorize("Bearer")]
    public class ParametarController : Controller
    {
        private readonly ILogger _logger;
        private readonly ISession _session;

        //private SmtpClient smtp;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IEmailSender _emailSender;
        //private readonly ISmsSender _smsSender;

        public ParametarController(
            ILoggerFactory loggerFactory,
            ISession session)
        {
            _logger = loggerFactory.CreateLogger<Parametar>();
            _session = session;
        }


        //[HttpGet]
        ////[Route("[Action]")]
        //public ActionResult Get()
        //{
        //    var query = _session.QueryOver<Parametar>();
           
        //    var result = query.List<Parametar>();
        //    return Ok(result);
        //}

        [HttpGet]
        //[Route("[Action]")]
        public ActionResult Get(string naziv)
        {
            var query = _session.QueryOver<Parametar>();
            if(! string.IsNullOrEmpty(naziv))
                query.WhereRestrictionOn(x => x.Naziv).IsInsensitiveLike(naziv);
            
            var result = query.List<Parametar>();
            return Ok(result);
        }
    }


}

