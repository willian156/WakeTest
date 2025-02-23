﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<ProductDTO> GetProducts()
        {
            var products = _context.Products.Select(prod => new ProductDTO(prod)).ToList();
            return products;
        }

        public async Task<ProductDTO> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);

            var dtoProduct = new ProductDTO(product);
            return dtoProduct;
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
