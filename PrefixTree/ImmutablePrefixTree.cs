using System.Collections.Generic;

namespace PrefixTree
{
    /// <summary>
    /// Provides a prefix tree, allowing for lookups of strings based
    /// on the starting string prefix.
    /// </summary>
    /// <remarks>
    /// This class is immutable, and suitable for calling from multiple 
    /// threads and callback functions without concurrency concerns. All 
    /// operations are side-effect free. 
    /// </remarks>
    public class ImmutablePrefixTree
    {
        /// <summary>
        /// This may be created via the PrefixTreeBuilder. The internal
        /// constructor is to prevent developer-creation of these, yet still
        /// enable the Unit Tests to create and poke at the implementation. 
        /// </summary>
        /// <param name="originalTree">The fully created, never again to be changed, PrefixTree.</param>
        internal ImmutablePrefixTree(PrefixTree originalTree) 
        {
            this.MutableTree = originalTree;
        }
        private PrefixTree MutableTree { get; }
        
        /// <summary>
        /// Given a prefix such as "tri", all strings that start
        /// with the prefix will be returned in a list. 
        /// </summary>
        /// <param name="prefix">The prefix for which matches are returned. The 
        /// prefix will be string normalized using Form C and run through a 
        /// culture invarient upper casing.</param>
        /// <returns>
        /// The returned set of strings are Unicode Form C normalized.
        /// </returns>
        public List<string> FindMatches(string prefix)
        {
            return this.MutableTree.FindMatches(prefix);
        }
    }
}