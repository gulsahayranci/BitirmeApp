namespace BitirmeApp3.Models{
     public class Announcement
    {
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? ImagePath { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? UserId { get; set; }
    public AppUser? User { get; set; }
}

}