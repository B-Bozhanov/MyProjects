namespace RealEstate.Web.Infrastructure.CustomAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static RealEstate.Common.GlobalConstants.Properties;

    public class YearValidatorAttribute : ValidationAttribute
    {
        public YearValidatorAttribute()
            : base()
        {
        }

        public YearValidatorAttribute(int minYear) 
            : this()
        {
           MinYear = minYear;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            if (value is int intValue)
            {
                if ((intValue >= MinYear && intValue <= DateTime.UtcNow.Year))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
