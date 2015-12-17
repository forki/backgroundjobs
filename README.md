## Build

You can run `build.cmd` to run the web site. This will:

    * download all dependencies
	* compile the website
	* start suave.io webserver
	* open a browser on http://localhost:8083 
	* watch for changes in the solution and rebuild automatically

There is also a Visual Studio solution which can be started with <kbd>F5</kbd> and allows debugging.

## Service Installation

* Run the `build.cmd`
* Copy bin folder to target machine
* Run `WebSite.Service.exe install` (in admin mode)


In production systems you would want to add unit tests and automate the release process. 
[ProjectScaffold](https://github.com/fsprojects/ProjectScaffold) gives you exactly that.
 
## Used technology

- [FAKE](http://fsharp.github.io/FAKE/)
- [Paket](http://fsprojects.github.io/Paket/)
- [Suave](http://suave.io/)
- [Topself](https://github.com/haf/Topshelf.FSharp)