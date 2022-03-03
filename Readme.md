## Compiling Source

~~~bash
$ dotnet build 
~~~

## Running unit tests

~~~bash
$ cd UnifiedProductCatalog.ImportTool.UnitTests 
$ dotnet test
~~~

## Running End2End tests

~~~bash
$ cd UnifiedProductCatalog.ImportTool.End2EndTests 
$ dotnet test
~~~

## Publish to be used as executable for win10-x64

~~~bash
$ cd UnifiedProductCatalog.ImportTool 
$ dotnet publish -c Release -r win10-x64 -o <desired output directory>
~~~
