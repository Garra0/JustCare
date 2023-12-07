using Microsoft.AspNetCore.Mvc;
using JustCare_MB.Dtos;
using JustCare_MB.Services.IServices;
using JustCare_MB.Models;


namespace LoadTheDb.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class UserController : Controller
    {
        private readonly IUsersService _userService;
        public UserController(IUsersService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index(UsersIndexDto usersIndexDto)
        {

            //IEnumerable<User> UserList;
            //UserList = await _userService.GetAllUsers(currentFilter, searchString);
            //return View(UserList);
            return View(await _userService.GetAllUsers(usersIndexDto));
        }

        //public async Task<IActionResult> GetAllUsersController()
        //{
        //    //IEnumerable<User> UserList;
        //    //UserList = await _userService.GetAllUsers();
        //    //return View(UserList);
        //    return View(await _userService.GetAllUsers());
        //}

        public IActionResult CreateUserRegister()
        {
            return View();
        }

        [HttpPost]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUserRegister(UserRegisterDto UserToCreate)
        {
            if (ModelState.IsValid)
            {
                bool IsSuccess = await _userService.CreateUser(UserToCreate);
                if (IsSuccess)
                {
                    TempData["success"] = "User Created Successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData["error"] = "Error encountered";
            return View(UserToCreate);
        }

        
        public async Task<IActionResult> Edit(int id)
        {
            UserDto UserUpdateDto = await _userService.UserToUserDtoMapper(id);
            return View(UserUpdateDto);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, UserDto userEdited)
        {
            bool IsSuccess = await _userService.UpdateUser(id, userEdited);
            if (ModelState.IsValid)
            {
                if (IsSuccess)
                {
                    TempData["success"] = "User Edited Successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData["error"] = "Error encountered";
            return View(userEdited);
        }

        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //        return NotFound();
        //    User user = await _userService.GetUserById(id);
        //    return View(user);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int? id) 
        //{
        //    bool IsSuccess = await _userService.DeleteUser(id);
        //    if (ModelState.IsValid)
        //    {
        //        if (IsSuccess)
        //        {
        //            TempData["success"] = "User deleted Successfully";
        //            return RedirectToAction(nameof(Index));
        //        }
        //    }
        //    TempData["error"] = "Error encountered";
        //    return RedirectToAction(nameof(Index));
        //}


        // delete on click...
        public async Task<IActionResult> Delete(int id)
        {
            bool IsSuccess = await _userService.DeleteUser(id);
            if (ModelState.IsValid)
            {
                if (IsSuccess)
                {
                    TempData["success"] = "User deleted Successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData["error"] = "Error encountered";
            return RedirectToAction(nameof(Index));
        }


    }
}
