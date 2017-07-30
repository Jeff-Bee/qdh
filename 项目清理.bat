@echo off
set nowPath=%cd%
cd \
cd %nowPath%


::for /r %nowPath% %%i in (*.vshost.*) do (del %%i)
for /r %nowPath% %%i in (*.pdb) do (del %%i)

echo 删除 bin 文件夹
for /r %nowPath% %%i in (bin\) do (@IF EXIST %%i RD /s /q %%i)
echo 删除 obj 文件夹
for /r %nowPath% %%i in (obj\) do (@IF EXIST %%i RD /s /q %%i)

echo 删除 Debug 文件夹
for /r %nowPath% %%i in (Debug\) do (@IF EXIST %%i RD /s /q %%i)

echo 删除 Release 文件夹
for /r %nowPath% %%i in (Release\) do (@IF EXIST %%i RD /s /q %%i)

echo OK
pause


::clear.bat最好放到要执行删除操作的目录(或上层目录)

::如果只要删除某个文件，可以将for /r %nowPath% %%i in (obj,bin) do (IF EXIST %%i RD /s /q %%i) 这一句去掉，
::然后将for /r %nowPath% %%i in (*.pdb,*.vshost.*) do (del %%i) 这一句中的(*.pdb,*.vshost.*)改成要删除的文件。