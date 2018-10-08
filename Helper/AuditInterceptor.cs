using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Transactions;
using FluentNHibernate.Data;
using FluentNHibernate.Utils;
using NHibernate;
using NHibernate.Type;
using NHibernate.Util;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NHibernate.SqlCommand;

using webkom.Helper;
using webkom.Hubs;
using webkom.Models;
using static Newtonsoft.Json.JsonConvert;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;
using ISession = NHibernate.ISession;

//using StandCert.Web.Hubs;
//using WebGrease.Css.Extensions;

namespace webkom.Helper
{

  [Serializable]
  public class AuditInterceptor : EmptyInterceptor  //EmptyInterceptorWithHub<AppHub>
  {
    private readonly ISession _session;
    private readonly ILogger _logger;
    private readonly IHttpContextAccessor _httpContext;

    public AuditInterceptor(ILoggerFactory loggerFactory)
    {
      //_session = NHibernateHelper.OpenSession();
      _logger = loggerFactory.CreateLogger<AuditInterceptor>();
    }
    public AuditInterceptor(ILoggerFactory loggerFactory, ISession sesession, IHttpContextAccessor httpContext)
    {
      _session = sesession;
      _logger = loggerFactory.CreateLogger<AuditInterceptor>();
      _httpContext = httpContext;
    }

    //private int _updates;
    //private int _creates;
    //private int _loads;
    //private readonly IHttpContextAccessor _contextAccessor;

    public override void OnDelete(object entity,
                                  object id,
                                  object[] state,
                                  string[] propertyNames,
                                  IType[] types)
    {
      if (entity is Entitet)
      {

        _logger.LogInformation("onDelete: " + entity.ToString() + " {@Entitet} ", entity);
        //return true;
      }
    }
    public class Promena
    {
      public Promena()
      {

      }
      public Promena(string entitet, string property, object id,  object staraVrednost, object novaVrednost)
      {
        Entitet = entitet;
        Property = property;
        Id = id;
        StaraVrednost = staraVrednost;
        NovaVrednost = novaVrednost;
      }
      public string Entitet { get; set; }
      public string Property { get; set; }
      public object Id { get; set; }
      public object StaraVrednost { get; set; }
      public object NovaVrednost { get; set; }
    }
    public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
    {
      //Log.Info("interceptuje update, user modified: " + Thread.CurrentPrincipal.Identity.Name);
      if (entity is Entitet)
      {
        //_updates++;

        //SetValue(currentState, propertyNames, "UserModified", HttpHelper.HttpContext?.User?.Identity?.Name ?? "");
        SetValue(currentState, propertyNames, "UserModified", _httpContext.HttpContext.User.Identity.Name);
        SetValue(currentState, propertyNames, "DateModified", DateTime.Now);



        return true;
      }
      if (entity is Meni || entity is MeniSettings)
      {
        var type = entity.GetType();
        var obj = _session.Get(type, id);
        var promene = new List<Promena>();
        for (var i = 0; i < currentState.Count(); i++)
        {
          if (types[i].IsCollectionType) continue;
          if (!currentState[i].Equals(previousState[i]))
          {
            promene.Add(new Promena() { Entitet = entity.ToString(), Id = id, StaraVrednost = previousState[i], NovaVrednost = currentState[i], Property = propertyNames[i] });
          }
        }
        _logger.LogInformation("onUpdate: " + entity.ToString() + ",id: " + id  + " {@Entitet} {@Promene}", entity, promene);
        return true;
      }
      return true;

    }
    // public override bool OnLoad(object entity, object id, object[] state, string[] propertyNames, IType[] types)
    // {
    //     if (entity is Entity)
    //     {
    //         _loads++;
    //     }
    //     return false;
    // }
    //private object GetVrednost(string property, Type type, Object obj)
    //{
    //  var staravrednost = new object();
    //  staravrednost = type.GetProperty(property).GetValue(obj, null);
    //  var sv = staravrednost as BazniEntitet;
    //  if (sv != null)
    //  {
    //    staravrednost = SerializeObject(sv,
    //        new JsonSerializerSettings
    //        {
    //          ContractResolver = new NHibernateContractResolver(),
    //          ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
    //          DateFormatHandling = DateFormatHandling.IsoDateFormat,
    //          DateTimeZoneHandling = DateTimeZoneHandling.Local
    //        });
    //  }
    //  return sv;
    //}
    public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
    {
      //Log.Info("interceptuje insert, user created: " + Thread.CurrentPrincipal.Identity.Name);
      if (entity is Entitet)
      {
        //SetValue(state, propertyNames, "UserCreated", HttpHelper.HttpContext?.User?.Identity?.Name ?? "");
        SetValue(state, propertyNames, "UserCreated", _httpContext.HttpContext.User.Identity.Name);
        SetValue(state, propertyNames, "DateCreated", DateTime.Now);
        SetValue(state, propertyNames, "DateModified", DateTime.Now);
        SetValueIfNull(state, propertyNames, "Zakljucan", false);
        SetValueIfNull(state, propertyNames, "Obrisan", false);

        _logger.LogInformation("onInsert: " + entity.ToString() + " {@Entitet} ", entity);

        return true;
      }
      if (entity is Meni)
      {
        _logger.LogInformation("onInsert: " + entity.ToString() + " {@Entitet} ", entity);
      }
      return true;
    }

    private void SetValue(Object[] currentState, IEnumerable<string> propertyNames, String propertyToSet, Object value)
    {
      var index = propertyNames.ToList().IndexOf(propertyToSet);
      if (index >= 0)
      {
        currentState[index] = value;
      }
    }

    private void SetValueIfNull(Object[] currentState, IEnumerable<string> propertyNames, String propertyToSet, Object value)
    {
      var index = propertyNames.ToList().IndexOf(propertyToSet);
      if (index >= 0)
      {
        if (currentState[index] == null)
        {
          currentState[index] = value;
        }
      }
    }

    // private void Izmene(Object[] currentState, IEnumerable<string> propertyNames)
    // {

    // }
    // public override void AfterTransactionCompletion(ITransaction tx)
    // {
    //     //if (tx.WasCommitted)
    //     //{
    //     //    //System.Console.WriteLine("Creations: " + creates + ", Updates: " + updates, "Loads: " + loads);
    //     //    Log.Info("Creations: " + _creates + ", Updates: " + _updates + "Loads: " + _loads);
    //     //}
    //     //_updates = 0;
    //     //_creates = 0;
    //     //_loads = 0;
    // }
    // public override SqlString OnPrepareStatement(SqlString sql)
    // {

    //     //Debug.WriteLine(sql);
    //     //_logger.LogError("Proba");
    //     //_logger.LogInformation(sql.ToString());
    //     return sql;
    // }
  }
}
