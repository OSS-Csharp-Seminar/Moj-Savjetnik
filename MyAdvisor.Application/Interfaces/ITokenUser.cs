using System;
using System.Collections.Generic;
using System.Text;

namespace MyAdvisor.Application.Interfaces
{
    public interface ITokenUser
    {
        int Id { get; }
        string Email { get; }
    }
}
