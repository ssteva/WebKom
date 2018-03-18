using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using webkom.Data;
using webkom.Models;
using System.Security;
using NHibernate;

namespace webkom.Helper
{
  public class Seed
  {
    private readonly ISession _session;
    private readonly KorisnikManager _userManager;
  
    // public Seed(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ISession session)
    // {
    //   _session = session;
    //   _roleManager = roleManager;
    //   _userManager = userManager;

    // }
        public Seed(KorisnikManager userManager, ISession session)
    {
      _session = session;
      _userManager = userManager;

    }
    public void SeedMenu()
    {
      // var m = _session.Query<Meni>().ToList();
      // if(m.Any()) return;
      
      // var homeSettings = new MeniSettings("home", "Administrator, Supervizor, Komercijalista");
      // var home = new Meni(1, ",Home", "home", "#home", "pages/home/home", "Home", true, true, homeSettings, null);

      // var administracijaSettings = new MeniSettings("wrench", "Administrator, Supervizor");
      // var administracija = new Meni(2, "-", "-", "#home", "pages/home/home", "Administracija", true, true, administracijaSettings, null);

      // var korisniciSettings = new MeniSettings("users", "Administrator, Supervizor");
      // var korisnici = new Meni(3, "korisnici", "korisnici", "#korisnici", "pages/administracija/korisnici/korisnici", "Korisnici", true, true, korisniciSettings, administracija);

      // administracija.Podmeni.Add(korisnici);


      // _session.Save(home);
      // _session.Save(homeSettings);
      // _session.Save(korisnici);
      // _session.Save(korisniciSettings);
      // _session.Save(administracija);
      // _session.Save(administracijaSettings);

      // _session.Flush();
      var query = _session.CreateSQLQuery("exec seed_meni");
      query.ExecuteUpdate();

      
    }
    public void SeedIdent(){
      var query = _session.CreateSQLQuery("exec seed_ident");
      query.ExecuteUpdate();

    }
    public void SeedSubjekt(){
      var query = _session.CreateSQLQuery("exec seed_subjekt");
      query.ExecuteUpdate();
    }
    public void SeedUsers()
    {

      // var dev = new ApplicationUser()
      // {
      //   Aktivan = true,
      //   Email = "stevo.sudjic@gmail.com",
      //   Ime = "Dev",
      //   Uloga = "Administrator",
      //   UserName = "dev"
      // };
      // SeedUser(dev, "ninja", "Administrator");
      var dev = new Korisnik()
      {
        PanteonId = 0,
        Aktivan = true,
        Email = "stevo.sudjic@gmail.com",
        Ime = "Dev",
        Prezime="",
        Naziv= "dev",
        Uloga = "Administrator",
        KorisnickoIme = "dev",
        Lozinka = "ninja"
      };
      SeedUserN(dev);
      var admin = new Korisnik()
      {
        PanteonId = 0,
        Aktivan = true,
        Email = "stevo.sudjic@gmail.com",
        Ime = "Administrator",
        Prezime="",
        Naziv= "dev",
        Uloga = "Administrator",
        KorisnickoIme = "admin",
        Lozinka = "admin"
      };
      SeedUserN(admin);
    }

    // private void SeedUser(ApplicationUser user, string pass, string role)
    // {
    //   var a = _userManager.FindByNameAsync(user.UserName).Result;
    //   if (a != null)
    //   {
    //     var obrisan = _userManager.DeleteAsync(a).Result;
    //   }
    //   var result = _userManager.CreateAsync(user, pass).Result;

    //   if (!result.Succeeded) return;
    //   var res1 = _userManager.AddToRoleAsync(user, role).Result;
    //   var res2 = _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role)).Result;
    // }
    private void SeedUserN(Korisnik user)
    {
      _userManager.KorisnikSnimi(user);
      //var res1 = _userManager.AddToRoleAsync(user, role).Result;
      //var res2 = _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role)).Result;
    }
    private void SeedRoles()
    {
      //if (_roleManager.RoleExistsAsync("Komercijalista").Result) return;
      //{
      //  var role = new IdentityRole { Name = "Komercijalista" };
      //  var roleResult = _roleManager.CreateAsync(role).Result;
      //}

      //if (_roleManager.RoleExistsAsync("Supervizor").Result) return;
      //{
      //  var role = new IdentityRole { Name = "Supervizor" };
      //  var roleResult = _roleManager.CreateAsync(role).Result;
      //}

      //if (_roleManager.RoleExistsAsync("Administrator").Result) return;
      //{
      //  var role = new IdentityRole { Name = "Administrator" };
      //  var roleResult = _roleManager.CreateAsync(role).Result;
      //}
    }
  }
}
