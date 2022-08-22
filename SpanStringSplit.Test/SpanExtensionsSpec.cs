using NUnit.Framework;
using System;

namespace SpanStringSplit.Test
{
    [TestFixture]
    public class SpanExtensionsSpec
    {
        [Test]
        public void SplitStringsViaSpan()
        {
            var tokens = ",,The,quick,brown,fox,jumped,over,the,lazy,,dog".SplitViaSpan(',', StringSplitOptions.RemoveEmptyEntries);
            CollectionAssert.AreEqual(new string[] { "The", "quick", "brown", "fox", "jumped", "over", "the", "lazy", "dog" }, tokens);
        }

        [Test]
        public void SplitStringsUsingSpan()
        {
            ReadOnlySpan<char> s = ",,The,quick,brown,fox,jumped,over,the,lazy,,dog".ToCharArray();                
            var tokens = s.SplitSpan(',', StringSplitOptions.RemoveEmptyEntries);
            CollectionAssert.AreEqual(new string[] { "The", "quick", "brown", "fox", "jumped", "over", "the", "lazy", "dog" }, tokens);
        }

    }
}
