using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NHibernate;
using System.Security;
using webkom.Models;

namespace webkom.Helper
{
  public class KorisnikManager
  {
    private readonly ISession _session;

    public KorisnikManager(ISession session)
    {
      _session = session;
    }
    public Korisnik KorisnikPostoji(string korisnickoIme)
    {
      var query = _session.CreateSQLQuery("exec KorisnikPostoji :korisnickoime");
      query.SetParameter("korisnickoime", korisnickoIme, NHibernateUtil.String);
      query.AddEntity(typeof(Korisnik));
      return query.UniqueResult<Korisnik>();
    }
    public bool KorisnikSnimi(Korisnik korisnik)
    {
      var query = _session.CreateSQLQuery("exec KorisnikSnimi :korisnickoime, :lozinka, :email, :ime, :prezime, :naziv, :aktivan, :uloga, :azukor");
      query.SetParameter("korisnickoime", korisnik.KorisnickoIme, NHibernateUtil.String);
      query.SetParameter("lozinka", korisnik.Lozinka, NHibernateUtil.String);
      query.SetParameter("email", korisnik.Email, NHibernateUtil.String);
      query.SetParameter("ime", korisnik.Ime, NHibernateUtil.String);
      query.SetParameter("prezime", korisnik.Prezime, NHibernateUtil.String);
      query.SetParameter("naziv", korisnik.Naziv, NHibernateUtil.String);
      query.SetParameter("aktivan", korisnik.Aktivan, NHibernateUtil.Boolean);
      query.SetParameter("uloga", korisnik.Uloga, NHibernateUtil.String);
      query.SetParameter("azukor", HttpHelper.HttpContext?.User?.Identity?.Name ?? "admin", NHibernateUtil.String);
      return query.UniqueResult<bool>();
      
    }
    public bool KorisnikProvera(string korisnickoIme, string lozinka)
    {
      var query = _session.CreateSQLQuery("exec KorisnikProveraLozinke :korisnickoime, :lozinka");
      query.SetParameter("korisnickoime", korisnickoIme, NHibernateUtil.String);
      query.SetParameter("lozinka", lozinka, NHibernateUtil.String);
      return query.UniqueResult<bool>();
    }

    public bool KorisnikPromenaLozinke(string korisnickoIme, string lozinka, string staralozinka)
    {
      var query = _session.CreateSQLQuery("exec KorisnikPromenaLozinke :korisnickoime, :staralozinka, :lozinka");
      query.SetParameter("korisnickoime", korisnickoIme, NHibernateUtil.String);
      query.SetParameter("staralozinka", staralozinka, NHibernateUtil.String);
      query.SetParameter("lozinka", lozinka, NHibernateUtil.String);
      return query.UniqueResult<bool>();
    }

    public bool KorisnikResetLozinke(string korisnickoIme, string lozinka)
    {
      var query = _session.CreateSQLQuery("exec KorisnikResetLozinke :korisnickoime, :staralozinka, :lozinka");
      query.SetParameter("korisnickoime", korisnickoIme, NHibernateUtil.String);
      query.SetParameter("lozinka", lozinka, NHibernateUtil.String);
      return query.UniqueResult<bool>();
    }
  }
}
