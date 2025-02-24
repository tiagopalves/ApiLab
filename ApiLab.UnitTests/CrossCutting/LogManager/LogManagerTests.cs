using ApiLab.CrossCutting.Issuer.Interfaces;
using ApiLab.CrossCutting.Issuer;
using ApiLab.CrossCutting.LogManager.Interfaces;
using Moq;
using ApiLab.CrossCutting.LogManager;

namespace ApiLab.UnitTests.CrossCutting.LogManager
{
    public class LogManagerTests
    {

        private readonly Mock<IIssuer> _mockIssuer;
        private readonly Mock<ILogService> _mockLogService;
        private readonly ApiLab.CrossCutting.LogManager.LogManager _logManager;

        public LogManagerTests()
        {
            _mockIssuer = new Mock<IIssuer>();
            _mockLogService = new Mock<ILogService>();
            _logManager = new ApiLab.CrossCutting.LogManager.LogManager(_mockIssuer.Object, _mockLogService.Object);

            // Configuração padrão para o mock do IIssuer
            _mockIssuer.Setup(i => i.Prefix).Returns("TEST");
            _mockIssuer.Setup(i => i.MakerProtocol(It.IsAny<Issues>()))
                .Returns<Issues>(issue => $"TEST-{(int)issue:D4}");
        }

        [Fact]
        public void AddTrace_ShouldCallLogServiceWithCorrectParameters()
        {
            // Arrange
            string message = "Trace message";
            string correlationId = "correlation123";
            string flowId = "flow123";
            var infoData = new { Property = "Value" };

            // Act
            _logManager.AddTrace(message, correlationId, flowId, infoData);

            // Assert
            _mockLogService.Verify(l => l.Write(
                It.Is<LogInfo>(info =>
                    info.Level == LoggingLevel.Trace &&
                    info.Message == message &&
                    info.CorrelationId == correlationId &&
                    info.FlowId == flowId &&
                    info.InformationData == infoData &&
                    info.Code == "TEST-0000" &&
                    info.Exception == null
                ), "TEST"), Times.Once);
        }

        [Fact]
        public void AddTrace_WithMinimalParameters_ShouldUseDefaultValues()
        {
            // Arrange
            string message = "Minimal trace message";

            // Act
            _logManager.AddTrace(message);

            // Assert
            _mockLogService.Verify(l => l.Write(
                It.Is<LogInfo>(info =>
                    info.Level == LoggingLevel.Trace &&
                    info.Message == message &&
                    info.CorrelationId == string.Empty &&
                    info.FlowId == string.Empty &&
                    info.InformationData == null &&
                    info.Code == "TEST-0000" &&
                    info.Exception == null
                ), "TEST"), Times.Once);
        }

        [Fact]
        public void AddInformation_ShouldCallLogServiceWithCorrectParameters()
        {
            // Arrange
            string message = "Info message";
            string correlationId = "correlation123";
            string flowId = "flow123";
            var infoData = new { Property = "Value" };

            // Act
            _logManager.AddInformation(message, correlationId, flowId, infoData);

            // Assert
            _mockLogService.Verify(l => l.Write(
                It.Is<LogInfo>(info =>
                    info.Level == LoggingLevel.Information &&
                    info.Message == message &&
                    info.CorrelationId == correlationId &&
                    info.FlowId == flowId &&
                    info.InformationData == infoData &&
                    info.Code == "TEST-0000" &&
                    info.Exception == null
                ), "TEST"), Times.Once);
        }

        [Fact]
        public void AddInformation_WithMinimalParameters_ShouldUseDefaultValues()
        {
            // Arrange
            string message = "Minimal info message";

            // Act
            _logManager.AddInformation(message);

            // Assert
            _mockLogService.Verify(l => l.Write(
                It.Is<LogInfo>(info =>
                    info.Level == LoggingLevel.Information &&
                    info.Message == message &&
                    info.CorrelationId == string.Empty &&
                    info.FlowId == string.Empty &&
                    info.InformationData == null &&
                    info.Code == "TEST-0000" &&
                    info.Exception == null
                ), "TEST"), Times.Once);
        }

