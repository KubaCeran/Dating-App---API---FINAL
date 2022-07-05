using Dating_App___API.Entities;

namespace Dating_App___API.Extensions
{
    public static class DateTimeExtensions
    {
        //public static int CalculateAge(DateTime dateOfBirth)
            //Alternative 
            public static int CalculateAge(this DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
