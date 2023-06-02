using LibraseMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraseMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioAPIController : ControllerBase
    {

        [HttpPost]
        [Route("/api/[controller]/cadastroUsuario")]
        public IActionResult cadastrarUsuario([FromBody] Usuario usuario) {

            return Ok(new { mensagem = usuario.cadastrarUsuario()});

        }

        [HttpPost]
        [Route("/api/[controller]/logarUsuario")]
        public IActionResult entrarUsuario([FromBody] Usuario usuario) {


            return Ok(new { mensagem = usuario.entrarContaUsuario()});

        }

        public IActionResult addAvatarUsuario([FromBody] Usuario usuario) {

            return Ok(new { mensagem = usuario.adicionarAvatar(usuario.Email)});


        }

        [HttpPut]
        [Route("/api/[controller]/descricaoUsuario")]
        public IActionResult addDescUsuario([FromBody] Usuario usuario) {

            return Ok(new { mensagem = usuario.addDescricao(usuario.Email)});


        }
        [HttpGet]
        [Route("/api/[controller]/exibirUsuario")]
        public Usuario exibirUsuario(string email) {
         

            return Usuario.exibirUsuario(email);

        }

        [HttpDelete]
        [Route("/api/[controller]/deletarUsuario")]

        public IActionResult deletarUsuario(string email)
        {
            return Ok(new { mensagem = Usuario.deletarUsuario(email)});
        }


    }
}
