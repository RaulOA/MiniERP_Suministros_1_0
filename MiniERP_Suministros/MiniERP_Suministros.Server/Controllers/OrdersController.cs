/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.Server/Controllers/OrdersController.cs
API REST para gestión de pedidos: listar, obtener por id, crear, actualizar parcial y eliminar.
*/

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniERP_Suministros.Core.Services.Account;
using MiniERP_Suministros.Core.Services.Shop;
using MiniERP_Suministros.Server.ViewModels.Shop;

namespace MiniERP_Suministros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController(IOrdersService service, IMapper mapper, IUserIdAccessor userIdAccessor) : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var userId = userIdAccessor.GetCurrentUserId();
            var isAdmin = User.IsInRole("administrator");
            var data = isAdmin ? service.GetAllOrdersData() : service.GetAllOrdersDataByCashier(userId!);
            return Ok(mapper.Map<IEnumerable<OrderVM>>(data));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var entity = service.GetById(id);
            if (entity == null) return NotFound();
            return Ok(mapper.Map<OrderVM>(entity));
        }

        [HttpPost]
        public IActionResult Post([FromBody] OrderCreateVM vm)
        {
            if (vm == null || vm.Items == null || vm.Items.Count == 0) return BadRequest("Invalid payload");
            var userId = userIdAccessor.GetCurrentUserId();

            var created = service.Create(userId!, vm.CustomerId, vm.Discount, vm.Comments,
                vm.Items.Select(i => new OrderItemDraft
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Discount = i.Discount
                }));

            var result = mapper.Map<OrderVM>(created);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, result);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] OrderVM vm)
        {
            try
            {
                var updated = service.UpdatePartial(id, vm.Discount, vm.Comments);
                return Ok(mapper.Map<OrderVM>(updated));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                service.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
