namespace BitirmeApp3.ViewModels
{
  public class AppliedAdvertisementsViewModel
{
    public string? ApplicationId { get; set; }
    public string? AdvertisementId { get; set; }
    public string? AdvertisementTitle { get; set; }
    public string? AdvertisementOwner { get; set; }
    public string? ImageUrl { get; set; }
    public string? Tag { get; set; }
    public string? Status { get; set; }
     public List<ApplicantViewModel>? Applicants { get; set; }
       
    public string? ApplicantName { get; set; }
    public bool IsApproved { get; set; }
    public string? OwnerName { get; set; }
    public bool IsRejected { get; set; }
}

}
