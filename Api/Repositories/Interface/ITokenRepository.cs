using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Entities;
using Microsoft.AspNetCore.Identity;

namespace Api.Repositories.Interface
{
    public interface ITokenRepository
    {
        Task<string> CreateJwtToken(ApplicationUser user);
    }
}