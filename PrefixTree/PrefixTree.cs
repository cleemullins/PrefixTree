using System.Collections.Generic;
using System.Text;

namespace PrefixTree
{
    /// <summary>
    /// Core implemenation of the prefix tree pattern. This class is intended 
    /// to be manipualted by the PrefixTreeBuilder, which is empowered to run
    /// mutating operations during object construction. Once fully constructed
    /// this is exposed to a developer via the ImmutablePrefixTree, thereby eliminating 
    /// concerns around aync usage, concurrency, and side effects.
    /// </summary>
    internal class PrefixTree
    {        
        /// <summary>
        /// This is internal (rather than private), as the addition and manipulation 
        /// of the tree structure during construction time is contained in the 
        /// builder class. This allowed for easier testing and seperation of logic, 
        /// and prevents construction time abstractions from leaking in the runtime
        /// implementation. 
        /// </summary>
        internal Node Root { get; } = new Node();

        /// <summary>
        /// Only the builder (and the unit tests) are intended to create instances 
        /// of this class. 
        /// </summary>
        internal PrefixTree()
        {
        }

        /// <summary>
        /// Given a prefix such as "tri", all strings that start
        /// with the prefix will be returned in a list. For example, for "tri", 
        /// the strings ("triced, tricep, and triceps") may be returned.
        /// </summary>
        /// <param name="prefix">The prefix for which matches are returned. The 
        /// prefix will be string normalized using Form C and run through a 
        /// culture invarient upper casing.</param>
        /// <param name="maxResults">The maximum number of results to return.</param>
        /// <returns>
        /// The returned set of strings are Unicode Form C normalized.
        /// </returns>        
        public List<string> FindMatches(string prefix, int maxResults)
        {
            List<string> results = new List<string>();
            if (string.IsNullOrWhiteSpace(prefix))
            {
                return results;
            }

            // If the client asked for a nonsensical number of results, just
            // return an empty result set. 
            if (maxResults < 1)
            {
                return results;
            }

            string normalizedPrefix = NormalizeString(prefix);

            Node visitingNode = this.Root;

            // Find starting node by walking to the end of the prefix. 
            foreach (var codepoint in normalizedPrefix)
            {
                // if we're walking the tree and don't have the 
                // codepoint, then there are ZERO matches. Can bail
                // out early and save time. 
                if (!visitingNode.Children.ContainsKey(codepoint))
                {
                    return results;
                }
                else
                {
                    visitingNode = visitingNode.Children[codepoint];
                }
            }

            // We are now at the "starting node". For example, given the below ascii tree:
            // Prefix: "ba"
            //  <root>
            //      +a
            //      ++t
            //      +b
            //      ++a    <-- should be here
            //      +++t
            //      +c
            //      ++a
            //      +++r
            //
            // We should be sitting on the "a" node, which has a "t" under it. 
            // From here we need to do a tree recurse looking for words that start with this prefix.

            List<string> allWords = new List<string>();

            // When the recursive tree traversal is started, we're *starting* on the 
            // last character of the prefix. To eliminate all special cases in the 
            // recursion (debugging recursion is hard), here I remove the final 
            // character of the prefix, knowing the each node in the recursion will
            // add itself (including the first node). 
            var fixedPrefix = normalizedPrefix.Remove(normalizedPrefix.Length - 1);
            FindAllWords(visitingNode, fixedPrefix, allWords, maxResults);

            return allWords;
        }

        /// <summary>
        /// Recursive depth first traversal of the tree from our starting node. Any 
        /// node that is marked as "completing a word" gets added to the list of 
        /// words found. Note that "word complete" is NOT a stopping condision, as 
        /// words such as "car", "cards", and "cars" are each words that need to be
        /// added. If recursion stopped on "car" the other words would be missed. 
        /// </summary>
        /// <param name="node">The node to examine. This changes as the function 
        /// recurses through the tree.</param>
        /// <param name="prefix">The current "total" prefix. "car" -> "card" -> "cards" </param>
        /// <param name="wordsFound">The unordered list of complete words found so far</param>
        /// <param name="maxResults">The maximum number of complete words to return.</param>
        private void FindAllWords(Node node, string prefix, List<string> wordsFound, int maxResults)
        {
            if (wordsFound.Count >= maxResults)
            {
                // A sufficent number of results have been found, so 
                // all futher searching can stop.
                return;
            }

            string wordSoFar = string.Concat(prefix, node.Prefix);

            if (node.CompleteWord)
            {
                wordsFound.Add(wordSoFar);
            }
                        
            foreach(var nodePrefixPair in node.Children)
            {                
                FindAllWords(nodePrefixPair.Value, wordSoFar, wordsFound, maxResults);
            }
        }
        /// <summary>
        /// Normalizes strings for comparision and accuracy. The unicode steps taken here
        /// allow for this codebase to work over a wide array of unicode words w/o any
        /// issues and unexpected side effects. All strings are run through this normalization
        /// process before being stored, and prior to compares.
        /// 
        /// For a refresher how this works in .Net, see this doc:
        /// https://docs.microsoft.com/en-us/dotnet/api/system.string.normalize?view=net-5.0
        /// </summary>
        /// <param name="word">The string to normalize following Unicode best practices.</param>
        /// <returns>The uppercase Unicode Form C normalized version of the string.</returns>
        public static string NormalizeString(string word)
        {
            string step1 = word.Normalize(NormalizationForm.FormC);
            
            // Unicode recommends doing this to elimiante issues around 
            // tri-state languages (such as the Turkish i)
            return step1.ToUpperInvariant();
        }
    }
}
