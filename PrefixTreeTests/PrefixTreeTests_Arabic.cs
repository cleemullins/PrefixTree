using PrefixTree;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace PrefixTreeTests
{
    public class PrefixTreeTests_Arabic
    {
        [Theory]
        // Format: <prefix to find>, <match that MUST be returned from tree>
        // The arabic word list has 15 words that being with "إرس". Arabic is a Right-To-Left 
        // language, which makes reading the strings below seem odd to LTR readers. 
        [InlineData("الأ", 
                new string[] { 
                    "الأب", "الأخضر","الأرض", "الأرقام", "الأزرق",
                    "الأساسية", "الأصفر", "الأصلي", "الأطفال", "الأقل",
                    "الأكسجين", "الأم", "الأمة", "الأنف", "الأول"
                })]
        public void ArabicMultiMatchesTheory(string prefixToFind, string[] exactMatches)
        {
            // Arrange
            ImmutablePrefixTree tree = CreateFromArabicWordList();
            List<string> normalizedMatches = new List<string>();
            foreach(string s in exactMatches)
            {
                normalizedMatches.Add(PrefixTree.PrefixTree.NormalizeString(s));
            }

            // Act
            var matchResults = tree.FindMatches(prefixToFind);
            foreach(var s in matchResults)
            {
                System.Diagnostics.Debug.WriteLine(s);
            }

            // Assert
            Assert.True(matchResults.Count == exactMatches.Length);
            foreach (var requiredMatch in normalizedMatches)
            {
                Assert.Contains(requiredMatch, matchResults);
            }
        }
        
        private static ImmutablePrefixTree CreateFromArabicWordList()
        {
            const string scrabbleWordFileName = "ArabicWords.txt";
            if (!File.Exists(scrabbleWordFileName))
            {
                throw new FileNotFoundException("Arabic Word file is missing");
            }

            var allWords = File.ReadAllLines(scrabbleWordFileName);                           
            return PrefixTreeBuilder.CreatePrefixTree(allWords);
        }
    }
}
