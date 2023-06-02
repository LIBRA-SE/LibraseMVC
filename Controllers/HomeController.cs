    using LibraseMVC.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

namespace LibraseMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger) {
            _logger = logger;
        }

        public IActionResult Index() {
            return View();
        }
        public IActionResult Login() {
            return View();
        }

        public IActionResult Cadastro() {
            return View();
        }

        public IActionResult Atividades() {
            return View();
        }

        public IActionResult Progresso() {

            if (HttpContext.Session.GetString("user") != null) {
                Usuario pegarId = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("user"));
                string emailUser = pegarId.Email;
                return View(Usuario.exibirUsuario(emailUser));
            }

            return View();
        }

        public IActionResult Equipe() {
            return View();
        }


        [HttpPost]
        //Adicionando foto de perfil do usuário
        public IActionResult AddAvatar(IFormFile picture__input) {
            Usuario addImg = new Usuario(picture__input);
            Usuario pegarId = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("user"));

            string resultado = addImg.adicionarAvatar(pegarId.Email);

            return RedirectToAction("Progresso");
        }

        [HttpPost]
        //Inserindo e salvando descrição do usuário
        public IActionResult AdicionarDesc(string sobre) {
            Usuario addDesc = new Usuario(null, sobre, null, null);
            Usuario pegarId = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("user"));

            string resultado = addDesc.addDescricao(pegarId.Email);

            return RedirectToAction("Progresso");
        }

        //Excluir conta do usuário
        public IActionResult apagarConta()
        {
            Usuario pegarEmail = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("user"));
            Usuario.deletarUsuario(pegarEmail.Email);
            HttpContext.Session.Remove("user");
            return View("Index");

        }

        [HttpPost]
        //Cadastrando a conta do usuário
        public IActionResult CadastroConta(string usuario, string email, string senha, string confirmar) {

            Usuario cadastrar = new Usuario(usuario, email, senha);

            if (senha.Equals(confirmar)) {
                TempData["estadoCadastro"] = cadastrar.cadastrarUsuario();
                return View("Cadastro");
            }

            return View("Cadastro");

        }

        [HttpPost]
        //Entrando na conta do usuário
        public IActionResult LoginConta(string email, string senha) {

            Usuario login = new Usuario(email, senha);
            TempData["resultadoLogin"] = login.entrarContaUsuario();

            if (TempData["resultadoLogin"] == "Sucesso") {
                //guardando na sessão
                HttpContext.Session.SetString("user", JsonConvert.SerializeObject(login));
                return View("Index");
            }
            return View("Login");
        }

        //Sair da conta
        public IActionResult Sair() {

            Usuario sair = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("user"));
            //removendo o usuário da sessão
            HttpContext.Session.Remove("user");

            return View("Index");

        }

        public IActionResult Privacy() {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
