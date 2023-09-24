using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientTest
{
    internal class TestService
    {
        public string host = "localhost";
        private HttpClient _client;

        public TestService(IHttpClientFactory clientFactory)
        {
           _client =  clientFactory.CreateClient("testClient");
        }

        public async Task<string> GetString()
        {
            return await _client.GetStringAsync("/test");
        }

        public async Task<string> GetHeader()
        {
            return await _client.GetStringAsync("/testHeader");
        }
    }
}
