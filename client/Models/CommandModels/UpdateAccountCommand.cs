using System;

namespace client.Models.CommandModels
{
    public class UpdateAccountCommand:ICommand
    {
        public UpdateAccountCommand()
        {
            this.Id=Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Name { get; set; }
        public string Tel { get; set; }
        public  string Address { get; set; }
    }
}
