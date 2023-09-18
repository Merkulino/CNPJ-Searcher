using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Grup_GPS.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Grup_GPS.Controllers;

public class HomeController : Controller
{
  public static List<Company> companiesList { get; } = new List<Company>();

  private readonly HttpClient httpClient;
  public HomeController()
  {
    httpClient = new HttpClient();
  }

  public IActionResult Index()
  {
    return View();
  }

  public IActionResult Companies()
  {
    return View(companiesList);
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }

  // Fazendo busca sem requisição AJAX, utilizando httpclient
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
        if (obj.nome is not null)
        {
          return View(obj);
        }
        else
        {
          ViewData["errorMessage"] = "Erro ao carregar empresa: CNPJ inválido.";
          return View();
        }
      }
    }
    catch (Exception e)
    {
      Console.WriteLine("Erro na requisição da API: " + e);
      throw new ApplicationException();
    }
    ViewData["errorMessage"] = "Ocorreu um erro: Não foi possível obter dados da empresa.";
    return View();
  }

  [HttpPost("SaveCompany", Name = "SaveCompany")]
  public ActionResult SaveCompany(Company company)
  {
    int listLength = companiesList.Count();
    company.id = listLength + 1;
    if (listLength == 0 || companiesList.Any(com => com.cnpj != company.cnpj))
    {
      companiesList.Add(company);
    }
    return RedirectToAction("Index");
  }


  // Tentativa de fazer uma busca via requisição AJAX, porém deu erro de CORS e não consegui resolver
  /*
  public ViewResult SearchCompany(string encondedCompany)
  {
    var decodedCompany = Uri.UnescapeDataString(encondedCompany);
    Company obj = JsonSerializer.Deserialize<Company>(decodedCompany);
    ViewData["nome"] = obj.nome;
    ViewData["cnpj"] = obj.cnpj;
    ViewData["situacao"] = obj.situacao;
    ViewData["uf"] = obj.uf;
    ViewData["capital_social"] = obj.capital_social;
    ViewData["cep"] = obj.cep;
    ViewData["tipo"] = obj.tipo;
    ViewData["telefone"] = obj.telefone;
    return View();
  }
  */
}
