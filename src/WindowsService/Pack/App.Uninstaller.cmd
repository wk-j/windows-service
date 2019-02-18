SET QUSER=BCircle.{exe}
SET QNAME=BCircle.{exe}

sc stop   %QNAME%
sc delete %QNAME%
net user  %QUSER% /delete