using Hubtel_payment_API.Repositories.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Hubtel_payment_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var obj = await _userRepository.GetAllUsersAsync();

                if (obj is null)
                {
                    return NotFound();
                }

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("PostUsers")]
        public async Task<IActionResult> PostUsers([FromBody]Entities.Users model)
        {
            if (!ModelState.IsValid) { return BadRequest(); }

            if (model is null) { return BadRequest(); }

            model.UserId = Guid.NewGuid();

            await _userRepository.AddUsersAsync(model);

            return Ok(model);
        }
    }
}
