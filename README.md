[![Prefix Tree](https://github.com/cleemullins/PrefixTree/actions/workflows/build.yml/badge.svg)](https://github.com/cleemullins/PrefixTree/actions/workflows/build.yml)

# PrefixTree
A C# Prefix Tree implementation with RTL language support and Unicode Normalization. The prefix tree is constructed via a builder and exposed to developers as an immutable data structure. The tree exposes only search semantics for runtime consumption. 

Internally, the tree contains a node for each letter (UTF-32 codepoint) and termination conditions on words. For a small tree, this represents as:

![image](https://user-images.githubusercontent.com/1165321/121821117-b1a62000-cc4b-11eb-8270-18cf06eaee84.png)

# Overview
Provides an implementation of a Prefix Tree API. 

```c#
public List<string> FindMatches(string prefix)
```
This API takes a standard string and returns all strings that 
match the prefix. 

In the tree pictured above, we would see the following results:
|Search   	|  Returned Words 	|
|---	|---	|
|A   	|A, AT, AM, ALL, ALT   	|
|AL 	|ALL, ALT   	|
|C   	|CAR   	|

# Interesting Aspects
1. **Immutable**. The PrefixTree, once constructed, is immutable. This allows consumption of the tree to be done without concerns around concurrency or async patterns. All mutating operations happen at object construction, allowing for the immutable pattern. 

2. **Unicode**. The Prefix Tree is Unicode aware. All input strings are Unicode Form C normalized, and all comparisons are done with fully normalized strings. This is tested, both with and without normalization, using English, Russian and Arabic. Tests explicitly include Emoji's, UTF-16 Combining characters, and UTF-32 codepoints. 

3. **Right-to-Left tested (RTL)**. The code is both Left-To-Right and Right-To-Left tested. I've never written RTL code before, and was looking to see what was needed. Plumbing that all the way through was very interesting! To confirm code, normalization and comparisons are working properly the Arabic word file is the top 1000 frequent Arabic words and the RTL nature of the tests appears to be working.

## Emoji's
Emoji's can be treated as words in many scenarios. A complex emoji may have mulitple UTF-32 Codepoints and is subject to composistion rules. For use in this Prefix Tree, Emoji's are treated as words, and may be encoded into the Prefix Tree and searched by prefix. A sample enoding of 3 Emoji's sharing compound prefixes is seen here:

![image](https://user-images.githubusercontent.com/1165321/121822824-0f3f6a00-cc56-11eb-9dc7-836f9fbee354.png)

The Unit Tests validate Emoji support through theory testing. An understanding of UTF-32, [UTF-16 Surrogate Pairs](https://en.wikipedia.org/wiki/UTF-16#Code_points_from_U+010000_to_U+10FFFF), and [grapheme's](https://unicode.org/reports/tr29/) may be helpful to understand these tests. 

# Future Considerations
1. This code is inefficient. There's no caching, and memory use is unoptimized, and the obvious algorithmic changes around word frequency and common letter combinations is not examined. Nevertheless, I was able to load in the entire English Scrabble dictionary (it's in the tests) with no trouble. 
2. There are far more efficient representations of words than the "1 character per node" solution here (the ZIP / huffman coding algorithm comes immediately to mind). Given the public API surface and the immutable nature of the runtime, different implementations could be swapped out without impact. 

# Unit Tests
The majority of Unit Tests are [XUnit Theory Tests](https://hamidmosalla.com/2017/02/25/xunit-theory-working-with-inlinedata-memberdata-classdata/). These allow the same Assemble/Action/Assert code to be used with multiple data set. An example is here, where you can see the data being tested is encapsulated in the Unit Test attribute. 

Here we see two different tests run using the same code logic. In the first test, a search pattern of "aal" is run, and expected to return an exact result set. In the 2nd example a pattern of "zymologi" is run and expected to return 5 results. 

![image](https://user-images.githubusercontent.com/1165321/121821276-e5357a00-cc4c-11eb-9be8-1b2b213e2e15.png)

This approach lets me write data oriented tests to validate logic, which increases the quality of the tests. 

# Testing in Codespaces
This project works in Github Codespaces. To use Codespaces, be sure to install the .Net Core Test Explorer into the VS Code container. 

Once the Test Exporer is installed, the tests can be viewed, run, and debugged all in the browser. 
![image](https://user-images.githubusercontent.com/1165321/121817765-2a9b7c80-cc38-11eb-8978-2e94ddd1dab1.png)

