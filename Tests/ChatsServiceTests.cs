using Application.ModelsDTO;
using Application.Services;
using Domain.Interfaces;
using Domain.Models;
using FluentAssertions;
using Moq;

namespace Tests.Services
{
    public class ChatsServiceTests
    {
        private readonly Mock<IChatsCRUDRepository> _crudRepoMock;
        private readonly ChatsService _sut;

        public ChatsServiceTests()
        {
            _crudRepoMock = new Mock<IChatsCRUDRepository>();
            _sut = new ChatsService(_crudRepoMock.Object);
        }

        [Fact]
        public async Task CreateChatAsync_ValidDto_ReturnsMappedResponse()
        {
            // Arrange
            var dto = new ChatCreateDTO { Title = "Math Class" };
            var chatId = Guid.NewGuid();

            _crudRepoMock
                .Setup(x => x.CreateChatAsync(It.IsAny<Chat>()))
                .ReturnsAsync(new Chat { Id = chatId, Title = dto.Title });

            // Act
            var result = await _sut.CreateChatAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(chatId);
            result.Title.Should().Be("Math Class");
        }

        [Fact]
        public async Task CreateChatAsync_CallsRepositoryExactlyOnce()
        {
            // Arrange
            var dto = new ChatCreateDTO { Title = "Physics" };
            _crudRepoMock
                .Setup(x => x.CreateChatAsync(It.IsAny<Chat>()))
                .ReturnsAsync(new Chat { Id = Guid.NewGuid(), Title = dto.Title });

            // Act
            await _sut.CreateChatAsync(dto);

            // Assert
            _crudRepoMock.Verify(x => x.CreateChatAsync(It.IsAny<Chat>()), Times.Once);
        }

        [Fact]
        public async Task DeleteChatAsync_ExistingChat_ReturnsMappedResponse()
        {
            // Arrange
            var chatId = Guid.NewGuid();
            var chat = new Chat { Id = chatId, Title = "ToDelete" };

            _crudRepoMock.Setup(x => x.DeleteChatAsync(chatId)).ReturnsAsync(chat);

            // Act
            var result = await _sut.DeleteChatAsync(chatId);

            // Assert
            result.Id.Should().Be(chatId);
            result.Title.Should().Be("ToDelete");
        }

        [Fact]
        public async Task GetChatAsync_ExistingChat_ReturnsMappedResponse()
        {
            // Arrange
            var chatId = Guid.NewGuid();
            _crudRepoMock
                .Setup(x => x.GetChatByIdAsync(chatId))
                .ReturnsAsync(new Chat { Id = chatId, Title = "General" });

            // Act
            var result = await _sut.GetChatAsync(chatId);

            // Assert
            result.Id.Should().Be(chatId);
            result.Title.Should().Be("General");
        }
    }
}
