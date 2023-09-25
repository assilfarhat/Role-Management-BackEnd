using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Role_Management_.NET.Context;
using Role_Management_BackEnd.Models;

namespace Role_Management_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
            private readonly AppDbContext _context;

            public ProductsController(AppDbContext context)
            {
                _context = context;
            }
            [Authorize]
            [HttpGet("GetProduct")]
            public async Task<ActionResult<IEnumerable<Products>>> GetProducts()
            {
                return await _context.Products.ToListAsync();
            }
            [Authorize]
            [HttpGet("GetProductById/{id}")]
            public async Task<ActionResult<Products>> GetProductById(int id)
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    return NotFound();
                }

                return product;
            }
            [Authorize]
            [HttpPost("CreateProduct")]
            public async Task<ActionResult<Products>> CreateProduct(Products product)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetProductById", new { id = product.Id }, product);
            }


            [Authorize]
            [HttpPut("UpdateProduct/{id}")]
            public async Task<IActionResult> UpdateProduct(int id, Products product)
            {
                if (id != product.Id)
                {
                    return BadRequest();
                }

                _context.Entry(product).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
        [Authorize]
        [HttpDelete("DeleteProduct/{id}")]
            public async Task<IActionResult> DeleteProduct(int id)
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            private bool ProductExists(int id)
            {
                return _context.Products.Any(e => e.Id == id);
            }
    }
}

