//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;
//using API5.Models;
//using API5.Repository.Interfaces;

//namespace API5.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class RegisterController : ControllerBase
//    {
//        private readonly IRegisterRepository _accountRepository;

//        public RegisterController(IRegisterRepository accountRepository)
//        {
//            _accountRepository = accountRepository;
//        }

//        [HttpPost]
//        public async Task<IActionResult> Register(RegisterVM registerVM)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            try
//            {
//                await _accountRepository.CreateAccountAsync(registerVM);
//                return Ok(new { Message = "User registered successfully." });
//            }
//            catch (ArgumentException ex)
//            {
//                return NotFound(new { Message = ex.Message });
//            }
//        }
//    }
//}

