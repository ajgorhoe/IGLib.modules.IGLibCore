
# IGLibCore (Investigative Generic Library)

<img src="https://ajgorhoe.github.io/icons/IGLibIcon_256x256.png" alt="[IGLib]" align="right" width="48pt"
  style="float: right; max-width: 30%; width: 48pt; margin-left: 8pt;" />

This repository contains basic portions of the restructured ***Investigative Generic Library*** (***IGLib***). **IGLib** is currently undergoing some changes.

Contents:

* [Links](#links) 
* [About the repository](#about-the-repository)
  * [Building and Running](#building-and-running)
  * [IGLib Restrcturing](#iglib-restructuring)

## Links

* [This repository - *IGLibCore*](https://github.com/ajgorhoe/IGLib.modules.IGLibCore) ([readme](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/master/README.md))
  * *[Container repository](https://github.com/ajgorhoe/iglibmodules)* ([readme](https://github.com/ajgorhoe/iglibmodules/blob/master/README.md)) used to clone this and other IGLib repositories, such that dependencies can be handled by a common Visual Studio repository
  * [Basic Legacy IGLib repository](https://github.com/ajgorhoe/IGLib.workspace.base.iglib) ([readme](https://github.com/ajgorhoe/IGLib.workspace.base.iglib-/blob/master/README.md))
* **[Development Wikis]()** (private repository)

# About the Repository

This repository is located at:
&nbsp;&nbsp;&nbsp; *<https://github.com/ajgorhoe/IGLib.modules.IGLibCore>*
To clone and work with this and other IGLib libraries (build, run, develop), you can the new *IGLib cotainer repository*:
&nbsp;&nbsp;&nbsp; <https://github.com/ajgorhoe/iglibmodules>

The legacy IGLib's base library is located at:
&nbsp;&nbsp;&nbsp; *<https://github.com/ajgorhoe/IGLib.workspace.base.iglib.git>*

*IGLib* consists of many other libraries and demo applications. You can find more details below and in the [links](#links).

## Building and Running

< To be added. >

### IGLib Container Repository

< To be added. >

## IGLib Restructuring

[Legacy IGLib libraries](https://github.com/ajgorhoe/IGLib.workspace.base.iglib/blob/master/README.md) contain lots of useful tools from different areas. Not all of these libraries were made publicly available, as many were developed within corporate environment. Due to complex dependency structure, some of these libraries were not ported to the new open source .NET (Core) when it was introduced by Microsoft in 2014. Some libraries could be ported to .NET but were slowly abandoned becaue the new .NET exosystem changed significantly and some of the crucial dependencies were not actively developed or supported any more.

Because of this, many libraries of the *legacy IGLib Framework* were phased out. Some others are not actively developed, but are used in existing applications. Only the [IGLib](https://github.com/ajgorhoe/IGLib.workspace.base.iglib) base library is still used in new applications, but it is also not developed, except for the necessary fixes needed in the applications that use it. The newer libraries were started after the new .NET Core (later renamed to .NET) became stable.

The figure below shows dependencies betweem some IGLib modules, some legacy ones as well as the newer modules developed on the newer .NET Core / .NET (click the image to view searchable version full screen, or [click here](https://ajgorhoe.github.io/IGLibFramework/images/IGLib/IGLibDependencyGraph.jpg) to open a bitmpap version):

<a href="https://ajgorhoe.github.io/IGLibFramework/images/IGLib/IGLibDependencyGraph.svg"><img src="./doc/images/IGLibDependencyGraph.svg" width="max(80%), 800px"></img></a>

### Legacy *IGLib* Libraries

Bulding legacy IGLib libraries is a bit more tricky due to their complex tependencies. See the [IGLib repo's readme](https://github.com/ajgorhoe/IGLib.workspace.base.iglib/blob/master/README.md) file to learn more about this.



