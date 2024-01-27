using iText.Svg.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DAMSecurityTest
{
    public class UnitTestSocket
    {
        [Test]
        public void TestSocketListReports()
        {
            DAMUtils.Socket.ServerExtended server;
            DAMUtils.Socket.Client client;
            int numReports = 0;
            server = new DAMUtils.Socket.ServerExtended(IPAddress.Any, 12345);
            client = new DAMUtils.Socket.Client("localhost", 12345);

            Thread thServer = new Thread(() => { server.Start(); });
            thServer.Start();
            Thread thClient = new Thread(() => {
                var reports = client.GetAvailableReports();
                numReports = reports.Count;
            });
            thClient.Start();
            thClient.Join();
            
            Assert.IsTrue(numReports == 2);
        }
    }
}
