using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMvcContext _context;

        public SalesRecordService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from salesRecord in _context.SalesRecord select salesRecord;

            if (minDate.HasValue)
            {
                result = result.Where(w => w.Date >= minDate.Value);
            }

            if (maxDate.HasValue)
            {
                result = result.Where(w => w.Date <= maxDate.Value);
            }

            return await result
                .Include(w => w.Seller)
                .Include(w => w.Seller.Department)
                .OrderByDescending(w => w.Date)
                .ToListAsync();
        }

        public async Task<List<IGrouping<Department,SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from salesRecord in _context.SalesRecord select salesRecord;

            if (minDate.HasValue)
            {
                result = result.Where(w => w.Date >= minDate.Value);
            }

            if (maxDate.HasValue)
            {
                result = result.Where(w => w.Date <= maxDate.Value);
            }

            return await result
                .Include(w => w.Seller)
                .Include(w => w.Seller.Department)
                .OrderByDescending(w => w.Date)
                .GroupBy(w => w.Seller.Department)
                .ToListAsync();
        }
    }
}
