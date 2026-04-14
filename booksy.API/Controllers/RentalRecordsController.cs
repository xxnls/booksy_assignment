using booksy.API.Models.DTOs;
using booksy.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace booksy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalRecordsController : ControllerBase
    {
        private readonly IRentalRecordService _rentalRecordService;

        public RentalRecordsController(IRentalRecordService rentalRecordService)
        {
            _rentalRecordService = rentalRecordService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RentalRecordDto>>> GetAll()
        {
            return Ok(await _rentalRecordService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RentalRecordDto>> GetById(int id)
        {
            var rental = await _rentalRecordService.GetByIdAsync(id);
            return rental is null ? NotFound() : Ok(rental);
        }

        [HttpPost("rent")]
        public async Task<ActionResult<RentalRecordDto>> Rent(CreateRentalRecordDto dto)
        {
            var rental = await _rentalRecordService.CreateAsync(dto);

            if (rental is null)
                return BadRequest("Hardware is unavailable or user does not exist.");

            return CreatedAtAction(nameof(GetById), new { id = rental.Id }, rental);
        }

        [HttpPost("return/{id}")]
        public async Task<IActionResult> Return(int id)
        {
            var success = await _rentalRecordService.ReturnAsync(id);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _rentalRecordService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
