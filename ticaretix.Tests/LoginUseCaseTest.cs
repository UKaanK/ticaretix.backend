using Moq;
using System;
using System.Threading.Tasks;
using ticaretix.Application.Dtos;
using ticaretix.Application.Interfaces;
using ticaretix.Application.UseCases;
using ticaretix.Core.Entities;
using ticaretix.Core.Interfaces;
using Xunit;

namespace ticaretix.Tests
{
    public class LoginUseCaseTests
    {
        private readonly Mock<IKullaniciRepository> _userRepositoryMock;
        private readonly Mock<IJwtService> _jwtServiceMock;
        private readonly Mock<IRedisService> _redisServiceMock;
        private readonly LoginUseCase _loginUseCase;

        public LoginUseCaseTests()
        {
            // Mock bağımlılıkları oluşturuyoruz
            _userRepositoryMock = new Mock<IKullaniciRepository>();
            _jwtServiceMock = new Mock<IJwtService>();
            _redisServiceMock = new Mock<IRedisService>();

            // LoginUseCase'i test için oluşturuyoruz
            _loginUseCase = new LoginUseCase(
                _userRepositoryMock.Object,
                _jwtServiceMock.Object,
                _redisServiceMock.Object
            );
        }

        [Fact]
        public async Task Login_ShouldReturnAccessToken_WhenCredentialsAreValid()
        {
            // Arrange
            var loginDto = new KullaniciLoginDto { Email = "murat@gmail.com", Sifre = "987654" };
            string deviceId = "device123";

            // User repository mock
            _userRepositoryMock
                .Setup(x => x.GetKullaniciByEmailAsync(loginDto.Email))
                .ReturnsAsync(new KullaniciEntity { KullaniciID = 3, Sifre = "987654" });

            // Jwt service mock
            _jwtServiceMock
                .Setup(x => x.GenerateToken(It.IsAny<KullaniciEntity>()))
                .Returns("access123");

            _jwtServiceMock
                .Setup(x => x.GenerateRefreshToken(It.IsAny<KullaniciEntity>()))
                .Returns("refresh123");

            // Redis service mock
            _redisServiceMock
                .Setup(x => x.IncrementDeviceLoginAttemptAsync(deviceId))
                .Returns(Task.CompletedTask);

            _redisServiceMock
                .Setup(x => x.ResetUserLoginAttemptsAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            _redisServiceMock
                .Setup(x => x.ResetDeviceLoginAttemptsAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            _redisServiceMock
                .Setup(x => x.SetRefreshToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            _redisServiceMock
                .Setup(x => x.SetUserToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _loginUseCase.ExecuteAsync(loginDto, deviceId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("access123", result.accessToken);
            Assert.Equal("refresh123", result.refreshToken);
        }
    }
}
