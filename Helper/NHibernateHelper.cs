using System;
using System.IO;
using System.Text;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using webkom.Mapping;
using webkom.Models;
using Environment = NHibernate.Cfg.Environment;
using NHibernate.Tool.hbm2ddl;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;
using Microsoft.Extensions.Logging;
using webkom.Helper;

namespace webkom.Helper
{
  public class NHibernateHelper
  {
    private static ISessionFactory _session;
    private static Configuration _configuration;
    private static string _connection;
    private readonly ILogger _logger;
    private readonly ILoggerFactory _loggerFactory;

    public NHibernateHelper()
    {

    }
    public NHibernateHelper(ILoggerFactory loggerFactory)
    {
      _logger = loggerFactory.CreateLogger<NHibernateHelper>();
      _loggerFactory = loggerFactory;
    }

    //Data Source=.\SQLEXPRESS;Initial Catalog=CodeCamper;Integrated Security=True;providerName='System.Data.SqlClient'
    private static readonly Action<string> ShemaCreate = x =>
    {
      using (var file = new FileStream(@"C:\Log\ShemaCreate.sql", FileMode.Append, FileAccess.Write))
      using (var sw = new StreamWriter(file))
      {
        sw.Write(x);
      }
    };
    private static Action<string> _shemaUpdate = x =>
    {
      using (var file = new FileStream(@"C:\Log\ShemaUpdate.sql", FileMode.Append, FileAccess.Write))
      using (var sw = new StreamWriter(file))
      {
        sw.Write(x);
      }
    };
    private static void CreateSessionFactory()
    {

      _configuration = Fluently.Configure()
          .Database(MsSqlConfiguration.MsSql2012.Driver<LoggerSqlClientDriver>()
              .ConnectionString(_connection
              //c => c.FromConnectionStringWithKey("TestConnection")
              //     c => c.FromConnectionStringWithKey("TestConnection2")
              //@"Data Source=10.100.129.144\bossql;Initial Catalog=PrijavaKvara;USER ID=ssis;PASSWORD=ssis"
              //@"Data Source=10.100.129.53;Initial Catalog=MasterOrfej;USER ID=ssis;PASSWORD=ssis"
              //@"Data Source=(local);Initial Catalog=PrijavaKvara;USER ID=ssis;PASSWORD=ssis"
              ).ShowSql)
          //.Mappings(m => m.FluentMappings.Conventions.Setup(x => x.Add(AutoImport.Never())))
          //.Mappings(m => m.HbmMappings.AddFromAssemblyOf<Otpis>())
          .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Entitet>()
          .Conventions.Add<CustomForeignKeyConvention>())
          //.Conventions.Add<i>())

          .ExposeConfiguration(cfg => cfg.SetProperty(
              Environment.CurrentSessionContextClass,
              "web")).BuildConfiguration();//.SetInterceptor(new AuditInterceptor());

      //export
      //var exporter = new SchemaExport(_configuration);
      //exporter.Execute(ShemaCreate, true, false);
      //update
      //var update = new SchemaUpdate(_configuration);
      //update.Execute(true, true);
      //var a = MsSqlConfiguration.MsSql2012.Driver<LoggerSqlClientDriver>();
      _session = _configuration.BuildSessionFactory();
      
    }

    public static ISession OpenSession()
    {
      return SessionFactory.OpenSession(); //new AuditInterceptor()
    }
    public static ISessionFactory OpenSessionFactory()
    {
      return SessionFactory;
    }

    public static Configuration GetConfiguration()
    {
      return Configuration;
    }
    private static ISessionFactory SessionFactory
    {
      get
      {
        if (_session == null)
          CreateSessionFactory();
        return _session;
      }
    }
    public static string ConnectionString
    {
      set => _connection = value;
    }
    private static Configuration Configuration
    {
      get
      {
        if (_configuration == null)
          CreateSessionFactory();
        return _configuration;
      }
    }


  }

}
