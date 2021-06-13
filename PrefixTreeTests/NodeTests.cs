using Xunit;

namespace PrefixTree
{
    /// <summary>
    /// Basic structural and functionality tests on Node. Includes
    /// tests around Unicode representation to verify encodings are 
    /// supported.
    /// </summary>
    public class NodeTests
    {
        [Fact]
        public void EmptyConstructor()
        {
            Node n = new Node();
            Assert.NotNull(n);
        }

        [Fact]
        public void PrefixConstructor()
        {
            Node n = new Node('a');
            Assert.True(n.Prefix == 'a');
        }

        [Fact]
        public void PrefixConstructorUTF16Codepoint()
        {
            Node n = new Node('✋'); // U+270B
            Assert.True(n.Prefix == '✋');
        }
        
        [Fact]
        public void ValidWordTrueConstructor()
        {
            Node n = new Node('a', true);
            Assert.True(n.Prefix == 'a');
            Assert.True(n.CompleteWord);
        }

        [Fact]
        public void ValidWordTrueConstructorUTF16Codepoint()
        {
            // Treating "✋" as a word seems a bit odd, but from the 
            // unicode perspective, is legit.
            Node n = new Node('✋', true);
            Assert.True(n.Prefix == '✋');
            Assert.True(n.CompleteWord);
        }

        [Fact]
        public void ValidWordFalseConstructor()
        {
            Node n = new Node('a', false);
            Assert.True(n.Prefix == 'a');
            Assert.False(n.CompleteWord);
        }
    }
}
