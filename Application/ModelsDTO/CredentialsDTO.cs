using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ModelsDTO
{
    public class CredentialsDTO
    {
        public string Login { get; set; } = null!; // email
        public string Password { get; set; } = null!;
    }
}
