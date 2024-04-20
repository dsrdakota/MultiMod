@echo off
echo Running 7zip ...
%17z.exe a %1MultiMod.zip %1..\MultiMod\bin\Debug\MultiMod.dll %1..\MultiMod\Readme.md %1..\MultiMod\manifest.json %1..\MultiMod\icon.png
echo Done