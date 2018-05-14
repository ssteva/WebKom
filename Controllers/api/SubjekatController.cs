using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using webkom.Helper.Kendo;
using webkom.Models;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;
using ISession = NHibernate.ISession;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace webkom.Controllers.api
{
    [Route("api/[controller]")]
    //[Authorize("Bearer")]
    public class SubjekatController : Controller
    {
        private readonly ISession _session;
        private readonly ILogger _logger;

        public SubjekatController(ISession session, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<KorisnikController>();
            _session = session;
        }

        [Route("[Action]")]
        [HttpGet]
        public ActionResult Odeljenja(int? id)
        {
            try
            {
                var upit = _session.QueryOver<Subjekt>()
                  .Where(x => !x.Obrisan)
                  .And(x => x.Odeljenje);
                var res = upit.List<Subjekt>();
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
        public ActionResult Skladista(int? id)
        {
            try
            {
                var upit = _session.QueryOver<Subjekt>()
                  .Where(x => !x.Obrisan)
                  .And(x => x.Skladiste);
                var res = upit.List<Subjekt>();
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
        public ActionResult MestaIsporuke(int? id)
        {
            try
            {
                var upit = _session.QueryOver<Subjekt>()
                  .Where(x => !x.Obrisan)
                  .And(x => x.Kupac);
                var res = upit.List<Subjekt>();
                return Ok(res);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.InnerException.Message);
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("[Action]")]
        public ActionResult ListaKupacaCombo([FromBody] KendoRequest kr) //Get([FromUri] FilterContainer filter, int take, int skip, int page, int pageSize)
        {
            Subjekt subjekt = null;
            Mesto Mesto = null;
            Mesto MestoIsporuke = null;

            //Subjekt platilac = null;
            var textInfo = CultureInfo.InvariantCulture.TextInfo;
            var upit = _session.QueryOver<Subjekt>(() => subjekt)
                .JoinAlias(x => x.Mesto, () => Mesto, JoinType.LeftOuterJoin)
                .JoinAlias(x => x.MestoIsporuke, () => MestoIsporuke, JoinType.LeftOuterJoin)
                //.JoinAlias(x => x.Platilac, () => platilac, JoinType.LeftOuterJoin)
                .Where(x => !x.Obrisan)
                .And(x => x.Kupac);
            upit.Take(20);
            try
            {
                if (kr.Filter != null && kr.Filter.Filters.Any())
                {
                    if (kr.Filter.Logic.ToLower() == "and")
                    {
                        foreach (FilterDescription filter in kr.Filter.Filters)
                        {
                            if (!string.IsNullOrEmpty(filter.Value))
                            {
                                var prop = textInfo.ToTitleCase(filter.Field);
                                if (filter.Operator.ToLower() == "contains")
                                {
                                    upit.And(Restrictions.InsensitiveLike(prop, filter.Value, MatchMode.Anywhere));
                                }
                            }
                        }
                    }
                    else //or
                    {
                        var disjunction = new Disjunction();
                        var ok = false;
                        foreach (FilterDescription filter in kr.Filter.Filters)
                        {

                            if (!string.IsNullOrEmpty(filter.Value))
                            {

                                var prop = textInfo.ToTitleCase(filter.Field);
                                if (filter.Operator.ToLower() == "contains")
                                {
                                    //upit.And(Restrictions.Disjunction().Add(Restrictions.InsensitiveLike(prop, filter.Value, MatchMode.Anywhere)));
                                    //disjunction.Add(Restrictions.On<Subjekt>(e=>e.Naziv).IsLike(filter.Value,MatchMode.Anywhere));
                                    disjunction.Add(Restrictions.InsensitiveLike(prop, filter.Value, MatchMode.Anywhere));
                                    ok = true;
                                }
                            }
                        }
                        if (ok)
                            upit.And(disjunction);
                    }
                }
                upit.OrderBy(x => x.Naziv);
                var res = upit.List<Subjekt>();
                return Ok(res);
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
        public ActionResult ListaKupacaComboSp(string filter, int? id) //Get([FromUri] FilterContainer filter, int take, int skip, int page, int pageSize)
        {
            try
            {
                var upit = _session.CreateSQLQuery("exec filKupacMesto :filter, :id");
                upit.SetParameter("filter", filter, NHibernateUtil.String);
                upit.SetParameter("id", id, NHibernateUtil.Int32);
                upit.AddEntity(typeof(Subjekt));
                var res = upit.List<Subjekt>();
                return Ok(res);
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
        public ActionResult ListaOdeljenjaComboSp(string filter) //Get([FromUri] FilterContainer filter, int take, int skip, int page, int pageSize)
        {
            try
            {
                var upit = _session.CreateSQLQuery("exec filOdeljenje :filter");
                upit.SetParameter("filter", filter, NHibernateUtil.String);
                upit.AddEntity(typeof(Subjekt));
                var res = upit.List<Subjekt>();
                return Ok(res);
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
        public ActionResult ListaSkladistaComboSp(string filter) //Get([FromUri] FilterContainer filter, int take, int skip, int page, int pageSize)
        {
            try
            {
                var upit = _session.CreateSQLQuery("exec filSkladiste :filter");
                upit.SetParameter("filter", filter, NHibernateUtil.String);
                upit.AddEntity(typeof(Subjekt));
                var res = upit.List<Subjekt>();
                return Ok(res);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.InnerException.Message);
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("[Action]")]
        public ActionResult ListaMestaIsporukeCombo([FromBody] KendoRequest kr) //Get([FromUri] FilterContainer filter, int take, int skip, int page, int pageSize)
        {
            Subjekt subjekt = null;
            Mesto Mesto = null;
            Mesto MestoIsporuke = null;

            Subjekt platilac = null;
            var textInfo = CultureInfo.InvariantCulture.TextInfo;
            var upit = _session.QueryOver<Subjekt>(() => subjekt)
                .JoinAlias(x => x.Mesto, () => Mesto, JoinType.LeftOuterJoin)
                .JoinAlias(x => x.MestoIsporuke, () => MestoIsporuke, JoinType.LeftOuterJoin)
                .JoinAlias(x => x.Platilac, () => platilac, JoinType.LeftOuterJoin)
                .Where(x => !x.Obrisan)
                .And(x => x.Kupac);
            upit.Take(20);
            try
            {
                if (kr.Filter != null && kr.Filter.Filters.Any())
                {
                    if (kr.Filter.Logic.ToLower() == "and")
                    {
                        foreach (FilterDescription filter in kr.Filter.Filters)
                        {
                            if (!string.IsNullOrEmpty(filter.Value))
                            {
                                var prop = textInfo.ToTitleCase(filter.Field);
                                if (filter.Operator.ToLower() == "contains")
                                {
                                    upit.And(Restrictions.InsensitiveLike(prop, filter.Value, MatchMode.Anywhere));
                                }
                            }
                        }
                    }
                    else //or
                    {
                        var disjunction = new Disjunction();
                        var ok = false;
                        foreach (FilterDescription filter in kr.Filter.Filters)
                        {
                            if (!string.IsNullOrEmpty(filter.Value))
                            {
                                var prop = textInfo.ToTitleCase(filter.Field);
                                if (prop.ToLower().Contains("platilac") && filter.Operator.ToLower() == "eq")
                                {
                                    upit.And(Restrictions.Eq(prop, int.Parse(filter.Value)));
                                }
                                if (filter.Operator.ToLower() == "contains")
                                {
                                    disjunction.Add(Restrictions.InsensitiveLike(prop, filter.Value, MatchMode.Anywhere));
                                    ok = true;
                                }
                            }
                        }
                        if (ok)
                            upit.And(disjunction);
                    }
                }
                upit.OrderBy(x => x.Naziv);
                var res = upit.List<Subjekt>();
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
