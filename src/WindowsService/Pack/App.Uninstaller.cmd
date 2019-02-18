SET QAPP={exe}

sc delete %QAPP%
net user BCircle.%QAPP% /delete