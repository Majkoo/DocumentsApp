﻿namespace DocumentsApp.Data.Models.AccountModels;

public class RegisterUserDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}