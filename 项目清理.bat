@echo off
set nowPath=%cd%
cd \
cd %nowPath%


::for /r %nowPath% %%i in (*.vshost.*) do (del %%i)
for /r %nowPath% %%i in (*.pdb) do (del %%i)

echo ɾ�� bin �ļ���
for /r %nowPath% %%i in (bin\) do (@IF EXIST %%i RD /s /q %%i)
echo ɾ�� obj �ļ���
for /r %nowPath% %%i in (obj\) do (@IF EXIST %%i RD /s /q %%i)

echo ɾ�� Debug �ļ���
for /r %nowPath% %%i in (Debug\) do (@IF EXIST %%i RD /s /q %%i)

echo ɾ�� Release �ļ���
for /r %nowPath% %%i in (Release\) do (@IF EXIST %%i RD /s /q %%i)

echo OK
pause


::clear.bat��÷ŵ�Ҫִ��ɾ��������Ŀ¼(���ϲ�Ŀ¼)

::���ֻҪɾ��ĳ���ļ������Խ�for /r %nowPath% %%i in (obj,bin) do (IF EXIST %%i RD /s /q %%i) ��һ��ȥ����
::Ȼ��for /r %nowPath% %%i in (*.pdb,*.vshost.*) do (del %%i) ��һ���е�(*.pdb,*.vshost.*)�ĳ�Ҫɾ�����ļ���