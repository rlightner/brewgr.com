What is Brewgr?
======
Brewgr is a free home brewing web application located at http://brewgr.com.  It offers homebrewers numerous features for creating recipes, tracking brew day, and collaborating with others.  Brewgr is now open source with the hopes that the community will contribute features and fixes so it can continue to grow and be a valuable tool for homebrewers.

Getting Started
----------------------------
If you're interested in contributing to Brewgr (and we hope you do), please pick something that interests you from the [Issue List](https://github.com/ctorx/brewgr/issues) and get started.  We'll try our best to merge any pull-requests that add value, but please, before embarking on a major new feature, please post it as an issue to get discussion going first.

Development Environment Setup
========

Technologies:
----------------------------
* Microsoft ASP.NET MVC - http://www.asp.net/mvc
* Microsoft Entity Framework - http://www.asp.net/entity-framework
* AutoMapper - https://github.com/AutoMapper/AutoMapper
* Ninject - https://github.com/ninject/ninject
* Exceptional - https://github.com/NickCraver/StackExchange.Exceptional
* Fluent Validation - https://github.com/JeremySkinner/FluentValidation
* Image Resizer - https://github.com/imazen/resizer
* jQuery - https://github.com/jquery/jquery
* Various jQuery plugins

Database Setup
--------------------------
1. Fork and clone the repository on your machine
2. Navigate to the "Setup\Database" folder in the repository root and follow the directions in the  [README](Setup/Database/README.md).

Connection String Setup
----------------------------
1. Determine what your valid connection string is based upon your database setup.  If you need help with the connection string, check out http://www.connectionstrings.com/sql-server/.
2. Create a system Enviornment Variable named "Brewgr_ConnectionString" and set the value to the connection string determined in step 1 above. 
3. You may need to reboot your machine for Visual Studio and IIS Express to recognize the variable.

Editor and Code Setup
----------------------------
1. We highly recommend you use Visual Studio 2015 for development.  It makes getting started much easier and we really like it.
2. Create a HOST file entry on your development machine that points the host "dev.brewgr.com" to "127.0.0.1".  This allows you to run Brewgr locally and maintain the correct "document.domain" which is used for cookies and such.  IIS Express will also use dev.brewgr.com, but only if you're using Visual Studio 2015, which uses individual applicationhost.config files per solution in the ".vs" folder.  If you can't use VS 2015, you can modify the applicationhost.config file that gets created in the "Documents\IISExpress\config" folder for your computer user account.
3. Launch Visual Studio (with elevated access -- necessary for the non-localhost URL)
4. Open the [Brewgr.Web.sln](Brewgr.Web.sln) file.  You'll need to make sure that you have Nuget installed and pull down the dependency packages.  NOTE: Brewgr uses a packages folder that is as the same level of as the solution folder, as noted in the Nuget.config file.
5. If you can build the solution sucessfully, you should be good to go.
 
Please let us know if you encounter setup errors and we'll help you out.  Cheers!


---------
Brewgr is Copyright &copy; 2011-2015 [Matthew Marksbury](https://github.com/ctorx) and [Jason Zimmerman](https://github.com/SingleSpeed) and other contributors under the [GNU General Public License v3.0](LICENSE.txt).

