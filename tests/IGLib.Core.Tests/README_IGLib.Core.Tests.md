
# The IGLib.Core.Tests Project

<a href="https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/README.md"><img src="https://ajgorhoe.github.io/icons/IGLibIcon_256x256.png" alt="[IGLib]" align="right" width="48pt"
  style="float: right; max-width: 30%; width: 48pt; margin-left: 8pt;" /></a>

* [Project repository](https://github.com/ajgorhoe/IGLib.modules.IGLibCoreExtended)
* [IGLib.CoreExtended project within the repository](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/tree/main/tests/IGLib.Core.tests)

This project contains unit tests and some lower level integration tests for the core utility projects of the **IGLib** (the **Investigative Generic Library**):

* [IGLib.Core](../../src/IGLib.Core/README_IGLib.Core.md)
* [IGLib.CoreExtended](../../src/IGLib.CoreExtended/README_IGLib.CoreExtended.md)

These projects contain some utilities that were transferred from the *[legacy IGLib](https://github.com/ajgorhoe/IGLib.workspace.base.iglib)* (see the [README file](https://github.com/ajgorhoe/IGLib.workspace.base.iglib/blob/master/README.md)). Many tests for this code were lost duding transition between repositories, and may be recreated in the future, but it is hard to say when. Therefore, don't panic if you are missing some tests, or you can help by adding them. More specialized libraries that use IGLib.Core or IGLib.CoreExtended (like IGLibGraphics3D, IGLib.Scripting, etc.) should have their own tests that should detect eventual regressions in the core libraries that affect those specialized libraries. Of course, improved test coverage would still be desired and necessary.
