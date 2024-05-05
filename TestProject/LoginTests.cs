using Backend.DataAccessObjects.Admin;
using Backend.DataAccessObjects.LoginDAO;
using Backend.EFCData;
using Microsoft.Extensions.Configuration;
using Shared;

namespace TestProject;

public class LoginTests
{
     [Test]
    public async Task ReturnAdmin_WhenLogin_WithValidCredentials()
    {
    
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
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
    public async Task ReturnNotValidAdmin_WhenLogin_WithWrongPassword()
    {
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        await context.Admins.AddAsync(new Admin {  Mail = "test@test.com", Password = "password" }); 
        await context.SaveChangesAsync();
        var service = new LoginImplementation(context);
    
        // Act
        var result = await service.GetLoginAdminIdAsync("test@test.com", "password1");
    
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(-1));
        Assert.That(result.Mail, Is.EqualTo("Credentials do not match"));
        await context.Database.EnsureDeletedAsync();
        }
    
    [Test]
    public async Task ReturnNotValidAdmin_WhenLogin_WithWrongEmail()
    {
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        await context.Admins.AddAsync(new Admin {  Mail = "test@test.com", Password = "password" }); 
        await context.SaveChangesAsync();
        var service = new LoginImplementation(context);
    
        // Act
        var result = await service.GetLoginAdminIdAsync("test1@test.com", "password");
    
        // Assert
        Assert.That(result, Is.Not.Null);
         Assert.That(result.Id, Is.EqualTo(-1));
         Assert.That(result.Mail, Is.EqualTo("Credentials do not match"));
        await context.Database.EnsureDeletedAsync();
    }
    
    [Test]
    public async Task ReturnNotValidAdmin_WhenLogin_WithWrongPasswordAndWrongEmail()
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
        Assert.That(result.Mail, Is.EqualTo("Credentials do not match"));
        Assert.That(result.Id, Is.EqualTo(-1));
        await context.Database.EnsureDeletedAsync();
    }
    [Test]
    public async Task ReturnNotValidAdmin_WhenLogin_WithEmptyFields()
    {
        // Arrange
        var context = new DataContext(new ConfigurationBuilder().Build(), useInMemoryDatabase: true);
        await context.Database.EnsureCreatedAsync();
        await context.Admins.AddAsync(new Admin {  Mail = "test@test.com", Password = "password" }); 
        await context.SaveChangesAsync();
        var service = new LoginImplementation(context);
    
        // Act
        var result = await service.GetLoginAdminIdAsync("", "");
    
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(-1));
        Assert.That(result.Mail, Is.EqualTo("password and mail should not be empty"));
        await context.Database.EnsureDeletedAsync();
    }
    
    
    
    /////////////////////////////////////////
   
}