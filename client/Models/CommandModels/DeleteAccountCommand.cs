using System;

namespace client.Models.CommandModels
{
    public class DeleteAccountCommand:ICommand
    {
        public DeleteAccountCommand()
        {
            this.Id=Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
    }
}
