
# The IGLib.TransNumericfer Project

<a href="https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/README.md"><img src="https://ajgorhoe.github.io/icons/IGLibIcon_256x256.png" alt="[IGLib]" align="right" width="48pt"
  style="float: right; max-width: 30%; width: 48pt; margin-left: 8pt;" /></a>

* [Project repository](https://github.com/ajgorhoe/IGLib.modules.IGLibCore)
* [IGLib.Numeric project within the repository](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/tree/main/src/IGLib.Numeric)
  * [This readme within repository](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/tree/main/src/IGLib.Numeric/README_IGLib.Numeric.md)

This project contains auxiliary **numerical utilities** *for use with the IGLib Core*.

The initial set of utilities is **transferred from the legacy IGLib Framework's** [base repository](https://github.com/ajgorhoe/IGLib.workspace.base.iglib/blob/master/README.md), especially from the [IGLib project](https://github.com/ajgorhoe/IGLib.workspace.base.iglib/tree/master/igbase). In these utilities, the namespaces were changed such that this project can be referenced simultaneously with the legacy `IGLib` project. The **quality of code** in these transferred utilities **should be improved** and *test coverage should be provided* (tests were mainly not transferred from the legacy IGLib), but this will oly be done **gradually as the utilities are being used more**.

New utilities added to this project (not transferred from the legacy IGLib Framework) should be made up to the standards.

## Basic Information

### Testing

**This is to be changed.**

**Unit tests** and some lower level integration tests are provided in the [IGLib.Core.Tests](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/tree/main/tests/IGLib.Core.Tests) project. Checked for changes that may have been applied already. Many utilities in this project are not yed covered by tests (this is mainly true for the utilities that were transferred from the legacy IGLib Framework).
