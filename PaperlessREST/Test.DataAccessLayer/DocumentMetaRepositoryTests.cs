using Dal;
using Microsoft.EntityFrameworkCore;
using Models;
using NUnit.Framework.Legacy;

namespace Test.DataAccessLayer;

[TestFixture]
public class DocumentMetaRepositoryTests
{
    private MetadataDbContext _dbContext;


    private MetadataDbContext GetInMemoryDbContext()
    {
        // setup for inMemory-db
        var options = new DbContextOptionsBuilder<MetadataDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new MetadataDbContext(options);
        
        // create new entries
        context.MetaData.AddRange(
            new MetaData {   
                id = "001",
                author = "Jaspher",
                description = "Hier liegt mein Stundeplan",
                filename = "schedule.excel",
                updated = "020900"
            },
            new MetaData {   
                id = "002",
                author = "Tom",
                description = "Hier liegt mein Stundeplan",
                filename = "schedule12.excel",
                updated = "123456"
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
        Assert.That("schedule.excel", Is.EqualTo(found.filename));
        Assert.That("Jaspher", Is.EqualTo(found.author));
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
            author = "Tom",
            description = "Mein Stundenplan wurde bearbeitet",
            filename = "schedule12_updated.excel",
            updated = "654321"
        };
        
        //Act : Edit Document
        await documentMetaRepo.EditDocument("002", toEdit);
        var found = await documentMetaRepo.GetDocument("002");

        //Assert : Entry should still exist
        ClassicAssert.IsNotNull(found);

        //Assert : Entries equals to new data
        Assert.That("schedule12_updated.excel", Is.EqualTo(found.filename));
        Assert.That("Tom", Is.EqualTo(found.author));
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