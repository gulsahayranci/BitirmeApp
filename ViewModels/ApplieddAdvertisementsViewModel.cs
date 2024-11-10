namespace BitirmeApp3.ViewModels{
    public class ApplieddAdvertisementsViewModel
{
    public string? AdvertisementId { get; set; }
    public string? AdvertisementTitle { get; set; }
    public string? Tag { get; set; }
    public string? AdvertisementOwner { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsApproved { get; set; }
    public bool IsRejected { get; set; }
    public string? ApplicationId { get; set; } // New field
    public string? ApplicantName { get; set; } // New field
}

}