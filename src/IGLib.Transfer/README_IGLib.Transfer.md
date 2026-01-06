
# The `IGLib.Transfer` Project

This project contains **utilities transferred from the legacy IGLib Framework** *for use with the IGLib Core*. This projects **temporarily hosts these utilities** such that namespaces, dependencies and replacements can be sorted out, before moving the utilities to their final destination projects.

Thi project is **not contained** in the basic solution `IGLibCore.sln` or the complete solution for the new IGLib libraries 'IGLibCore_All.sln'. Use the `IGLibCore_Extended.sln` from the current repository in order to work with this project. It is **permitted that the project cannot be built**, even on the main branch. Therefore, the solution `IGLibCore_Extended.sln` cannot be included in CI/CD pipelines.

**Table of Contents**:

<a href="https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/README.md"><img src="https://ajgorhoe.github.io/icons/IGLibIcon_256x256.png" alt="[IGLib]" align="right" width="48pt"
  style="float: right; max-width: 30%; width: 48pt; margin-left: 8pt;" /></a>

* [Additional Information](#additional-information)
  * [Transferring Utilities from IG.Num](#transferring-utilities-from-ignum)
    * [Details of Transferring Numerical Utilities](#details-of-transferring-numerical-utilities)
    * **[To Be Done](#to-be-done)**
  * **[Testing](#testing)**
* Other:
* [Repository's README](../../README.md) ([on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/README.md))
  * [About the Repository](../../README.md#about-this-repository---iglibcore)
    * [Projects within IGLib.Core repository](../../README.md#projects-within-the-iglibcore-repository)
  * [The Investigative Generic Library (IGLib)](../../README.md#the-investigative-generic-library-iglib) - information about IGLib as a whole
* [this document on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/src/IGLib.Transfer/README_IGLib.Transfer.md); [project directory on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/src/IGLib.Transfer)

## Additional Information

### Transferring Utilities from IG.Num

Started on **December 2025**. This is to transfer utilities from the numerical utilities from the legacy IGLib Framework (mainly the namespace IG.Num). The utilities will be copied rather than moved. These utilities would need to be improved, updated and covered more with tests. However, IGLib Core needs these utilities for development in 3D graphics and other areas. Therefore these utilities are transferred before they are refactored to a higher standard, and refactoring to deal with the technical dept will be done later, as these libraries are used.

The intention is to move these utilities to IGLib.Numeric. Before moving:

* Transferred utilities should **build without errors** in the current project.
* **Most of the utilities** from the IG.Num namespace should be **transferred** (copied).
* Namespaces should be changed, such that the **legacy IGLib and** the **IGLib.Numeric** library **can be referenced simultaneously**.

#### Details of Transferring Numerical Utilities

Utilities from **Geometry/PointClouds** were **not transferred**. Point clouds will be reimplemented if they are needed.

In the **MatrixBase class**, two **renames** were performed:

* `double DeterminantSlow(IMatrix A)` => `double CalculateDeterminantSlow(IMatrix A)` and 
* `double Determinant(IMatrix A, ref int[] auxPermutations, ref IMatrix auxLU)` => `double CalculateDeterminant(IMatrix A, ref int[] auxPermutations, ref IMatrix auxLU)`.
* 
* The `Determinant(...)` had to be renamed because it was otherwise in **name conflict with the `Determinant` property** in the derived classes `Matrix2D` and `Matrix3D`.

#### To Be Done

In IGLib.Numeric, **Remove all dependencies on Console**. These dependencies can be kept in a higher level libraries, for example via static utility methods or extension methods.

### Testing

**This is to be changed**.

**Unit tests** and some lower level integration tests are provided in the [IGLib.Core.Tests](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/tree/main/tests/IGLib.Core.Tests) project.
