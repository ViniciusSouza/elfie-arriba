using Arriba.Communication.Server.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Arriba.Test.Services
{
    [TestClass]
    public partial class ArribaQueryServicesTests : ArribaServiceBase
    {
        private readonly IArribaQueryServices _queryService;
        private readonly NameValueCollection parameters = new NameValueCollection();
        public ArribaQueryServicesTests() : base()
        {
            _queryService = _host.GetService<IArribaQueryServices>();

        }
    }
}
