using PrefixTree;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace PrefixTreeTests
{
    public class PrefixTreeTests_Emoji
    {
        [Theory]
        // Format: <prefix to find>, <match that MUST be returned from tree>, [Words to add to Tree]       
        //        
        // This is an odd tests, as it's treating complex Emoji's as words. In reality, most emoji's
        // are grapheme's that combine many "letters" into a single word, by treating each letter as 
        // an attribute. This mechanism allows us leveraget the prefix list. As the first "letter" of
        // an emoji is typed, the subset of possible emoji's is returned. For example, the first flag 
        // below is made from 7 UTF-32 Codepoints that "look" like a single character on the screen.
        // The Unicode list of Emoji's is here: http://unicode.org/emoji/charts/full-emoji-list.html
        // The ones selected below are in the #1555 (Skull + Crossbones), #1814 (flag of England), 
        // #1815 (flag of Scotland), #1816 (flag of Wales)
        //        
        // Note: The first codepoint in each of the 4 Emoji's below is U+1F3F4. 
        // The Full "Word" for the first flag (Note UTF-32 Codepoints) is:
        //      U+1F3F4 U+E0067 U+E0062 U+E0065 U+E006E U+E0067 U+E007F
        // The full "Word" of the Skull and Crossbones 🏴‍☠️ is: (Note that different fonts render this differently). 
        //      U+1F3F4 U+200D U+2620 U+FE0F
        // The UTF-16 encoding of this first "letter" (U+1F3F4) is: U+D83C U+DFF4
        [InlineData("\uD83C\uDFF4", new string[] { "🏴󠁧󠁢󠁥󠁮󠁧󠁿", "🏴󠁧󠁢󠁳󠁣󠁴󠁿", "🏴󠁧󠁢󠁷󠁬󠁳󠁿", "🏴‍☠️" })]
        public void EmojiMultiMatchesTheory(string prefixToFind, string[] exactMatches)
        {
            // Arrange
            ImmutablePrefixTree tree = CreateFromEmojiWordList();

            // The strings that come in for exact match may be non-normalized. As the 
            // prefix tree returns only normlaized strings, make sure these are properly
            // stringprepped for comparison. See here for a refresher if needed:
            // https://docs.microsoft.com/en-us/dotnet/api/system.string.normalize?view=net-5.0
            List<string> normalizedMatches = new List<string>();
            foreach (string s in exactMatches)
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

        private static ImmutablePrefixTree CreateFromEmojiWordList()
        {
            const string fileName = "Emoji.txt";
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("Emoji file is missing");
            }

            var allWords = File.ReadAllLines(fileName);
            return PrefixTreeBuilder.CreatePrefixTree(allWords);
        }
    }
}
