using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SportFieldBooking.Helper.Exceptions;
using AutoMapper;

namespace SportFieldBooking.Helper.Pagination
{
    public class Page<TDest> : List<TDest>
    {
        public long PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public long TotalPages { get; private set; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public Page (List<TDest> items, long pageIndex, int pageSize, long total)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling((double)total / pageSize);
            this.AddRange(items);
        }

        // Constructor khong chay duoc asynchronous code -> su dung ham nay de khoi tao Page
        public static async Task<Page<TDest>> CreateAsync<TSource, TDest>(IMapper mapper, IQueryable<TSource> source, long pageIndex, int pageSize)
        {
            var total = await source.CountAsync();
            var items = await source.Skip(((int) pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            var mappedItems = mapper.Map<List<TDest>>(items);
            var page = new Page<TDest>(mappedItems, pageIndex, pageSize, total);
            return page;
        }
    }
}
