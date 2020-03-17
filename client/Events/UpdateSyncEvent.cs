using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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
        private readonly ISyncModel _req;
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _url;

        public List<AccountHistory> Histories => _histories;

        public UpdateSyncEvent(IHttpClientFactory clientFactory, ISyncModel req, string url)
        {
            _req = req;
            _clientFactory = clientFactory;
            _url = url;
        }

        public async Task<bool> Push()
        {
            var client = _clientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(_req);

            try
            {
                var response = await client.PostAsync(_url, new StringContent(json, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    _histories = JsonConvert.DeserializeObject<List<AccountHistory>>(responseString);

                    return true;
                }
                else { _histories = new List<AccountHistory>(); }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
