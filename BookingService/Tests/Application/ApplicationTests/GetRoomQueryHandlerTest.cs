using Application.Room.Queries;
using Domain.Entities;
using Domain.Room.Ports;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationTests
{
    public class GetRoomQueryHandlerTests
    {
        [Test]
        public async Task ShouldReturnRoom()
        {
            var query = new GetRoomQuery { Id = 1 };

            var repoMock = new Mock<IRoomRepository>();
            var fakeRoom = new Room() { Id = 1 };
            repoMock.Setup(x => x.Get(query.Id)).Returns(Task.FromResult(fakeRoom));

            var handler = new GetRoomQueryHandler(repoMock.Object);
            var response = await handler.Handle(query, CancellationToken.None);

            Assert.True(response.Success);
            Assert.NotNull(response.Data);
        }

        [Test]
        public async Task ShouldReturnProperErrorMessageWhenRoomNotFound()
        {
            var query = new GetRoomQuery { Id = 1 };

            var repoMock = new Mock<IRoomRepository>();

            var handler = new GetRoomQueryHandler(repoMock.Object);
            var response = await handler.Handle(query, CancellationToken.None);

            Assert.False(response.Success);
            Assert.AreEqual(response.ErrorCode, Application.ErrorCodes.ROOM_NOT_FOUND);
            Assert.AreEqual(response.Message, "Could not find a Room with the given Id");
        }
    }
}