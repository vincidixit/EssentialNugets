

using CalculatorApp;
using System.Collections;

namespace CalculatorAppTest
{
    public class BasicCalculatorTest
    {
        private readonly BasicCalculator _calculator;

        public BasicCalculatorTest()
        {
            _calculator = new BasicCalculator();
        }

        [Fact]
        public void Calculator_Add_Fact()
        {
            int result = 5;
            _calculator.Add(2);
            _calculator.Add(3);
            Assert.Equal(expected: result, actual: _calculator.CurrentValue);
        }

        [Theory]
        [InlineData(2, 3, 5)]
        [InlineData(10, 3, 13)]
        [InlineData(1, 3, 4)]
        public void Calculator_Add_Theory_InlineData(int num1, int num2, int result)
        {
            _calculator.Add(num1);
            _calculator.Add(num2);

            Assert.Equal(expected: result, actual: _calculator.CurrentValue);
        }

        [Theory]
        [InlineData(3, 3)]
        [InlineData(7, 3, 4)]
        [InlineData(15, 10, 4, 1)]
        public void Calculator_Add_Theory_InlineData_Nested(int result, params int[] nums)
        {
            foreach (var num in nums)
            {
                _calculator.Add(num);
            }
            Assert.Equal(expected: result, actual: _calculator.CurrentValue);
        }

        [Theory]
        [MemberData(nameof(AdditionData))]
        public void Calculator_Add_Theory_MemberData(int num1, int num2, int result)
        {
            _calculator.Add(num1);
            _calculator.Add(num2);

            Assert.Equal(expected: result, actual: _calculator.CurrentValue);
        }

        public static IEnumerable<object[]> AdditionData()
        {
            yield return new object[] { 3, 3, 6 };
            yield return new object[] { 1, 4, 5 };
            yield return new object[] { 3, 6, 9 };
        }

        [Theory]
        [ClassData(typeof(MultiplicationData))]
        public void Calculator_Multiply_Theory_ClassData(int result, params int[] nums)
        {
            foreach (var num in nums)
            {
                _calculator.Multiply(num);
            }

            Assert.Equal(expected: result, actual: _calculator.CurrentValue);
        }

        class MultiplicationData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { 120, new int[] { 1, 2, 3, 4, 5 } };
                yield return new object[] { 36_28_800, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 } };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}