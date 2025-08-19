/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.Server/Controllers/CustomerController.cs
API REST de clientes. Se agrega soporte de GET por id y actualización parcial para edición inline del cliente desde Angular.
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
    }
}
