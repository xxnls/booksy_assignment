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
            var rentals = await _rentalRecordService.GetAllAsync();
            return Ok(rentals);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RentalRecordDto>> GetById(int id)
        {
            var rental = await _rentalRecordService.GetByIdAsync(id);
            if (rental == null) return NotFound();
            return Ok(rental);
        }

        [HttpPost]
        public async Task<ActionResult<RentalRecordDto>> Create(CreateRentalRecordDto createRentalDto)
        {
            var rental = await _rentalRecordService.CreateAsync(createRentalDto);
            return CreatedAtAction(nameof(GetById), new { id = rental.Id }, rental);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateRentalRecordDto updateRentalDto)
        {
            var success = await _rentalRecordService.UpdateAsync(id, updateRentalDto);
            if (!success) return NotFound();
            return NoContent();
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
