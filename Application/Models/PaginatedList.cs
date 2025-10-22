﻿using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class PaginatedList<T>
    {
        #region Ctor's
        public PaginatedList()
        {
        }

        public PaginatedList(List<T> Items, int Count, int PageIndex, int PageSize)
        {
            pageIndex = PageIndex;
            totalPages = (int)Math.Ceiling(Count / (double)PageSize);
            totalCount = Count;
            items = Items;
        }
        #endregion

        #region Prop's

        public List<T> items { get; set; }
        public int pageIndex { get; }
        public int totalPages { get; }
        public int totalCount { get; }

        public bool hasPreviousPage => pageIndex > 1;
        public bool hasNextPage => pageIndex < totalPages;

        #endregion

        #region Method's
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize,
            CancellationToken cancellationToken)
        {
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            pageSize = pageSize <= 0 ? 5 : pageSize;
            var count = await source.CountAsync();
            var items = await source
                .Skip((pageIndex - 1) * pageSize).Take(pageSize) // Apply pagination.
                .ToListAsync(cancellationToken); // Execute query command.
            return new PaginatedList<T>(items, count, pageIndex, pageSize); // Cast to paginated list.
        }

        public static async Task<PaginatedList<TDestination>> CreateAsync<TOrigin, TDestination>(IQueryable<TOrigin> source,
            int pageIndex, int pageSize, TypeAdapterConfig? config = default, CancellationToken cancellationToken = default)
        {
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            pageSize = pageSize <= 0 ? 5 : pageSize;
            var count = await source.CountAsync();
            var items = await source
                .Skip((pageIndex - 1) * pageSize).Take(pageSize) // Apply pagination.
                .ProjectToType<TDestination>(config) // Cast to view model.
                .ToListAsync(cancellationToken); // Execute query command.
            return new PaginatedList<TDestination>(items, count, pageIndex, pageSize); // Cast to paginated list.
        }
        #endregion
    }
}
