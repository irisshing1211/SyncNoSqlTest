using System;

namespace client.Models.CommandModels
{
    public class AddAccountCommand:ICommand
    {
        public AddAccountCommand(AccountRequestModel req)
        {
            this.Id = Guid.NewGuid();
            Name = req.Name;
            Tel = req.Tel;
            Address = req.Address;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
    }
}
