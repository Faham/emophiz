VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CSimpleRMS"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Attribute VB_Ext_KEY = "SavedWithClassBuilder6" ,"Yes"
Attribute VB_Ext_KEY = "Top_Level" ,"Yes"

'local variable(s) to hold property value(s)
Private mvarAveragingSampleCount As Integer 'local copy
Private mvarSampleCount As Integer 'local copy
Private mvarSQRTotal As Double
Public Sub Reset()
  mvarSQRTotal = 0
  mvarSampleCount = 0
End Sub


Public Property Let SampleCount(ByVal vData As Integer)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.SampleCount = 5
    mvarSampleCount = vData
End Property


Public Property Get SampleCount() As Integer
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.SampleCount
    SampleCount = mvarSampleCount
End Property



Public Property Let AveragingSampleCount(ByVal vData As Integer)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.AveragingSampleCount = 5
    mvarAveragingSampleCount = vData
End Property


Public Property Get AveragingSampleCount() As Integer
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.AveragingSampleCount
    AveragingSampleCount = mvarAveragingSampleCount
End Property



Public Function GetRMSValue() As Single
  Dim RMS As Double
  RMS = mvarSQRTotal / mvarSampleCount
  RMS = Sqr(RMS)
  GetRMSValue = RMS
End Function

Public Function AddSample(ByVal fSample As Single) As Boolean
  If mvarSampleCount >= mvarAveragingSampleCount Then
    Reset
  End If
  
  mvarSQRTotal = mvarSQRTotal + (fSample * fSample)
  mvarSampleCount = mvarSampleCount + 1
  If mvarSampleCount >= mvarAveragingSampleCount Then
    AddSample = True
  Else
    AddSample = False
  End If
  
End Function

