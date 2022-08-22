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

        [Test]
        [TestCase(",, The, quick, brown, fox, jumped, over, the, lazy,, dog", 5, "fox")]
        [TestCase(",, The, quick, brown, fox, jumped, over, the, lazy,, dog", 0, "")]
        [TestCase(",, The, quick, brown, fox, jumped, over, the, lazy,, dog", 1, "")]
        [TestCase(",, The, quick, brown, fox, jumped, over, the, lazy,, dog", 2, "The")]
        [TestCase(",, The, quick, brown, fox, jumped, over, the, lazy,, dog", 3, "quick")]
        [TestCase(",, The, quick, brown, fox, jumped, over, the, lazy,, dog", 7, "over")]
        [TestCase(",, The, quick, brown, fox, jumped, over, the, lazy,, dog", 11, "dog")]
        [TestCase(",, The, quick, brown, fox, jumped, over, the, lazy,, dog", 12, null)]
        [TestCase(",, The, quick, brown, fox, jumped, over, the, lazy,, dog", 13, null)]
        public void GetNthWord(string input, int nthWord, string expectedWord)
        {
            ReadOnlySpan<char> s = ",,The,quick,brown,fox,jumped,over,the,lazy,,dog".ToCharArray();
            var word = s.GetNthToken(',', nthWord);
            Assert.AreEqual(word, expectedWord); 
        }

    }
}
