using IlanSistemi.Models;
using IlanSistemi.Models.Entitites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IlanSistemi.Controllers
{
    public class AdvertisementsController : Controller
    {
        SqlConnection connection = new SqlConnection();
        public AdvertisementsController(IConfiguration configuration)
        {
            connection.ConnectionString = configuration.GetConnectionString("IlanSistemi");
        }
        public IActionResult Index()
        {
            SqlDataAdapter sqlData = new SqlDataAdapter("select * from dbo.Advertisements as a inner join dbo.Categories as c on c.CategoryId=a.CategoryId order by PublicationDate desc", connection);
            DataTable table = new DataTable();
            sqlData.Fill(table);

            List<AdViewModel> adViews = new List<AdViewModel>();

            foreach (DataRow data in table.Rows)
            {
                AdViewModel model = new AdViewModel
                {
                    TitleId = Convert.ToInt32(data["TitleId"]),
                    TitleName = (data["TitleName"]).ToString(),
                    CategoryId = Convert.ToInt32(data["CategoryId"]),
                    CategoryName = (data["CategoryName"]).ToString(),
                    Description = (data["Description"]).ToString(),
                    ListelemeUrl = (data["ListelemeUrl"]).ToString(),
                    DetayUrl = (data["DetayUrl"]).ToString(),
                    Price = Convert.ToDecimal(data["Price"]),
                    PublicationDate = Convert.ToDateTime(data["PublicationDate"]),
                };
                adViews.Add(model);
            }
            return View(adViews);
        }

        public IActionResult Create()
        {
            AdCategoryListView listView = new AdCategoryListView();
            listView.Advertisement = new Advertisement();
            listView.Categories = GetCategories();

            return View(listView);
        }

        [HttpPost]
        public IActionResult Create(AdCategoryListView model) //index view inda asp-for-titleid olarak göndermeliyiz çünkü parametremizin adı titleid olarak verdik
        {
            if (ModelState.IsValid)
            {
                SqlCommand cmd = new SqlCommand("insert into dbo.Advertisements values (@titleName, @price, @publicationDate ,@description,@categoryId, @listelemeUrl, @detayUrl)", connection);
                cmd.Parameters.AddWithValue("titleName", model.Advertisement.TitleName);
                cmd.Parameters.AddWithValue("price", model.Advertisement.Price);
                cmd.Parameters.AddWithValue("publicationDate", model.Advertisement.PublicationDate);
                cmd.Parameters.AddWithValue("description", model.Advertisement.Description);
                cmd.Parameters.AddWithValue("categoryId", model.Advertisement.CategoryId);
                cmd.Parameters.AddWithValue("listelemeUrl", model.Advertisement.ListelemeUrl);
                cmd.Parameters.AddWithValue("detayUrl", model.Advertisement.DetayUrl);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                AdCategoryListView listView = new AdCategoryListView
                {
                    Advertisement = model.Advertisement,
                    Categories = GetCategories(),
                };

                return View(listView);
            }


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

        public Advertisement GetAdvertisement(int id)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Advertisements where TitleId=@TitleId", connection);
            da.SelectCommand.Parameters.AddWithValue("TitleId", id);
            DataTable dt = new DataTable();
            da.Fill(dt);

            Advertisement advertisement = new Advertisement
            {
                TitleId = Convert.ToInt32(dt.Rows[0]["TitleId"]),
                TitleName = dt.Rows[0]["TitleName"].ToString(),
                CategoryId = Convert.ToInt32(dt.Rows[0]["CategoryId"]),
                Description = dt.Rows[0]["Description"].ToString(),
                DetayUrl = dt.Rows[0]["DetayUrl"].ToString(),
                ListelemeUrl = dt.Rows[0]["ListelemeUrl"].ToString(),
                Price = Convert.ToDecimal(dt.Rows[0]["Price"]),
                PublicationDate = Convert.ToDateTime(dt.Rows[0]["PublicationDate"]),
            };
            return advertisement;
        }

        public AdViewModel GetAdViewModel(int id)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Advertisements as a inner join dbo.Categories as c on c.CategoryId=a.CategoryId", connection);
            da.SelectCommand.Parameters.AddWithValue("CategoryId", id);
            DataTable table = new DataTable();
            da.Fill(table);

            AdViewModel adViewModel = new AdViewModel
            {
                TitleId = Convert.ToInt32(table.Rows[0]["TitleId"]),
                TitleName = (table.Rows[0]["TitleName"]).ToString(),
                CategoryName = (table.Rows[0]["CategoryName"]).ToString(),
                CategoryId = Convert.ToInt32(table.Rows[0]["CategoryId"]),
                Description = (table.Rows[0]["Description"]).ToString(),
                DetayUrl = (table.Rows[0]["DetayUrl"]).ToString(),
                ListelemeUrl = (table.Rows[0]["ListelemeUrl"]).ToString(),
                Price = Convert.ToDecimal(table.Rows[0]["Price"]),
                PublicationDate = Convert.ToDateTime(table.Rows[0]["PublicationDate"]),
            };

            return adViewModel;
        }



        public IActionResult Edit(int id) 
        {
            ViewBag.KategoriListesi = GetCategories();


            return View(GetAdvertisement(id));
        }


        [HttpPost]
        public IActionResult Edit(Advertisement advertisement)
        {
            if (ModelState.IsValid)
            {
                SqlCommand cmd = new SqlCommand("update dbo.Advertisements set TitleName=@titleName, Price=@price, PublicationDate=@publicationDate, Description=@description, CategoryId=@categoryId, ListelemeUrl=@listelemeUrl, DetayUrl=@detayUrl where TitleId=@titleId", connection);

                cmd.Parameters.AddWithValue("titleName", advertisement.TitleName);
                cmd.Parameters.AddWithValue("price", advertisement.Price);
                cmd.Parameters.AddWithValue("publicationDate", advertisement.PublicationDate);
                cmd.Parameters.AddWithValue("description", advertisement.Description);
                cmd.Parameters.AddWithValue("categoryId", advertisement.CategoryId);
                cmd.Parameters.AddWithValue("listelemeUrl", advertisement.ListelemeUrl);
                cmd.Parameters.AddWithValue("detayUrl", advertisement.DetayUrl);
                cmd.Parameters.AddWithValue("titleId", advertisement.TitleId);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.KategoriListesi = GetCategories();

                return View(advertisement);
            }

        }


        public IActionResult Delete(int id)
        {
            return View(GetAdViewModel(id));
        }


        [HttpPost]
        public IActionResult Delete(Advertisement adv)
        {
            AdViewModel viewModel = GetAdViewModel(adv.TitleId);
            if(viewModel == null)
            {
                ModelState.AddModelError(string.Empty, adv.TitleId + "numaralı kayıt bulunamadı.");
                return View(viewModel);
            }
            else
            {
                SqlCommand cmd = new SqlCommand("delete from dbo.Advertisements where TitleId=@id",connection);
                cmd.Parameters.AddWithValue("id", adv.TitleId);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();

                return RedirectToAction(nameof(Index));
            }

        }

    }
}
