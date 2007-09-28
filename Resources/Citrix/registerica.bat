echo n | xcopy *.dll "%systemroot%\"
echo n | xcopy *.ocx "%systemroot%\"
regsvr32 /s "%systemroot%\Wfica.ocx"
pause