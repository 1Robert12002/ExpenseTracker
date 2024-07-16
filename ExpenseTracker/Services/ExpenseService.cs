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

    }
}
