using Business.Abstract;
using Entities.DTOs;
using Entities.DTOs.Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET api/users/getbyid?id=3
        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _userService.GetById(id);
            return result.Success ? Ok(result.Data) : NotFound(result.Message);
        }

        // DELETE api/users/delete?id=3
        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            var result = _userService.Delete(id);
            return result.Success ? Ok(result.Message) : NotFound(result.Message);
        }

        // PUT api/users/update
        [HttpPut("update")]
        public IActionResult Update(UserForUpdateDto userForUpdateDto)
        {
            var result = _userService.Update(userForUpdateDto);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        // PUT api/users/changepassword
        [HttpPut("changepassword")]
        public IActionResult ChangePassword(UserForChangePasswordDto userForChangePasswordDto)
        {
            var result = _userService.ChangePassword(userForChangePasswordDto);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

    }
}
