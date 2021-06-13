using PrefixTree;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace PrefixTreeTests
{
    public class PrefixTreeTests_Russian
    {
        [Theory]
        // Format: <prefix to find>, <match that MUST be returned from tree>, [Words to add to Tree]
        // The russing world list has 4 words that being with "вз".     
        // NOTE: The below text is NOT Unicode normalized. To do an exact compare,
        // the strings need to be normlaized. 
        [InlineData("вз", new string[] { "взгляд", "взглянуть", "взрослый", "взять" })]
        public void RussianMultiMatchesTheory(string prefixToFind, string[] exactMatches)
        {
            // Arrange
            ImmutablePrefixTree tree = CreateFromRussianWordList();

            // The strings that come in for exact match may be non-normalized. As the 
            // prefix tree returns only normlaized strings, make sure these are properly
            // stringprepped for comparison. See here for a refresher if needed:
            // https://docs.microsoft.com/en-us/dotnet/api/system.string.normalize?view=net-5.0
            List<string> normalizedMatches = new List<string>();
            foreach(string s in exactMatches)
            {
                normalizedMatches.Add(PrefixTree.PrefixTree.NormalizeString(s));
            }

            // Act
            var matchResults = tree.FindMatches(prefixToFind);

            // Assert
            Assert.True(matchResults.Count == exactMatches.Length);
            foreach (var requiredMatch in normalizedMatches)
            {
                Assert.Contains(requiredMatch, matchResults);
            }
        }

        private static ImmutablePrefixTree CreateFromRussianWordList()
        {
            const string scrabbleWordFileName = "RussianWords.txt";
            if (!File.Exists(scrabbleWordFileName))
            {
                throw new FileNotFoundException("Russian Word file is missing");
            }

            var allWords = File.ReadAllLines(scrabbleWordFileName);
            return PrefixTreeBuilder.CreatePrefixTree(allWords);
        }
    }
}
