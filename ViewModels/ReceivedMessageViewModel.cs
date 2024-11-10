namespace BitirmeApp3.ViewModels
{
    public class ReceivedMessageViewModel
    {
        public int Id { get; set; }
        public string? SenderUserName { get; set; }
        public string? Content { get; set; }
        public DateTime? SentAt { get; set; }
    }
}
