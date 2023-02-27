using ApiEmpleadosOAuth.Helpers;
using ApiEmpleadosOAuth.Models;
using ApiEmpleadosOAuth.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ApiEmpleadosOAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private RepositoryEmpleados repo;
        private HelperOAuthToken helper;

        public AuthController(RepositoryEmpleados repo,
            HelperOAuthToken helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        //NECESITAMOS UN METODO PARA VALIDAR EL USUARIO/PASSWORD
        //Y LO RECIBIREMOS MEDIANTE LOGINMODEL
        //LOS ENDPOINTS PARA LAS VALIDACIONES SON CON POST
        [HttpPost]
        [Route("[action]")]
        public ActionResult Login(LoginModel model)
        {
            Empleado empleado =
                this.repo.ExisteEmpleado(model.UserName
                , int.Parse(model.Password));
            if (empleado == null) {
                //NO SON CORRECTOS LOS DATOS
                return Unauthorized();
            }
            else
            {
                //UN TOKEN CONTIENE UNAS CREDENCIALES
                SigningCredentials credentials =
                    new SigningCredentials(this.helper.GetKeyToken()
                    , SecurityAlgorithms.HmacSha256);
                //ES EL MOMENTO DE GENERAR UN TOKEN
                //EL TOKEN ESTA COMPUESTO POR ISSUER, AUDIENCE, CREDENTIALS
                //Y POR UN TIEMPO DETERMINADO
                JwtSecurityToken token =
                    new JwtSecurityToken
                    (
                        issuer: this.helper.Issuer,
                        audience: this.helper.Audience,
                        signingCredentials: credentials,
                        expires: DateTime.UtcNow.AddMinutes(30),
                        notBefore: DateTime.UtcNow
                        );
                return Ok(new
                {
                    response =
                    new JwtSecurityTokenHandler().WriteToken(token)
                }); 
            }
        }
    }
}
