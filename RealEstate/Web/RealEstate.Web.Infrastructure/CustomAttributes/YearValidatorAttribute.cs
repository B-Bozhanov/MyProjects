namespace RealEstate.Web.Infrastructure.CustomAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class YearValidatorAttribute : ValidationAttribute
    {
        public YearValidatorAttribute()
            : base()
        {
        }

        public YearValidatorAttribute(int minYear) 
            : this()
        {
            this.MinYear = minYear;
        }

        public int MinYear { get; init; } = 1800;

        public override bool IsValid(object value)
        {
            if (value is int intValue)
            {
                if (intValue >= 1800 && intValue <= DateTime.Now.Year)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
