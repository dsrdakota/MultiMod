@echo off
echo Running 7zip ...
.\7z a MultiMod.zip ..\MultiMod\bin\Debug\MultiMod.dll ..\MultiMod\Readme.md ..\MultiMod\manifest.json ..\MultiMod\icon.png
echo Done