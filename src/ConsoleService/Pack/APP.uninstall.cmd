rem SET QUSER=BCircle.{exe}

SET QNAME=BCircle.{exe}

sc stop   %QNAME%
sc delete %QNAME%

rem net user  %QUSER% /delete