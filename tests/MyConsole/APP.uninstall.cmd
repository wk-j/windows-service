rem SET QUSER=BCircle.MyConsole.exe
SET QNAME=BCircle.MyConsole.exe

sc stop   %QNAME%
sc delete %QNAME%
rem net user  %QUSER% /delete