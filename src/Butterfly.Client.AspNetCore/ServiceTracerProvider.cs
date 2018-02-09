﻿using System.Net;
using Butterfly.Client.Tracing;
using Butterfly.OpenTracing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace Butterfly.Client.AspNetCore
{
    public class ServiceTracerProvider : IServiceTracerProvider
    {
        private readonly ITracer _tracer;
        private readonly ButterflyOptions _options;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ServiceTracerProvider(ITracer tracer, IHostingEnvironment hostingEnvironment, IOptions<ButterflyOptions> options)
        {
            _tracer = tracer;
            _options = options.Value;
            _hostingEnvironment = hostingEnvironment;
        }

        public IServiceTracer GetServiceTracer()
        {
            var environmentName = _hostingEnvironment.EnvironmentName;
            var host = Dns.GetHostName();
            var identity = string.IsNullOrEmpty(_options.ServiceIdentity) ? $"{host}@{_hostingEnvironment.ApplicationName}" : _options.ServiceIdentity;
            return new ServiceTracer(_tracer, _options.Service, environmentName, identity, host);
        }
    }
}