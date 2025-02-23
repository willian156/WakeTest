using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WakeTest.Domain.Entities;
using WakeTest.Infrastructure.Repositories;
using WakeTest.Application.Services;
using WakeTest.Application.Interfaces;
using WakeTest.Application.DTOs.ProductDTO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WakeTest.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IProductService _productService;
        public ProductsController(DataContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            try
            {
                var products = _productService.GetProducts();

                return products;
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            try
            {
                var product = await _productService.GetProductById(id);

                if (product == null)
                {
                    return NotFound(new { message = "Product not found!" });
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, UpdateProductDTO product)
        {
            try
            {
                var oldProduct = _productService.GetProductById(id).Result;


                if (id != oldProduct.Id)
                {
                    return BadRequest("URL's Id and body object Id doesn't match!");
                }

                var updatedProduct = await _productService.UpdateProduct(id, product);

                if (updatedProduct == null)
                {
                    return NotFound(new { message = "Product not found!" });
                }
                else
                {
                    return Ok(new { message = $"Product with Id:{updatedProduct.Id} updated!" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostProduct(PostProductDTO product)
        {
            try
            {
                var post = await _productService.PostProduct(product);

                if (post != null)
                {
                    if (post.Id != 0)
                    {
                        return CreatedAtAction(nameof(GetProduct), new { id = post.Id }, post);
                    }
                }
                else
                {
                    return BadRequest(new { message = "Product doesn't saved!" });
                }

                return BadRequest(new {message = new Exception().Message});
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }

        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var delProduct = _productService.DeleteProduct(id);

                if (delProduct == null)
                {
                    return NotFound(new {message = "Product not found!"});
                }
                else
                {
                    return Ok(new {message = $"Product with Id:{id} successfully deleted!"});
                }

            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
