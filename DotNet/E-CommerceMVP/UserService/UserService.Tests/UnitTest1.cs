using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using UserService.Application.DTOs;
using UserService.Application.Services;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;
using Xunit;

namespace UserService.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockRepository;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly IUserService _userService;

        public UserServiceTests()
        {
            _mockRepository = new Mock<IUserRepository>();
            _mockConfiguration = new Mock<IConfiguration>();

            // Setup mocked configuration
            _mockConfiguration
                .Setup(x => x["JwtSettings:SecretKey"])
                .Returns("fPXxcJw8TW5sA+S4rl4tIPcKk+oXAqoRBo+1s2yjUS4=");
            _mockConfiguration
                .Setup(x => x["JwtSettings:Issuer"])
                .Returns("UserService.API");
            _mockConfiguration
                .Setup(x => x["JwtSettings:AccessTokenExpirationMinutes"])
                .Returns("15");

            _userService = new Application.Services.UserService(_mockRepository.Object, _mockConfiguration.Object);
        }

        [Fact]
        public async Task RegisterAsync_WithValidData_ShouldSucceed()
        {
            // Arrange
            var registerDto = new RegisterDTO
            {
                UserName = "testuser",
                Email = "test@example.com",
                Password = "SecurePass123!",
                FullName = "Test User",
                PhoneNumber = "+1234567890"
            };

            _mockRepository
                .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User?)null);

            _mockRepository
                .Setup(x => x.FindByUserNameAsync(It.IsAny<string>()))
                .ReturnsAsync((User?)null);

            _mockRepository
                .Setup(x => x.CreateUserAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            _mockRepository
                .Setup(x => x.AddUserToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var result = await _userService.RegisterAsync(registerDto);

            // Assert
            result.Should().BeTrue();
            _mockRepository.Verify(x => x.FindByEmailAsync(registerDto.Email), Times.Once);
            _mockRepository.Verify(x => x.FindByUserNameAsync(registerDto.UserName), Times.Once);
            _mockRepository.Verify(x => x.CreateUserAsync(It.IsAny<User>(), registerDto.Password), Times.Once);
            _mockRepository.Verify(x => x.AddUserToRoleAsync(It.IsAny<User>(), "Customer"), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_WithExistingEmail_ShouldFail()
        {
            // Arrange
            var registerDto = new RegisterDTO
            {
                UserName = "testuser",
                Email = "existing@example.com",
                Password = "SecurePass123!"
            };

            var existingUser = new User { Id = Guid.NewGuid(), Email = registerDto.Email };

            _mockRepository
                .Setup(x => x.FindByEmailAsync(registerDto.Email))
                .ReturnsAsync(existingUser);

            // Act
            var result = await _userService.RegisterAsync(registerDto);

            // Assert
            result.Should().BeFalse();
            _mockRepository.Verify(x => x.CreateUserAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetProfileAsync_WithValidUserId_ShouldReturnProfile()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                UserName = "testuser",
                Email = "test@example.com",
                FullName = "Test User",
                PhoneNumber = "+1234567890",
                LastLoginAt = DateTime.UtcNow,
                ProfilePhotoUrl = "https://example.com/photo.jpg"
            };

            _mockRepository
                .Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.GetProfileAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result!.UserId.Should().Be(userId);
            result.UserName.Should().Be("testuser");
            result.Email.Should().Be("test@example.com");
            result.FullName.Should().Be("Test User");
            _mockRepository.Verify(x => x.FindByIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetProfileAsync_WithInvalidUserId_ShouldReturnNull()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _mockRepository
                .Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _userService.GetProfileAsync(userId);

            // Assert
            result.Should().BeNull();
            _mockRepository.Verify(x => x.FindByIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task UpdateProfileAsync_WithValidData_ShouldSucceed()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var updateDto = new UpdateProfileDTO
            {
                UserId = userId,
                FullName = "Updated Name",
                PhoneNumber = "+9876543210",
                ProfilePhotoUrl = "https://example.com/new-photo.jpg"
            };

            var existingUser = new User
            {
                Id = userId,
                UserName = "testuser",
                Email = "test@example.com"
            };

            _mockRepository
                .Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(existingUser);

            _mockRepository
                .Setup(x => x.UpdateUserAsync(It.IsAny<User>()))
                .ReturnsAsync(true);

            // Act
            var result = await _userService.UpdateProfileAsync(updateDto);

            // Assert
            result.Should().BeTrue();
            _mockRepository.Verify(x => x.FindByIdAsync(userId), Times.Once);
            _mockRepository.Verify(x => x.UpdateUserAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task IsUserExistsAsync_WithExistingUser_ShouldReturnTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _mockRepository
                .Setup(x => x.IsUserExistsAsync(userId))
                .ReturnsAsync(true);

            // Act
            var result = await _userService.IsUserExistsAsync(userId);

            // Assert
            result.Should().BeTrue();
            _mockRepository.Verify(x => x.IsUserExistsAsync(userId), Times.Once);
        }

        [Fact]
        public async Task IsUserExistsAsync_WithNonExistingUser_ShouldReturnFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _mockRepository
                .Setup(x => x.IsUserExistsAsync(userId))
                .ReturnsAsync(false);

            // Act
            var result = await _userService.IsUserExistsAsync(userId);

            // Assert
            result.Should().BeFalse();
            _mockRepository.Verify(x => x.IsUserExistsAsync(userId), Times.Once);
        }
    }
}

