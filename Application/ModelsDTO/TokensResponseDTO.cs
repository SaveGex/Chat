using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
