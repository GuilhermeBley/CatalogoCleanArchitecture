using Catalogo.Application.DTOs;
using Catalogo.Application.Exceptions;
using Catalogo.Application.Interfaces;
using Catalogo.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalogo.API.Controllers
{
    [Route("api/v1/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;
        public CategoriasController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
        {
            IEnumerable<CategoriaDTO> categorias;

            try
            {
                categorias = await _categoriaService.GetCategorias();
            }
            catch (CatalogoException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }

            return Ok(categorias);
        }

        [HttpGet("{id}", Name = "GetCategoria")]
        public async Task<ActionResult<Categoria>> Get(int id)
        {
            CategoriaDTO categoria;

            try
            {
                categoria = await _categoriaService.GetById(id);
            }
            catch (CatalogoException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }

            if (categoria == null)
            {
                return NotFound();
            }
            return Ok(categoria);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CategoriaDTO categoriaDto)
        {
            try
            {
                await _categoriaService.Add(categoriaDto);
            }
            catch (CatalogoException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }


            return new CreatedAtRouteResult("GetCategoria",
                new { id = categoriaDto.Id }, categoriaDto);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.Id)
            {
                return BadRequest();
            }

            try
            {
                await _categoriaService.Update(categoriaDto);
            }
            catch (CatalogoException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }

            return Ok(categoriaDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Categoria>> Delete(int id)
        {
            var categoriaDto = await _categoriaService.GetById(id);
            if (categoriaDto == null)
            {
                return NotFound();
            }

            try
            {
                await _categoriaService.Remove(id);
            }
            catch (CatalogoException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }

            return Ok(categoriaDto);
        }
    }
}
