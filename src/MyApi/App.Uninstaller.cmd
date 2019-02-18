SET QAPP=MyApi.exe

sc delete %QAPP%
net user BCircle.%QAPP% /delete