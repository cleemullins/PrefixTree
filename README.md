# PrefixTree
A C# Prefix Tree implementation with RTL language support and Unicode Normalization

# Overview
Provides an implementation of a Prefix Tree API.
```c#
public List<string> FindMatches(string prefix)
```
This API takes a standard string and returns all strings that 
match the prefix. 

# Interesting Aspects
1. The PrefixTree, once constructed, is immutable. This allows consumption of the tree to be done without concerns around concurrency or async patterns. All mutating operations happen at object construction, allowing for the immutable pattern. 

2. The Prefix Tree is Unicode aware. All input strings are Unicode Form C normalized, and all comparisions are done with fully normalized strings. This is tested, both with and without normalization, using English, Russin and Arabic. Tests explicitly include include Emoji's, UTF-16 Combining characters, and UTF-32 codepoints. 

3. The code is both Left-To-Right and Right-To-Left tested. I've never written RTL code before, and was looking to see what was needed. Plumbing that all the way through was very interesting! To confirm code, normalization and comparisions are working properly the Arabic word file is the top 1000 frequent arabic words and the RTL nature of the tests appears to be working.

# Future Considerations
1. This code is inefficent, although that seems fine. There's no caching, and memory use is unoptimized. Nevertheless, I was able to load in the entire English Scrabble dictionary (it's in the tests) with no trouble. 

