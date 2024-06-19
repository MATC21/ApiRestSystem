using ApiRestSystem.Context;
using ApiRestSystem.Custom;
using ApiRestSystem.Models;
using ApiRestSystem.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;

namespace ApiRestSystem.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly Utilidades _utilidades;

        public AccesoController(AppDbContext context, Utilidades utilidades)
        {
            _context = context;
            _utilidades = utilidades;
        }

        [HttpPost]
        [Route("Registrarse")]
        public async Task<IActionResult> Registrarser(UsuarioDTO modelo)
        {
            var modeloUsuario = new Usuarios
            {
                Nombre = modelo.Nombre,
                Correo = modelo.Correo,
                Clave = _utilidades.encriptarSHA256(modelo.Clave)
            };

            await _context.Usuarios.AddAsync(modeloUsuario);
            await _context.SaveChangesAsync();

            if (modeloUsuario.IdUsuario != 0)
            {
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false });
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDTO modelo)
        {
            var usuarioEncontrado = await _context.Usuarios
                .Where(u => 
                u.Correo == modelo.Correo && 
                u.Clave == _utilidades.encriptarSHA256(modelo.Clave))
                .FirstOrDefaultAsync();

            if (usuarioEncontrado == null)
            {
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, token = "" });
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token = _utilidades.generarJWT(usuarioEncontrado)});
            }
        }



    }
}
