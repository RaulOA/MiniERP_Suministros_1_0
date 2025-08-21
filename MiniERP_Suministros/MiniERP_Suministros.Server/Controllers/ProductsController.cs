/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.Server/Controllers/ProductsController.cs
Descripción: API REST de productos. GET/GET id/POST/PUT parcial/DELETE. Mutaciones restringidas a rol 'administrator'.
*/

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniERP_Suministros.Core.Services.Shop;
using MiniERP_Suministros.Server.ViewModels.Shop;

namespace MiniERP_Suministros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IProductService _service;

        public ProductsController(IMapper mapper, ILogger<ProductsController> logger, IProductService service)
        {
            _mapper = mapper;
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var data = _service.GetAllProductsData();
            return Ok(_mapper.Map<IEnumerable<ProductVM>>(data));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();
            return Ok(_mapper.Map<ProductVM>(entity));
        }

        [HttpPost]
        [Authorize(Roles = "administrator")] // Solo administradores pueden crear
        public IActionResult Post([FromBody] ProductVM value)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            if (string.IsNullOrWhiteSpace(value.Name)) return BadRequest("Name is required");

            try
            {
                var created = _service.Create(
                    name: value.Name!,
                    description: value.Description,
                    icon: value.Icon,
                    buyingPrice: value.BuyingPrice,
                    sellingPrice: value.SellingPrice,
                    unitsInStock: value.UnitsInStock,
                    isActive: value.IsActive,
                    isDiscontinued: value.IsDiscontinued,
                    productCategoryId: value.ProductCategoryId,
                    parentId: value.ParentId
                );

                var result = _mapper.Map<ProductVM>(created);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, result);
            }
            catch (MiniERP_Suministros.Core.Services.Shop.ProductException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "administrator")] // Solo administradores pueden editar
        public IActionResult Put(int id, [FromBody] ProductPatchVM value)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            try
            {
                var updated = _service.UpdatePartial(id,
                    name: value.Name,
                    description: value.Description,
                    icon: value.Icon,
                    buyingPrice: value.BuyingPrice,
                    sellingPrice: value.SellingPrice,
                    unitsInStock: value.UnitsInStock,
                    isActive: value.IsActive,
                    isDiscontinued: value.IsDiscontinued,
                    productCategoryId: value.ProductCategoryId,
                    parentId: value.ParentId);

                return Ok(_mapper.Map<ProductVM>(updated));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (MiniERP_Suministros.Core.Services.Shop.ProductException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "administrator")] // Solo administradores pueden eliminar
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
            catch (MiniERP_Suministros.Core.Services.Shop.ProductException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}
