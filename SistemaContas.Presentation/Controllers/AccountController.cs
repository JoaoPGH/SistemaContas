using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaContas.Data.Entities;
using SistemaContas.Data.Repositories;
using SistemaContas.Presentation.Models;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SistemaContas.Presentation.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Logout()
        {
         
            HttpContext.SignOutAsync
            (CookieAuthenticationDefaults.AuthenticationScheme);
          
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Login()
        {
           

            return View();
        }


        [HttpPost]
        public IActionResult Login(AccountLoginModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioRepository = new UsuarioRepository();
                    var usuario = usuarioRepository.ObterPorEmailESenha(model.Email, model.Senha);

                    if(usuario != null)
                    {
                        var usuarioModel = new UsuarioModel();
                        usuarioModel.IdUsuario = usuario.IdUsuario;

                        usuarioModel.Nome = usuario.Nome;
                        usuarioModel.Email = usuario.Email;
                        usuarioModel.DataHoraAcesso = DateTime.Now;


                        var identity = new ClaimsIdentity(new[]{new Claim(ClaimTypes.Name, JsonConvert.SerializeObject(usuarioModel))}, CookieAuthenticationDefaults.AuthenticationScheme);

                        
                        var principal = new ClaimsPrincipal(identity);
                        HttpContext.SignInAsync

                        (CookieAuthenticationDefaults.AuthenticationScheme, principal);

                        return RedirectToAction("Index", "Dashboard");
                    }

                    else
                    {
                        TempData["MensagemAlerta"] = $"Acesso negado. Não foi encontrado o usuário: '{model.Email}' ";
                    }

                }
                catch (Exception e)
                {

                    TempData["MesangemErro"] = $"Falha ao cadastrar usuário: {e.Message}";
                }
            }


            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(AccountRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioRepository = new UsuarioRepository();
                    if (usuarioRepository.ObterPorEmail(model.Email) != null)
                    {
                        TempData["MensagemAlerta"] = $"O email '{model.Email}' já está cadastrado para outro usuário";
                    }

                    else
                    {
                        var usuario = new Usuario();

                        usuario.IdUsuario = Guid.NewGuid();
                        usuario.Nome = model.Nome;
                        usuario.Email = model.Email;
                        usuario.Senha = model.Senha;
                        usuario.DataHoraCriacao = DateTime.Now;

                        
                        usuarioRepository.Inserir(usuario);

                        TempData["MensagemSucesso"] = $"Parabéns, {usuario.Nome}, sua conta de usuário foi criada com sucesso!";
                        ModelState.Clear();
                    }

                }
                catch (Exception e)
                {
                    TempData["MesangemErro"] = $"Falha ao cadastrar usuário: {e.Message}"; 
                }
            }

            return View();
        }

    
    }
}
