using System;

namespace client.Models.CommandModels
{
    public class UpdateAccountCommand:ICommand
    {
        public UpdateAccountCommand(AccountRequestModel req)
        {
            this.Id=Guid.NewGuid();
            AccountId = req.Id.Value;
            Name = req.Name;
            Tel = req.Tel;
            Address = req.Address;
        }
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Name { get; set; }
        public string Tel { get; set; }
        public  string Address { get; set; }
    }
}
