using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace IlanSistemi.Models.Entitites
{
    public class Advertisement
    {
        [DisplayName("İlan Id")]
        public int TitleId { get; set; }


        [DisplayName("Başlık")]
        [Required(ErrorMessage = "Başlık zorunlu alandır.")]
        public string TitleName { get; set; }


        [DisplayName("Fiyat")]
        [Required(ErrorMessage = "Fiyat zorunlu alandır.")]
        public decimal Price { get; set; }


        [DisplayName("Eklenme Tarihi")]
        public DateTime PublicationDate { get; set; }


        [DisplayName("Detay")]
        [Required(ErrorMessage = "'Detay' zorunlu alandır.")]
        public string Description { get; set; }


        [DisplayName("Kategori No")]
        public int CategoryId { get; set; }


        [DisplayName("Listeleme Görsel Url")]
        public string ListelemeUrl { get; set; }


        [DisplayName("Detay Görsel Url")]
        public string DetayUrl { get; set; }
    }
}
