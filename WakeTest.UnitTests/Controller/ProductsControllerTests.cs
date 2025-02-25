using WakeTest.API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using WakeTest.Application.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using WakeTest.Application.DTOs.ProductDTO;

namespace WakeTest.UnitTests.Controller
{
    public class ProductsControllerTests
    {

        private readonly IProductService _productService;
        public ProductsControllerTests()
        {
            _productService = A.Fake<IProductService>();
        }


        [Fact]
        public async Task ProductController_GetProducts_ReturnsOk()
        {
            //arrange
            var fakeProducts = new List<ProductDTO>
                {
                    new ProductDTO{ Id = 1 , Name = "Produto Fake 1", Stock = 1000 , Value = (float)1.99},
                    new ProductDTO{ Id = 2 , Name = "Produto Fake 2", Stock = 1001 , Value = (float)2.00},
                    new ProductDTO{ Id = 3 , Name = "Produto Fake 3", Stock = 1002 , Value = (float)2.01},
                    new ProductDTO{ Id = 4 , Name = "Produto Fake 4", Stock = 1003 , Value = (float)2.02},
                    new ProductDTO{ Id = 5 , Name = "Produto Fake 5", Stock = 1004 , Value = (float)2.03}
                };

            A.CallTo(() => _productService.GetProducts()).Returns(fakeProducts);

            var controller = new ProductsController(_productService);

            //act
            var result = await controller.GetProducts();

            //assert
            result.Result.Should().BeOfType<OkObjectResult>();

            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(fakeProducts);

        }


        [Fact]
        public async Task ProductController_GetProduct_ReturnsOk()
        {
            //arrange
            var inputProductId = 1;
            var fakeProduct = new ProductDTO { Id = 1, Name = "Produto Fake 1", Stock = 1000, Value = (float)1.99 };

            A.CallTo(() => _productService.GetProductById(inputProductId)).Returns(fakeProduct);

            var controller = new ProductsController(_productService);

            //act
            var result = await controller.GetProduct(inputProductId);

            //assert
            result.Result.Should().BeOfType<OkObjectResult>();

            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(fakeProduct);
        }


        [Fact]
        public async Task ProductController_GetProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            //arrange
            var nonExistentProductId = 99;

            A.CallTo(() => _productService.GetProductById(nonExistentProductId)).Returns(Task.FromResult<ProductDTO>(null));

            var controller = new ProductsController(_productService);

            //act
            var result = await controller.GetProduct(nonExistentProductId);

            //assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();

            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Value.Should().BeEquivalentTo(new { message = "Product not found!" });
        }


        [Fact]
        public async Task ProductController_PutProduct_ReturnsOk()
        {
            //arrange
            int InputProductId = 1;
            var InputUpdateProduct = new UpdateProductDTO
            {
                Name = "Produto Fake Atualizado 1",
                Stock = 1010,
                Value = (float)3.99
            };

            var oldProduct = new ProductDTO
            {
                Id = 1,
                Name = "Produto Fake Desatualizado 1",
                Stock = 999,
                Value = (float)3.50
            };

            var updatedProduct = new ProductDTO
            {
                Id = 1,
                Name = "Produto Fake Atualizado 1",
                Stock = 1010,
                Value = (float)3.99
            };

            A.CallTo(() => _productService.GetProductById(InputProductId)).Returns(oldProduct);
            A.CallTo(() => _productService.UpdateProduct(InputProductId, InputUpdateProduct)).Returns(updatedProduct);

            var controller = new ProductsController(_productService);

            //act
            var result = controller.PutProduct(InputProductId, InputUpdateProduct);

            //assert
            result.Result.Should().BeOfType<OkObjectResult>();

            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(new { message = $"Product with Id:{updatedProduct.Id} updated!" });
        }

