
# Representing Line Continuation in Console Abstractions without the Write Method

The **`System.Console`** class has several **`Write` method overloads**, which do not by themselves cause creating a line break after the written text. **Some systems do not have this possibility**. For example the **xUnit test environment** has the possibility of `ITestOutputHelper` object being injected into test classes, which enable tests to write to test's standard output. However, the `ITestOutputHelper` interface **provides** the `WriteLine` method, but **no `Write` method**. One can only generate individual outputs that will insert the line breaks at the end.

One way to **emulate the `Write` method** in system where one can only write to the output by inserting line break at the end, is to **add a special sign that signifies that what appears in the next line should actually be appended to the current line** without additional whitespace.

## A Special Character to Signify Line Continuation

> **Q:** I am looking for a suitable Unicode character that I can use at the end of line of text to signify that the next line of text is continuation of the current line, such as in "word wrap" mode in editors, or to substitute the `Write` method in systems that only support the `WriteLine` method, such as the .NET xUnit test system.
>
> The character should be graphically intuitive and easily associated with its purpose, but it should not be often used in texts. Ideally, it should be part of the Unicode Basic Multilingual Plane (BMP), to be useful in systems that cannot handle the characters beyond that, and it should also be commonly present in fonts.

This is a good way to handle the constraints of systems like xUnit’s `ITestOutputHelper`, which forces a newline with every call. To find the right character, we need to balance **readability** (it looks like a continuation) with **rarity** (it won't be confused with actual text).

Here are a few suggestions from the **Unicode Basic Multilingual Plane (BMP)** that are widely supported in monospaced fonts (like `Courier`, `Consolas`, or `Cascadia Code`), which are commonly used to render standard output.

### The "Return" or "Elbow" Arrows

These are the most intuitive because they mimic the "Return" key on a keyboard or the "Word Wrap" icon in IDEs like VS Code or Notepad++.

* **↵ (U+21B5): Downwards Arrow with Corner Leftwards**
* **Why:** This is the universal symbol for a carriage return/line feed.
* **Vibe:** "The line ends here, but moves down and back to the start."

### The Downwards Arrow with Corner Rightwards

* **↴ (U+21B4): Downwards Arrow with Corner Rightwards**
* **Why:** Often used in code editors to show that a long line has been wrapped to the next line.
* **Vibe:** Very modern "wrapped text" look.

