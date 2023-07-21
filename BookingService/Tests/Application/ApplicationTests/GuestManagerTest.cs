using NUnit.Framework;
using Application;
using Application.Guest;
using Domain.Ports;
using System.Threading.Tasks;
using Application.Guest.DTO;
using Application.Guest.Requests;
using Moq;
using Domain.Entities;

namespace ApplicationTests;
public class GuestManagerTest
{
    GuestManager guestManager;

    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public async Task HappyPath()
    {
        var guestDTO = new GuestDTO()
        {
            Name = "Maria",
            Surname = "Santos",
            Email = "test@test.com",
            IdNumber = "78569",
            IdTypeCode = 1
        };

        int expectedId = 222;

        var request = new CreateGuestRequest()
        {
            Data = guestDTO,
        };

        var mockRepo = new Mock<IGuestRepository>();

        mockRepo.Setup(x => x.Create(
            It.IsAny<Guest>())).Returns(Task.FromResult(expectedId));

        guestManager = new GuestManager(mockRepo.Object);

        var response = await guestManager.CreateGuest(request);
        Assert.IsNotNull(response);
        Assert.True(response.Success);
        Assert.AreEqual(response.Data.Id, expectedId);
        Assert.AreEqual(response.Data.Name, guestDTO.Name);
    }

    [TestCase("a")]
    [TestCase("ab")]
    [TestCase("abc")]
    [TestCase("")]
    [TestCase(null)]
    public async Task ShouldReturnInvalidDocumentIdExceptionWhenDocumentAreInvalid(string documentNumber)
    {
        var guestDTO = new GuestDTO()
        {
            Name = "Maria",
            Surname = "Santos",
            Email = "test@test.com",
            IdNumber = documentNumber,
            IdTypeCode = 1
        };

        var request = new CreateGuestRequest()
        {
            Data = guestDTO,
        };

        var mockRepo = new Mock<IGuestRepository>();

        mockRepo.Setup(x => x.Create(
            It.IsAny<Guest>())).Returns(Task.FromResult(222));

        guestManager = new GuestManager(mockRepo.Object);

        var response = await guestManager.CreateGuest(request);
        Assert.IsNotNull(response);
        Assert.False(response.Success);
        Assert.AreEqual(response.ErrorCode, ErrorCodes.INVALID_DOCUMENT_ID);
        Assert.AreEqual(response.Message, "Invalid Id");
    }

    [TestCase("", "Da Silva", "email@test.com")]
    [TestCase(null, "Da Silva", "email@test.com")]
    [TestCase("Fulano", "", "email@test.com")]
    [TestCase("Fulano", null, "email@test.com")]
    [TestCase("Fulano", "Da Silva", "")]
    [TestCase("Fulano", "Da Silva", null)]
    public async Task ShouldReturnMissingRequiredInformationExceptionWhenDocumentAreInvalid(
        string name, 
        string surname, 
        string email)
    {
        var guestDTO = new GuestDTO()
        {
            Name = name,
            Surname = surname,
            Email = email,
            IdNumber = "78542",
            IdTypeCode = 1
        };

        var request = new CreateGuestRequest()
        {
            Data = guestDTO,
        };

        var mockRepo = new Mock<IGuestRepository>();

        mockRepo.Setup(x => x.Create(
            It.IsAny<Guest>())).Returns(Task.FromResult(222));

        guestManager = new GuestManager(mockRepo.Object);

        var response = await guestManager.CreateGuest(request);
        Assert.IsNotNull(response);
        Assert.False(response.Success);
        Assert.AreEqual(response.ErrorCode, ErrorCodes.MISSING_REQUIRED_INFORMATION);
        Assert.AreEqual(response.Message, "Missing required information");
    }

    [TestCase("b@b.com")]
    public async Task ShouldReturnInvalidEmailExceptionWhenEmailAreInvalid(string email)
    {
        var guestDTO = new GuestDTO()
        {
            Name = "Cicrano",
            Surname = "Dos Santos",
            Email = email,
            IdNumber = "245776",
            IdTypeCode = 1
        };

        var request = new CreateGuestRequest()
        {
            Data = guestDTO,
        };

        var mockRepo = new Mock<IGuestRepository>();

        mockRepo.Setup(x => x.Create(
            It.IsAny<Guest>())).Returns(Task.FromResult(222));

        guestManager = new GuestManager(mockRepo.Object);

        var response = await guestManager.CreateGuest(request);
        Assert.IsNotNull(response);
        Assert.False(response.Success);
        Assert.AreEqual(response.ErrorCode, ErrorCodes.INVALID_EMAIL);
        Assert.AreEqual(response.Message, "Invalid Email");
    }

    [Test]
    public async Task ShouldReturnGuestNotFoundWhenGuestDoesntExist()
    {
        var mockRepo = new Mock<IGuestRepository>();

        mockRepo.Setup(x => x.Get(333)).Returns(Task.FromResult<Guest?>(null));

        guestManager = new GuestManager(mockRepo.Object);

        var response = await guestManager.GetGuest(333);

        Assert.IsNotNull(response);
        Assert.False(response.Success);
        Assert.AreEqual(response.ErrorCode, ErrorCodes.GUEST_NOT_FOUND);
        Assert.AreEqual(response.Message, "No Guest record was found with the given Id");
    }

    [Test]
    public async Task ShoulReturnGuestSuccess()
    {
        var mockRepo = new Mock<IGuestRepository>();

        var mockGuest = new Guest
        {
            Id = 333,
            Name = "Test",
            DocumentId = new Domain.ValueObjects.PersonId
            {
                DocumentType = Domain.Enums.DocumentType.CPF,
                IdNumber = "123"
            }
        };

        mockRepo.Setup(x => x.Get(333)).Returns(Task.FromResult((Guest?)mockGuest));

        guestManager = new GuestManager(mockRepo.Object);

        var response = await guestManager.GetGuest(333);

        Assert.IsNotNull(response);
        Assert.True(response.Success);
        Assert.AreEqual(response.Data.Id, mockGuest.Id);
        Assert.AreEqual(response.Data.Name, mockGuest.Name);
    }
}   
