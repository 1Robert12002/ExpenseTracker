using ExpenseTracker.Data;
using ExpenseTracker.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTracker.Services
{
    public class ExpenseService
    {
        private readonly ExpenseTrackerContext _context;

        public ExpenseService(ExpenseTrackerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Expense>> GetAllExpensesAsync()
        {
            return await _context.Expenses.Include(e => e.Category).ToListAsync();
        }


        public async Task<Expense> GetExpenseByIdAsync(int id)
        {
            return await _context.Expenses.FindAsync(id);
        }

        public async Task AddExpenseAsync(Expense expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateExpenseAsync(Expense expense)
        {
            _context.Entry(expense).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteExpenseAsync(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense != null)
            {
                _context.Expenses.Remove(expense);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Expense>> GetExpensesByCategoryAsync(int categoryId)
        {
            return await _context.Expenses
                .Include(e => e.Category)
                .Where(e => e.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalExpenseAsync()
        {
            return await _context.Expenses.SumAsync(e => (decimal)e.Amount);
        }

        public async Task<decimal> GetTotalExpenseForCurrentMonthAsync()
        {
            var startOfMonth = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, 1);
            return await _context.Expenses
                .Where(e => e.Date >= startOfMonth)
                .SumAsync(e => (decimal)e.Amount);
        }

        public async Task<DateOnly?> GetFirstExpenseDateAsync()
        {
            if (await _context.Expenses.AnyAsync())
            {
                return await _context.Expenses.MinAsync(e => e.Date);
            }
            return null;
        }

        public async Task<DateOnly?> GetLastExpenseDateAsync()
        {
            if (await _context.Expenses.AnyAsync())
            {
                return await _context.Expenses.MaxAsync(e => e.Date);
            }
            return null;
        }

        public async Task<decimal> GetTotalPlannedExpensesAsync()
        {
            return await _context.Expenses
                .Where(e => e.Planned)
                .SumAsync(e => (decimal)e.Amount);
        }

        public async Task<decimal> GetTotalUnplannedExpensesAsync()
        {
            return await _context.Expenses
                .Where(e => !e.Planned)
                .SumAsync(e => (decimal)e.Amount);
        }

        public async Task<string> GetCategoryWithHighestExpensesAsync()
        {
            var categoryWithHighestExpenses = await _context.Expenses
                .GroupBy(e => e.CategoryId)
                .Select(group => new
                {
                    CategoryId = group.Key,
                    TotalAmount = group.Sum(e => e.Amount)
                })
                .OrderByDescending(g => g.TotalAmount)
                .FirstOrDefaultAsync();

            if (categoryWithHighestExpenses != null)
            {
                var category = await _context.Categories.FindAsync(categoryWithHighestExpenses.CategoryId);
                return category?.Name ?? "Unknown";
            }

            return "No Expenses";
        }
        public async Task<List<CategoryReports>> GetTopExpenseCategoriesAsync()
        {
            var expenses = await _context.Expenses
                .Where(e => e.Category != null)
                .GroupBy(e => e.Category.Name)
                .Select(group => new CategoryReports
                {
                    Category = group.Key,
                    TotalAmount = (decimal)group.Sum(e => e.Amount)
                })
                .OrderByDescending(x => x.TotalAmount)
                .Take(3)
                .ToListAsync();

            return expenses;
        }


    }
}
