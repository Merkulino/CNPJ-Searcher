using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Grup_GPS.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

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
      HttpResponseMessage response = await httpClient.GetAsync($"https://receitaws.com.br/v1/cnpj/{cnpj}");
      if (response.IsSuccessStatusCode)
      {
        string json = await response.Content.ReadAsStringAsync();
        Company obj = JsonSerializer.Deserialize<Company>(json);
        if (obj != null)
        {
          Console.Write("nao null");
          ViewData["nome"] = obj.nome;
          ViewData["cnpj"] = obj.cnpj;
          ViewData["situacao"] = obj.situacao;
          ViewData["uf"] = obj.uf;
          ViewData["capital_social"] = obj.capital_social;
          ViewData["cep"] = obj.cep;
          ViewData["tipo"] = obj.tipo;
          return View();
        }
        else
        {
          Console.Write("null");
          ViewData["errorMessage"] = "Não foi possível obter dados da empresa.";
        }
      }
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
