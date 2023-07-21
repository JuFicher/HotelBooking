namespace Application
{
    public enum ErrorCodes
    {
        // Guests related codes 1 to 99
        NOT_FOUND = 1,
        COULD_NOT_STORE_DATA = 2,
        INVALID_DOCUMENT_ID = 3,
        MISSING_REQUIRED_INFORMATION = 4,
        INVALID_EMAIL = 5,
        GUEST_NOT_FOUND = 6,

        // Rooms related codes 100 to 199
        ROOM_NOT_FOUND = 100,
        ROOM_COULD_NOT_STORE_DATA = 101,
        ROOM_INVALID_PERSON_ID = 102,
        ROOM_MISSING_REQUIRED_INFORMATION = 103,
        ROOM_INVALID_EMAIL = 104,
        ROOM_INVALID_PERMISSION = 105,

        // Booking related codes 200 to 399
        BOOKING_NOT_FOUND = 200,
        BOOKING_COULD_NOT_STORE_DATA = 201,
        BOOKING_INVALID_PERSON_ID = 202,
        BOOKING_MISSING_REQUIRED_INFORMATION = 203,
        BOOKING_INVALID_EMAIL = 204,
        BOOKING_GUEST_NOT_FOUND = 205,
        BOOKING_ROOM_CANNOT_BE_BOOKED = 206,

        //Payment related codes 400 to 1200
        PAYMENT_INVALID_PAYMENT_INTENTION = 400,
        PAYMENT_PROVIDER_NOT_IMPLEMENTED = 401
    }
    public abstract class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ErrorCodes ErrorCode { get; set; }
        
    }
}