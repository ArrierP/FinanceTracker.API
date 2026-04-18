using FinanceTracker.API.DTOs.Wallet;
using FinanceTracker.API.Entities;

namespace FinanceTracker.API.Services.Interfaces
{
    public interface IWalletService
    {
        Task<List<Wallet>> GetWalletAsync();
        Task<Wallet> GetWalletByIdAsync(int id);
        Task<Wallet> CreateWalletAsync(CreateWalletDto createWalletDto);
        Task UpdateWalletAsync(int id, UpdateWalletDto updateWalletDto);
        Task DeleteWalletAsync(int id);
    }
}
