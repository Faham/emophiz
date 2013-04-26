@echo off
del /S /Q vssver.scc
del /S /Q /F mssccprj.scc
del /S /Q *.aps
del /S /Q *.clw
del /S /Q *.opt
del /S /Q *.ncb
del /S /Q *.plg
del /S /Q *.ilk
del /S /Q *.pdb
del /S /Q *.cdb
del /S /Q TTLLiveCtrl.tl?
rmdir /S /Q build
