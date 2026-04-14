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
        private readonly IAISearchService _aiSearchService;

        public HardwareController(IHardwareService hardwareService, IAISearchService aiSearchService)
        {
            _hardwareService = hardwareService;
            _aiSearchService = aiSearchService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HardwareDto>>> GetAll()
        {
            var hardware = await _hardwareService.GetAllAsync();
            return Ok(hardware);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<HardwareDto>>> AISearch([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q)) return await GetAll();

            var allHardware = await _hardwareService.GetAllAsync();
            var relevantIds = await _aiSearchService.SearchHardwareIdsAsync(q, allHardware);

            if (!relevantIds.Any()) return Ok(Enumerable.Empty<HardwareDto>());

            // Return items in the order of relevance returned by AI
            var results = relevantIds
                .Select(id => allHardware.FirstOrDefault(h => h.Id == id))
                .Where(h => h != null)
                .Cast<HardwareDto>();

            return Ok(results);
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

            if (hardware == null)
            {
                ModelState.AddModelError(nameof(createHardwareDto.AssignedTo),
                    $"User with email '{createHardwareDto.AssignedTo}' was not found.");

                return ValidationProblem(ModelState);
            }

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

            if (!success)
            {
                return BadRequest(new { error = "Cannot remove hardware. It either does not exist or is currently assigned to a rental record." });
            }

            return NoContent();
        }
    }
}
