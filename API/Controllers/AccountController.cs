using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API;

public class AccountController: BaseApiController
{
    private readonly DataContext _context; // Declare the _context variable
    private readonly ITokenService _tokenService; // Declare the _tokenService variable

    public AccountController(DataContext context, ITokenService tokenService) 
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto) // Specify the return type as Task<IActionResult>
    {

        if (await UserExists(registerDto.Username)) return BadRequest("Username is taken"); // Check if a user with the specified username already exists
       
        
        // Add your code here
        using var hmac = new HMACSHA512(); // Create a new instance of HMACSHA512
        var user = new AppUser // Create a new instance of AppUser
        {
            UserName = registerDto.Username.ToLower(), // Set the UserName property to the value of the username parameter
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)), // Set the PasswordHash property to the hash of the password parameter
            PasswordSalt = hmac.Key // Set the PasswordSalt property to the key of the hmac instance
        };

        _context.Users.Add(user); // Add the user to the Users DbSet
        await _context.SaveChangesAsync(); // Save changes to the database

        return new UserDto 
        {
            UserName = user.UserName,
            Token = _tokenService.CreateToken(user)
        
        }; // Return the user

    }
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto) // Specify the return type as Task<IActionResult>
    {
        var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName.ToLower() == loginDto.Username.ToLower()); // Retrieve the user with the specified username
        if (user == null) return Unauthorized("Invalid User"); // Check if the user is null and return an error message if it is

        using var hmac = new HMACSHA512(user.PasswordSalt); // Create a new instance of HMACSHA512 with the user's password salt
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password)); // Compute the hash of the password parameter
        for (int i = 0; i < computedHash.Length; i++) // Iterate through the computed hash
        {
            if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password"); // Check if the hashes match and return an error message if they don't
        }

       return new UserDto 
        {
            UserName = user.UserName,
            Token = _tokenService.CreateToken(user)
        
        };
    }
    private async Task<bool> UserExists(string username) // Specify the return type as Task<bool>
    {
        return await _context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower()); // Return a boolean indicating whether a user with the specified username exists
    }
}
