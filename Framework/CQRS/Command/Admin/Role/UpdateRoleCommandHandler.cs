using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Framework.Factories.Identity.Role;
using Framework.ViewModels.Role;
using MediatR;

namespace Framework.CQRS.Command.Admin.Role
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleViewModel, Response>
    {
        private readonly IRoleFactory _roleFactory;

        public UpdateRoleCommandHandler(IRoleFactory roleFactory)
        {
            _roleFactory = roleFactory;
        }
        public async Task<Response> Handle(UpdateRoleViewModel request, CancellationToken cancellationToken)
        {
            return await _roleFactory.UpdateRoleAsync(request, cancellationToken);
        }
    }
}
