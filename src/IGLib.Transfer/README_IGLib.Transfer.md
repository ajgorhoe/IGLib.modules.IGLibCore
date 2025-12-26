
# The IGLib.Transfer Project

<a href="https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/README.md"><img src="https://ajgorhoe.github.io/icons/IGLibIcon_256x256.png" alt="[IGLib]" align="right" width="48pt"
  style="float: right; max-width: 30%; width: 48pt; margin-left: 8pt;" /></a>

* [Project repository](https://github.com/ajgorhoe/IGLib.modules.IGLibCore)
* [IGLib.Core project within the repository](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/tree/main/src/IGLib.Transfer)
  * [This readme within repository](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/src/IGLib.Transfer/README_IGLib.Transfer.md)

This project contains **utilities transferred from the legacy IGLib Framework** *for use with the IGLib Core*. This projects **temporarily hosts these utilities** such that namespaces, dependencies and replacements can be sorted out, before moving the utilities to their final destination projects.

Thi project is **not contained** in the basic solution `IGLibCore.sln` or the complete solution for the new IGLib libraries 'IGLibCore_All.sln'. Use the `IGLibCore_Extended.sln` from the current repository in order to work with this project. It is **permitted that the project cannot be built**, even on the main branch. Therefore, the solution `IGLibCore_Extended.sln` cannot be included in CI/CD pipelines.

## Additional Information

### Transfering Utilities from IG.Num

Started on **December 2025**. This is to transfer utilities from the numerical usilities from the legacy IGLib Framework (mainly the namespace IG.Num). The utilities will be copied ratherr than moved. These utilities would need to be improved, updated and covered more with tests. However, IGLib Core needs these utiliities for development in 3D graphics and other areas. Therefore these utilities are transferred before they are refactored to a higher standard, and refactoring to deal withthe technical dept will be done later, as these libraries are used.

The intention is to move these utilities to IGLib.Numeric. Before moving:

* Transferred utilities should **build without errors** in the current project.
* **Most of the utilities** from the IG.Num namespace should be **transferred** (copied).
* Namespaces should be changed, such that the **legacy IGLib and** the **IGLib.Numeric** library **can be referenced simultaneously**.

### Testing

**This is to be changed**.

**Unit tests** and some lower level integration tests are provided in the [IGLib.Core.Tests](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/tree/main/tests/IGLib.Core.Tests) project.
