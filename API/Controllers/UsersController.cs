﻿using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.UsersController;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly DataContext _context; // Declare the _context variable

    public UsersController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]   // Add this method to the controller
    public  async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();
        return users;
    }

    [HttpGet("{id}")]   // Add this method to the controller
    public  async Task<ActionResult<AppUser>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        return user;
    }
 
}