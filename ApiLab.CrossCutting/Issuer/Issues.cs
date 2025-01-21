namespace ApiLab.CrossCutting.Issuer
{
    public enum Issues
    {
        None,

        GeneralError_500, //ExceptionMiddleware
        GeneralError_501, //ApiServicesExtensions

        LogManagerError_5001, //LogManager

        FilterError_0001, //ApiKeyValidationFilter
        FilterError_0002, //ApiKeyValidationFilter

        IServiceCollectionError_1001, //HealthCheck

        AppServiceError_2001, //GoogleAnalyticsAppService
        AppServiceError_2002, //GoogleAnalyticsAppService

        ControllerError_4001, //GoogleAnalyticsController
        ControllerError_4002, //GoogleAnalyticsController
        ControllerWarning_4101, //GoogleAnalyticsController
        ControllerWarning_4102, //GoogleAnalyticsBatchController
    }
}
