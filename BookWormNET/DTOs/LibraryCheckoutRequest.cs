namespace BookWormNET.DTOs
{
    public class LibraryCheckoutRequest
    {
        public int UserId { get; set; }
        public int PackageId { get; set; }
        public List<int> ProductIds { get; set; } = new();
    }
}
