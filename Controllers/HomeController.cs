using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Grup_GPS.Models;

namespace Grup_GPS.Controllers;

public class HomeController : Controller
{
  private readonly HttpClient httpClient;
  public HomeController()
  {
    httpClient = new HttpClient();
  }

  public IActionResult Index()
  {
    return View();
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

  [HttpGet("SearchCompany")]
  public async Task<ViewResult> SearchCompany(string cnpj)
  {
    try
    {
      Console.Write(cnpj);
      string response = await httpClient.GetStringAsync($"https://receitaws.com.br/v1/cnpj/{cnpj}");
      Console.Write(response);
    }
    catch (System.Exception)
    {
      throw;
    }
    return View();
  }

  public void SaveCompany()
  {

  }
}
