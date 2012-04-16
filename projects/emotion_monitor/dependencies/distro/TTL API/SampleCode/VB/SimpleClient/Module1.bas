Attribute VB_Name = "Module1"
Option Explicit

Type LARGE_INTEGER
  LowPart As Long
  highPart As Long
End Type

Public Declare Function QueryPerformanceCounter Lib "kernel32" (lpPerformanceCount As LARGE_INTEGER) As Boolean
Public Declare Function SetTimer Lib "user32" (ByVal HWnd As Long, ByVal nIDEvent As Long, ByVal uElapse As Long, ByVal lpTimerFunc As Long) As Long
Public Declare Function KillTimer Lib "user32" (ByVal HWnd As Long, ByVal nIDEvent As Long) As Long
Public Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Long)

Private TimerID As Long

Sub StartTimer(nMsInterval As Long)
    'TimerSeconds = 1 ' how often to "pop" the timer.
    TimerID = SetTimer(0&, 0&, nMsInterval&, AddressOf TimerProc)
End Sub

Sub EndTimer()
    On Error Resume Next
    KillTimer 0&, TimerID
End Sub

Sub TimerProc(ByVal HWnd As Long, ByVal uMsg As Long, ByVal nIDEvent As Long, ByVal dwTimer As Long)
    '
    ' The procedure is called by Windows. Put your
    ' timer-related code here.
    '
    FormMain.Process
    
End Sub



