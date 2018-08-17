using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
//using Microsoft.Extensions.Logging;
using NHibernate.AdoNet;
using NHibernate.Driver;
//using Microsoft.Extensions.Logging;
//using Serilog;
using Microsoft.Extensions.Logging;


namespace webkom.Helper
{
  public class LoggerSqlClientDriver : SqlClientDriver, IEmbeddedBatcherFactoryProvider
  {
    private readonly ILogger _logger;

    public LoggerSqlClientDriver()
    {
      ILoggerFactory loggerFactory = HttpHelper.GetLoggerFactory;
      _logger = loggerFactory.CreateLogger<LoggerSqlClientDriver>();
    }

    public LoggerSqlClientDriver(ILoggerFactory loggerFactory)
    {
      //      _logger = loggerFactory.CreateLogger<LoggerSqlClientDriver>();
      _logger = loggerFactory.CreateLogger<LoggerSqlClientDriver>();
    }

    public override void AdjustCommand(DbCommand command)
    {

      _logger.LogInformation("SQL log: " + command.CommandText + " {@SqlParameter} ", command.Parameters);
      
      
      // foreach (DbParameter parameter in command.Parameters)
      // {
      //   _logger.LogInformation(parameter.ParameterName + " " + parameter.Value);
      // }
      base.AdjustCommand(command);
      //base.AdjustCommand(command);
    }



    //protected override void OnBeforePrepare(IDbCommand command)
    //{
    //    //log here
    //    base.OnBeforePrepare(command);
    //}
  }
}
