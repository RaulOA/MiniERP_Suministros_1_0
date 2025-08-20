/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.Server/Controllers/ProductCategoryController.cs
API REST de categorías de producto. GET, GET por id, POST (crear), PUT (actualización parcial) y DELETE.
*/

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MiniERP_Suministros.Core.Services.Shop;
using MiniERP_Suministros.Server.ViewModels.Shop;

namespace MiniERP_Suministros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IProductCategoryService _service;

        public ProductCategoryController(IMapper mapper, ILogger<ProductCategoryController> logger,
            IProductCategoryService service)
        {
            _mapper = mapper;
            _logger = logger;
            _service = service;
        }

        /// <summary>Obtiene todas las categorías con datos relacionados.</summary>
        [HttpGet]
        public IActionResult Get()
        {
            var data = _service.GetAllCategoriesData();
            return Ok(_mapper.Map<IEnumerable<ProductCategoryVM>>(data));
        }

        /// <summary>Obtiene una categoría por id.</summary>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();
            return Ok(_mapper.Map<ProductCategoryVM>(entity));
        }

        /// <summary>Crea una nueva categoría.</summary>
        [HttpPost]
        public IActionResult Post([FromBody] ProductCategoryVM value)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            if (string.IsNullOrWhiteSpace(value.Name)) return BadRequest("Name is required");

            var created = _service.Create(value.Name!, value.Description, value.Icon);
            var result = _mapper.Map<ProductCategoryVM>(created);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, result);
        }

        /// <summary>Actualiza parcialmente una categoría (solo campos no nulos).</summary>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ProductCategoryVM value)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            try
            {
                var updated = _service.UpdatePartial(id,
                    name: value.Name,
                    description: value.Description,
                    icon: value.Icon);
                return Ok(_mapper.Map<ProductCategoryVM>(updated));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>Elimina una categoría. Retorna 409/Conflict si tiene productos relacionados.</summary>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _service.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}
