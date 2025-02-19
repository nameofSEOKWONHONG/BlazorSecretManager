﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlazorSecretManager.Entities;

public class User : IdentityUser<string>
{
    public string RoleName { get; set; }
    public string UserKey { get; set; }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.RoleName).HasMaxLength(256);
        builder.Property(x => x.UserKey).HasMaxLength(256);
        builder.HasIndex(x => x.UserKey);
    }
}