using Garagato.MVC.Models.Authentication.Dto;
using Garagato.MVC.Models.Authentication.Mappers;
using Garagato.MVC.Models.DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Garagato.MVC.Controllers
{
    public class RegisterController : Controller
    {

        private readonly DataBaseConfig _context;

        public RegisterController(DataBaseConfig context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Register")]
        public IActionResult Register(UsuarioDto usuarioDto)
        {
            if (ModelState.IsValid)
            {
                if (_context.Usuarios.Any(us => us.Name == usuarioDto.Name || us.Email == usuarioDto.Email))
                {
                    ModelState.AddModelError("", "El nombre de usuario o el correo electronico ya están en uso");
                    return View(usuarioDto);
                }

                var usuario = RegisterMapper.ToEntity(usuarioDto);

                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                return RedirectToAction("Bienvenida");
            }

            return View(usuarioDto);
        }
    }
}
