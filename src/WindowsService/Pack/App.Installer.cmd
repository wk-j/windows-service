SET QUSER=BCircle.{exe}
SET QPASSWORD=123456
SET QAPP={exe}
SET QPATH=%cd%
SET QHOST=%COMPUTERNAME%
SET QNAME=BCircle.%QAPP%

net user %QUSER% %QPASSWORD% /add
icacls "%QPATH%" /grant %QUSER%:(OI)(CI)WRX /t
sc create %QNAME% binPath="%QPATH%\%QAPP%" obj=".\LocalSystem" password="%QPASSWORD%"
sc start %QNAME%