using System;
using client.Entities;

namespace client.Models.CommandModels
{
    public class DeleteAccountCommand:ICommand
    {
        public DeleteAccountCommand(Guid id)
        {
            this.Id=Guid.NewGuid();
            AccountId = id;
        }
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
    }
}
