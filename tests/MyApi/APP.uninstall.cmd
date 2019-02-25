SET QUSER=BCircle.MyApi.exe
SET QNAME=BCircle.MyApi.exe

sc stop   %QNAME%
sc delete %QNAME%
net user  %QUSER% /delete