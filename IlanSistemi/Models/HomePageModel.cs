using IlanSistemi.Models.Entitites;

namespace IlanSistemi.Models
{
    public class HomePageModel
    {
        public List<Category> Categories { get; set; }
        public List<AdViewModel> AdvertisementsModel { get; set; }

        public string CategoryName { get; set; }
    }
}
