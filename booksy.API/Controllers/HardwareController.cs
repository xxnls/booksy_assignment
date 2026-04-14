using booksy.API.Models.DTOs;
using booksy.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace booksy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HardwareController : ControllerBase
    {
        private readonly IHardwareService _hardwareService;

        public HardwareController(IHardwareService hardwareService)
        {
            _hardwareService = hardwareService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HardwareDto>>> GetAll()
        {
            var hardware = await _hardwareService.GetAllAsync();
            return Ok(hardware);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HardwareDto>> GetById(int id)
        {
            var hardware = await _hardwareService.GetByIdAsync(id);
            if (hardware == null) return NotFound();
            return Ok(hardware);
        }

        [HttpPost]
        public async Task<ActionResult<HardwareDto>> Create(CreateHardwareDto createHardwareDto)
        {
            var hardware = await _hardwareService.CreateAsync(createHardwareDto);
            return CreatedAtAction(nameof(GetById), new { id = hardware.Id }, hardware);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateHardwareDto updateHardwareDto)
        {
            var success = await _hardwareService.UpdateAsync(id, updateHardwareDto);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _hardwareService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
