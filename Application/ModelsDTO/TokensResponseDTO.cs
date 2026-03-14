using System.Text.Json.Serialization;

namespace Application.ModelsDTO
{
    public class TokensResponseDTO
    {
        [JsonPropertyName("access_token")]
        public TokenDTO? AccessToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public TokenDTO? RefreshToken { get; set; }
    }
}
