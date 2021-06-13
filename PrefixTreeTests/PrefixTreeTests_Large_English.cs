using PrefixTree;
using System.IO;
using Xunit;

namespace PrefixTreeTests
{
    public class PrefixTreeTests_Large_English
    {        
        [Theory]
        // Format: <prefix to find>, <match that MUST be returned from tree>, [Words to add to Tree]
        // The english scrabble dictionary has 4 words that being with "AAL". 
        // AAL, AALII, AALIIS, AALS    
        [InlineData("aal", 
            new string[] { "AAL", "AALII", "AALIIS", "AALS" })]

        // 5 words that being with "zymologi". 
        //  ZYMOLOGIC, ZYMOLOGICAL, ZYMOLOGIES, ZYMOLOGIST, ZYMOLOGISTS
        [InlineData("zymologi", 
            new string[] { "ZYMOLOGIC", "ZYMOLOGICAL", "ZYMOLOGIES", "ZYMOLOGIST", "ZYMOLOGISTS" })]
        public void EnglighMultiMatchesTheory(string prefixToFind, string[] exactMatches)
        {
            // Arrange
            ImmutablePrefixTree tree = CreateFromEnglishScrabbleWordList();

            // Act
            var matchResults = tree.FindMatches(prefixToFind);

            // Assert
            Assert.True(matchResults.Count == exactMatches.Length);
            foreach (var requiredMatch in exactMatches)
            {
                Assert.Contains(requiredMatch, matchResults);
            }
        }


        private static ImmutablePrefixTree CreateFromEnglishScrabbleWordList()
        {
            const string scrabbleWordFileName = "EnglishScrabbleWords.txt";
            if (!File.Exists(scrabbleWordFileName))
            {                
                throw new FileNotFoundException("English Scrabble Word file is missing");
            }

            var allWords = File.ReadAllLines(scrabbleWordFileName);
            return PrefixTreeBuilder.CreatePrefixTree(allWords);
        }
    }
}
