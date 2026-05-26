using Application.Models;
using FluentAssertions;

namespace Tests.Domain
{
    public class GroupIdentifierTests
    {
        [Fact]
        public void ToString_ReturnsCorrectFormat()
        {
            // Arrange
            var id = new Guid("11111111-1111-1111-1111-111111111111");
            var identifier = new GroupIdentifier("MathClass", id);

            // Act
            var result = identifier.ToString();

            // Assert
            result.Should().Be($"MathClass_{id}");
        }

        [Fact]
        public void ToString_WithSpacesInTitle_PreservesSpaces()
        {
            // Arrange
            var id = Guid.NewGuid();
            var identifier = new GroupIdentifier("Math Class 10 A", id);

            // Act
            var result = identifier.ToString();

            // Assert
            result.Should().StartWith("Math Class 10 A_");
            result.Should().EndWith(id.ToString());
        }

        [Fact]
        public void Constructor_SetsPropertiesCorrectly()
        {
            // Arrange
            var id = Guid.NewGuid();
            const string title = "History";

            // Act
            var identifier = new GroupIdentifier(title, id);

            // Assert
            identifier.ChatTitle.Should().Be(title);
            identifier.ChatId.Should().Be(id);
        }
    }
}
