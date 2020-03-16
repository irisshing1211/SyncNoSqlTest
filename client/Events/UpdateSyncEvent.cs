using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using client.Entities;
using client.Models;
using Newtonsoft.Json;

namespace client.Events
{
    /// <summary>
    /// call cloud update with sync api to update data to cloud + get latest histories
    /// </summary>
    public class UpdateSyncEvent : IEvents
    {
        private List<AccountHistory> _histories;
        private readonly AccountSyncModel _req;
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _url;
        private readonly DateTime? _lastSync;

        public List<AccountHistory> Histories => _histories;

        public UpdateSyncEvent(IHttpClientFactory clientFactory, AccountSyncModel req, string url, DateTime? lastSync)
        {
            _req = req;
            _clientFactory = clientFactory;
            _url = url;
            _lastSync = lastSync;
        }

        public async Task<bool> Push()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _url);
            var client = _clientFactory.CreateClient();

            var response = await client.PostAsync(_url,
                                                  new StringContent(
                                                      JsonConvert.SerializeObject(
                                                          new AccountSyncUpdateRequestModel
                                                          {
                                                              History = _req, LastSync = _lastSync
                                                          })));

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
