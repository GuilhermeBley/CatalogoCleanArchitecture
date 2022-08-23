using Catalogo.Application.DTOs;
using Catalogo.Application.Exceptions;
using Catalogo.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalogo.API.Controllers
{
    [Route("api/v1/[Controller]")]
    [ApiController]
    public class ProdutosController : Controller
    {
        private readonly IProdutoService _produtoService;

        public ProdutosController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        // api/produtos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get()
        {
            IEnumerable<ProdutoDTO> produtos;

            try
            {
                produtos = await _produtoService.GetProdutos();
            }
            catch (CatalogoException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }
            
            return Ok(produtos);
        }

        [HttpGet("{id}", Name = "GetProduto")]
        public async Task<ActionResult<ProdutoDTO>> Get(int id)
        {
            ProdutoDTO produto;

            try
            {
                produto = await _produtoService.GetById(id);
            }
            catch (CatalogoException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }

            if (produto == null)
            {
                return NotFound();
            }

            return Ok(produto);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProdutoDTO produtoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _produtoService.Add(produtoDto);
            }
            catch (CatalogoException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }

            return new CreatedAtRouteResult("GetProduto",
                new { id = produtoDto.Id }, produtoDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProdutoDTO produtoDto)
        {
            if (id != produtoDto.Id)
            {
                return BadRequest();
            }
            
            try
            {
                await _produtoService.Update(produtoDto);
            }
            catch (CatalogoException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }

            return Ok(produtoDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProdutoDTO>> Delete(int id)
        {
            var produtoDto = await _produtoService.GetById(id);
            if (produtoDto == null)
            {
                return NotFound();
            }
            await _produtoService.Remove(id);
            return Ok(produtoDto);
        }
    }
}
