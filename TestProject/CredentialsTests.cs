using Backend.DataAccessObjects.Admin;
using Backend.DataAccessObjects.LoginDAO;
using Backend.EFCData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Shared;

namespace TestProject;

public class CredentialsTests
{
    [Test]
    public async Task ShouldGetErrorMessage_WhenEditPassword_WithCurrentPasswordIsWrong()
    {
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        await context.Admins.AddAsync(new Admin {  Mail = "test@test.com", Password = "password" }); 
        await context.SaveChangesAsync();
        var service = new CredentialsImplementation(context);
    
        // Act
        var result = await service.EditPasswordAsync(1, "password1", "newPassword", "newPassword");
    
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("Current password is incorrect"));
        await context.Database.EnsureDeletedAsync();
    }
    [Test]
    public async Task ShouldGetErrorMessage_WhenEditPassword_WithNewPasswordAndReEnterPasswordDoNotMatch()
    {
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        await context.Admins.AddAsync(new Admin {  Mail = "test@test.com", Password = "password" }); 
        await context.SaveChangesAsync();
        var service = new CredentialsImplementation(context);
    
        // Act
        var result = await service.EditPasswordAsync(1, "password", "newPassword", "newPassword1");
    
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("New Password and re-entered Password do not match"));
        await context.Database.EnsureDeletedAsync();
    }
    
    [Test]
    public async Task ShouldGetErrorMessage_WhenEditPassword_WithEmptyFields()
    {
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        await context.Admins.AddAsync(new Admin {  Mail = "test@test.com", Password = "password" }); 
        await context.SaveChangesAsync();
        var service = new CredentialsImplementation(context);
    
        // Act
        var result = await service.EditPasswordAsync(1, "password", "", "");
    
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("New Password and re-entered Password cannot be empty"));
        await context.Database.EnsureDeletedAsync();
    }
    
    [Test]
    public async Task ShouldGetConfirmationMessage_WhenEditPassword_WithNewPasswordAndReEnterPasswordDoMatchAndCurrentPasswordIsValid()
    {
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        await context.Admins.AddAsync(new Admin {  Mail = "test@test.com", Password = "password" }); 
        await context.SaveChangesAsync();
        var service = new CredentialsImplementation(context);
    
        // Act
        var result = await service.EditPasswordAsync(1, "password", "newPassword", "newPassword");
    
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("Password Changed"));
        await context.Database.EnsureDeletedAsync();
    }

 
    
     [Test]
    public async Task ShouldGetErrorMessage_WhenEditEmail_WhitCurrentPasswordIsWrong()
    {
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        await context.Admins.AddAsync(new Admin {  Mail = "test@test.com", Password = "password" }); 
        await context.SaveChangesAsync();
        var service = new CredentialsImplementation(context);
    
        // Act
        var result = await service.EditMailAsync(1, "password1", "newEmail@test.com", "newEmail@test.com");
    
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("Current password is incorrect"));
        await context.Database.EnsureDeletedAsync();
    }
    [Test]
    public async Task ShouldGetErrorMessage_WithEditEmail_WithNewEmailAndReEnterEmailDoNotMatch()
    {
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        await context.Admins.AddAsync(new Admin {  Mail = "test@test.com", Password = "password" }); 
        await context.SaveChangesAsync();
        var service = new CredentialsImplementation(context);
    
        // Act
        var result = await service.EditMailAsync(1, "password", "newEmail@test.com", "newEmail123@test.com");
    
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("New Mail and re-entered Mail do not match"));
        await context.Database.EnsureDeletedAsync();
    }
    
    [Test]
    public async Task ShouldGetErrorMessage_WithEditEmail_WithEmptyFields()
    {
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        await context.Admins.AddAsync(new Admin {  Mail = "test@test.com", Password = "password" }); 
        await context.SaveChangesAsync();
        var service = new CredentialsImplementation(context);
    
        // Act
        var result = await service.EditMailAsync(1, "password", "", "");
    
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("New Mail and re-entered Mail cannot be empty"));
        await context.Database.EnsureDeletedAsync();
    }
    
    [Test]
    public async Task ShouldGetConfirmationMessage_WhenEditEmail_WithNewEmailAndReEnterEmailDoMatchAndCurrentPasswordIsValid()
    {
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        await context.Admins.AddAsync(new Admin {  Mail = "test@test.com", Password = "password" }); 
        await context.SaveChangesAsync();
        var service = new CredentialsImplementation(context);
    
        // Act
        var result = await service.EditMailAsync(1, "password", "newEmail@test.com", "newEmail@test.com");
    
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("EMail Changed"));
        await context.Database.EnsureDeletedAsync();
    }
    

}