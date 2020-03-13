using System;

namespace client.Models.CommandModels
{
    public interface ICommand
    {
        Guid Id { get; }
    }
}
