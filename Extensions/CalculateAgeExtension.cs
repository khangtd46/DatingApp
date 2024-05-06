namespace DatingApp.Extensions
{
    public static class CalculateAgeExtension
    {
        public static int CalculateAge(this DateOnly Dob)
        {
            DateOnly now = DateOnly.FromDateTime(DateTime.Now);
            return (now.Year == Dob.Year) ? 1 : now.Year - Dob.Year;
        }
    }
}
