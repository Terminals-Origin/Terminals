echo n | xcopy VMKeyboardHook.dll "%systemroot%\"
echo n | xcopy VMRCActiveXClient.dll "%systemroot%\"
echo n | xcopy VMRCActiveXClient.inf "%systemroot%\"
echo n | xcopy AxInterop.VMRCClientControlLib.dll "%systemroot%\"

regsvr32 /s "%systemroot%\VMRCActiveXClient.dll"
regsvr32 /s "%systemroot%\AxInterop.VMRCClientControlLib.dll"
pause