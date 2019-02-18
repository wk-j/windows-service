SET QUSER=BCircle.MyApi.exe
SET QNAME=BCircle.MyApi.exe

sc delete %QUSER%
net user %QNAME% /delete