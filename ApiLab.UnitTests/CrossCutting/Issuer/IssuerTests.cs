using ApiLab.CrossCutting.Issuer;

namespace ApiLab.UnitTests.CrossCutting.Issuer
{
    public class IssuerTests
    {
        [Fact]
        public void Constructor_ShouldInitializeIssuerDataWithDefaultValues()
        {
            // Arrange & Act
            var issuer = new ApiLab.CrossCutting.Issuer.Issuer();

            // Assert
            Assert.NotNull(issuer.IssuerData);
            Assert.Equal("XX0", issuer.IssuerData.Sigla);
            Assert.Equal("ApiLab", issuer.IssuerData.ProjectName);
            Assert.Equal("XX0.ApiLab", issuer.IssuerData.Prefix);
        }

        [Fact]
        public void Prefix_ShouldReturnIssuerDataPrefix()
        {
            // Arrange
            var issuer = new ApiLab.CrossCutting.Issuer.Issuer();

            // Act
            var result = issuer.Prefix;

            // Assert
            Assert.Equal("XX0.ApiLab", result);
        }

        [Fact]
        public void MakerCode_WithGeneralErrorIssue_ShouldExtractCodeFromIssueName()
        {
            // Arrange
            var issuer = new ApiLab.CrossCutting.Issuer.Issuer();
            var issue = Issues.GeneralError_500;

            // Act
            var result = issuer.MakerCode(issue);

            // Assert
            Assert.Equal("500", result);
        }

        [Fact]
        public void MakerProtocol_WithFilterErrorIssue_ShouldCreateFormattedProtocol()
        {
            // Arrange
            var issuer = new ApiLab.CrossCutting.Issuer.Issuer();
            var issue = Issues.FilterError_0002;

            // Act
            var result = issuer.MakerProtocol(issue);

            // Assert
            Assert.Equal("XX0.ApiLab.0002", result);
            Assert.Equal("0002", issuer.IssuerData.IssuerNumber);
        }

        [Theory]
        [InlineData(Issues.None_000, "000")]
        [InlineData(Issues.GeneralError_500, "500")]
        [InlineData(Issues.LogManagerError_5001, "5001")]
        [InlineData(Issues.FilterError_0001, "0001")]
        [InlineData(Issues.ControllerError_4001, "4001")]
        [InlineData(Issues.ControllerWarning_2001, "2001")]
        public void MakerCode_WithDifferentIssues_ShouldExtractCorrectCodes(Issues issue, string expectedCode)
        {
            // Arrange
            var issuer = new ApiLab.CrossCutting.Issuer.Issuer();

            // Act
            var result = issuer.MakerCode(issue);

            // Assert
            Assert.Equal(expectedCode, result);
        }

        [Fact]
        public void MakerProtocol_ShouldUseCustomSiglaAndProjectName()
        {
            // Arrange
            var issuer = new ApiLab.CrossCutting.Issuer.Issuer
            {
                IssuerData = new IssuerData
                {
                    Sigla = "ABC",
                    ProjectName = "CustomProject"
                }
            };
            var issue = Issues.ControllerWarning_2006;

            // Act
            var result = issuer.MakerProtocol(issue);

            // Assert
            Assert.Equal("ABC.CustomProject.2006", result);
            Assert.Equal("2006", issuer.IssuerData.IssuerNumber);
        }

        [Fact]
        public void MakerCode_WithStringParameter_ShouldDelegateToBaseImplementation()
        {
            // Arrange
            var issuer = new ApiLab.CrossCutting.Issuer.Issuer();
            var issueString = "ControllerError_4006";

            // Act
            var baseResult = issuer.MakerCode(issueString);
            var enumResult = issuer.MakerCode(Issues.ControllerError_4006);

            // Assert
            Assert.Equal("4006", baseResult);
            Assert.Equal(baseResult, enumResult);
        }

        [Fact]
        public void MakerProtocol_WithComplexIssueName_ShouldUseCorrectIssuerNumber()
        {
            // Arrange
            var issuer = new ApiLab.CrossCutting.Issuer.Issuer();
            var issue = Issues.LogManagerError_5001;

            // Act
            var result = issuer.MakerProtocol(issue);

            // Assert
            Assert.Equal("XX0.ApiLab.5001", result);
            Assert.Equal("5001", issuer.IssuerData.IssuerNumber);
        }

        [Fact]
        public void MakerProtocol_WithMultipleCallsToMakerProtocol_ShouldUpdateIssuerNumber()
        {
            // Arrange
            var issuer = new ApiLab.CrossCutting.Issuer.Issuer();

            // Act
            var result1 = issuer.MakerProtocol(Issues.ControllerError_4001);
            var result2 = issuer.MakerProtocol(Issues.ControllerWarning_2003);

            // Assert
            Assert.Equal("XX0.ApiLab.4001", result1);
            Assert.Equal("XX0.ApiLab.2003", result2);
            Assert.Equal("2003", issuer.IssuerData.IssuerNumber);
        }

        [Fact]
        public void MakerCode_ShouldHandleValuesThatContainUnderscoresBeforeNumber()
        {
            // Arrange
            var issuer = new ApiLab.CrossCutting.Issuer.Issuer();
            var issueString = "Controller_Warning_2005";

            // Act
            var result = issuer.MakerCode(issueString);

            // Assert
            Assert.Equal("2005", result);
        }
    }
}