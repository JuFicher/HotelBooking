using Application.Room.Commands;
using Domain.Entities;
using Domain.Room.Ports;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationTests
{
    public class CreateRoomCommandHandlerTests
    {
        [Test]
        public async Task ShouldNotCreateRoomIfNameIsNotProvided()
        {
            var command = new CreateRoomCommand()
            {
                RoomDto = new Application.Room.DTO.RoomDto()
                {
                    InMaintenance = false,
                    Level = 1,
                    Currency = Domain.Enums.AcceptedCurrencies.Dollar,
                    Name = "",
                    Price = 100
                }
            };

            var repoMock = new Mock<IRoomRepository>();
            repoMock.Setup(x => x.Create(It.IsAny<Room>()))
                .Returns(Task.FromResult(1));
            var handler = new CreateRoomCommandHandler(repoMock.Object);

            var res = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(res);
            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, Application.ErrorCodes.ROOM_MISSING_REQUIRED_INFORMATION);
            Assert.AreEqual(res.Message, "Missing required information passed");
        }

        [Test]
        public async Task ShouldNotCreateRoomIfPriceIsInvalid()
        {
            var command = new CreateRoomCommand()
            {
                RoomDto = new Application.Room.DTO.RoomDto()
                {
                    InMaintenance = false,
                    Level = 1,
                    Currency = Domain.Enums.AcceptedCurrencies.Dollar,
                    Name = "Room test",
                    Price = 9
                }
            };

            var repoMock = new Mock<IRoomRepository>();
            repoMock.Setup(x => x.Create(It.IsAny<Room>()))
                .Returns(Task.FromResult(1));
            var handler = new CreateRoomCommandHandler(repoMock.Object);

            var res = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(res);
            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, Application.ErrorCodes.ROOM_MISSING_REQUIRED_INFORMATION);
            Assert.AreEqual(res.Message, "Room price is invalid");
        }


        [Test]
        public async Task ShouldCreateRoom()
        {
            var command = new CreateRoomCommand()
            {
                RoomDto = new Application.Room.DTO.RoomDto()
                {
                    InMaintenance = false,
                    Level = 1,
                    Currency = Domain.Enums.AcceptedCurrencies.Dollar,
                    Name = "Room test",
                    Price = 100
                }
            };

            var repoMock = new Mock<IRoomRepository>();
            repoMock.Setup(x => x.Create(It.IsAny<Room>()))
                .Returns(Task.FromResult(1));
            var handler = new CreateRoomCommandHandler(repoMock.Object);

            var res = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(res);
            Assert.True(res.Success);
            Assert.NotNull(res.Data);
            Assert.AreEqual(res.Data.Id, 1);
        }
    }
}