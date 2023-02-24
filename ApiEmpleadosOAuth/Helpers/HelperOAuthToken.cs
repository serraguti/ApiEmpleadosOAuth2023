using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiEmpleadosOAuth.Helpers
{
    public class HelperOAuthToken
    {
        private string Issuer { get; set; }
        private string Audience { get; set; }
        private string SecretKey { get; set; }

        public HelperOAuthToken(IConfiguration configuration)
        {
            this.Issuer = configuration.GetValue<string>("ApiOAuth:Issuer");
            this.Audience = configuration.GetValue<string>("ApiOAuth:Audience");
            this.SecretKey = configuration.GetValue<string>("ApiOAuth:SecretKey");
        }

        //NECESITAMOS UN METODO PARA GENERAR EL TOKEN A PARTIR DEL SECRET KEY
        public SymmetricSecurityKey GetKeyToken()
        {
            byte[] data =
                Encoding.UTF8.GetBytes(this.SecretKey);
            return new SymmetricSecurityKey(data);
        }

        //DEBEMOS CONFIGURAR LAS OPCIONES PARA LA VALIDACION
        //DE NUESTRO TOKEN
        public Action<JwtBearerOptions> GetJwtOptions()
        {
            Action<JwtBearerOptions> options =
                new Action<JwtBearerOptions>(options =>
                {
                    options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateAudience = true, 
                        ValidateLifetime = true,
                        ValidateActor = true,
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidIssuer = this.Issuer,
                        ValidAudience = this.Audience,
                        IssuerSigningKey = this.GetKeyToken()
                    };
                });
            return options;
        }

        public Action<AuthenticationOptions> GetAuthenticationOptions()
        {
            Action<AuthenticationOptions> options =
                new Action<AuthenticationOptions>(options =>
                {
                    options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme =
                    JwtBearerDefaults.AuthenticationScheme;
                });
            return options;
        }
    }
}
