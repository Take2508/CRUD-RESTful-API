using Microsoft.AspNetCore.Mvc;
using FastDeliveryApi.Data;
using FastDeliveryApi.Entity;

namespace FastDeliveryApi.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersControllers : ControllerBase
{
    private readonly FastDeliveryDbContext _context;

    public CustomersControllers(FastDeliveryDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Customer>> Get()
    {
        var customers = _context.Customers.ToList();
        return Ok(customers);
    }

    [HttpPost("Create-Customer/")]
    public async Task<ActionResult> Create(Customer customer)
    {
        if (ModelState.IsValid)
        {
            var customers = _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }
        return Ok(customer);

    }

    [HttpPut("Modify-Customer/{id}")]
    public async Task<ActionResult> Modify( Customer customer)
    {
        var result = _context.Customers.Update(customer);
        await _context.SaveChangesAsync();

        return NoContent();

    }

    [HttpDelete("Delete-Customer/{id}")]
    public ActionResult Delete(int id)
    {
        var BorrarId = _context.Customers.Find(id);
        if (BorrarId != null)
        {
            var result = _context.Customers.Remove(BorrarId);
            _context.SaveChangesAsync();
        }

        return NoContent();

    }

    [HttpGet("Search-Customer/{id}")]
    [ActionName(nameof(GetcustomersById))]
    public ActionResult<Customer> GetcustomersById(int id)
    {
        var customerById = _context.GetcustomerById(id);
        if (customerById == null)
        {
            return NotFound();
        }
        return Ok(customerById);
    }
}