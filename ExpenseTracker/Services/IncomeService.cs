using ExpenseTracker.Data;
using ExpenseTracker.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTracker.Services
{
    public class IncomeService
    {
        private readonly ExpenseTrackerContext _context;

        public IncomeService(ExpenseTrackerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Income>> GetAllIncomesAsync()
        {
            return await _context.Incomes.ToListAsync();
        }

        public async Task<Income> GetIncomeByIdAsync(int id)
        {
            return await _context.Incomes.FindAsync(id);
        }

        public async Task AddIncomeAsync(Income income)
        {
            _context.Incomes.Add(income);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateIncomeAsync(Income income)
        {
            _context.Entry(income).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteIncomeAsync(int id)
        {
            var income = await _context.Incomes.FindAsync(id);
            if (income != null)
            {
                _context.Incomes.Remove(income);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<decimal> GetTotalIncomeAsync()
        {
            return await _context.Incomes.SumAsync(i => (decimal)i.Amount);
        }

        public async Task<decimal> GetTotalIncomeForCurrentMonthAsync()
        {
            var startOfMonth = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, 1);
            return await _context.Incomes
                .Where(i => i.Date >= startOfMonth)
                .SumAsync(i => (decimal)i.Amount);
        }

        public async Task<DateOnly?> GetFirstIncomeDateAsync()
        {
            if (await _context.Incomes.AnyAsync())
            {
                return await _context.Incomes.MinAsync(i => i.Date);
            }
            return null;
        }

        public async Task<DateOnly?> GetLastIncomeDateAsync()
        {
            if (await _context.Incomes.AnyAsync())
            {
                return await _context.Incomes.MaxAsync(i => i.Date);
            }
            return null;
        }

        public async Task<List<CategoryReports>> GetTopIncomeCategoriesAsync()
        {
            var incomes = await _context.Incomes.ToListAsync();

            return incomes
                .GroupBy(i => i.Type.ToString())
                .Select(group => new CategoryReports
                {
                    Category = group.Key,
                    TotalAmount = (decimal)group.Sum(i => i.Amount)
                })
                .OrderByDescending(x => x.TotalAmount)
                .Take(3)
                .ToList();
        }

    }
}
