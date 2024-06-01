using IlanSistemi.Models.Entitites;
using System.ComponentModel;

namespace IlanSistemi.Models
{
    public class AdViewModel : Advertisement
    {
        [DisplayName("Kategori Adı:")]
        public string CategoryName { get; set; }
    }
}
