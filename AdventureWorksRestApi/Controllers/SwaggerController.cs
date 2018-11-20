using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using AdventureWorksRestApi.Models;
using Swashbuckle.Swagger.Annotations;
using Serilog;

namespace AdventureWorksRestApi.Controllers
{
    [RoutePrefix("api")]
    public class SwaggerController : ApiController
    {
        AdventureWorks2017Entities _context = new AdventureWorks2017Entities();
        
        public SwaggerController()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.File(
                    @"D:\Azure Training\Task2\AdventureWorksRestApi\AdventureWorksRestApi\log.txt",
                    outputTemplate:
                    "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
        }
        [HttpGet]
        [Route("products")]
        public IHttpActionResult Read()
        {
            try
            {
                var products = _context.Products.Select(product => product);
                Log.Information("Products successfully read");
                return Ok(products);
            }
            catch
            {
                Log.Information("There are some errors in the read method");
                throw;
            }
        }
        [HttpPost]
        [Route("createProduct")]
        public IHttpActionResult Create(Product product)
        {
            try
            {
                var newProduct = _context.Products.Add(product);
                _context.SaveChanges();
                Log.Information("New product successfully created");
                return Ok(newProduct);
            }
            catch
            {
                Log.Information("Smth wrong in the create method");
                throw;
            }

        }

        [HttpPut]
        [Route("updateProduct")]
        public IHttpActionResult Update(Product product)
        {
            try
            {
                _context.Entry(product).State = EntityState.Modified;
                _context.SaveChanges();
                Log.Information("New product successfully updated");
                return Ok();
            }
            catch
            {
                Log.Information("Smth wrong in the update method");
                throw;
            }
        }

        [HttpDelete]
        [Route("deleteProduct/{productId}")]
        public IHttpActionResult Delete(int productId)
        {
            try
            {
                var product = _context.Products.SingleOrDefault(pr => pr.ProductID == productId);
                if (product == null)
                {
                    return this.NotFound();
                }
                _context.Products.Remove(product);
                _context.SaveChanges();
                Log.Information("New product successfully deleted");
                return Ok();
            }
            catch
            {
                Log.Information("Smth wrong in the delete method");
                throw;
            }
        }

    }
}