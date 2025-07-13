using System;

namespace SQE_PROJECT.API.Contracts
{
    public class BookingRequest
    {
        public int HotelId { get; set; }
        public DateTime CheckInDate { get; set; } = DateTime.Now;
        public DateTime CheckOutDate { get; set; } = DateTime.Now.AddDays(1);
        public int GuestCount { get; set; }
    }
}