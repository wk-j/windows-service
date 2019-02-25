SET QUSER=BCircle.MyConcole.exe
SET QNAME=BCircle.MyConcole.exe

sc stop   %QNAME%
sc delete %QNAME%
net user  %QUSER% /delete