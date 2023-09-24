using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientTest
{
    internal class LinliuService
    {
        private readonly HttpClient _client;

        public LinliuService(HttpClient httpClient)
        {
            _client = httpClient;
        }

        public async Task<string> GetString()
        {
            return await _client.GetStringAsync("/test");
        }

        public async Task<string> GetHeader()
        {
            return await _client.GetStringAsync("/testHeader");
        }

        public async Task<string> TestPost()
        {
            var res = await _client.PostAsync("/post", JsonContent.Create(new { kui = "bule"}));

            return await res.Content.ReadAsStringAsync();
        }
    }
}
