using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _customerService;

        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Route("/api/Customer/GetItems")]
        public ActionResult<PaginationList<Customer>> GetItems(int? pageNumber)
        {
            var page = _customerService.GetItems(pageNumber);
            return Ok(page);
        }

        //GET: api/Customer
        [HttpGet]
        public ActionResult<IEnumerable<Customer>> Get()
        {
            var customers =  _customerService.Get();
            return Ok(customers.ToList());
        }

        // GET: api/Customer/5eaeeb0014ec6403a84e0384
        [HttpGet("{id:length(24)}", Name = "Get")]
        public async Task<ActionResult<Customer>> Get(string id)
        {
            Customer customer = await _customerService.Get(id);
            if (customer==null)
            {
                return NotFound(); 
            }
            return Ok(customer); 
        }

        // POST: api/Customer
        [HttpPost]
        public async Task<ActionResult<Customer>> Post([FromBody]Customer customer)
        {
            if(customer==null)
            {
                return BadRequest();
            }
            await _customerService.Create(customer);
            return CreatedAtRoute("Get", new { id = customer.Id.ToString() }, customer);
        }

        // PUT: api/Customer
        [HttpPut]
        public async Task<ActionResult<Customer>> Put([FromBody]Customer customer)
        {
            Customer detectedCustomer = await _customerService.Get(customer.Id);
            if (detectedCustomer == null)
            {
                return NotFound();
            }
            await _customerService.Update(customer);
            return Ok(customer);
        }

        // DELETE: api/ApiWithActions/5eaeeb0014ec6403a84e0384
        [HttpDelete("{id:length(24)}", Name = "Delete")]
        public async Task<ActionResult> Delete(string id)
        {
            Customer detectedCustomer = await _customerService.Get(id);
            if(detectedCustomer==null)
            {
                return NotFound();
            }    
            await _customerService.Delete(id);
            return Ok();
        }
    }
}
