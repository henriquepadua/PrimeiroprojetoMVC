using System.Npgsql;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using teste_mvc_webapp.Models;

namespace teste_mvc_webapp.Controllers;

public class HomeController : Controller
{
    private readonly string ConnectionString ="User ID=ti2cc;Password=ti@cc;Host=localhost;Port=5433;Database=crud";

    
    //[HttpPost]
    public IActionResult Index(){
            IDbConnection con;

            try{
                string selecaoQuery="SELECT * FROM crud";
                con = new NpgsqlConnection(ConnectionString);
                con.Open();
                IEnumerable<crud> listaPessoa = con.Query<crud>(selecaoQuery).ToList();
                return View(listaPessoa);
            }catch(Exception ex){
                throw ex;
            }
    }

    [HttpGet]
    public IActionResult Create(){return View();}

    [HttpPost]
    public IActionResult Create(crud cd)
    {
        if(ModelState.IsValid){
              
          IDbConnection con;

            try{
                string insercaoQuery = "INSERT INTO crud(id,nome,email,senha) VALUES(@id,@nome,@email,@senha)";
               using( con = new NpgsqlConnection(ConnectionString)){
                   con.Open();
                   con.Execute(insercaoQuery,cd);
                   con.Close();
                     return RedirectToAction(nameof(Index));

               }
                
            }catch(Exception ex){
                throw ex;
            }
            
                
                    
        }
        return View(cd);
    }

    [HttpGet]
    public IActionResult Edit(int id){
        IDbConnection con;

        try{
            string selecaoQuery = "SELECT * FROM crud WHERE id = @id" ;
            con = new NpgsqlConnection(ConnectionString);
            con.Open();
            crud pessoa = con.Query<crud>(selecaoQuery,new {id = id}).FirstORDefault();
            con.Close();
            return View(pessoa);
        }catch(Exception ex){
            throw ex;
        }
    }
    
    [HttpPost]

    public IActionResult Edit(int id,crud cd){
        
        if(id != cd.Id){
            return NotFound();
        }
        if(ModelState.IsValid){
            IDbConnection con;

            try{
                con = new NpgsqlConnection(ConnectionString);
                string atualizarQuery = "UPDATE pessoa SET nome = @nome, email = @email, senha = @senha WHERE Id = @id";
                con.Open();
                con.Execute(atualizarQuery,cd);
                con.Close();
                return RedirectToAction(nameof(Index));
            }catch(Exception ex){
                throw ex;
            }
        }
        return View(cd);
    }

    [HttpPost]
    public IActionResult Delete(int id){
        IDbConnection con;

        try{
            string ExcluirQuery = "DELETE * FROM crud WHERE id = @id" ;
            con = new NpgsqlConnection(ConnectionString);
            con.Open();
            con.Execute(ExcluirQuery,new {id = id});
            con.Close();

            return RedirectToAction(nameof(Index));
        }catch(Exception ex){
            throw ex;
        }
    }

    [HttpPost]
    public IActionResult Resultado(Pessoa pessoa)
    {
        return View(pessoa);
    }


    /*[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new Pessoa { PessoaId = (int)Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }*/
}
