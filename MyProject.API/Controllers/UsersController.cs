using Microsoft.AspNetCore.Mvc;
using MyProject.Services.Entities;
using MyProject.Services.Services;
using OfficeOpenXml;
using MyProject.Services.Interfaces;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyProject.API.Controllers
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

        [HttpPost("import")]
        public async Task<Response<List<User>>> Import(IFormFile formFile, CancellationToken cancellationToken)
        {
            
                
            return await _userService.FilterUsers(formFile, cancellationToken);

        }
    }
}
