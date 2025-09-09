using getzgroup_test.DTOs;
using getzgroup_test.Services;
using Microsoft.AspNetCore.Mvc;

namespace getzgroup_test.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService) => _userService = userService;

        // GET /users?search=&page=&pageSize=
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var items = await _userService.GetUsersAsync(search, page, pageSize);
            var total = items.Count();
            return Ok(new
            {
                items,
                page,
                pageSize,
                total,
            });
        }

        // POST /users
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var created = await _userService.CreateAsync(dto);
                return Created("Create user successfully", created);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict("Create user fail: " + ex.Message);
            }
        }

        //PUT /users/{id} — update name/title/email
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var updated = await _userService.UpdateAsync(id, dto);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict("Update user fail: " + ex.Message);
            }
        }

        //DELETE /users/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await _userService.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
