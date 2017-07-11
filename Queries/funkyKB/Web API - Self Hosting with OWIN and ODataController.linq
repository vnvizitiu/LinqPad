<Query Kind="Program">
  <Connection>
    <ID>cd884946-dbf0-42d1-ac90-d3a615f50162</ID>
    <Driver>AstoriaAuto</Driver>
    <Server>http://services.odata.org/AdventureWorksV3/AdventureWorks.svc</Server>
  </Connection>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <NuGetReference>EntityFramework</NuGetReference>
  <NuGetReference>Microsoft.AspNet.OData</NuGetReference>
  <NuGetReference>Microsoft.AspNet.WebApi.Client</NuGetReference>
  <NuGetReference>Microsoft.AspNet.WebApi.Owin</NuGetReference>
  <NuGetReference>Microsoft.Net.Http</NuGetReference>
  <NuGetReference>Microsoft.Owin</NuGetReference>
  <NuGetReference>Microsoft.Owin.SelfHost</NuGetReference>
  <NuGetReference Prerelease="true">morelinq</NuGetReference>
  <NuGetReference>Owin</NuGetReference>
  <Namespace>Microsoft.Owin.Hosting</Namespace>
  <Namespace>MoreLinq</Namespace>
  <Namespace>Owin</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Web.Http</Namespace>
  <Namespace>System.Web.Http.Dispatcher</Namespace>
  <Namespace>System.Web.OData</Namespace>
  <Namespace>System.Web.OData.Builder</Namespace>
  <Namespace>System.Web.OData.Extensions</Namespace>
  <Namespace>System.Web.OData.Query</Namespace>
  <Namespace>System.Web.OData.Routing</Namespace>
</Query>

void Main()
{
    var baseAddress = "http://localhost:9000/"; 

    var client = new HttpClient(); 
    try
    {
        using (WebApp.Start<Startup>(url: baseAddress)) 
        {
            HttpResponseMessage response;
            
            Action<string> callOwin = path =>
            {
                response = client.GetAsync(baseAddress + path).Result; 
                response.Dump(); 
                response.Content.ReadAsStringAsync().Result.Dump(); 
            };

            callOwin("api/$metadata"); 
            callOwin("api/Product?$count=true"); 
            callOwin("api/Product(2)"); 
            callOwin("api/Product?$filter=Category+eq+'Bakery'+and+indexof(Name,'Tortillas')+ne+-1"); 
        }
    }
    finally
    {
        client.Dispose();
    }
}

public interface IProduct
{
    int Id { get; set; }
    string Name { get; set; }
    decimal Price { get; set; }
    string Category { get; set; }
}

public class Product : IProduct
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
}

public class ProductRepository
{
    public IQueryable<IProduct> LoadProducts()
    {
        var data = new []
        {
            new Product{ Id = 1, Category = "Bakery", Name = "Spinach/Rice Tortillas", Price = 6.99M },
            new Product{ Id = 2, Category = "Bakery", Name = "Organic Corn Tortillas", Price = 1.99M },
            new Product{ Id = 3, Category = "Bakery", Name = "Flower Flour Thins", Price = 5.99M },
            new Product{ Id = 4, Category = "Canned", Name = "Ranchero Beans", Price = 1.69M },
            new Product{ Id = 5, Category = "Canned", Name = "Wild Caught Sardines in Water", Price = 3.99M },
            new Product{ Id = 6, Category = "Canned", Name = "Wild Caught Salmon in Water", Price = 6.99M },
            new Product{ Id = 7, Category = "Grocery", Name = "Organic Swiss Chard", Price = 2.89M },
            new Product{ Id = 8, Category = "Grocery", Name = "Organic Kale", Price = 2.99M },
            new Product{ Id = 9, Category = "Grocery", Name = "Organic Red Potatoes", Price = 2.49M },
        };
        
        return data.AsQueryable();
    }
}

public class ProductController : ODataController
{
    public ProductController()
    {
        this._repository = new ProductRepository();
        this._repository.Dump("controller and repository loaded");
    }

    [EnableQuery(PageSize = 3, AllowedQueryOptions = AllowedQueryOptions.All)]
    public IQueryable<IProduct> Get()
    {
        return this._repository.LoadProducts();
    }

    [EnableQuery]
    public SingleResult<IProduct> Get([FromODataUri] int key)
    {
        IQueryable<IProduct> result = this._repository.LoadProducts().Where(p => p.Id == key);
        return SingleResult.Create(result);
    }

    ProductRepository _repository;
}

public class ControllerResolver : DefaultHttpControllerTypeResolver 
{
    public override ICollection<Type> GetControllerTypes(IAssembliesResolver assembliesResolver) 
    {
        var types = Assembly.GetExecutingAssembly().GetExportedTypes();
        var controllerType = typeof(System.Web.Http.Controllers.IHttpController);
        var list = types.Where(i => controllerType.IsAssignableFrom(i)).ToList();
        list.Add(typeof(MetadataController));
        list.Dump(this.GetType().Name);
        return list;
    }
}

public class Startup
{
    public void Configuration(IAppBuilder appBuilder)
    {
        var config = new HttpConfiguration();

        config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
        config.Services.Replace(typeof(IHttpControllerTypeResolver), new ControllerResolver());

        var builder = new ODataConventionModelBuilder();
        builder
            .EntitySet<Product>("Product")
            .EntityType.DerivesFrom<IProduct>();
        var model = builder.GetEdmModel();

        config.MapODataServiceRoute("odata", "api", model);

        appBuilder.UseWebApi(config);

        config.EnsureInitialized();
    }
}