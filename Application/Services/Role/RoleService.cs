﻿using Application.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Role
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<RoleEntity> _roleManager;

        public RoleService(RoleManager<RoleEntity> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<Response> CreateRoleAsync(RoleEntity role, CancellationToken cancellation)
        {
            if (cancellation.IsCancellationRequested)
            {
                //Todo Log
                return Response.Fail();
            }

            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return Response.Succeded();
            }

            foreach (var error in result.Errors)
            {
                //Todo Log
            }

            return Response.Fail("امکان ثبت نقش کاربری وجود ندارد.");
        }

        public async Task<Response> UpdateRoleAsync(RoleEntity role, CancellationToken cancellation)
        {
            if (cancellation.IsCancellationRequested)
            {
                //todo
                //
            }

            if (role == null)
            {
                //todo}

            }

            Response response = new();
            var result = await _roleManager.UpdateAsync(role!);
            if (result.Succeeded)
            {
                response.IsSuccess = true;

            }
            else
            {
                string message = string.Empty;
                foreach (var error in result.Errors)
                {
                    message = error.Description + "\n\r";
                }

                response.Message = message;
            }

            return response;
        }

        public async Task<Response> DeleteRoleAsync(RoleEntity role, CancellationToken cancellation)
        {
          if(cancellation.IsCancellationRequested) { //todo
                                                     }

          if (role == null)
          {
              //todo
          }
          Response response = new();
          var result=await _roleManager.DeleteAsync(role!); if (result.Succeeded)
          {
              response.IsSuccess = true;

          }
          else
          {
              string message = string.Empty;
              foreach (var error in result.Errors)
              {
                  message = error.Description + "\n\r";
              }

              response.Message = message;
          }

          return response;
        }
    }
}
