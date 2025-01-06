namespace apiRestaurant.Utility
{
    public class PasswordUtility
    {
        public static string HashPassword(string plainPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.HashPassword(plainPassword);
            }
            catch (Exception ex)
            {
                // Log exception and handle error gracefully
                throw new InvalidOperationException("Password hashing failed", ex);
            }
        }
    }
}
