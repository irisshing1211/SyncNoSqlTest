using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using client.Entities;
using Newtonsoft.Json;

namespace client.Events
{
    public class SyncEvent : IEvents
    {
        private List<AccountHistory> _histories;
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _url;
        private readonly DateTime? _lastSync;
        public List<AccountHistory> Histories => _histories;

        public SyncEvent(IHttpClientFactory clientFactory, string url, DateTime? lastSync)
        {
            _clientFactory = clientFactory;
            _url = url;
            _lastSync = lastSync;
        }
        public async Task<bool> Push()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_url}/{_lastSync.ToString()}");
            var client = _clientFactory.CreateClient();

            var response = await client.GetAsync(_url);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                _histories = JsonConvert.DeserializeObject<List<AccountHistory>>(responseString);

                return true;
            }
            else { _histories = new List<AccountHistory>(); }

            return false;
        }
    }
}
