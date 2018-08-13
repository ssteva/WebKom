using System;

using System.IdentityModel.Tokens.Jwt;
//using System.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

//using System.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
//using Microsoft.AspNet.Authorization;
using System.Security.Principal;
using System.Threading.Tasks;
//using log4net;
//using Microsoft.AspNet.Authentication.JwtBearer;
//using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NHibernate;
using webkom.Helper;
//using Microsoft.IdentityModel.Tokens;
using webkom.Models;
//using SecurityTokenDescriptor = System.IdentityModel.Tokens.SecurityTokenDescriptor;

using SecurityTokenDescriptor = Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor;
using System.Collections.Generic;

namespace webkom.Controllers
{
  [Route("[controller]")]
  public class TokenController : Controller
  {
    private readonly TokenAuthOptions _tokenOptions;
    private readonly KorisnikManager _userManager;
    //private int _satiT = 6;
    //private int _satiRT = 7;
    private int _minT;
    private int _minRT;
    private readonly ISession _session;
    //private readonly IConfiguration _configuration;

    //private readonly ILog _log;

    //private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    public TokenController(TokenAuthOptions tokenOptions, KorisnikManager userManager, ISession session)
    {
      _tokenOptions = tokenOptions;
      _userManager = userManager;

      _session = session;
      _minT = tokenOptions.MinutaToken;
      _minRT = tokenOptions.MinuntaRefreshToken;
      // _configuration = confuguration;
      //this.bearerOptions = options.Value;
      //this.signingCredentials = signingCredentials;
    }


    public class AuthRequest
    {
      public string username { get; set; }
      public string password { get; set; }
    }
    public class RefreshTokenData
    {
      public string grant_type { get; set; }
      public string refresh_token { get; set; }
    }


    [HttpPost]
    [Route("[action]")]
    //[Authorize("Bearer")]
    public ActionResult RefreshToken([FromBody] RefreshTokenData refresh)
    {
      var re = Request;
      var headers = re.Headers;
      string original = null;
      if (headers.ContainsKey("Authorization"))
      {
        original = headers["Authorization"].ToString().Replace("Bearer ", "");
      }
  


      if (string.IsNullOrEmpty(original))
        return Unauthorized(); // new { authenticated = false });
      if (refresh.grant_type != "refresh_token")
        return Unauthorized();//new { authenticated = false };

      var handler = new JwtSecurityTokenHandler();


      var validationParametersRefresh = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidIssuer = _tokenOptions.Issuer,
        ValidateAudience = true,
        ValidAudience = _tokenOptions.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = _tokenOptions.Key,
        ValidateLifetime = true
      };
      var validationParametersOriginal = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidIssuer = _tokenOptions.Issuer,
        ValidateAudience = true,
        ValidAudience = _tokenOptions.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = _tokenOptions.Key,
        ValidateLifetime = false
      };

