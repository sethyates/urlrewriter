README.TXT for Unit Test Assembly
=================================

!IMPORTANT NOTE!

If you are using Visual Studio 2010 and the solution file Intelligencia.UrlRewriter.vs2010.sln,
to get the unit test running you need to add the following app setting to your local
devenv.exe.config file:

	<appSettings>
		<add key="TestProjectRetargetTo35Allowed" value="true" />
	</appSettings>

The config file should be in the same directory as the Visual Studio executable file (devenv.exe),
usually at the following location:

	C:\Program Files\Microsoft Visual Studio 10.0\Common7\IDE

More info: http://go.microsoft.com/fwlink/?LinkId=201405


Please note that this file is ALSO used to test the "exists" conditition.