        [Fact]
        public async Task ProductController_PutProduct_ReturnsNotFound_WhenUpdatingProduct()
        {
            //arrange
            int notFoundInputProductId = 99;
            var InputUpdateProduct = new UpdateProductDTO
            {
                Name = "Produto Fake Atualizado 1",
                Stock = 1010,
                Value = (float)3.99
            };

            var oldProduct = new ProductDTO
            {
                Id = 99,
                Name = "Produto Fake Desatualizado 1",
                Stock = 999,
                Value = (float)3.50
            };

            

            A.CallTo(() => _productService.GetProductById(notFoundInputProductId)).Returns(oldProduct);
            A.CallTo(() => _productService.UpdateProduct(notFoundInputProductId, InputUpdateProduct)).Returns(Task.FromResult<ProductDTO>(null));

            var controller = new ProductsController(_productService);

            //act
            var result = controller.PutProduct(notFoundInputProductId, InputUpdateProduct);

            //assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();

            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Value.Should().BeEquivalentTo(new { message = "Product not found!" });
        }

        [Fact]
        public async Task ProductController_PutProduct_ReturnsNotFound_WhenSearchingObject()
        {
            //arrange
            int notFoundInputProductId = 99;
            var InputUpdateProduct = new UpdateProductDTO
            {
                Name = "Produto Fake Atualizado 1",
                Stock = 1010,
                Value = (float)3.99
            };

            A.CallTo(() => _productService.GetProductById(notFoundInputProductId)).Returns(Task.FromResult<ProductDTO>(null));
            
            var controller = new ProductsController(_productService);

            //act
            var result = controller.PutProduct(notFoundInputProductId, InputUpdateProduct);

            //assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();

            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Value.Should().BeEquivalentTo(new { message = "Product not found!" });
        }

        [Fact]
        public async Task ProductController_PostProduct_ReturnsCreatedAtAction()
        {
            //arrange
            var inputProduct = new PostProductDTO { Name = "Produto Fake Criado 1", Stock = 500, Value = (float)1.99 };
            var fakeSavedProduct = new ProductDTO { Id = 1, Name = "Produto Fake Criado 1", Stock = 500, Value = (float)1.99 };

            A.CallTo(() => _productService.PostProduct(inputProduct)).Returns(Task.FromResult(fakeSavedProduct));

            var controller = new ProductsController(_productService);

            //act
            var result = await controller.PostProduct(inputProduct);

            //assert
            result.Should().BeOfType<CreatedAtActionResult>();

            var createdResult = result as CreatedAtActionResult;
            createdResult.ActionName.Should().Be(nameof(controller.GetProduct));
            createdResult.RouteValues["id"].Should().Be(fakeSavedProduct.Id);
            createdResult.Value.Should().BeEquivalentTo(fakeSavedProduct);
        }

        [Fact]
        public async Task ProductController_PostProduct_ReturnsBadRequest_WhenObjectDoesntSaved()
        {
            //arrange
            var inputProduct = new PostProductDTO { Name = "Produto Fake Criado 1", Stock = 500, Value = (float)1.99 };

            A.CallTo(() => _productService.PostProduct(inputProduct)).Returns(Task.FromResult<ProductDTO?>(null));

            var controller = new ProductsController(_productService);

            //act
            var result = controller.PostProduct(inputProduct);

            //assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();

            var badRequest = result.Result as BadRequestObjectResult;
            badRequest.Value.Should().BeEquivalentTo(new { message = "Product doesn't saved!" });
        }

        [Fact]
        public async Task ProductController_DeleteProduct_ReturnsOk()
        {
            //arrange
            int inputId = 1;
            A.CallTo(() => _productService.DeleteProduct(inputId)).Returns(1);
            var controller = new ProductsController(_productService);
            
            //act
            var result = controller.DeleteProduct(inputId);

            //assert
            result.Result.Should().BeOfType<OkObjectResult>();

            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(new { message = $"Product with Id:{inputId} successfully deleted!" });
        }


        [Fact]
        public async Task ProductController_DeleteProduct_ReturnsNotFound_WhenDeletingObject()
        {
            //arrange
            int notFoundInputId = 999999989;
            A.CallTo(() => _productService.DeleteProduct(notFoundInputId)).Returns(Task.FromResult<int?>(null));
            var controller = new ProductsController(_productService);

            //act
            var result = await controller.DeleteProduct(notFoundInputId);

            //assert
            result.Should().BeOfType<NotFoundObjectResult>();

            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult.Value.Should().BeEquivalentTo(new { message = "Product not found!" });
        }
    }
}
