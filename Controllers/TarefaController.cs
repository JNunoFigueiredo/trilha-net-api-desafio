using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Migrations;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            // Made: Buscar o Id no banco utilizando o EF
            var myWork = _context.Tarefas.Find(id);
            // Made: Validar o tipo de retorno. Se não encontrar a tarefa, retornar NotFound,
            if (myWork == null)
                return NotFound();
            // caso contrário retornar OK com a tarefa encontrada
            return Ok(myWork);
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()

        {   // TODO: Buscar todas as tarefas no banco utilizando o EF
            var listWorks = _context.Tarefas.ToList();
            return Ok( listWorks);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            // Made: Buscar  as tarefas no banco utilizando o EF, que contenha o titulo recebido por parâmetro
            var listWorks = _context.Tarefas.Where( x => x.Titulo.Contains( titulo));
            // Dica: Usar como exemplo o endpoint ObterPorData
            return Ok( listWorks);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            // Made: Buscar  as tarefas no banco utilizando o EF, que contenha o status recebido por parâmetro
            var listWorks = _context.Tarefas.Where(x => x.Status == status);
            // Dica: Usar como exemplo o endpoint ObterPorData
            //var tarefa = _context.Tarefas.Where(x => x.Status == status);
            return Ok( listWorks);
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // Made: Adicionar a tarefa recebida no EF e salvar as mudanças (save changes)
            _context.Add( tarefa);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
            // Made: Atualizar as informações da variável tarefaBanco com a tarefa recebida via parâmetro
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Status = tarefa.Status;
            // made: Atualizar a variável tarefaBanco no EF e salvar as mudanças (save changes)
            _context.Tarefas.Update(tarefaBanco);
            _context.SaveChanges();
            return Ok(tarefaBanco);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            // Made: Remover a tarefa encontrada através do EF e salvar as mudanças (save changes)
            _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
