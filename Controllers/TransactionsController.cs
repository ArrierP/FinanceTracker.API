using FinanceTracker.Api.Enums;
using FinanceTracker.API.DTOs.Transaction;
using FinanceTracker.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] // Yêu cầu JWT Token để xác thực người dùng
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    // Bước "Tiêm" (Injection): Yêu cầu hệ thống cung cấp một bản thực thi của ITransactionService
    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    // 1. Lấy danh sách giao dịch (Có hỗ trợ lọc dữ liệu)
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? walletId,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] TransactionType? type)
    {
        var transactions = await _transactionService.GetAllAsync(walletId, fromDate, toDate, type);
        return Ok(transactions);
    }

    // 2. Lấy chi tiết một giao dịch theo ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var transaction = await _transactionService.GetByIdAsync(id);
        if (transaction == null)
            return NotFound("Transaction not found.");

        return Ok(transaction);
    }

    // 3. Tạo mới một giao dịch
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTransactionDto dto)
    {
        var transaction = await _transactionService.CreateAsync(dto);
        if (transaction == null)
            return BadRequest("Invalid data. Please check the Wallet or Category.");

        return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
    }

    // 4. Cập nhật giao dịch
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTransactionDto dto)
    {
        var result = await _transactionService.UpdateAsync(id, dto);
        if (!result) return BadRequest("Update failed. Please check the ownership or input data.");

        return NoContent();
    }

    // 5. Xóa giao dịch
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _transactionService.DeleteAsync(id);
        if (!result) return NotFound("Delete failed. Transaction not found or you do not have permission to delete it.");

        return NoContent();
    }
}