@SET CONFIGURATION=Release
@SET NETFX_PATH=C:\Windows\Microsoft.NET\Framework\v4.0.30319
@SET VS_PATH=C:\Program Files (x86)\Microsoft Visual Studio 11.0

@ECHO Compiling solution
%NETFX_PATH%\MSBuild.exe "%~dp0..\src\StoreAndForward.sln" /property:Configuration=%CONFIGURATION%
@SET EXITCODE=%ERRORLEVEL%
@IF %ERRORLEVEL% NEQ 0 GOTO error

@ECHO Running unit tests
"%VS_PATH%\Common7\IDE\CommonExtensions\Microsoft\TestWindow\VSTest.Console.exe" "%~dp0..\src\Tests\Bin\x86\%CONFIGURATION%\Tests_%CONFIGURATION%_x86.xap" /Logger:trx
@SET EXITCODE=%ERRORLEVEL%
@IF %ERRORLEVEL% NEQ 0 GOTO error

:error
@ECHO build script return code: %EXITCODE%
@EXIT /B %EXITCODE%

:exit
@ECHO Coverage requirements met      
@ECHO build succeeded
@EXIT /B 0