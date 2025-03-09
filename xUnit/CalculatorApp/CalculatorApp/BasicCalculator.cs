using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorApp
{
    public class BasicCalculator
    {
        public double CurrentValue { get; private set; }
        public bool IsActive { get; private set; } = false;

        private void EnsureActive(double value)
        {
            if (!IsActive)
            {
                CurrentValue = value;
                IsActive = true;
            }
        }

        public void Add(double value)
        {
            if (!IsActive)
            {
                EnsureActive(value);
            }
            else
            {
                CurrentValue += value;
            }
        }

        public void Subtract(double value)
        {
            if (!IsActive)
            {
                EnsureActive(value);
            }
            else
            {
                CurrentValue -= value;
            }
        }

        public void Multiply(double value)
        {
            if (!IsActive)
            {
                EnsureActive(value);
            }
            else
            {
                CurrentValue *= value;
            }
        }

        public void Divide(double value)
        {
            if (value == 0)
            {
                throw new DivideByZeroException("Cannot divide by zero.");
            }

            if (!IsActive)
            {
                EnsureActive(value);
            }
            else
            {
                CurrentValue /= value;
            }
        }

        public void Reset()
        {
            CurrentValue = 0;
            IsActive = false;
        }
    }
}
