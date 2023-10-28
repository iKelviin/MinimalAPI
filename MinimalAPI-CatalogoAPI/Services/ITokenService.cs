using MinimalAPI_CatalogoAPI.Models;

namespace MinimalAPI_CatalogoAPI.Services;

public interface ITokenService
{
    string GerarToken(string key, string issuer, string audience, UserModel user);
}
