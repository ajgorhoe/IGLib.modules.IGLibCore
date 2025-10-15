
Thic project just performs common initialization tasks for the projects that reference it, such as running scripts to clone source repositories of dependencies referenced via source code projects.


This initialization project is configured much like a normal C# library project, with some
specifics:

* Only .NET Standard 2.0 is targeted, to maximize compatibility with other projects
* Configuration prevents creating AssemblyInfo.cs and prevents output of a DLL
* A post-build event runs a script to perform initialization tasks
* The project will be referenced via ProjectReference in other projects
  * Usually with attribute ReferenceOutputAssembly=`false`, to avoid build dependencies
    * and PrivateAssets="All" to avoid propagating the reference to other projects

Example usage in a .csproj file of a referencing project (paths need to be updated):

<ItemGroup>
	<ProjectReference Include="..\0InitModules\InitModulesGraphics3D\InitModulesGraphics3D.csproj"
		ReferenceOutputAssembly="false" PrivateAssets="All"  />
</ItemGroup>

