using PrefixTree;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PrefixTreeTests
{
    public class ImmutablePrefixTreeTests
    {
        [Fact]
        public void EmptyTree()
        {
            // Arrange
            ImmutablePrefixTree tree = PrefixTreeBuilder.CreatePrefixTree(string.Empty);

            // Check all 8-bit ascii codes. Could extend to UTF-8            
            for (int i = 0; i < 255; i++)
            {
                string prefixToFind = char.ConvertFromUtf32(i);

                // Act
                var matches = tree.FindMatches(prefixToFind);

                // Assert
                Assert.True(matches.Count == 0);
            }
        }

        [Fact]
        public void SingleLetterTreeMatch()
        {
            // Arrange
            ImmutablePrefixTree tree = PrefixTreeBuilder.CreatePrefixTree("A");

            // Act
            var matches = tree.FindMatches("A");

            // Assert
            Assert.True(matches.Count == 1);
            Assert.True(matches[0] == "A"); // Note: Normalized value
        }

        [Fact]
        public void SingleLetterTreeMatchUTF16()
        {
            // Arrange
            ImmutablePrefixTree tree = PrefixTreeBuilder.CreatePrefixTree("✋");

            // Act
            var matches = tree.FindMatches("✋");

            // Assert
            Assert.True(matches.Count == 1);
            Assert.True(matches[0] == "✋"); // Note: Normalized value
        }

        [Fact]
        public void SingleLetterTreeMatchUTF16SurrogatePair()
        {
            // Arrange
            // D83D + DE03 => \u1F603
            string surrogatePair = "\ud83d\uDE03"; // UTF32 is U+1F603, 😃            

            ImmutablePrefixTree tree = PrefixTreeBuilder.CreatePrefixTree(surrogatePair);

            // Act
            var matches = tree.FindMatches(surrogatePair);            

            // Assert
            Assert.True(matches.Count == 1);
            Assert.True(matches[0] == char.ConvertFromUtf32(0x1f603)); // Note: Normalized value
        }

        [Fact]
        public void SingleLetterTreeNoMatch()
        {
            // Arrange
            ImmutablePrefixTree tree = PrefixTreeBuilder.CreatePrefixTree("A");

            // Act
            var matches = tree.FindMatches("B");

            // Assert
            Assert.True(matches.Count == 0);
        }

        [Fact]
        public void ZeroMaxResults()
        {
            // Arrange
            ImmutablePrefixTree tree = PrefixTreeBuilder.CreatePrefixTree("A", "B");

            // Act            
            var matches = tree.FindMatches("A", 0); // Ask for 0 results,

            // Assert
            // Even though the tree has a match, this test asked for 0 results. 
            Assert.True(matches.Count == 0); 
        }

        [Fact]
        public void ThereCanBeOnlyOne()
        {
            // Arrange
            ImmutablePrefixTree tree = PrefixTreeBuilder.CreatePrefixTree("A", "AA", "AB");

            // Act            
            var matches = tree.FindMatches("A", 1); // Ask for 1 results

            // Assert
            // Verify only a single result was returned, even though the 
            // tree actually has 3 matches. 
            Assert.True(matches.Count == 1);
        }

        [Fact]
        public void ThereCanBeOnlyTwo()
        {
            // Arrange
            ImmutablePrefixTree tree = PrefixTreeBuilder.CreatePrefixTree("A", "AA", "AB");

            // Act            
            var matches = tree.FindMatches("A", 2); // Ask for 2 results

            // Assert
            // Verify only two result was returned, even though the 
            // tree actually has 3 matches. 
            Assert.True(matches.Count == 2);
        }

        // Format: <prefix to find>, <match from tree>, [Words to add to Tree]
        [Theory]
        [InlineData("a", "A", new string[] { "a", "b", "c" })]
        [InlineData("b", "B", new string[] { "a", "b", "c" })]
        [InlineData("c", "C", new string[] { "a", "b", "c" })]
        [InlineData("aa", "AA", new string[] { "aa", "bb", "cc" })]
        [InlineData("bb", "BB", new string[] { "aa", "bb", "cc" })]
        [InlineData("cc", "CC", new string[] { "aa", "bb", "cc" })]
        public void SingleMatchTheory(string prefixToFind, string exactMatch, string[] wordsToAdd)
        {
            // Arrange
            ImmutablePrefixTree tree = PrefixTreeBuilder.CreatePrefixTree(wordsToAdd);

            // Act
            var matches = tree.FindMatches(prefixToFind);

            // Assert
            Assert.True(matches.Count == 1);
            Assert.True(matches[0] == exactMatch);
        }

        // Format: <prefix to find>, <match that MUST be returned from tree>, [Words to add to Tree]
        [Theory]
        [InlineData("a", new string[] { "A", "AA" }, new string[] { "a", "aa" })]
        [InlineData("a", new string[] { "A", "AA" }, new string[] { "a", "aa", "b" })]
        [InlineData("a", new string[] { "A", "AA", "AAA" }, new string[] { "a", "aa", "aaa" })]
        [InlineData("aal", new string[] { "AAL", "AALII", }, new string[] { "AAL", "AALII" })]
        //[InlineData("aal", new string[] { "AAL", "AALII", "AALIIS", "AALS" }, new string[] { "AAL", "AALII", "AALIIS", "AALS" })]
        public void MultiMatchesTheory(string prefixToFind, string[] exactMatches, string[] wordsToAdd)
        {
            // Arrange
            ImmutablePrefixTree tree = PrefixTreeBuilder.CreatePrefixTree(wordsToAdd);

            // Act
            var matchResults = tree.FindMatches(prefixToFind);

            // Assert
            Assert.True(matchResults.Count == exactMatches.Length);
            foreach (var requiredMatch in exactMatches)
            {
                Assert.Contains(requiredMatch, matchResults);
            }
        }
    }
}

