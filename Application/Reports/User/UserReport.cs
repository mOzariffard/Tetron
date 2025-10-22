﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Extensions;
using Application.Models;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Reports.User
{
    public class UserReport:IUserReport
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IDapper _dapper;

        public UserReport(UserManager<UserEntity> userManager, IDapper dapper)
        {
            _userManager = userManager;
            _dapper = dapper;
        }
        public async Task<UserEntity?> GetUserByIdAsync(Guid userId, CancellationToken cancellation=default)
        {
            if (cancellation.IsCancellationRequested)
            {
                //todo
            }

            if (userId == null)
            {
                //todo
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                //todo
            }

            return user;
        }

        public async Task<PaginatedList<TDestination>> GetAllPaginatedAsync<TDestination>(PaginatedSearchWithSize pagination,
            CancellationToken cancellationToken = default)
        {
            var query = _userManager.Users.AsNoTracking().AsQueryable();

            // Apply search filter.
            if (!string.IsNullOrEmpty(pagination.Keyword))
            {
                query = query
                    .Where(r => r.PhoneNumber!.Contains(pagination.Keyword) || r.UserName!.Contains(pagination.Keyword))
                    .AsQueryable();
            }

            return await query.PaginatedListAsync<UserEntity, TDestination>(pagination.Page, 
                pagination.PageSize,
                config: null, cancellationToken);
        }





        public async Task<string?> GetUserRoleByUserIdAsync(UserEntity? user, CancellationToken cancellation)
        {
            if (cancellation.IsCancellationRequested)
            {
                //todo
            }

            if (user == null)
            {
                //todo
            }

            var roles = await _userManager.GetRolesAsync(user!);
            var role = roles.FirstOrDefault();
            return role;
        }
        public async Task<Response> ExistUserAsync(string national)
        {
            Response response = new();
            response.IsSuccess = await _userManager.Users.AnyAsync(
                a => a.UserName == national

            );
            return response;
        }
        public async Task<Response> ActiveUserAsync(string national)
        {
            Response response = new();
            response.IsSuccess = await _userManager.Users.AnyAsync(
                a => a.UserName == national

                     &&
                     a.PhoneNumberConfirmed == true && a.EmailConfirmed==true
            );
            return response;
        }

        public async Task<UserEntity?> GetUserByUserName(string username, CancellationToken cancellationToken = default)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<UserEntity?> GetUserByIdWithOtherInfoAsync(Guid userId)
        {
            var query =  _userManager.Users.AsQueryable();
            query = query.Include(i => i.Address).ThenInclude(t=>t!.City);
            query = query.Include(i => i.Address).ThenInclude(t=>t!.Province);
          
            return await query.SingleOrDefaultAsync(s => s.Id == userId);
        }

        public async Task<IEnumerable<UserEntity>> GetUserForSelectAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<List<TModel>?> GetUserAdvertising<TModel>(Guid userId)
        {
            string query = $"SELECT * FROM [dbo].[Advertising] WHERE UserId='{userId}' ";
            return await _dapper.ExecuteQuery<TModel>(query);
        }
    }
}
