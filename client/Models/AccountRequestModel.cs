using System;

namespace client.Models
{
    public class AccountRequestModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
    }
}
