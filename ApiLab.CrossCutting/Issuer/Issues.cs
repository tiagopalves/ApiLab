namespace ApiLab.CrossCutting.Issuer
{
    public enum Issues
    {
        None_000,

        GeneralError_500, //ExceptionHandler

        LogManagerError_5001, //LogManager

        FilterError_0001, //ApiKeyValidationFilter
        FilterError_0002, //ApiKeyValidationFilter
        FilterError_0003, //ApiKeyValidationFilter

        ControllerError_4001, //ClientesController
        ControllerError_4002, //ClientesController
        ControllerError_4003, //ClientesController
        ControllerError_4004, //ClientesController
        ControllerError_4005, //ClientesController
        ControllerError_4006, //ClientesController

        AppServiceWarning_2001, //ClientesService
        AppServiceWarning_2002, //ClientesService
    }
}
