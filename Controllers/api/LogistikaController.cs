using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Transform;
using webkom.Helper;
using webkom.Helper.Kendo;
using webkom.Models;
using webkom.Models.DTO;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;
using ISession = NHibernate.ISession;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace webkom.Controllers.api
{
    [Route("api/[controller]")]
    [Authorize("Bearer")]
    public class LogistikaController : Controller
    {
        private readonly ISession _session;
        private readonly ILogger _logger;

        public LogistikaController(ISession session, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<LogistikaController>();
            _session = session;
        }

        // [HttpGet]
        // public ActionResult Get()
        // {

        //   var upit = _session.QueryOver<Meni>();
        //   upit.OrderBy(m=>m.Rbr);
        //   var lista = upit.List<Meni>();

        //   return Ok(lista);
        // }
        [HttpPost]
        [Route("[Action]")]
        public KendoResult<Lager> PregledGrid([FromBody]KendoRequest kr)
        {
            var query = _session.CreateSQLQuery("exec Logistika :count, :take, :skip, :sortfield, :sortdir, :id, :dan, :nalog, :ident, :proizvod, :kolicina, :kolicina2, :registracija, :vozac") 

            query.SetParameter("count", false, NHibernateUtil.Boolean);
            query.SetParameter("take", kr.Take, NHibernateUtil.Int32);
            query.SetParameter("skip", kr.Skip, NHibernateUtil.Int32);
            query.SetParameter("sortfield", "Ident", NHibernateUtil.String);
            query.SetParameter("sortdir", "ASC", NHibernateUtil.String);
            query.SetParameter("id", null, NHibernateUtil.Int32);
            query.SetParameter("dan", null, NHibernateUtil.String);
            query.SetParameter("nalog", null, NHibernateUtil.String);
            query.SetParameter("ident", null, NHibernateUtil.String);
            query.SetParameter("proizvod", null, NHibernateUtil.String);
            query.SetParameter("kolicina", null, NHibernateUtil.Decimal);
            query.SetParameter("kolicina2", null, NHibernateUtil.Decimal);
            query.SetParameter("registracija", null, NHibernateUtil.String);
            query.SetParameter("vozac", null, NHibernateUtil.String);

            if (kr.Filter != null && kr.Filter.Filters.Any())
            {
                foreach (FilterDescription filter in kr.Filter.Filters)
                {
                    query.SetParameter(filter.Field, filter.Value);
                    
                }
            }

            if (kr.Sort != null && kr.Sort.Count > 0)
            {
                foreach (Sort sort in kr.Sort)
                {
                    query.SetParameter("sortfield", sort.Field);
                    
                }
            }

            query.Future<Logistika>();
            rows.Future<int>();
            var redova = rows.UniqueResult() as int? ?? 0;

            query.ExecuteUpdate();
            query.SetResultTransformer(new AliasToBeanResultTransformer(typeof(Logistika)));
            var lista = query.List<Logistika>();

            var res = new KendoResult<Logistika>
            {
                Data = lista,
                Total = redova
            };
            return res;

        }
    }
}
