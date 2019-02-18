SET QUSER=BCircle.MyApi.exe
SET QNAME=BCircle.MyApi.exe
SET QPASSWORD=123456
SET QAPP=MyApi.exe
SET QPATH=%cd%
SET QHOST=%COMPUTERNAME%

net user %QUSER% %QPASSWORD% /add
icacls "%QPATH%" /grant %QUSER%:(OI)(CI)WRX /t
sc create %QNAME% binPath="%QPATH%\%QAPP%" obj=".\LocalSystem" password="%QPASSWORD%"
sc start %QNAME%