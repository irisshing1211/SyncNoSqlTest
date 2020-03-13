using System;

namespace client.Models.CommandModels
{
    public class AddAccountCommand:ICommand
    {
        public AddAccountCommand() { this.Id = Guid.NewGuid(); }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
    }
}
