﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

using Arriba.Monitoring;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Arriba.Communication
{
    internal class RequestContext : IRequestContext
    {
        private readonly ITelemetry _telemetry;

        private IRequest _request;

        public RequestContext(IRequest request)
        {
            _telemetry = new Telemetry(MonitorEventLevel.Verbose, "HTTP", null);
            _request = request;
        }

        public IRequest Request
        {
            get
            {
                return _request;
            }
        }

        public IDictionary<string, double> TraceTimings
        {
            get
            {
                return _telemetry.MonitorEvents().GroupBy(e => e.Start.Name)
                                  .ToDictionary(e => e.Key, e => e.Sum(s => s.Stop == null ? s.CurrentRuntime : s.Stop.RuntimeMilliseconds), StringComparer.OrdinalIgnoreCase);
            }
        }

        public IDisposable Monitor(MonitorEventLevel level, string name, string type = null, string identity = null, object detail = null)
        {
            return _telemetry.Monitor(level, name, type, identity, detail);
        }

        public List<MonitorEventScope> MonitorEvents() => _telemetry.MonitorEvents();

    }
}
