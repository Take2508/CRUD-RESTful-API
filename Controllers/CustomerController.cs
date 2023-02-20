using Microsoft.AspNetCore.Mvc;
using Mapster;

using FastDeliveryAPI.Data;
using FastDeliveryAPI.Entity;
using FastDeliveryAPI.Repositories.Interfaces;
using FastDeliveryAPI.Models;
using FastDeliveryAPI.Exceptions;

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

        var customer = request.Adapt<Customer>();

        customer.ValidateNumber(request.PhoneNumber);

        _customerRepository.Add(customer);

        await _unitOfWork.SaveChangeAsync(cancellationToken);

        var response = customer.Adapt<CustomerReqponse>();

        return CreatedAtAction(
            nameof(GetCustomerById),
            new {id = response.Id},
            response);
    }

     [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCustomer(int id,[FromBody] UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        if (request.Id != id)
        {
            throw new BadRequestException ("Body Id is not equal than Url Id");
        }
        var customer = await _customerRepository.GetCustomerById(id);
        if (customer is null)
        {
            throw new NotFoundException("Customer", id);
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
        var customer = await _customerRepository.GetCustomerById(id, cancellationToken);
        if (customer is null)
        {
            throw new NotFoundException("Customer", id);
        }

        var response = customer.Adapt<CustomerReqponse>();

        return Ok (response);
}

}
