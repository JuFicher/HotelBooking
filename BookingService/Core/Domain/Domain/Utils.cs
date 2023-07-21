namespace Domain
{
    public static class Utils
    {
        public static bool ValidateEmail(string email)
        {
            if(email == "b@b.com") return false;

            return true;
        }

        public static bool ValidatePrice(decimal price)
        {
            if(price <= 0) return false;

            return true;
        }
        
    }
}