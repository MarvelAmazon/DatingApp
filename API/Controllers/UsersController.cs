using System.Security.Claims;
using API.Data;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.UsersController;


[Authorize] // Add the [Authorize] attribute to the controller
public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository; // Declare the _context variable
    private readonly IMapper _mapper;

    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [HttpGet]   // Add this method to the controller
    public  async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        // var users = await _userRepository.GetUsersAsync();
        // var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);
        var  users = await _userRepository.GeMembersAsync();
        return Ok(users);
    }

    [HttpGet("{username}")]   // Add this method to the controller
    public  async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        // var user = await _userRepository.GetUserByUsernameAsync(username);
        // var userToReturn = _mapper.Map<MemberDto>(user);
        var user = await _userRepository.GetMemberAsync(username);
        return user;
    }

    [HttpPut]   // Add this method to the controller
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _userRepository.GetUserByUsernameAsync(username);
        if (user == null) return NotFound();
        _mapper.Map(memberUpdateDto, user);
        _userRepository.Update(user);
        if (await _userRepository.SaveAllAsync()) return NoContent();
        return BadRequest("Failed to update user");
    }
 
}
