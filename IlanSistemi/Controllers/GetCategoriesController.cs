using IlanSistemi.Models;
using IlanSistemi.Models.Entitites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NuGet.Protocol.Plugins;
using System.Data;

namespace IlanSistemi.Controllers
{
    public class GetCategoriesController : Controller
    {

        SqlConnection connection = new SqlConnection();
        public GetCategoriesController(IConfiguration configuration)
        {
            connection.ConnectionString = configuration.GetConnectionString("IlanSistemi");
        }
        public IActionResult Index(int id)
        {
            GetCategoriesModel getCategories = new GetCategoriesModel();
            getCategories.Advertisements = GetAdvertisement(id);
            getCategories.Category = GetCategory(id);
            return View(getCategories);
        }


        public Category GetCategory(int categoryId)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Categories where CategoryId=@CategoryId", connection);
            da.SelectCommand.Parameters.AddWithValue("CategoryId", categoryId);
            DataTable dt = new DataTable();
            da.Fill(dt);

            Category category = new Category
            {
                CategoryId = Convert.ToInt32(dt.Rows[0]["CategoryId"]),
                CategoryName = dt.Rows[0]["CategoryName"].ToString()
            };

            return category;
        }


        public List<Advertisement> GetAdvertisement(int titleId)
        {

            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Advertisements where TitleId=@TitleId", connection);
            da.SelectCommand.Parameters.AddWithValue("TitleId", titleId);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<Advertisement> menus = new List<Advertisement>();

            foreach (DataRow row in dt.Rows)
            {
                Advertisement menu = new Advertisement
                {
                    TitleId = Convert.ToInt32(row["TitleId"]),
                    TitleName = row["TitleName"].ToString(),
                    CategoryId = Convert.ToInt32(row["CategoryId"]),
                    Description = row["Description"].ToString(),
                    DetayUrl = row["DetayUrl"].ToString(),
                    ListelemeUrl = Convert.ToString(row["ListelemeUrl"]),
                    Price = Convert.ToDecimal(row["Price"]),
                    PublicationDate = Convert.ToDateTime(row["PublicationDate"])

                };
                menus.Add(menu);
            }

            return menus;
        }

    }
}
