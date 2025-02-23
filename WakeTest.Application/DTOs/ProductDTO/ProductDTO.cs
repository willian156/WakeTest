using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WakeTest.Domain.Entities;

namespace WakeTest.Application.DTOs.ProductDTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public float Value { get; set; }

        public ProductDTO() { }

        public ProductDTO(Product prod)
        {
            Id = prod.Id;
            Name = prod.Name;
            Stock = prod.Stock;
            Value = prod.Value;
        }
    }

}
