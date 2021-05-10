using Domain.Extensions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Domain.UnitTests.Extensions
{
    public class EnumerableExtensionsTests
    {
        #region Auxiliary classes
        public class Type
        {
            public int Number { get; set; }
        }

        public class TypeA : Type { }

        public class TypeB : Type { }
        #endregion

        public class Intersect
        {
            [Fact]
            public void IntersectOfAllDifferentElements_Should_ReturnEmpty()
            {
                // Arrange
                List<TypeA> list_odd = new()
                {
                    new() { Number = 1, },
                    new() { Number = 3, },
                    new() { Number = 5, },
                    new() { Number = 7, },
                };

                List<TypeB> list_even = new()
                {
                    new() { Number = 0, },
                    new() { Number = 2, },
                    new() { Number = 4, },
                    new() { Number = 6, },
                };

                // Act
                var result_odd_even = list_odd.Intersect(list_even, odd => odd.Number, even => even.Number);
                var result_even_odd = list_even.Intersect(list_odd, even => even.Number, old => old.Number);

                // Assert
                result_odd_even.Should().BeEmpty();
                result_even_odd.Should().BeEmpty();
            }

            [Fact]
            public void IntersectOfElementsInCommon_Should_ReturnCommonElements()
            {
                // Arrange
                List<TypeA> list_a = new()
                {
                    new() { Number = 1, },
                    new() { Number = 3, },
                    new() { Number = 5, },
                    new() { Number = 7, },
                };

                List<TypeB> list_b = new()
                {
                    new() { Number = 0, },
                    new() { Number = 3, },
                    new() { Number = 5, },
                    new() { Number = 10, },
                };

                var expected_intersection_a_b = new List<TypeA>()
                {
                    list_a[1],
                    list_a[2],
                };

                var expected_intersection_b_a = new List<TypeB>()
                {
                    list_b[1],
                    list_b[2],
                };

                // Act
                var result_a_b = list_a.Intersect(list_b, a => a.Number, b => b.Number)
                    .ToList();

                var result_b_a = list_b.Intersect(list_a, b => b.Number, a => a.Number)
                    .ToList();

                // Assert
                result_a_b.Should().Equal(expected_intersection_a_b);
                result_b_a.Should().Equal(expected_intersection_b_a);
            }

            [Fact]
            public void IntersectNullSource_Should_ReturnNull()
            {
                // Arrange
                List<TypeA>? list_a = null;

                List<TypeB>? list_b = new() 
                {
                    new() { Number = 1 },
                    new() { Number = 2 },
                    new() { Number = 3 }
                };

                // Act
                var result = list_a?.Intersect(list_b, a => a.Number, b => b.Number);

                // Assert
                result.Should().BeNull();
            }

            [Fact]
            public void IntersectNullTarget_Should_ReturnEmpty()
            {
                // Arrange
                List<TypeA>? list_a = new()
                {
                    new() { Number = 1 },
                    new() { Number = 2 },
                    new() { Number = 3 }
                }; ;

                List<TypeB>? list_b = null;

                // Act
                var result = list_a?.Intersect(list_b, a => a.Number, b => b.Number);

                // Assert
                result.Should().BeEmpty();
            }

            [Fact]
            public void IntersectEmptySource_Should_ReturnNull()
            {
                // Arrange
                List<TypeA>? list_a = new();

                List<TypeB>? list_b = new()
                {
                    new() { Number = 1 },
                    new() { Number = 2 },
                    new() { Number = 3 }
                };

                // Act
                var result = list_a.Intersect(list_b, a => a.Number, b => b.Number);

                // Assert
                result.Should().BeEmpty();
            }

            [Fact]
            public void IntersectEmptyTarget_Should_ReturnEmpty()
            {
                // Arrange
                List<TypeA>? list_a = new()
                {
                    new() { Number = 1 },
                    new() { Number = 2 },
                    new() { Number = 3 }
                };

                List<TypeB>? list_b = null;

                // Act
                var result = list_a?.Intersect(list_b, a => a.Number, b => b.Number);

                // Assert
                result.Should().BeEmpty();
            }

        }

        public class Except
        {
            [Fact]
            public void ExceptOfAllDifferentElements_Should_ReturnSourceEnumeration()
            {
                // Arrange
                List<TypeA> list_odd = new()
                {
                    new() { Number = 1, },
                    new() { Number = 3, },
                    new() { Number = 5, },
                    new() { Number = 7, },
                };

                List<TypeB> list_even = new()
                {
                    new() { Number = 0, },
                    new() { Number = 2, },
                    new() { Number = 4, },
                    new() { Number = 6, },
                };

                // Act
                var result_odd_even = list_odd.Except(list_even, odd => odd.Number, even => even.Number);
                var result_even_odd = list_even.Except(list_odd, even => even.Number, old => old.Number);

                // Assert
                result_odd_even.Should().Equal(list_odd);
                result_even_odd.Should().Equal(list_even);
            }

            [Fact]
            public void Except_Should_ReturnNotInCommonElements()
            {
                // Arrange
                List<TypeA> list_a = new()
                {
                    new() { Number = 1, },
                    new() { Number = 3, },
                    new() { Number = 5, },
                    new() { Number = 7, },
                };

                List<TypeB> list_b = new()
                {
                    new() { Number = 0, },
                    new() { Number = 3, },
                    new() { Number = 5, },
                    new() { Number = 10, },
                };

                var expected_intersection_a_b = new List<TypeA>()
                {
                    list_a[0],
                    list_a[3],
                };

                var expected_intersection_b_a = new List<TypeB>()
                {
                    list_b[0],
                    list_b[3],
                };

                // Act
                var result_a_b = list_a.Except(list_b, a => a.Number, b => b.Number)
                    .ToList();

                var result_b_a = list_b.Except(list_a, b => b.Number, a => a.Number)
                    .ToList();

                // Assert
                result_a_b.Should().Equal(expected_intersection_a_b);
                result_b_a.Should().Equal(expected_intersection_b_a);
            }

            [Fact]
            public void ExceptNullSource_Should_ReturnNull()
            {
                // Arrange
                List<TypeA>? list_a = null;

                List<TypeB>? list_b = new()
                {
                    new() { Number = 1 },
                    new() { Number = 2 },
                    new() { Number = 3 }
                };

                // Act
                var result = list_a?.Except(list_b, a => a.Number, b => b.Number);

                // Assert
                result.Should().BeNull();
            }

            [Fact]
            public void ExceptNullTarget_Should_ReturnSourceEnumeration()
            {
                // Arrange
                List<TypeA>? list_a = new()
                {
                    new() { Number = 1 },
                    new() { Number = 2 },
                    new() { Number = 3 }
                };

                List<TypeB>? list_b = null;

                // Act
                var result = list_a?.Except(list_b, a => a.Number, b => b.Number);

                // Assert
                result.Should().Equal(list_a);
            }

            [Fact]
            public void ExceptEmptySource_Should_ReturnEmpty()
            {
                // Arrange
                List<TypeA>? list_a = new();

                List<TypeB>? list_b = new()
                {
                    new() { Number = 1 },
                    new() { Number = 2 },
                    new() { Number = 3 }
                };

                // Act
                var result = list_a.Except(list_b, a => a.Number, b => b.Number);

                // Assert
                result.Should().BeEmpty();
            }

            [Fact]
            public void ExceptEmptyTarget_Should_ReturnSourceEnumeration()
            {
                // Arrange
                List<TypeA>? list_a = new()
                {
                    new() { Number = 1 },
                    new() { Number = 2 },
                    new() { Number = 3 }
                };

                List<TypeB>? list_b = new();

                // Act
                var result = list_a.Except(list_b, a => a.Number, b => b.Number);

                // Assert
                result.Should().Equal(list_a);
            }

        }
    }
}
