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
    public class AtividadeController : ControllerBase
    {

        [HttpPost]
        [Route("/api/[controller]/adicionarAcerto")]
        public IActionResult entrarUsuario( Atividade atividade)
        {


            return Ok(new { mensagem = atividade.adicionarAcerto() });

        }
    }
}
