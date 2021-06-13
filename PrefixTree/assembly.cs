using System.Runtime.CompilerServices;

// Enable the Unit Test assembly to test internal classes and 
// internal representations. This allows limiting the publically 
// visible surface area, yet still test internal classes. 
[assembly: InternalsVisibleTo("PrefixTreeTests")]
