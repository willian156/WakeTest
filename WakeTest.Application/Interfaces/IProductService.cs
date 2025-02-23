using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WakeTest.Application.DTOs.ProductDTO;
using WakeTest.Domain.Entities;

namespace WakeTest.Application.Interfaces
{
    public interface IProductService
    {
        List<ProductDTO> GetProducts();

        Task<ProductDTO> GetProductById(int id);

        Task<ProductDTO> UpdateProduct(int id , UpdateProductDTO product);

        Task<ProductDTO?> PostProduct(PostProductDTO product);

        Task<int?> DeleteProduct(int id);   
    }
}
