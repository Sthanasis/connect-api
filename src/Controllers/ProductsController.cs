using connect.Models;
using connect.Services;
using connect.Utilities;
using Microsoft.AspNetCore.Mvc;
namespace connect.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;
    private readonly AppErrorUtility _appErrorUtils = new();
    public ProductsController(ProductService productService) =>
        _productService = productService;

    [HttpGet]
    public async Task<ActionResult<AppResult<List<Product>>>> Get()
    {
        try
        {
            List<Product> products = await _productService.GetAllAsync();
            AppResult<List<Product>> result = new() { Data = products };
            return Ok(result);
        }
        catch (Exception ex)
        {
            return _appErrorUtils.SendServerError(ex.Message);
        }
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<AppResult<Product>>> Get(string id)
    {
        try
        {
            var product = await _productService.GetAsync(id);

            if (product is null)
            {
                return _appErrorUtils.SendClientError("Not Found");
            }

            return Ok(new AppResult<Product>()
            {
                Data = product
            });
        }
        catch (Exception ex)
        {
            return _appErrorUtils.SendServerError(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post(Product newProduct)
    {
        await _productService.CreateAsync(newProduct);

        return CreatedAtAction(nameof(Get), new { id = newProduct.Id }, newProduct);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Product updatedProduct)
    {
        var product = await _productService.GetAsync(id);

        if (product is null)
        {
            return _appErrorUtils.SendClientError("not found");
        }

        updatedProduct.Id = product.Id;

        await _productService.UpdateAsync(id, updatedProduct);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var book = await _productService.GetAsync(id);

        if (book is null)
        {
            return _appErrorUtils.SendClientError("not found");
        }

        await _productService.RemoveAsync(id);

        return NoContent();
    }
}