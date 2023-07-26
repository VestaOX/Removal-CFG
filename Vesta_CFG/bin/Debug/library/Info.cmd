@echo off
files\ideviceinfo.exe | files\grep.exe -w BasebandStatus | files\awk.exe '{printf $NF}'