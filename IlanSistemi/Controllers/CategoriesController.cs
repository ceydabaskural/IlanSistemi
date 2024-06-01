using IlanSistemi.Models.Entitites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NuGet.Protocol.Plugins;
using System.Data;

namespace IlanSistemi.Controllers
{
    public class CategoriesController : Controller
    {
        SqlConnection sqlConnection = new SqlConnection();
        public CategoriesController(IConfiguration configuration)
        {
            sqlConnection.ConnectionString = configuration.GetConnectionString("IlanSistemi");
        }
        public IActionResult Index()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select * from dbo.Categories order by CategoryId", sqlConnection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);


            List<Category> list = new List<Category>();

            foreach (DataRow row in dt.Rows)
            {
                Category category = new Category();
                category.CategoryId = Convert.ToInt32(row["CategoryId"]);
                category.CategoryName = row["CategoryName"].ToString();
                category.ImageUrl = row["ImageUrl"].ToString();

                list.Add(category);
            }

            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Category model)
        {
            if (ModelState.IsValid)
            {
                SqlCommand sqlCommand = new SqlCommand("insert into dbo.Categories values (@CategoryName, @ImageUrl)", sqlConnection);
                sqlCommand.Parameters.AddWithValue("CategoryName", model.CategoryName);
                sqlCommand.Parameters.AddWithValue("ImageUrl", model.ImageUrl);

                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(model);
            }


        }


        public IActionResult Edit(int CategoryId)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Categories where CategoryId=@CategoryId", sqlConnection);
            da.SelectCommand.Parameters.AddWithValue("CategoryId", CategoryId);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Category category = new Category();
                category.CategoryId = Convert.ToInt32(dt.Rows[0]["CategoryId"]);
                category.CategoryName = dt.Rows[0]["CategoryName"].ToString();
                category.ImageUrl = dt.Rows[0]["ImageUrl"].ToString();

                return View(category);
            };
        }


        [HttpPost]
        public IActionResult Edit(Category model)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Categories where CategoryId=@CategoryId", sqlConnection);
            da.SelectCommand.Parameters.AddWithValue("CategoryId", model.CategoryId);

            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                ModelState.AddModelError(string.Empty, model.CategoryId + "numaralı kayıt bulunamadı.");
                return View(model);
            }
            else
            {
                SqlCommand cmd = new SqlCommand("update dbo.Categories set CategoryName=@CategoryName, ImageUrl=@ImageUrl where CategoryId=@CategoryId", sqlConnection);
                cmd.Parameters.AddWithValue("CategoryId", model.CategoryId);
                cmd.Parameters.AddWithValue("CategoryName", model.CategoryName);
                cmd.Parameters.AddWithValue("ImageUrl", model.ImageUrl);

                sqlConnection.Open();
                cmd.ExecuteNonQuery();
                sqlConnection.Close();

                return RedirectToAction(nameof(Index));
            }
        }



        public IActionResult Delete(int CategoryId)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select * from dbo.Categories where CategoryId=@CategoryId", sqlConnection);
            adapter.SelectCommand.Parameters.AddWithValue("CategoryId", CategoryId);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            if (dataTable.Rows.Count == 0)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Category category = new Category();
                category.CategoryId = Convert.ToInt32(dataTable.Rows[0]["CategoryId"]);
                category.CategoryName = dataTable.Rows[0]["CategoryName"].ToString();
                category.ImageUrl = dataTable.Rows[0]["ImageUrl"].ToString();

                return View(category);
            }

        }


        [HttpPost]
        public IActionResult Delete(Category model)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Categories where CategoryId=@CategoryId", sqlConnection);
            da.SelectCommand.Parameters.AddWithValue("CategoryId", model.CategoryId);

            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                ModelState.AddModelError(string.Empty, model.CategoryId + "numaralı kayıt bulunamadı.");
                return View(model);
            }
            else
            {
                SqlCommand cmd = new SqlCommand("delete from dbo.Categories where CategoryId=@CategoryId", sqlConnection);
                cmd.Parameters.AddWithValue("CategoryId", model.CategoryId);

                sqlConnection.Open();
                cmd.ExecuteNonQuery();
                sqlConnection.Close();

                return RedirectToAction(nameof(Index));
            }
        }
    }
}
