﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webkom.Helper
{
  public static class HttpHelper
  {
    private static IHttpContextAccessor HttpContextAccessor;
    private static ILoggerFactory _loggerFactory;
    public static void Configure(IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory)
    {
      HttpContextAccessor = httpContextAccessor;
      _loggerFactory = loggerFactory;
    }

    public static ILoggerFactory GetLoggerFactory
    {
      get
      {
        return _loggerFactory;
      }
    }

    public static HttpContext HttpContext
    {
      get
      {
        return HttpContextAccessor.HttpContext;
      }
    }
  }
}
