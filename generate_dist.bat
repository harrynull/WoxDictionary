@echo off

IF NOT EXIST dicts\ecdict.db echo Dictionary file missing. It may not work. Please download it!
mkdir dist
mkdir dist\config
mkdir dist\x64
mkdir dist\x86
mkdir dist\Images
xcopy dicts dist\dicts /i /s /y
copy Images\plugin.png dist\Images
copy bin\Release\Dictionary.dll dist
copy bin\Release\Newtonsoft.Json.dll dist
copy bin\Release\x64\SQLite.Interop.dll dist\x64\
copy bin\Release\x86\SQLite.Interop.dll dist\x86\
copy bin\Release\System.Data.SQLite.dll dist
copy plugin.json dist
pause