using Microsoft.AspNetCore.Mvc;

using FastDeliveryAPI.Data;
using FastDeliveryAPI.Entity;
using FastDeliveryAPI.Repositories.Interfaces;
using FastDeliveryAPI.Models;

namespace FastDeliveryAPI.Controllers;

[ApiController]
[Route("api/customer")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWorks _unitOfWork;
    public CustomerController(ICustomerRepository customerRepository, IUnitOfWorks unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> Get()
    {
        var customers = await _customerRepository.GetAll();
        return Ok(customers);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = new Customer(request.Name,
            request.PhoneNumber,
            request.Email,
            request.Address
        );

        _customerRepository.Add(customer);

        await _unitOfWork.SaveChangeAsync();

        return CreatedAtAction(
            nameof(GetCustomerById),
            new {id = customer.Id},
            customer);
    }

     [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCustomer(int id,[FromBody] UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        if (request.Id != id)
        {
            return BadRequest("Body Id is not equal than Url Id");
        }
        var customer = await _customerRepository.GetCustomerById(id);
        if (customer is null)
        {
            return NotFound($"Customer Not Found With the Id {id}");
        }


        customer.ChangeName(request.Name);
        customer.ChangePhoneNumber(request.PhoneNumber);
        customer.ChangeAddress(request.Address);
        customer.ChangeEmail(request.Email);
        customer.ChangeStatus(request.Status);
        

        _customerRepository.Update(customer);

        await _unitOfWork.SaveChangeAsync();

        return NoContent();
    
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCustomerById(int id, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetCustomerById(id);
        if (customer is null)
        {
            NotFound("Customer Not Found With the Id {id}");
        }

        return Ok (customer);
}

}
