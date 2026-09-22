using PaperlessREST.Dal;
using Microsoft.EntityFrameworkCore;
using PaperlessREST.Models;
using NUnit.Framework.Legacy;
using DbContext = PaperlessREST.Dal.DbContext;

namespace Test.DataAccessLayer;

[TestFixture]
public class DocumentMetaRepositoryTests
{
    private DbContext _dbContext;


    private DbContext GetInMemoryDbContext()
    {
        // setup for inMemory-db
        var options = new DbContextOptionsBuilder<DbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new DbContext(options);
        
        // create new entries
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

    //Arrange - Objekte vorbereiten (Muss nicht gemacht werden, weil schon im Setup arranged wurde)
    //Act - Methode aufrufen
    //Assert - Erwartung prüfen
    [Test]
    public async Task ReceiveMetadataThatExistTest()
    {
        //Arrange : prepare db context / create repo-instance
        var context = GetInMemoryDbContext();
        var documentMetaRepo = new DocumentMetaRepository(context);
        
        //Act : Get a document
        var found = await documentMetaRepo.GetDocument("001");
        
        //Assert : Entry is not Null
        ClassicAssert.IsNotNull(found);

        //Assert : Entriesequals to data
        Assert.That("schedule.excel", Is.EqualTo(found.Filename));
        Assert.That("Jaspher", Is.EqualTo(found.Author));
    }


    [Test]
    public async Task MetadataDoesntExistTest()
    {
        //Arrange : prepare db context / create repo-instance
        var context = GetInMemoryDbContext();
        var documentMetaRepo = new DocumentMetaRepository(context);
        
        //Act : Try to get "non-existent" document
        var found = await documentMetaRepo.GetDocument("003");
        
        //Assert : should be null
        ClassicAssert.IsNull(found);
    }
    
    
    [Test]
    public async Task MetadataEditSuccessTest()
    {
        //Arrange : Prepare DbContext / create repo-instance / Create MetaData-Object
        var context = GetInMemoryDbContext();
        var documentMetaRepo = new DocumentMetaRepository(context);
        var toEdit = new MetaData()
        {
            Author = "Tom",
            Description = "Mein Stundenplan wurde bearbeitet",
            Filename = "schedule12_updated.excel",
            Updated = DateTime.UtcNow
        };
        
        //Act : Edit Document
        await documentMetaRepo.EditDocument(toEdit);
        var found = await documentMetaRepo.GetDocument("002");

        //Assert : Entry should still exist
        ClassicAssert.IsNotNull(found);

        //Assert : Entries equals to new data
        Assert.That("schedule12_updated.excel", Is.EqualTo(found.Filename));
        Assert.That("Tom", Is.EqualTo(found.Author));
    }
    
    [Test]
    public async Task MetadataDeleteSuccessTest()
    {
        //Arrange : prepare db context / create repo-instance
        var context = GetInMemoryDbContext();
        var documentMetaRepo = new DocumentMetaRepository(context);
        
        //Act : Delete Entry
        await documentMetaRepo.RemoveDocument("002");
        var found = await documentMetaRepo.GetDocument("002");

        //Assert : Entry should be removed
        ClassicAssert.IsNull(found);
    }
}