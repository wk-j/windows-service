SET QUSER=BCircle.{exe}
SET QNAME=BCircle.{exe}
SET QPASSWORD=123456
SET QAPP={exe}
SET QPATH=%cd%
SET QHOST=%COMPUTERNAME%

net user %QUSER% %QPASSWORD% /add
icacls "%QPATH%" /grant %QUSER%:(OI)(CI)WRX /t
sc create %QNAME% binPath= "%QPATH%\%QAPP% --urls=http://*:9000" obj= ".\LocalSystem" password= "%QPASSWORD%"
sc start %QNAME%