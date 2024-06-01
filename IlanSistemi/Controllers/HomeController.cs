using IlanSistemi.Models;
using IlanSistemi.Models.Entitites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NuGet.Protocol.Plugins;
using System.Data;
using System.Diagnostics;

namespace IlanSistemi.Controllers
{
    public class HomeController : Controller
    {
        SqlConnection connection = new SqlConnection();
        public HomeController(IConfiguration configuration)
        {
            connection.ConnectionString = configuration.GetConnectionString("IlanSistemi");
        }

        public IActionResult Index()
        {
            HomePageModel model = new HomePageModel();
            model.Advertisements = GetAdvertisements();
            model.Categories = GetCategories();

            return View(model);
        }


        public List<Category> GetCategories()
        {
            List<Category> list = new List<Category>();

            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Categories", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);


            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Category
                {
                    CategoryId = Convert.ToInt32(row["CategoryId"]),
                    CategoryName = row["CategoryName"].ToString(),
                    ImageUrl = row["ImageUrl"].ToString()
                });
            }

            return list;
        }


        public List<Advertisement> GetAdvertisements()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Advertisements", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);


            List<Advertisement> advertisements = new List<Advertisement>();
            foreach (DataRow row in dt.Rows)
            {
                advertisements.Add(new Advertisement
                {
                    TitleId = Convert.ToInt32(row["TitleId"]),
                    TitleName = row["TitleName"].ToString(),
                    CategoryId = Convert.ToInt32(row["CategoryId"]),
                    Description = row["Description"].ToString(),
                    DetayUrl = row["DetayUrl"].ToString(),
                    ListelemeUrl = row["ListelemeUrl"].ToString(),
                    Price = Convert.ToDecimal(row["Price"]),
                    PublicationDate = Convert.ToDateTime(row["PublicationDate"]),
                });
            }

            return advertisements;
        }

    }
}
