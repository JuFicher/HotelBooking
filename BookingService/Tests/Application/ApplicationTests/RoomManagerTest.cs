using Application;
using Application.Room.DTO;
using Application.Room.Requests;
using Application.Room;
using Domain.Entities;
using Domain.Room.Ports;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using Domain.Users;
using System.Collections.Generic;

namespace ApplicationTests
{
    public class Tests
    {
        [Test]
        public async Task HappyPath()
        {
            var roomDto = new RoomDto
            {
                Name = "Test",
                Level = 7,
                InMaintenance = true,
                Price = 280,
                Currency = 0
            };

            int expectedId = 222;

            var request = new CreateRoomRequest()
            {
                Data = roomDto,
            };

            var mockRepo = new Mock<IRoomRepository>();

            mockRepo.Setup(x => x.Create(
                It.IsAny<Room>())).Returns(Task.FromResult(expectedId));

            var fakeUser = new User();
            var userMockRepo = new Mock<IUserRepository>();
            userMockRepo.Setup(x => x.Get(It.IsAny<string>())).Returns(Task.FromResult(fakeUser));

            var roomManager = new RoomManager(mockRepo.Object, userMockRepo.Object);

            var response = await roomManager.CreateRoom(request);
            Assert.IsNotNull(response);
            Assert.True(response.Success);
            Assert.AreEqual(response.Data.Id, expectedId);
            Assert.AreEqual(response.Data.Name, roomDto.Name);  
        }

        [TestCase(0)]
        [TestCase(null)]
        [TestCase(-200)]
        public async Task ShouldReturnInvalidRoomPriceExceptionWhenPriceAreInvalid(decimal price)
        {
            var roomDto = new RoomDto
            {
                Id = 1,
                Name = "Test",
                Level = 7,
                InMaintenance = true,
                Price = price,
                Currency = 0
            };

            var request = new CreateRoomRequest()
            {
                Data = roomDto,
            };

            var mockRepo = new Mock<IRoomRepository>();

            mockRepo.Setup(x => x.Create(
                It.IsAny<Room>())).Returns(Task.FromResult(333));

            var fakeUser = new User();
            var userMockRepo = new Mock<IUserRepository>();
            userMockRepo.Setup(x => x.Get(It.IsAny<string>())).Returns(Task.FromResult(fakeUser));

            var roomManager = new RoomManager(mockRepo.Object, userMockRepo.Object);

            var response = await roomManager.CreateRoom(request);

            Assert.IsNotNull(response);
            Assert.False(response.Success);
            Assert.AreEqual(response.ErrorCode, ErrorCodes.ROOM_COULD_NOT_STORE_DATA);
            Assert.AreEqual(response.Message, "The room price is incorrect");
        }

        [Test]
        public async Task ShouldReturnRoomNotFoundWhenRoomDoesntExist()
        {
            var mockRepo = new Mock<IRoomRepository>();

            mockRepo.Setup(x => x.Get(888)).Returns(Task.FromResult<Room?>(null));

            var fakeUser = new User();
            var userMockRepo = new Mock<IUserRepository>();
            userMockRepo.Setup(x => x.Get(It.IsAny<string>())).Returns(Task.FromResult(fakeUser));

            var roomManager = new RoomManager(mockRepo.Object, userMockRepo.Object);

            var response = await roomManager.GetRoom(888);

            Assert.IsNotNull(response);
            Assert.False(response.Success);
            Assert.AreEqual(response.ErrorCode, ErrorCodes.ROOM_NOT_FOUND);
            Assert.AreEqual(response.Message, "Could not find a Room with the given Id");
        }

        [Test]
        public async Task ShouldReturnRoomSuccessfully()
        {
            var mockRepo = new Mock<IRoomRepository>();

            var mockRoom = new Room
            {
                Id = 1,
                Name = "Test",
                Level = 7,
                InMaintenance = true,
                Price = new Domain.ValueObjects.Price
                {
                    Value = 280,
                    Currency = 0
                }
            };

            mockRepo.Setup(x => x.Get(333)).Returns(Task.FromResult((Room?)mockRoom));

            var fakeUser = new User();
            var userMockRepo = new Mock<IUserRepository>();
            userMockRepo.Setup(x => x.Get(It.IsAny<string>())).Returns(Task.FromResult(fakeUser));

            var roomManager = new RoomManager(mockRepo.Object, userMockRepo.Object);

            var response = await roomManager.GetRoom(333);

            Assert.IsNotNull(response);
            Assert.True(response.Success);
            Assert.AreEqual(response.Data.Id, mockRoom.Id);
            Assert.AreEqual(response.Data.Name, mockRoom.Name);
        }

        [Test]
        public async Task ShouldCreateRoomWhenUserIsManager()
        {
            var mockRepo = new Mock<IRoomRepository>();

            var request = new CreateRoomRequest()
            {
                UserName = "Manager Test"
            };

            var userMockRepo = new Mock<IUserRepository>();

            var fakeUser = new User()
            {
                Id = 1,
                Name = request.UserName,
                Roles = new List<string>() {"manager", "admin"}
            };
            
            userMockRepo.Setup(x => x.Get(request.UserName)).Returns(Task.FromResult(fakeUser));

            var manager = new RoomManager(mockRepo.Object, userMockRepo.Object);
            var response = await manager.CreateRoom(request);

            Assert.True(response.Success);
            Assert.NotNull(response.Data);
        }

        [Test]
        public async Task ShouldNotCreateRoomIfUserIsNotManager()
        {
            var mockRepo = new Mock<IRoomRepository>();

            var request = new CreateRoomRequest()
            {
                UserName = "Receptionist Test"
            };

            var userMockRepo = new Mock<IUserRepository>();

            var fakeUser = new User()
            {
                Id = 2,
                Name = request.UserName,
                Roles = new List<string>() {"receptionist"}
            };
            
            userMockRepo.Setup(x => x.Get(request.UserName)).Returns(Task.FromResult(fakeUser));

            var manager = new RoomManager(mockRepo.Object, userMockRepo.Object);
            var response = await manager.CreateRoom(request);

            Assert.False(response.Success);
            Assert.AreEqual(response.ErrorCode, ErrorCodes.ROOM_INVALID_PERMISSION);
            Assert.AreEqual(response.Message, "User does not have permission to perform this action");
        }

        [Test]
        public async Task ShouldReturnProperResponseWhenRoomNotFound()
        {
            var roomId = 1;
            var mockRepo = new Mock<IRoomRepository>();

            var fakeUser = new User();
            var userMockRepo = new Mock<IUserRepository>();
            userMockRepo.Setup(x => x.Get(It.IsAny<string>())).Returns(Task.FromResult(fakeUser));

            var manager = new RoomManager(mockRepo.Object, userMockRepo.Object);
            var response = await manager.GetRoom(roomId);


            Assert.False(response.Success);
            Assert.AreEqual(response.ErrorCode, ErrorCodes.ROOM_NOT_FOUND);
            Assert.AreEqual(response.Message, "Room not found");
        }
    }
}