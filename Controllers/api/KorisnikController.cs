using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NHibernate;
//using VoxServis.Services;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;
//using ISession = NHibernate.ISession;
using System.Globalization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using NHibernate.Criterion;

using webkom.Data;
using webkom.Models;
using webkom.Helper;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace webkom.Controllers.api
{
  [Route("api/[controller]")]
  [Authorize("Bearer")]
  public class KorisnikController : Controller
  {
    private readonly ILogger _logger;
    private readonly ISession _session;
    private readonly KorisnikManager _userManager;
    public KorisnikController(ISession session, ILoggerFactory loggerFactory, KorisnikManager userManager)
    {
      _logger = loggerFactory.CreateLogger<KorisnikController>();
      _session = session;
      _userManager = userManager;
    }

    // [HttpGet("{id}")]
    // public Korisnik Get(int id)
    // {
    //   var korisnik = _session.Get<Korisnik>(id);
    //   return korisnik;
    // }
    [HttpGet("{id}")]
    public Korisnik Get(string id)
    {
        var upit = _session.QueryOver<Korisnik>()
            .Where(x => x.Obrisan == false && x.KorisnickoIme == id);
        //.And(x => x.Servis.CentralniEtg == true);
        var lista = upit.List<Korisnik>().FirstOrDefault();
        return lista;
    }
    [HttpGet()]
    [Route("[Action]")]
    public IList<Korisnik> ListaKorisnika()
    {
      var query = _session.QueryOver<Korisnik>()
                  .Where(x => !x.Obrisan)
                  .And(x => x.Aktivan);
      var lista = query.List<Korisnik>();
      return lista;
    }

    [HttpPost]
    public Korisnik Post([FromBody] Korisnik korisnik)
    {
      _userManager.KorisnikSnimi(korisnik);
      return korisnik;
    }
    [HttpPut]
    public Korisnik Put([FromBody] Korisnik korisnik)
    {
      _userManager.KorisnikSnimi(korisnik);
      return korisnik;
    }

    [HttpPost]
    [Route("[Action]")]
    public ActionResult ResetLozinke([FromBody] Korisnik korisnik)
    {
      //var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
      if (!User.IsInRole("Administrator"))
        return Unauthorized();

      var result = _userManager.KorisnikResetLozinke(korisnik.KorisnickoIme, "Test123");

      if (!result) return BadRequest("Greška prilikom reseta lozinke!");

      return Ok("Uspešno promenjena lozinka");

    }

    [HttpPost]
    [Route("[Action]")]
    public ActionResult PromenaLozinke([FromBody] JObject novaLozinka)
    {
      //var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
      //if (!User.IsInRole("Administrator"))
      //    return Unauthorized();
      //var user = _session.QueryOver<Korisnik>()
      //    .Where(u => u.KorisnickoIme == User.Identity.Name || u.Email == User.Identity.Name)
      //    .List<Korisnik>()
      //    .FirstOrDefault();

      //if (user == null)
      //{
      //    return BadRequest("Greška prilikom postavljanja lozinke!");
      //}


      return Ok("Uspešno promenjena lozinka");

    }
  }
}
