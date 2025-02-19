namespace Apilab.Application.AppServices.Interfaces
{
    public interface ITokenService
    {
        /// <summary>
        /// Gera um token JWT Simplificado
        /// </summary>
        /// <returns>Token gerado</returns>
        string GenerateToken();
    }
}