        [Fact]
        public void AddWarning_ShouldCallLogServiceWithCorrectParameters()
        {
            // Arrange
            Issues issue = Issues.ControllerWarning_2001;
            string message = "Warning message";
            string correlationId = "correlation123";
            string flowId = "flow123";
            var exception = new Exception("Test exception");
            var infoData = new { Property = "Value" };

            _mockIssuer.Setup(i => i.MakerProtocol(issue)).Returns("TEST-2001");

            // Act
            _logManager.AddWarning(issue, message, correlationId, flowId, exception, infoData);

            // Assert
            _mockLogService.Verify(l => l.Write(
                It.Is<LogInfo>(info =>
                    info.Level == LoggingLevel.Warning &&
                    info.Message == message &&
                    info.CorrelationId == correlationId &&
                    info.FlowId == flowId &&
                    info.InformationData == infoData &&
                    info.Code == "TEST-2001" &&
                    info.Exception == exception
                ), "TEST"), Times.Once);
        }

        [Fact]
        public void AddWarning_WithMinimalParameters_ShouldUseDefaultValues()
        {
            // Arrange
            Issues issue = Issues.ControllerWarning_2001;
            string message = "Minimal warning message";

            _mockIssuer.Setup(i => i.MakerProtocol(issue)).Returns("TEST-2001");

            // Act
            _logManager.AddWarning(issue, message);

            // Assert
            _mockLogService.Verify(l => l.Write(
                It.Is<LogInfo>(info =>
                    info.Level == LoggingLevel.Warning &&
                    info.Message == message &&
                    info.CorrelationId == string.Empty &&
                    info.FlowId == string.Empty &&
                    info.InformationData == null &&
                    info.Code == "TEST-2001" &&
                    info.Exception == null
                ), "TEST"), Times.Once);
        }

        [Fact]
        public void AddError_ShouldCallLogServiceWithCorrectParameters()
        {
            // Arrange
            Issues issue = Issues.ControllerError_4001;
            string message = "Error message";
            var exception = new Exception("Test exception");
            string correlationId = "correlation123";
            string flowId = "flow123";
            var infoData = new { Property = "Value" };

            _mockIssuer.Setup(i => i.MakerProtocol(issue)).Returns("TEST-4001");

            // Act
            _logManager.AddError(issue, message, exception, correlationId, flowId, infoData);

            // Assert
            _mockLogService.Verify(l => l.Write(
                It.Is<LogInfo>(info =>
                    info.Level == LoggingLevel.Error &&
                    info.Message == message &&
                    info.CorrelationId == correlationId &&
                    info.FlowId == flowId &&
                    info.InformationData == infoData &&
                    info.Code == "TEST-4001" &&
                    info.Exception == exception
                ), "TEST"), Times.Once);
        }

        [Fact]
        public void AddError_WithMinimalParameters_ShouldUseDefaultValues()
        {
            // Arrange
            Issues issue = Issues.ControllerError_4001;
            string message = "Minimal error message";
            var exception = new Exception("Test exception");

            _mockIssuer.Setup(i => i.MakerProtocol(issue)).Returns("TEST-4001");

            // Act
            _logManager.AddError(issue, message, exception);

            // Assert
            _mockLogService.Verify(l => l.Write(
                It.Is<LogInfo>(info =>
                    info.Level == LoggingLevel.Error &&
                    info.Message == message &&
                    info.CorrelationId == string.Empty &&
                    info.FlowId == string.Empty &&
                    info.InformationData == null &&
                    info.Code == "TEST-4001" &&
                    info.Exception == exception
                ), "TEST"), Times.Once);
        }

        [Fact]
        public void LogServiceWrite_WhenLogServiceThrowsException_ShouldLogError()
        {
            // Arrange
            string message = "Test message";
            var originalException = new Exception("Original exception");

            _mockLogService.Setup(l => l.Write(
                It.Is<LogInfo>(info => info.Level == LoggingLevel.Trace),
                It.IsAny<string>()))
                .Throws(originalException);

            _mockIssuer.Setup(i => i.MakerProtocol(Issues.LogManagerError_5001))
                .Returns("TEST-5001");

            // Act
            _logManager.AddTrace(message);

            // Assert
            _mockLogService.Verify(l => l.Write(
                It.Is<LogInfo>(info =>
                    info.Level == LoggingLevel.Error &&
                    info.Code == "TEST-5001" &&
                    info.Message.Contains(message) &&
                    info.Exception == originalException
                ), "TEST"), Times.Once);
        }
    }
}
