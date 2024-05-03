using Backend.DataAccessObjects.Admin;
using Backend.DataAccessObjects.LoginDAO;
using Backend.EFCData;
using Microsoft.Extensions.Configuration;
using Shared;

namespace TestProject;

public class LoginServiceTests
{
     [Test]
    public async Task GetLoginAdminIdAsync_ValidCredentials_ReturnsAdmin()
    {
    
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Admins.AddAsync(new Admin {  Mail = "test@test.com", Password = "password" }); 
            await context.SaveChangesAsync();
        var service = new LoginImplementation(context);
    
            // Act
            var result = await service.GetLoginAdminIdAsync("test@test.com", "password");
    
            // Assert
           Assert.That(result, Is.Not.Null);
           Assert.That(result.Id, Is.EqualTo(1));
          await context.Database.EnsureDeletedAsync();
    }
    
    
    [Test]
    public async Task GetLoginAdminIdAsync_NotValidPassword_ReturnsNoValidAdmin()
    {
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Admins.AddAsync(new Admin {  Mail = "test@test.com", Password = "password" }); 
        await context.SaveChangesAsync();
        var service = new LoginImplementation(context);
    
        // Act
        var result = await service.GetLoginAdminIdAsync("test@test.com", "password1");
    
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(-1));
        await context.Database.EnsureDeletedAsync();
        }
    
    [Test]
    public async Task GetLoginAdminIdAsync_NotValidEMail_ReturnsNoValidAdmin()
    {
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Admins.AddAsync(new Admin {  Mail = "test@test.com", Password = "password" }); 
        await context.SaveChangesAsync();
        var service = new LoginImplementation(context);
    
        // Act
        var result = await service.GetLoginAdminIdAsync("test1@test.com", "password");
    
        // Assert
        Assert.That(result, Is.Not.Null);
         Assert.That(result.Id, Is.EqualTo(-1));
        await context.Database.EnsureDeletedAsync();
    }
    
    [Test]
    public async Task GetLoginAdminIdAsync_NotValidEMailAndPassword_ReturnsNoValidAdmin()
    {
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Admins.AddAsync(new Admin {  Mail = "test@test.com", Password = "password" }); 
        await context.SaveChangesAsync();
        var service = new LoginImplementation(context);
    
        // Act
        var result = await service.GetLoginAdminIdAsync("test1@test.com", "password1");
    
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(-1));
        await context.Database.EnsureDeletedAsync();
    }
    [Test]
    public async Task GetLoginAdminIdAsync_EmptyFields_ReturnsNoValidAdmin()
    {
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Admins.AddAsync(new Admin {  Mail = "test@test.com", Password = "password" }); 
        await context.SaveChangesAsync();
        var service = new LoginImplementation(context);
    
        // Act
        var result = await service.GetLoginAdminIdAsync("", "");
    
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(-1));
        await context.Database.EnsureDeletedAsync();
    }
    
    
    
    /////////////////////////////////////////
    [Test]
    public async Task GetLoginAdminIdAsync_ValidCredentials_ReturnsAdmin123()
    {
    
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Admins.AddAsync(new Admin {  Mail = "test@test.com", Password = "password" }); 
        await context.SaveChangesAsync();
        var service = new LoginImplementation(context);
    
        // Act
        var result = await service.GetLoginAdminIdAsync("test@test.com", "password");
    
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(1));
        await context.Database.EnsureDeletedAsync();
    }
}