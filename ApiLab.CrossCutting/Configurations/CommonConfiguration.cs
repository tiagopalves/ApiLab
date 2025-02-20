using System.Diagnostics.CodeAnalysis;

namespace ApiLab.CrossCutting.Configurations
{
    [ExcludeFromCodeCoverage]
    public class CommonConfiguration
    {
        public string AppName { get; set; } = string.Empty;
    }
}
