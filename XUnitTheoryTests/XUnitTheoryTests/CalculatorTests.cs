using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace XUnitTheoryTests
{
    public class CalculatorTests
    {
        [Fact]
        public void CanAdd()
        {
            var calculator = new Calculator();

            int value1 = 1;
            int value2 = 2;

            var result = calculator.Add(value1, value2);

            Assert.Equal(3, result);
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(-4, -6, -10)]
        [InlineData(-2, 2, 0)]
        [InlineData(int.MinValue, -1, int.MaxValue)]
        public void CanAddTheory(int value1, int value2, int expected)
        {
            var calculator = new Calculator();

            var result = calculator.Add(value1, value2);

            Assert.Equal(expected, result);
        }

        [Theory]
        [ClassData(typeof(CalculatorTestData))]
        public void CanAddTheoryClassData(int value1, int value2, int expected)
        {
            var calculator = new Calculator();

            var result = calculator.Add(value1, value2);

            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void CanAddTheoryMemberDataProperty(int value1, int value2, int expected)
        {
            var calculator = new Calculator();

            var result = calculator.Add(value1, value2);

            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(GetData), parameters: 3)]
        public void CanAddTheoryMemberDataMethod(int value1, int value2, int expected)
        {
            var calculator = new Calculator();

            var result = calculator.Add(value1, value2);

            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(Data), MemberType = typeof(CalculatorData))]
        public void CanAddTheoryMemberDataPropertyOnOtherClass(int value1, int value2, int expected)
        {
            var calculator = new Calculator();

            var result = calculator.Add(value1, value2);

            Assert.Equal(expected, result);
        }

        [Theory]
        [JsonFileData("all_data.json")]
        public void CanAddTheoryJsonFile(int value1, int value2, int expected)
        {
            var calculator = new Calculator();

            var result = calculator.Add(value1, value2);

            Assert.Equal(expected, result);
        }

        [Theory]
        [JsonFileData("data.json", "AddData")]
        public void CanAddTheoryJsonFileFromProperty(int value1, int value2, int expected)
        {
            var calculator = new Calculator();

            var result = calculator.Add(value1, value2);

            Assert.Equal(expected, result);
        }

        [Theory]
        [JsonFileData("data.json", "SubtractData")]
        public void CanSubtractTheoryJsonFileFromProperty(int value1, int value2, int expected)
        {
            var calculator = new Calculator();

            var result = calculator.Subtract(value1, value2);

            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
            new object[] { 1, 2, 3 },
            new object[] { -4, -6, -10 },
            new object[] { -2, 2, 0 },
            new object[] { int.MinValue, -1, int.MaxValue },
            };

        public static IEnumerable<object[]> GetData(int numTests)
        {
            var allData = new List<object[]>
        {
            new object[] { 1, 2, 3 },
            new object[] { -4, -6, -10 },
            new object[] { -2, 2, 0 },
            new object[] { int.MinValue, -1, int.MaxValue },
        };

            return allData.Take(numTests);
        }
    }
}

