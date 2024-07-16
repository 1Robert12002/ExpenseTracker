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
    }
}
