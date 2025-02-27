﻿using BlazorSecretManager.Entities;
using BlazorSecretManager.Services.Auth.Abstracts;
using eXtensionSharp;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlazorSecretManager.Services.Auth;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserRepository(AppDbContext dbContext,
        IPasswordHasher<User> passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> CreateUser(User insertUser)
    {
        var exists = await _dbContext.Users.FirstOrDefaultAsync(m => m.Email == insertUser.Email);
        if (exists.xIsNotEmpty()) return false;


        await _dbContext.Users.AddAsync(insertUser);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<User> GetUser(string id)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(m => m.Email == email);
    }
}