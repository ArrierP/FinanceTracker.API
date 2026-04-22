using FinanceTracker.Api.Data;
using FinanceTracker.API.DTOs.Wallet;
using FinanceTracker.API.Entities;
using FinanceTracker.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.API.Services.Implements
{
    public class WalletService : IWalletService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ApplicationDbContext _context;

        public WalletService(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<List<Wallet>> GetWalletAsync()
        {
            var userId = _currentUserService.UserId;
            if (userId == null)
            {
                throw new Exception("User not authenticated.");
            }
            return await _context.Wallets.Where(w => w.UserId == userId).ToListAsync();
        }

        public async Task<Wallet> GetWalletByIdAsync(int id)
        {
            var userId = _currentUserService.UserId;
            if (userId == null)
            {
                throw new Exception("User not authenticated.");
            }
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);
            if (wallet == null)
            {
                throw new Exception("Wallet not found or you do not have permission.");
            }
            return wallet;
        }

        public async Task UpdateWalletAsync(int id, UpdateWalletDto updateWalletDto)
        {
            var wallet = await GetWalletByIdAsync(id);

            wallet.Name = updateWalletDto.Name;
            wallet.Balance = updateWalletDto.InitialBalance;

            _context.Wallets.Update(wallet);
            await _context.SaveChangesAsync();
        }

        public async Task<Wallet> CreateWalletAsync(CreateWalletDto createWalletDto)
        {
            var userId = _currentUserService.UserId;
            if (userId == null)
            {
                throw new Exception("User not authenticated.");
            }
            var wallet = new Wallet
            {
                Name = createWalletDto.Name,
                Balance = createWalletDto.InitialBalance,
                UserId = userId.Value
            };
            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync();
            return wallet;
        }


        public async Task DeleteWalletAsync(int id)
        {
            var wallet = await GetWalletByIdAsync(id);
            _context.Wallets.Remove(wallet);
            await _context.SaveChangesAsync();

        }
    }
}
