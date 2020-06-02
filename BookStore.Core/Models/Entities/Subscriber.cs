namespace BookStore.Core.Models.Entities
{
    public class Subscriber : User
    {
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Cnp { get; set; }
    }
}