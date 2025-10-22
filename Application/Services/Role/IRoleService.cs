﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Domain.Entities;

namespace Application.Services.Role
{
    public interface IRoleService
    {
        Task<Response> CreateRoleAsync(RoleEntity role,CancellationToken cancellation);
        Task<Response> UpdateRoleAsync(RoleEntity role, CancellationToken cancellation);
        Task<Response> DeleteRoleAsync(RoleEntity role, CancellationToken cancellation);
    }
}
