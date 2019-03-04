rem SET QUSER=BCircle.MyConsole.exe
SET QNAME=BCircle.MyConsole.exe
SET QPASSWORD=
SET QAPP=MyConsole.exe
SET QPATH=%cd%
SET QHOST=%COMPUTERNAME%

rem net user %QUSER% %QPASSWORD% /add
rem icacls "%QPATH%" /grant %QUSER%:(OI)(CI)WRX /t
sc create %QNAME% binPath= "%QPATH%\%QAPP% --urls=http://*:9000" obj= ".\LocalSystem" password= "%QPASSWORD%"
sc start %QNAME%