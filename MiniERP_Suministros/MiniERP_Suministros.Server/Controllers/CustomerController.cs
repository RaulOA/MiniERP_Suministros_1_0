/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.Server/Controllers/CustomerController.cs
API REST de clientes. Add: GET por id, POST (crear), PUT (actualización parcial) y DELETE. Compatible con edición inline y alta desde Angular.
*/

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MiniERP_Suministros.Core.Services.Shop;
using MiniERP_Suministros.Server.ViewModels.Shop;

namespace MiniERP_Suministros.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ICustomerService _customerService;

        public CustomerController(IMapper mapper, ILogger<CustomerController> logger,
            ICustomerService customerService)
        {
            _mapper = mapper;
            _logger = logger;
            _customerService = customerService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var allCustomers = _customerService.GetAllCustomersData();
            return Ok(_mapper.Map<IEnumerable<CustomerVM>>(allCustomers));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var customer = _customerService.GetById(id);
            if (customer == null) return NotFound();
            return Ok(_mapper.Map<CustomerVM>(customer));
        }

        [HttpPost]
        public IActionResult Post([FromBody] CustomerVM value)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            // Validaciones básicas del lado servidor (coinciden con restricciones del modelo)
            if (string.IsNullOrWhiteSpace(value.Name)) return BadRequest("Name is required");
            if (string.IsNullOrWhiteSpace(value.Email)) return BadRequest("Email is required");

            var created = _customerService.Create(
                name: value.Name!,
                email: value.Email!,
                phoneNumber: value.PhoneNumber,
                address: value.Address,
                city: value.City,
                gender: value.Gender
            );

            var result = _mapper.Map<CustomerVM>(created);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, result);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CustomerVM value)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            try
            {
                // Aplica actualización parcial: solo campos no nulos en el VM
                var updated = _customerService.UpdatePartial(id,
                    name: value.Name,
                    email: value.Email,
                    phoneNumber: value.PhoneNumber,
                    address: value.Address,
                    city: value.City,
                    gender: value.Gender);

                return Ok(_mapper.Map<CustomerVM>(updated));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _customerService.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