      ClaimsPrincipal principalOriginal = null;
      try
      {
        principalOriginal = handler.ValidateToken(original, validationParametersOriginal, out var tokiOriginal);

        handler.ValidateToken(original, validationParametersRefresh, out _);
      }
      catch
      {

        return Unauthorized(); // new {authenticated = false};
      }
      var ime = principalOriginal.Claims.First(c => c.Type == "Ime");
      var prezime = principalOriginal.Claims.First(c => c.Type == "Prezime");
      var username = principalOriginal.Claims.First(c => c.Type == ClaimTypes.Name);
      var email = principalOriginal.Claims.First(c => c.Type == ClaimTypes.Email);
      var userid = principalOriginal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);
      var subj = new ClaimsIdentity();

      var user = _userManager.KorisnikPostoji(username.Value);
      if (user == null) return Unauthorized(); //new { authenticated = false };

      //var roles = user.Claims.Where(c => c.ClaimType == ClaimTypes.Role).ToList();
      var roles = user.Uloge;

      subj.AddClaim(new Claim("Ime", ime.Value));
      subj.AddClaim(new Claim("Prezime", prezime.Value));
      subj.AddClaim(new Claim(ClaimTypes.Email, email.Value));
      subj.AddClaim(new Claim(ClaimTypes.Name, username.Value));
      subj.AddClaim(new Claim(JwtRegisteredClaimNames.UniqueName, username.Value));
      subj.AddClaim(new Claim(ClaimTypes.NameIdentifier, userid.Value));
      subj.AddClaims(roles.Select(r => new Claim(ClaimTypes.Role, r)));

      //var expires = DateTime.UtcNow.AddHours(_satiT);
      var expires = DateTime.UtcNow.AddMinutes(_minT);


      var accessToken = GetToken(username.Value, expires, subj);


      //expires = DateTime.UtcNow.AddHours(_satiRT);
      expires = DateTime.UtcNow.AddMinutes(_minRT);
      var refreshToken = GetRefreshToken(username.Value, expires);


      var query = _session.CreateSQLQuery("exec LogKorisnika :username, :akcija");
      query.SetParameter("username", username.Value, NHibernateUtil.String);
      query.SetParameter("akcija", "Refresh", NHibernateUtil.String);
      query.ExecuteUpdate();

      return Ok(new { authenticated = true, userId = userid.Value, access_token = accessToken, refresh_token = refreshToken });


    }

    [HttpPost]
    public ActionResult Post([FromBody] AuthRequest req)
    {
      var user = _userManager.KorisnikPostoji(req.username);

      if (user==null) return Unauthorized(); //new { authenticated = false };

      //var roles = user.Claims.Where(c => c.ClaimType == ClaimTypes.Role).ToList();

      var roles = user.Uloge;


      var logovan = _userManager.KorisnikProvera(req.username, req.password);
      if (!logovan) return Unauthorized(); //new { authenticated = false };

      //var minutes = _configuration.GetSection("AuthSettings")["TokenValid"];
      //DateTime expires = DateTime.UtcNow.AddHours(_satiT);
      var expires = DateTime.UtcNow.AddMinutes(_minT);

      try
      {
        var subj = new ClaimsIdentity(new Claim[]
        {
                  new Claim("Ime", user.Ime ),
                  new Claim("Prezime", user.Prezime ?? "" ),
                  new Claim(ClaimTypes.Email, user.Email),
                  new Claim(ClaimTypes.Name, user.KorisnickoIme),
                  new Claim(JwtRegisteredClaimNames.UniqueName, user.KorisnickoIme),
                  new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        });
        subj.AddClaims(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var accessToken = GetToken(req.username, expires, subj);

        //expires = DateTime.UtcNow.AddHours(_satiRT);
        expires = DateTime.UtcNow.AddMinutes(_minRT);
        var refreshToken = GetRefreshToken(req.username, expires);


        var query = _session.CreateSQLQuery("exec LogKorisnika :username, :akcija");
        query.SetParameter("username", user.KorisnickoIme);
        query.SetParameter("akcija", "Login");
        query.ExecuteUpdate();

        return Ok(new { authenticated = true, userId = user.Id, access_token = accessToken, refresh_token = refreshToken, roles = roles });

      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return BadRequest();
      }


    }

    private string GetToken(string user, DateTime expires, ClaimsIdentity claims)
    {
      var handler = new JwtSecurityTokenHandler();
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Issuer = _tokenOptions.Issuer,
        Audience = _tokenOptions.Audience,
        SigningCredentials = _tokenOptions.SigningCredentials,
        Subject = claims,
        Expires = expires
      };
      var toki = handler.CreateToken(tokenDescriptor);

      return handler.WriteToken(toki);
    }
    private string GetRefreshToken(string user, DateTime expires)
    {
      var handler = new JwtSecurityTokenHandler();
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Issuer = _tokenOptions.Issuer,
        Audience = _tokenOptions.Audience,
        SigningCredentials = _tokenOptions.SigningCredentials,
        Expires = expires
      };
      var toki = handler.CreateToken(tokenDescriptor);

      return handler.WriteToken(toki);
    }
  }
}
