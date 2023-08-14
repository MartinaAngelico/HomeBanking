using HomeBanking.Models;
using HomeBanking.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;

namespace HomeBanking.Controllers
{
        [Route("api/auth")]
        [ApiController]
        public class AuthController : ControllerBase //tendra los metodos de login y logout
        {
            private IClientRepository _clientRepository; //nueva variable de repositorio para hacer peticiones a la base
            public AuthController(IClientRepository clientRepository)
            {
                _clientRepository = clientRepository;
            }

            [HttpPost("login")] //RUTA 1, tiene que recibir desde el front los datos para poder iniciar sesion, necesita el mail y la contraseña
            public async Task<IActionResult> Login([FromBody] Client client) 
            {
                try
                {
                    Client user = _clientRepository.FindByEmail(client.Email); //buscamos al cliente en la base de datos, del cliente recibido le pasamos el mail
                    if (user == null || !String.Equals(user.Password, client.Password))
                        return Unauthorized(); //devolvemos que no esta autorizado a no hacer login en mi aplicacion 

                    var claims = new List<Claim> //variable de tipo demanda y creamos una nueva lista 
                    {
                    new Claim("Client", user.Email), //necesitamos darle permiso al CLIENT la cual el valor de ese reclamo es el mail de esa persona
                     };

                    var claimsIdentity = new ClaimsIdentity( //le damos autorizacion y creamos un obejto
                        claims, //le pasamos la lista de los clientes autorizados a entrar en mi app
                        CookieAuthenticationDefaults.AuthenticationScheme
                        );

                    await HttpContext.SignInAsync( //este metodo esta preconfigurado, le decimos como manejamos el logeo de la persona
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity)); //creamos nuevo claim con la identidad que creamos
                                                              //le manda al navegador todo lo necesario para que obtenga las cookie 
                        return Ok();

                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }

            [HttpPost("logout")] //endpoint de tipo post
            public async Task<IActionResult> Logout()
            {
                try
                {
                    await HttpContext.SignOutAsync( //metodo signount
                    CookieAuthenticationDefaults.AuthenticationScheme); //le borramos la cookie al cliente y le cerramos la sesion dentro del Back
                    return Ok();
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
        }
}
