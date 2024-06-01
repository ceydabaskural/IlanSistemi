using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace IlanSistemi.Models.Entitites
{
    public class Category
    {
        [DisplayName("Kategori No")]
        public int CategoryId { get; set; }


        [DisplayName("Kategori Adı")]
        [Required(ErrorMessage = "'Kategori Adı' zorunlu alandır.")]
        public string CategoryName { get; set; }


        [DisplayName("Kategori Görsel Url")]
        public string ImageUrl { get; set; }
    }
}
