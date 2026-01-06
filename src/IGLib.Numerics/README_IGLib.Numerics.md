
# The IGLib.Numerics Project

This project contains auxiliary **numerical utilities** *for use with the IGLib Core*.

The initial set of utilities is **transferred from the legacy IGLib Framework's** [base repository](https://github.com/ajgorhoe/IGLib.workspace.base.iglib/blob/master/README.md), especially from the [IGLib project](https://github.com/ajgorhoe/IGLib.workspace.base.iglib/tree/master/igbase). In these utilities, the namespaces were changed such that this project can be referenced simultaneously with the legacy `IGLib` project. The **quality of code** in these transferred utilities **should be improved** and *test coverage should be provided* (tests were mainly not transferred from the legacy IGLib), but this will oly be done **gradually as the utilities are being used more**.

New utilities added to this project (not transferred from the legacy IGLib Framework) should be made up to the standards.

<a href="https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/README.md"><img src="https://ajgorhoe.github.io/icons/IGLibIcon_256x256.png" alt="[IGLib]" align="right" width="48pt"
  style="float: right; max-width: 30%; width: 48pt; margin-left: 8pt;" /></a>

* [Additional Information about the Project](#additional-information-about-the-project)
  * [Testing](#testing)
* Other:
* [Repository's README](../../README.md) ([on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/README.md))
  * [About the Repository](../../README.md#about-this-repository---iglibcore)
    * [Projects within IGLib.Core repository](../../README.md#projects-within-the-iglibcore-repository)
  * [The Investigative Generic Library (IGLib)](../../README.md#the-investigative-generic-library-iglib) - information about IGLib as a whole
* [this document on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/src/IGLib.Core/README_IGLib.Core.md); [project directory on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/src/IGLib.Core)

## Additional Information about the Project

### Testing

**Unit tests** and some lower level integration tests are provided in the [IGLib.Core.Tests](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/tree/main/tests/IGLib.Core.Tests) project. Checked for changes that may have been applied already. Many utilities in this project are not yed covered by tests (this is mainly true for the utilities that were transferred from the legacy IGLib Framework).
