using System;
using System.Collections.Generic;
using System.Linq;

namespace PrefixTree
{
    /// <summary>
    /// Creates an Immutable Prefix tree from a set of strings. All mutable operations 
    /// are intended to happen during the construction phase of the Prefix Tree, freeing
    /// the consumer from considerations around concurrency and async patterns. 
    /// </summary>
    public static class PrefixTreeBuilder
    {
        /// <summary>
        /// Creates an Immutable Prefix Tree from a set of arbitary parameters.
        /// </summary>
        /// <param name="wordsToAdd">The words to add into the Prefix Tree</param>
        /// <returns>A fully constructed, normalized, ImmutablePrefixTree</returns>
        public static ImmutablePrefixTree CreatePrefixTree(params string [] wordsToAdd)
        {
            var w = wordsToAdd.ToList();
            return CreatePrefixTree(w);
        }

        /// <summary>
        /// Creates an Immutable Prefix Tree from a set of strings.
        /// </summary>
        /// <param name="wordsToAdd">The list of words to add</param>
        /// <returns>A fully constructed, normalized, ImmutablePrefixTree</returns>
        public static ImmutablePrefixTree CreatePrefixTree(List<string> wordsToAdd)
        {
            // Spiffy C# 7 syntax for null checking. 
            _ = wordsToAdd ?? throw new ArgumentNullException(nameof(wordsToAdd));

            PrefixTree prefixTree = new PrefixTree();
            foreach(string word in wordsToAdd)
            {
                AddWord(prefixTree, word);
            }

            return new ImmutablePrefixTree(prefixTree);
        }

        /// <summary>
        /// Adds words into the Prefix tree, mutating the tree as new words
        /// are added. 
        /// </summary>
        /// <param name="prefixTree">The already constructed prefix tree that will be 
        /// (potentially) mutated by the addition of a new word.</param>
        /// <param name="wordToAdd">The word to add. Each word will be Unicode 
        /// normalized during addition, to make runtime comparisons easier.</param>
        private static void AddWord(PrefixTree prefixTree, string wordToAdd)
        {
            string normalizedWordToAdd = PrefixTree.NormalizeString(wordToAdd);
            Node visitingNode = prefixTree.Root;

            foreach (var codepoint in normalizedWordToAdd)
            {
                if (visitingNode.Children.ContainsKey(codepoint))
                {
                    // codepoint already exists. Nothing more to do. 
                    visitingNode = visitingNode.Children[codepoint];
                }
                else
                {
                    Node newNode = new Node(codepoint);                    
                    visitingNode.Children.Add(newNode.Prefix, newNode);
                    visitingNode = visitingNode.Children[codepoint];
                }
            }
            visitingNode.CompleteWord = true;
        }
    }
}
