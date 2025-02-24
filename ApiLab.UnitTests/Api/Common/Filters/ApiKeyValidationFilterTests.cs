using ApiLab.Api.Common.Filters;
using ApiLab.CrossCutting.Common.Constants;
using ApiLab.CrossCutting.Configurations;
using ApiLab.CrossCutting.LogManager.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Moq;

namespace ApiLab.UnitTests.Api.Common.Filters
{
    public class ApiKeyValidationFilterTests
    {
        private readonly Mock<ILogManager> _mockLogManager;
        private readonly Mock<IOptionsMonitor<AccessConfiguration>> _mockOptions;
        private readonly ApiKeyValidationFilter _filter;

        public ApiKeyValidationFilterTests()
        {
            _mockLogManager = new Mock<ILogManager>();
            _mockOptions = new Mock<IOptionsMonitor<AccessConfiguration>>();

            _mockOptions.Setup(x => x.CurrentValue).Returns(new AccessConfiguration
            {
                AccessRestriction = true,
                AuthorizedApiKeys = "valid-api-key"
            });

            _filter = new ApiKeyValidationFilter(_mockLogManager.Object, _mockOptions.Object);
        }

        [Fact]
        public void OnActionExecuting_ValidApiKey_AllowsExecution()
        {
            var context = CreateActionExecutingContext("valid-api-key");

            _filter.OnActionExecuting(context);

            Assert.Null(context.Result);
        }

        [Fact]
        public void OnActionExecuting_InvalidApiKey_ReturnsUnauthorized()
        {
            var context = CreateActionExecutingContext("invalid-api-key");

            _filter.OnActionExecuting(context);

            Assert.IsType<UnauthorizedResult>(context.Result);
        }

        [Fact]
        public void OnActionExecuting_MissingApiKey_ReturnsUnauthorized()
        {
            var context = CreateActionExecutingContext(string.Empty);

            _filter.OnActionExecuting(context);

            Assert.IsType<UnauthorizedResult>(context.Result);
        }

        [Fact]
        public void OnActionExecuting_AccessRestrictionDisabled_AllowsExecution()
        {
            _mockOptions.Setup(x => x.CurrentValue).Returns(new AccessConfiguration
            {
                AccessRestriction = false
            });

            var context = CreateActionExecutingContext(string.Empty);

            _filter.OnActionExecuting(context);

            Assert.Null(context.Result);
        }

        private static ActionExecutingContext CreateActionExecutingContext(string apiKey)
        {
            var httpContext = new DefaultHttpContext();
            if (apiKey != null)
            {
                httpContext.Request.Headers[Constants.API_KEY_HEADER_KEY] = apiKey;
            }

            var actionContext = new ActionContext
            {
                HttpContext = httpContext,
                RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
                ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()
            };

            return new ActionExecutingContext(
                actionContext,
                [],
                new Dictionary<string, object?>(),
                new object());
        }
    }
}
