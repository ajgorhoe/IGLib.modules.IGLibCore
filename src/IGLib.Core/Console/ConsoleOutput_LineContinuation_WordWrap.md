
# Representing Line Continuation in Console Abstractions without the Write Method

The **`System.Console`** class has several **`Write` method overloads**, which do not by themselves cause creating a line break after the written text. **Some systems do not have this possibility**. For example the **XUnit test environment** has the possibility of `IOutputHelper` object being injected into test classes, which enable tests to write to test's standard output. However, the `IOutputHelper` interface **provides** the `WriteLine` method, but **no `Write` method**. One can only generate individual outputs that will insert the line breaks at the end.

One way to **emulate the `Write` method** in system where one can only write to the output by inserting line break at the end, is to **add a special sign that signifies that what appears in the next line should actually be appended to the current line** without additional whitespace.

## A Special Character to Signify Line Continuation

> **Q:** I am looking for a suitable Unicode character that I can use at the end of line of text to signify that the next line of text is continuation of the current line, such as in "word wrap" mode in editors, or to substitute the `Write` method in systems that only support the `WriteLine` method, such as the .NET XUnit test system.
>
> The character should be graphically intuitive and easily associated with its purpose, but it should not be often used in texts. Ideally, it should be part of the Unicode Basic Multilingual Plane (BMP), to be useful in systems that cannot handle the characters beyond that, and it should also be commonly present in fonts.



