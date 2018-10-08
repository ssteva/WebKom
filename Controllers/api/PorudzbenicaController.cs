using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using System;
using System.Linq;
using webkom.Helper;
using webkom.Helper.Kendo;
using webkom.Models;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;
using ISession = NHibernate.ISession;
using Newtonsoft.Json;
using System.Web;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using webkom.Models.DTO;
using NHibernate.Transform;
using NHibernate;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace webkom.Controllers.api
{
    [Route("api/[controller]")]
    [Authorize("Bearer")]
    public class PorudzbenicaController : Controller
    {
        private readonly ISession _session;
        private readonly ILogger _logger;
        //private readonly IHttpContextAccessor _httpContextAccessor;

        public PorudzbenicaController(ISession session, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<KorisnikController>();
            _session = session;
            //_httpContextAccessor = httpContextAccessor;
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
        public ActionResult PorudzbenicaTip() //Get([FromUri] FilterContainer filter, int take, int skip, int page, int pageSize)
        {
            try
            {
                var upit = _session.CreateSQLQuery("exec PorudzbenicaTip");
                var res = upit.SetResultTransformer(Transformers.AliasToBean(typeof(PorudzbenicaTip))).List<PorudzbenicaTip>();
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

        [HttpPost]
        [Route("[Action]")]
        public ActionResult Storno([FromQuery]string vrsta, int id)
        {
            var upit = _session.CreateSQLQuery("exec Storno :vrsta, :id");
            upit.SetParameter("vrsta", vrsta);
            upit.SetParameter("id", id);
            var res = upit.ExecuteUpdate();
            return Ok(res);
        }

        [HttpPost]
        [Route("[Action]")]
        public ActionResult BrisiStavku([FromQuery] int id)
        {

            var upit = _session.CreateSQLQuery("exec BrisiStavku :id");
            upit.SetParameter("id", id);
            var res = upit.ExecuteUpdate();
            return Ok(res);
        }
        [HttpPost]
        [Route("[Action]")]
        public KendoResult<PorudzbenicaDto> PregledGridProc([FromBody]KendoRequest kr)
        {

            //         @tip NVARCHAR(20) = NULL,
            // @broj INT = NULL,
            // @kupac INT = NULL,
            // @mestoIsporuke INT = NULL,
            // @datum1 DATE = NULL,
            // @datum2 DATE = NULL,
            // @datumVazenja1 DATE = NULL,
            // @datumVazenja2 DATE = NULL,
            // @status int = NULL
            var upit = _session.CreateSQLQuery("exec PregledPorudzbenica :count, :take, :skip, :sortfield, :sortdir, :tip, :broj, :kupac , :mestoIsporuke, :datum1, :datum2, :datumVazenja1, :datumVazenja2, :status");
            upit.SetParameter("count", false, NHibernateUtil.Boolean);
            upit.SetParameter("take", kr.Take, NHibernateUtil.Int32);
            upit.SetParameter("skip", kr.Skip, NHibernateUtil.Int32);
            upit.SetParameter("sortfield", "Datum", NHibernateUtil.String);
            upit.SetParameter("sortdir", "ASC", NHibernateUtil.String);
            upit.SetParameter("tip", null, NHibernateUtil.String);
            upit.SetParameter("broj", null, NHibernateUtil.Int32);
            upit.SetParameter("kupac", null, NHibernateUtil.Int32);
            upit.SetParameter("mestoIsporuke", null, NHibernateUtil.Int32);
            upit.SetParameter("datum1", null, NHibernateUtil.Date);
            upit.SetParameter("datum2", null, NHibernateUtil.Date);
            upit.SetParameter("datumVazenja1", null, NHibernateUtil.Date);
            upit.SetParameter("datumVazenja2", null, NHibernateUtil.Date);
            upit.SetParameter("status", null, NHibernateUtil.Int32);

            if (kr.Filter != null && kr.Filter.Filters.Any())
            {
                foreach (FilterDescription filter in kr.Filter.Filters)
                {
                    string prop = filter.Field;

                    if (prop ==("datum"))
                    {
                        var d = Convert.ToDateTime(filter.Value);
                        //filter.Value = d.ToLocalTime().ToString("yyyyMMdd");
                        if (filter.Operator == "gte")
                        {
                             upit.SetParameter("datum1", d, NHibernateUtil.Date);
                        }
                        if (filter.Operator == "lte")
                        {
                            //d = d.AddHours(23).AddMinutes(59).AddSeconds(59);

                            //upit.And(x => x.Datum <= d);
                            //rows.And(x => x.Datum <= d);
                             upit.SetParameter("datum2", d, NHibernateUtil.Date);
                        }
                    }if (prop ==("datumVazenja"))
                    {
                        var d = Convert.ToDateTime(filter.Value);
                        //filter.Value = d.ToLocalTime().ToString("yyyyMMdd");
                        if (filter.Operator == "gte")
                        {
                             upit.SetParameter("datumVazenja1", d, NHibernateUtil.Date);
                        }
                        if (filter.Operator == "lte")
                        {
                            //d = d.AddHours(23).AddMinutes(59).AddSeconds(59);

                            //upit.And(x => x.Datum <= d);
                            //rows.And(x => x.Datum <= d);
                             upit.SetParameter("datumVazenja2", d, NHibernateUtil.Date);
                        }
                    }
                    else
                    {
                        upit.SetParameter(prop, filter.Value);
                    }

                }
            }
            if (kr.Sort.Any())
            {
                foreach (var sort in kr.Sort)
                {
                    
                    upit.SetParameter("sortfield", sort.Field);
                    upit.SetParameter("sortdir", sort.Dir );
                }
            }
            var lista = upit.SetResultTransformer(Transformers.AliasToBean(typeof(PorudzbenicaDto))).List<PorudzbenicaDto>();
            var res = new KendoResult<PorudzbenicaDto>
            {
                Data = lista,
                Total = lista.Count
            };
            return res;
        }
        [HttpPost]
        [Route("[Action]")]
        public KendoResult<Porudzbenica> PregledGrid([FromBody]KendoRequest kr)
        {
            Porudzbenica dok = null;
            Subjekt Kupac = null;
            Subjekt MestoIsporuke = null;
            //Status Status = null;
            var upit = _session.QueryOver<Porudzbenica>(() => dok)
              .JoinAlias(x => x.Kupac, () => Kupac, JoinType.InnerJoin)
              .JoinAlias(x => x.MestoIsporuke, () => MestoIsporuke, JoinType.InnerJoin)
              //.JoinAlias(x => x.Status, () => Status, JoinType.InnerJoin)
              .Where(x => !x.Obrisan);
            var rows = _session.QueryOver<Porudzbenica>(() => dok)
              .JoinAlias(x => x.Kupac, () => Kupac, JoinType.InnerJoin)
              .JoinAlias(x => x.MestoIsporuke, () => MestoIsporuke, JoinType.InnerJoin)
              //.JoinAlias(x => x.Status, () => Status, JoinType.InnerJoin)
              .Where(x => !x.Obrisan);
            if (kr.Filter != null && kr.Filter.Filters.Any())
            {
                foreach (FilterDescription filter in kr.Filter.Filters)
                {


                    string prop = filter.Field.UpperCaseFirst();
                    if (prop.Contains("Kupac"))
                    {
                        upit.AndRestrictionOn(() => Kupac.Naziv).IsInsensitiveLike(filter.Value, MatchMode.Anywhere);
                        rows.AndRestrictionOn(() => Kupac.Naziv).IsInsensitiveLike(filter.Value, MatchMode.Anywhere);
                    }
                    else if (prop.Contains("MestoIsporuke"))
                    {
                        upit.AndRestrictionOn(() => MestoIsporuke.Naziv).IsInsensitiveLike(filter.Value, MatchMode.Anywhere);
                        rows.AndRestrictionOn(() => MestoIsporuke.Naziv).IsInsensitiveLike(filter.Value, MatchMode.Anywhere);
                    }
                    else if (prop.Contains("Status"))
                    {
                        upit.And(x => x.Status.Id == int.Parse(filter.Value));
                        rows.And(x => x.Status.Id == int.Parse(filter.Value));
                    }
                    else if (prop.Contains("Datum"))
                    {
                        var d = Convert.ToDateTime(filter.Value);
                        filter.Value = d.ToLocalTime().ToString("yyyyMMdd");
                        if (filter.Operator == "gte")
                        {
                            upit.And(x => x.Datum >= d);
                            rows.And(x => x.Datum >= d);
                        }
                        if (filter.Operator == "lte")
                        {
                            d = d.AddHours(23).AddMinutes(59).AddSeconds(59);

                            upit.And(x => x.Datum <= d);
                            rows.And(x => x.Datum <= d);
                        }
                    }
                    else
                    {
                        upit.And(Restrictions.InsensitiveLike(prop, filter.Value, MatchMode.Anywhere));
                        rows.And(Restrictions.InsensitiveLike(prop, filter.Value, MatchMode.Anywhere));
                    }

                }
            }
            if (kr.Sort.Any())
            {
                foreach (Sort sort in kr.Sort)
                {
                    string prop = sort.Field.UpperCaseFirst();
                    if (prop.Contains("Kupac"))
                        prop = "Kupac.Naziv";
                    if (prop.Contains("MestoIsporuke"))
                        prop = "MestoIsporuke.Naziv";
                    if (prop.Contains("Status"))
                        prop = "Status.Id";
                    upit.UnderlyingCriteria.AddOrder(new Order(prop, sort.Dir.ToLower() == "asc"));
                }
            }
            else
            {
                upit.UnderlyingCriteria.AddOrder(new Order("dok.Id", false));
            }

            upit.Future<Porudzbenica>();


            rows.Select(Projections.Count(Projections.Id()));

            var redova = rows.FutureValue<int>().Value;

            var lista = upit.List<Porudzbenica>();
            //_logger.LogInformation(JsonConvert.SerializeObject(lista,
            //    new JsonSerializerSettings
            //    {
            //        ContractResolver = new NHibernateContractResolver(),
            //        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            //        DateFormatHandling = DateFormatHandling.IsoDateFormat,
            //        DateTimeZoneHandling = DateTimeZoneHandling.Local
            //    }));
            //opt.SerializerSettings.ContractResolver = new Helper.NHibernateContractResolver();
            //opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //opt.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            //opt.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            //opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            var res = new KendoResult<Porudzbenica>
            {
                Data = lista,
                Total = redova
            };
            return res;

        }

        [HttpPost]
        public ActionResult Post([FromBody]Porudzbenica porudzbenica)
        {
            var anid = Guid.NewGuid();

            if (porudzbenica.Uid == null)
                porudzbenica.Uid = anid;

            if (porudzbenica.Id == 0)
                porudzbenica.Broj = Helper.RedniBroj(_session, porudzbenica.Tip);


            using (var trans = _session.BeginTransaction())
            {
                try
                {
                    foreach (var stavka in porudzbenica.Stavke.ToList().Where(s => !s.Obrisan))
                    {
                        if (stavka.Ident == null)
                        {
                            porudzbenica.Stavke.Remove(stavka);
                        }
                        else
                        {
                            stavka.Porudzbenica = porudzbenica;
                            _session.SaveOrUpdate(stavka);
                        }
                    }
                    _session.SaveOrUpdate(porudzbenica);
                    trans.Commit();
                    return Ok(porudzbenica);
                }
                catch (System.Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return BadRequest(porudzbenica);
                }


            }

        }
        [HttpPost]
        [Route("[Action]")]
        public ActionResult SnimiStavke([FromQuery] int id, [FromBody] List<PorudzbenicaStavka> stavke)
        {
            var porudzbenica = _session.Get<Porudzbenica>(id);

            using (var trans = _session.BeginTransaction())
            {
                try
                {
                    //_session.SaveOrUpdate(stavke);
                    foreach (var stavka in stavke.Where(s => !s.Obrisan))
                    {
                        stavka.Porudzbenica = porudzbenica;
                        _session.SaveOrUpdate(stavka);
                    }
                    trans.Commit();
                    return Ok(stavke);
                }
                catch (System.Exception)
                {
                    return BadRequest(porudzbenica);
                }


            }

        }
        // foreach (var stavka in porudzbenica.Stavke.Where(s => !s.Obrisan))
        // {
        //     stavka.Porudzbenica = porudzbenica;
        //     _session.SaveOrUpdate(stavka);
        // }
        // using (var trans = _session.BeginTransaction())
        // {
        //     _session.SaveOrUpdate(porudzbenica);
        //     trans.Commit();
        //     return Ok(porudzbenica);
        // }

        [HttpPost]
        [Route("[Action]")]
        public KendoResult<PorudzbenicaStavka> StavkePregledGrid([FromBody]KendoRequest kr) //Get([FromUri] FilterContainer filter, int take, int skip, int page, int pageSize)
        {

            var upit = _session.QueryOver<PorudzbenicaStavka>().Where(x => x.Obrisan == false);
            var rowcount = _session.QueryOver<PorudzbenicaStavka>();
            upit.Where(x => x.Obrisan == false);
            TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
            //Dokument rez1 = null;
            if (kr.Filter != null && kr.Filter.Filters.Any())
            {
                foreach (FilterDescription filter in kr.Filter.Filters)
                {
                    //upit.JoinAlias(x => x.Prijemnica, () => rez1, NHibernate.SqlCommand.JoinType.InnerJoin)
                    //    .Where(x => x.Prijemnica.Id == int.Parse(filter.Value));
                    //.AndRestrictionOn(() => rez1.Id == int.Parse(filter.Value));
                    //rowcount.JoinAlias(x => x.Prijemnica, () => rez1, NHibernate.SqlCommand.JoinType.InnerJoin)
                    //    .Where(x => x.Prijemnica.Id == int.Parse(filter.Value));
                    //.AndRestrictionOn(() => rez1.Id == int.Parse(filter.Value));

                    string prop = filter.Field;
                    upit.And(Restrictions.Eq(prop, int.Parse(filter.Value)));
                    rowcount.And(Restrictions.Eq(prop, int.Parse(filter.Value)));
                }
            }

            upit.Skip(kr.Skip);
            upit.Take(kr.Take);

            if (kr.Sort.Any())
            {
                foreach (Sort sort in kr.Sort)
                {
                    string prop = textInfo.ToTitleCase(sort.Field);
                    upit.UnderlyingCriteria.AddOrder(new Order(prop, sort.Dir.ToLower() == "asc"));
                }
            }

            upit.Future<PorudzbenicaStavka>();


            rowcount.Select(Projections.Count(Projections.Id()));

            var redova = rowcount.FutureValue<int>().Value;

            var lista = upit.List<PorudzbenicaStavka>();
            var res = new KendoResult<PorudzbenicaStavka>
            {
                Data = lista,
                Total = redova
            };
            return res;
        }
    }
}

