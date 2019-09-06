using System;
using System.ComponentModel.DataAnnotations;

namespace Magalu.Challenge.Application.DataAnnotations
{
    public class MaxPrecisionAttribute : ValidationAttribute
    {
        private readonly int value;

        public MaxPrecisionAttribute(int value)
        {
            if (value < 0)
                throw new ArgumentException("Value must be greater or equal than zero.", nameof(value));

            this.value = value;
        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                var castedValue = Convert.ToDecimal(value);
                var multiplier = (decimal)Math.Pow(10, this.value);
                var multipliedValue = castedValue * multiplier;

                if (multipliedValue - Math.Floor(multipliedValue) != 0)
                    return false;
            }

            return true;
        }
    }
}
