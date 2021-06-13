using System.Collections.Generic;

namespace PrefixTree
{
    /// <summary>
    /// Represents a node in the Prefix tree. Each node represents a 
    /// letter (Form C normalized UTF16 Codepoint), and a value indiciate if 
    /// this node termiantes a word. Nodes also have child nodes that represent 
    /// follow-on characters in potential words. 
    /// </summary>
    /// <remarks>
    /// ONE. This class is MUTABLE. As new nodes are added, 
    /// the CompleteWord property may change. For example, if the word 
    /// "manual" is added as the first word, then only the "l" node would have the 
    /// CompletedWord attribute set. If the 2nd word added is "man", then while no new 
    /// nodes are added, the "n" has the CompletedWord attribute set. This prevents the
    /// class from being immutable. 
    /// The representation internal, so reasoning over these side effects still allows
    /// the overall Prefix Tree to be immutable once the creation process is complete. 
    /// 
    /// TWO. As a future space optimization, this class could be turned into a struct. I kept it
    /// deliberatly simple to enable that, but have chosen not to do it at this time
    /// as classes are more common, and structures may have unexpected behaviors. Ports
    /// to other lanauges are also easier the more 'normal' this remains. 
    /// </remarks>
    internal class Node
    {
        public Node() { } 

        public Node(char prefix) : this()
        {            
            this.Prefix = prefix;
        }

        public Node(char prefix, bool completedWord) : this(prefix)
        {
            this.CompleteWord = completedWord;
        }

        public char Prefix { get; }
        
        public bool CompleteWord { get; set; } = false; 
        
        /// <summary>
        /// This Dictionary has easy semantics for the PrefixTree to check sub-nodes, 
        /// as the tree can simply check "Exists()" on the dictionary key. 
        /// The tradeoff is the dictionary is big, and many nodes are created. A 
        /// potential future optimization would be to simply have this be an array and trade
        /// off quite a bit of space for a bit of additional logic. Here I've opted for 
        /// code clarity over space. 
        /// </summary>
        public Dictionary<char, Node> Children { get; }  = new Dictionary<char, Node>();
    }   
}
