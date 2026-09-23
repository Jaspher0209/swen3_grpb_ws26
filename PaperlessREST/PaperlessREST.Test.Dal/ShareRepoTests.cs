using System.Runtime.InteropServices.JavaScript;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Legacy;
using PaperlessREST.Dal;
using PaperlessREST.Models;
using DbContext = PaperlessREST.Dal.DbContext;

namespace Test.DataAccessLayer;

[TestFixture]
public class ShareRepoTests
{
    private DbContext _dbContext;
    private DbContext GetInMemoryDbContext()
    {
        // setup for inMemory-db
        var options = new DbContextOptionsBuilder<DbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new DbContext(options);
        
        // create new entries - share link
        context.ShareLink.AddRange(
            new ShareLink()
            {
                Guid = "abcdefg12345",
                DocumentId = "002",
                ExpireDate = DateTime.UtcNow,
                Password = "pass1234"
            });

        // create new entries - meta data
        context.MetaData.AddRange(
            new MetaData {   
                Id = "001",
                Author = "Jaspher",
                Description = "Hier liegt mein Stundeplan",
                Filename = "schedule.excel",
                Updated = DateTime.UtcNow
            },
            new MetaData {   
                Id = "002",
                Author = "Tom",
                Description = "Hier liegt mein Stundeplan",
                Filename = "schedule12.excel",
                Updated = DateTime.UtcNow
            });
        
        // save db changes
        context.SaveChanges();

        return context;
    }


    [Test]
    public async Task ShareLinkCreatedSuccessfullyTest()
    {
        
        // Arrange
        var context = GetInMemoryDbContext();
        var shareLinkRepo = new ShareRepository(context);

        // Act: create a new share link
        var newLink = await shareLinkRepo.CreateShareLinkAsync("003", "1234", DateTime.Today);
        
        Console.WriteLine();
        // Assert: is link created?
        ClassicAssert.IsNotNull(newLink);
    }
    [Test]
    public async Task ShareLinkGetLinkTest()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var shareLinkRepo = new ShareRepository(context);

        // Act: create a new share link
        var link = await shareLinkRepo.GetLinkAsync("abcdefg12345");
        
        // Assert: is link created?
        ClassicAssert.IsNotNull(link);
    }
}