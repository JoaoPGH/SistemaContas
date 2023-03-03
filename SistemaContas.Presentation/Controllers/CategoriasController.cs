using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaContas.Data.Entities;
using SistemaContas.Data.Repositories;
using SistemaContas.Presentation.Models;

namespace SistemaContas.Presentation.Controllers
{
    [Authorize]
    public class CategoriasController : Controller
    {
        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(CategoriasCadastroModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var usuarioModel = JsonConvert.DeserializeObject<UsuarioModel>(User.Identity.Name);


                    var categoria = new Categoria();
                    categoria.IdCategoria = Guid.NewGuid();
                    categoria.Nome = model.Nome;
                    categoria.IdUsuario = usuarioModel.IdUsuario;

                    var categoriaRepository = new CategoriaRepository();
                    categoriaRepository.Inserir(categoria);

                    TempData["MensagemSucesso"] = "Categoria cadastrada com sucesso.";
                    ModelState.Clear();
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = $"Falha ao cadastrar categoria: {e.Message}";
                }
            }
            else
            {
                TempData["MensagemAlerta"] = "Ocorreram erros no preenchimento do formulário, por favor verifique.";
            }

            return View();
        }


        public IActionResult Consulta()
        {
            var lista = new List<CategoriasConsultaModel>();

            try
            {

                var usuarioModel = JsonConvert.DeserializeObject<UsuarioModel>(User.Identity.Name);


                var categoriaRepository = new CategoriaRepository();
                var categorias = categoriaRepository.ObterTodos(usuarioModel.IdUsuario);

                foreach (var item in categorias)
                {
                    var model = new CategoriasConsultaModel();
                    model.IdCategoria = item.IdCategoria;
                    model.Nome = item.Nome;
                    lista.Add(model);
                }
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = $"Falha ao consultar categoria: { e.Message}";
            }

            return View(lista);
        }


        public IActionResult Exclusao(Guid id)
        {
            try
            {

                var usuarioModel = JsonConvert.DeserializeObject<UsuarioModel>(User.Identity.Name);

                var categoriaRepository = new CategoriaRepository();
                var categoria = categoriaRepository.ObterPorId(id);


                if (categoria != null && categoria.IdUsuario == usuarioModel.IdUsuario)
                {
                    var qtdContas = categoriaRepository.ObterQuantidadeContas(categoria.IdCategoria);

                    if (qtdContas == 0)
                    {

                        categoriaRepository.Excluir(categoria);
                        TempData["MensagemSucesso"] = "Categoria excluída com sucesso.";
                    }
                    else
                    {
                        TempData["MensagemAlerta"] = $"Não é possível excluir a categoria, pois ela possui {qtdContas} conta(s) vinculada(s).";
                    }
                }
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = $"Falha ao cadastrar categoria: {e.Message}";
            }

            return RedirectToAction("Consulta");
        }

        public IActionResult Edicao(Guid id)
        {
            var model = new CategoriasEdicaoModel();

            try
            {

                var usuarioModel = JsonConvert.DeserializeObject<UsuarioModel>(User.Identity.Name);


                var categoriaRepository = new CategoriaRepository();
                var categoria = categoriaRepository.ObterPorId(id);


                if (categoria != null && categoria.IdUsuario == usuarioModel.IdUsuario)
                {
                    model.IdCategoria = categoria.IdCategoria;
                    model.Nome = categoria.Nome;
                }
                else
                {
                    return RedirectToAction("Consulta");
                }
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = $"Falha ao obter a categoria: {e.Message}";
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Edicao(CategoriasEdicaoModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var categoria = new Categoria();
                    categoria.IdCategoria = model.IdCategoria;
                    categoria.Nome = model.Nome;

                    var categoriaRepository = new CategoriaRepository();
                    categoriaRepository.Atualizar(categoria);

                    TempData["MensagemSucesso"] = "Categoria atualizada com sucesso.";
                    return RedirectToAction("Consulta");
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = $"Falha ao atualizar categoria: {e.Message}";
                }
            }

            return View(model);
        }

    }
}



