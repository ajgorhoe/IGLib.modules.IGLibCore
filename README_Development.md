
# IGLibCore (Investigative Generic Library) - Information for Developers

<img src="https://ajgorhoe.github.io/icons/IGLibIcon_256x256.png" alt="[IGLib]" align="right" width="48pt"
  style="float: right; max-width: 30%; width: 48pt; margin-left: 8pt;" />

This repository contains basic portions of the restructured ***Investigative Generic Library*** (***IGLib***). **IGLib** is currently undergoing some changes.

Contents:

* [To Do](#things-to-be-done)

## Things to be Done

## Unify Directory Structure of .NET IGLib Modules

IGLib Modules bases on .NET should be rearranged such that they have uniform structure.

Establish **Uniform Naming** of modules. **Distinguishing names from the legacy** IGLib Framework is **not a priority**. It is more important to have good and concise names.

**IGlibCore** project should **move to src/IGLib** fromm src/, such that more projects can be included in the src/ .

### Rearranging IGLib Legacy

Rearrange IGLib legacy such that the **IGLib project** can be migrated to the latest .NET (first .NET 8, then .NET 9 and later).

The IGLib should target frameworks compatible with IGLibCore.

### Moving Immature Things to the Sandbox

Utilities that are not mature enough should be moved to the IGLibSandbox module.

* Parser utilities
* CommandLine parsing
