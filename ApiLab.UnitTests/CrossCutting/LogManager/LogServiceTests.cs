using ApiLab.CrossCutting.LogManager;
using Microsoft.Extensions.Logging;
using Moq;

namespace ApiLab.UnitTests.CrossCutting.LogManager
{
    public class LogServiceTests
    {
        private readonly Mock<ILogger<LogService>> _loggerMock;
        private readonly LogService _logService;

        public LogServiceTests()
        {
            _loggerMock = new Mock<ILogger<LogService>>();
            _logService = new LogService(_loggerMock.Object);
        }

        [Fact]
        public void LogInformation_ShouldCallLogWithCorrectParameters()
        {
            // Arrange
            var message = "Test information message";
            var data = new { Id = 1, Name = "Test" };

            // Act
            _logService.LogInformation(message, data);

            // Assert
            VerifyLoggerWasCalled(LogLevel.Information, message, null, data);
        }

        [Fact]
        public void LogWarning_WithoutException_ShouldCallLogWithCorrectParameters()
        {
            // Arrange
            var message = "Test warning message";
            var data = new { Id = 1, Name = "Test" };

            // Act
            _logService.LogWarning(message, null, data);

            // Assert
            VerifyLoggerWasCalled(LogLevel.Warning, message, null, data);
        }

        [Fact]
        public void LogWarning_WithException_ShouldCallLogWithCorrectParameters()
        {
            // Arrange
            var message = "Test warning message";
            var exception = new InvalidOperationException("Test exception");
            var data = new { Id = 1, Name = "Test" };

            // Act
            _logService.LogWarning(message, exception, data);

            // Assert
            VerifyLoggerWasCalled(LogLevel.Warning, message, exception, data);
        }

        [Fact]
        public void LogError_WithoutException_ShouldCallLogWithCorrectParameters()
        {
            // Arrange
            var message = "Test error message";
            var data = new { Id = 1, Name = "Test" };

            // Act
            _logService.LogError(message, null, data);

            // Assert
            VerifyLoggerWasCalled(LogLevel.Error, message, null, data);
        }

        [Fact]
        public void LogError_WithException_ShouldCallLogWithCorrectParameters()
        {
            // Arrange
            var message = "Test error message";
            var exception = new InvalidOperationException("Test exception");
            var data = new { Id = 1, Name = "Test" };

            // Act
            _logService.LogError(message, exception, data);

            // Assert
            VerifyLoggerWasCalled(LogLevel.Error, message, exception, data);
        }

        [Fact]
        public void Write_SingleLogInfo_WithCodeAndNoPrefix_ShouldCallLogWithCodeInMessage()
        {
            // Arrange
            var logInfo = new LogInfo
            {
                Level = LoggingLevel.Information,
                Message = "Test message",
                Code = "CODE123",
                InformationData = new { Id = 1, Name = "Test" }
            };

            // Act
            _logService.Write(logInfo);

            // Assert
            var expectedMessage = "CODE123 - Test message";
            VerifyLoggerWasCalled(LogLevel.Information, expectedMessage, null, logInfo.InformationData);
        }

        [Fact]
        public void Write_SingleLogInfo_WithPrefixAndNoCode_ShouldCallLogWithPrefixInMessage()
        {
            // Arrange
            var logInfo = new LogInfo
            {
                Level = LoggingLevel.Warning,
                Message = "Test message",
                InformationData = new { Id = 1, Name = "Test" }
            };
            var prefix = "PREFIX";

            // Act
            _logService.Write(logInfo, prefix);

            // Assert
            var expectedMessage = "PREFIX - Test message";
            VerifyLoggerWasCalled(LogLevel.Warning, expectedMessage, null, logInfo.InformationData);
        }

        [Fact]
        public void Write_SingleLogInfo_WithCodeAndPrefix_ShouldPrioritizeCode()
        {
            // Arrange
            var logInfo = new LogInfo
            {
                Level = LoggingLevel.Error,
                Message = "Test message",
                Code = "CODE123",
                InformationData = new { Id = 1, Name = "Test" }
            };
            var prefix = "PREFIX";

            // Act
            _logService.Write(logInfo, prefix);

            // Assert
            var expectedMessage = "CODE123 - Test message";
            VerifyLoggerWasCalled(LogLevel.Error, expectedMessage, null, logInfo.InformationData);
        }

        [Fact]
        public void Write_SingleLogInfo_WithCorrelationAndFlowId_ShouldAddPropertiesToLogContext()
        {
            // Arrange
            var logInfo = new LogInfo
            {
                Level = LoggingLevel.Information,
                Message = "Test message",
                CorrelationId = "correlation-123",
                FlowId = "flow-456"
            };

            // Act
            _logService.Write(logInfo);

            // Assert
            var expectedMessage = "Test message";
            VerifyLoggerWasCalled(LogLevel.Information, expectedMessage, null, string.Empty);
        }

        [Fact]
        public void Write_MultipleLogInfos_ShouldCallLogMultipleTimes()
        {
            // Arrange
            var logInfos = new List<LogInfo>
            {
                new() { Level = LoggingLevel.Information, Message = "Info message" },
                new() { Level = LoggingLevel.Warning, Message = "Warning message" },
                new() { Level = LoggingLevel.Error, Message = "Error message", Exception = new Exception("Test") }
            };
            var prefix = "PREFIX";

            // Act
            _logService.Write(logInfos, prefix);

            // Assert
            // Verificar cada chamada individualmente em vez de usar o método auxiliar
            _loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v != null && v.ToString()!.Contains("PREFIX - Info message")),
                    It.Is<Exception>(e => e == null),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

            _loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Warning),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v != null && v.ToString()!.Contains("PREFIX - Warning message")),
                    It.Is<Exception>(e => e == null),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

            _loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v != null && v.ToString()!.Contains("PREFIX - Error message")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public void Write_LogInfoWithDataAndNoSeparator_ShouldFormatMessageCorrectly()
        {
            // Arrange
            var logInfo = new LogInfo
            {
                Level = LoggingLevel.Information,
                Message = "Test message"
            };

            // Act
            _logService.Write(logInfo);

            // Assert
            VerifyLoggerWasCalled(LogLevel.Information, "Test message", null, string.Empty);
        }

        [Fact]
        public void Write_DefaultLogInfo_ShouldUseInformationLevel()
        {
            // Arrange
            var logInfo = new LogInfo
            {
                Message = "Test default level message"
            };

            // Act
            _logService.Write(logInfo);

            // Assert
            VerifyLoggerWasCalled(LogLevel.Information, "Test default level message", null, null);
        }

        private void VerifyLoggerWasCalled(LogLevel level, string message, Exception? exception, object? data)
        {
            var expectedFormattedMessage = data != null ? $"{message} - {{@InformationData}}" : $"{message}{{@InformationData}}";

            _loggerMock.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == level),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v != null && v.ToString()!.Contains(message)),
                It.Is<Exception?>(e => exception == null ? e == null : e!.GetType() == exception.GetType()),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
        }
    }
}
