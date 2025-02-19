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

        ControllerWarning_2001, //ClientesController
        ControllerWarning_2002, //ClientesController
        ControllerWarning_2003, //ClientesController
        ControllerWarning_2004, //ClientesController
        ControllerWarning_2005, //ClientesController
        ControllerWarning_2006, //ClientesController
    }
}
