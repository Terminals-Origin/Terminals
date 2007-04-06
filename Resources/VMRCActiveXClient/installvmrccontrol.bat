echo n | xcopy VMKeyboardHook.dll "%systemroot%\"
echo n | xcopy VMRCActiveXClient.dll "%systemroot%\"
echo n | xcopy VMRCActiveXClient.inf "%systemroot%\"
regsvr32 /s "%systemroot%\VMRCActiveXClient.dll"
pause