using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Mango.Web.Models;
using Newtonsoft.Json;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;

namespace Mango.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService _productService;

    public HomeController(ILogger<HomeController> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        List<ProductDto>? list = new();
        ResponseDto? response = await _productService.GetAllProductAsync();
        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
        }
        else
        {
            TempData["error"] = response.Message;
        }
        return View(list);
    }

    [Authorize]
    public async Task<IActionResult> ProductDetails(int productId)
    {
        ProductDto dto = new();
        ResponseDto? response = await _productService.GetProductByIdAsync(productId);
        if (response != null && response.IsSuccess)
        {
            dto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
        }
        else
        {
            TempData["error"] = response.Message;
        }
        return View(dto);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

