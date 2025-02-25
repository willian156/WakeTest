using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WakeTest.Application.DTOs.ProductDTO;
using WakeTest.Application.Interfaces;
using WakeTest.Domain.Entities;
using WakeTest.Infrastructure.Repositories;

namespace WakeTest.Application.Services
{
    public class ProductService: IProductService
    {
        private readonly DataContext _context;
        public ProductService(DataContext context)
        {
            _context = context;
        }

        public List<ProductDTO> GetProducts(string sortBy, string order)
        {
            var query = _context.Products.AsQueryable();

            sortBy = sortBy.ToLower();
            sortBy = char.ToUpper(sortBy[0]) + sortBy.Substring(1);

            order = order.ToLower();

            if (!string.IsNullOrEmpty(sortBy))
            {
                var property = typeof(Product).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property != null)
                {
                    query = order == "desc" ? query.OrderByDescending(e => EF.Property<object>(e, sortBy)) : query.OrderBy(e => EF.Property<object>(e, sortBy));
                }
            }
            var list = query.Select(prod => new ProductDTO(prod)).ToList();
            return list;
        }

        public async Task<ProductDTO> GetProductById(int id)
        {

            var product = await _context.Products.FindAsync(id);
            if(product != null)
            {
                var dtoProduct = new ProductDTO(product);
                return dtoProduct;
            }
            return null;
        }

        public async Task<ProductDTO> GetProductByName(string name)
        {

            var product = await _context.Products.FirstOrDefaultAsync(x => x.Name == name);
            if (product != null)
            {
                var dtoProduct = new ProductDTO(product);
                return dtoProduct;
            }
            return null;
        }

        public async Task<ProductDTO?> UpdateProduct(int id, UpdateProductDTO product) 
        {
            
            try
            {
                if (product.Value < 0)
                {
                    throw new ArgumentException("Value can't be negative!", nameof(product.Value));
                }

                var oldProduct = await _context.Products.FindAsync(id);

                if (oldProduct == null)
                {
                    return null;
                }

                oldProduct.Name = product.Name;
                oldProduct.Stock = product.Stock;
                oldProduct.Value = product.Value;

                _context.Entry(oldProduct).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                var dtoProduct = new ProductDTO(oldProduct);
                return dtoProduct;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
           
            }


        }

        public async Task<ProductDTO?> PostProduct(PostProductDTO product)
        {
            try
            {
                if(product.Value < 0){
                    throw new ArgumentException("Value can't be negative!", nameof(product.Value));
                }


                var newProduct = new Product()
                {
                    Name = product.Name,
                    Stock = product.Stock,
                    Value = product.Value
                };

                _context.Products.Add(newProduct);
                var post = await _context.SaveChangesAsync();
                if (post > 0)
                {
                    var dtoProduct = new ProductDTO(newProduct);
                    return dtoProduct;
                }
                else if (post == 0) 
                { 
                    return new ProductDTO();
                }
                else
                {
                    return null;
                }

            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<int?> DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return null;
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return id;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
