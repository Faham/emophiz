VERSION 5.00
Object = "{EA85D9B2-B3F3-43F4-ADF5-256669A83D06}#2.1#0"; "TTLLiveCtrl.dll"
Begin VB.Form FormMain 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Simple Visual Basic Client"
   ClientHeight    =   8475
   ClientLeft      =   30
   ClientTop       =   255
   ClientWidth     =   8835
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   ScaleHeight     =   890.148
   ScaleMode       =   0  'User
   ScaleWidth      =   698.8
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame Frame2 
      Caption         =   "Data Flow Controls"
      Height          =   1935
      Left            =   120
      TabIndex        =   27
      Top             =   960
      Width           =   4335
      Begin VB.CommandButton CommandStartData 
         Caption         =   "Start Data"
         Height          =   495
         Left            =   120
         TabIndex        =   29
         Top             =   360
         Width           =   1935
      End
      Begin VB.CommandButton CommandStopData 
         BackColor       =   &H000000FF&
         Caption         =   "Stop Data"
         Height          =   495
         Left            =   2280
         MaskColor       =   &H000000FF&
         TabIndex        =   28
         Top             =   360
         UseMaskColor    =   -1  'True
         Width           =   1935
      End
   End
   Begin VB.Frame FrameEncoderInfo 
      Caption         =   "Encoder/Connection Informations"
      Height          =   1935
      Left            =   4560
      TabIndex        =   18
      Top             =   960
      Width           =   4215
      Begin VB.Label Label6 
         Alignment       =   1  'Right Justify
         BorderStyle     =   1  'Fixed Single
         Caption         =   "Encoder Type"
         Height          =   315
         Left            =   120
         TabIndex        =   26
         Top             =   720
         Width           =   2055
      End
      Begin VB.Label LabelEncoderType 
         Alignment       =   2  'Center
         BorderStyle     =   1  'Fixed Single
         Caption         =   "---"
         Height          =   315
         Left            =   2280
         TabIndex        =   25
         Top             =   720
         Width           =   1815
      End
      Begin VB.Label Label9 
         Alignment       =   1  'Right Justify
         BorderStyle     =   1  'Fixed Single
         Caption         =   "Protocol Type"
         Height          =   315
         Left            =   120
         TabIndex        =   24
         Top             =   1080
         Width           =   2055
      End
      Begin VB.Label LabelProtocolType 
         Alignment       =   2  'Center
         BorderStyle     =   1  'Fixed Single
         Caption         =   "---"
         Height          =   315
         Left            =   2280
         TabIndex        =   23
         Top             =   1080
         Width           =   1815
      End
      Begin VB.Label LabelEncoderStatus 
         Alignment       =   2  'Center
         BorderStyle     =   1  'Fixed Single
         Caption         =   "---"
         Height          =   315
         Left            =   2280
         TabIndex        =   22
         Top             =   360
         Width           =   1815
      End
      Begin VB.Label Label10 
         Alignment       =   1  'Right Justify
         BorderStyle     =   1  'Fixed Single
         Caption         =   "Encoder Status"
         Height          =   315
         Left            =   120
         TabIndex        =   21
         Top             =   360
         Width           =   2055
      End
      Begin VB.Label LabelDataErrorCount 
         Alignment       =   2  'Center
         BorderStyle     =   1  'Fixed Single
         Caption         =   "---"
         Height          =   315
         Left            =   2280
         TabIndex        =   20
         Top             =   1440
         Width           =   1815
      End
      Begin VB.Label Label11 
         Alignment       =   1  'Right Justify
         BorderStyle     =   1  'Fixed Single
         Caption         =   "Data Error Count"
         Height          =   315
         Left            =   120
         TabIndex        =   19
         Top             =   1440
         Width           =   2055
      End
   End
   Begin VB.Frame FrameChannel0 
      Caption         =   "First Channel"
      Height          =   5415
      Left            =   120
      TabIndex        =   5
      Top             =   3000
      Width           =   8655
      Begin VB.PictureBox PictureChannelBottom 
         Height          =   1812
         Left            =   120
         Picture         =   "Form1.frx":0000
         ScaleHeight     =   117
         ScaleMode       =   3  'Pixel
         ScaleWidth      =   557
         TabIndex        =   11
         Top             =   3480
         Width           =   8412
      End
      Begin VB.ComboBox ComboDisplayedPhysicalChannel 
         Height          =   315
         Left            =   2640
         Style           =   2  'Dropdown List
         TabIndex        =   9
         Top             =   240
         Width           =   1815
      End
      Begin VB.ComboBox ComboDisplayedUnitType 
         Height          =   315
         Left            =   2640
         Style           =   2  'Dropdown List
         TabIndex        =   7
         Top             =   600
         Width           =   1815
      End
      Begin VB.PictureBox PictureChannelTop 
         ForeColor       =   &H0000C000&
         Height          =   1812
         Left            =   120
         Picture         =   "Form1.frx":19BB0
         ScaleHeight     =   117
         ScaleMode       =   3  'Pixel
         ScaleWidth      =   557
         TabIndex        =   6
         Top             =   1320
         Width           =   8412
      End
      Begin VB.Label Label8 
         Alignment       =   1  'Right Justify
         BorderStyle     =   1  'Fixed Single
         Caption         =   "Samples Received"
         Height          =   315
         Left            =   4560
         TabIndex        =   17
         Top             =   600
         Width           =   2055
      End
      Begin VB.Label LabelSamplesReceived 
         Alignment       =   2  'Center
         BorderStyle     =   1  'Fixed Single
         Height          =   315
         Left            =   6720
         TabIndex        =   16
         Top             =   600
         Width           =   1815
      End
      Begin VB.Label LabelSensorID 
         Alignment       =   2  'Center
         BorderStyle     =   1  'Fixed Single
         Caption         =   "---"
         Height          =   315
         Left            =   6720
         TabIndex        =   15
         Top             =   240
         Width           =   1815
      End
      Begin VB.Label Label5 
         Alignment       =   1  'Right Justify
         BorderStyle     =   1  'Fixed Single
         Caption         =   "Reported Sensor ID"
         Height          =   315
         Left            =   4560
         TabIndex        =   14
         Top             =   240
         Width           =   2055
      End
      Begin VB.Label Label4 
         Caption         =   "Channel Unit Display"
         Height          =   195
         Left            =   120
         TabIndex        =   13
         Top             =   1080
         Width           =   1815
      End
      Begin VB.Label Label2 
         Caption         =   "Channel RMS Display"
         Height          =   255
         Left            =   120
         TabIndex        =   12
         Top             =   3240
         Width           =   8415
      End
      Begin VB.Label Label3 
         Alignment       =   1  'Right Justify
         BorderStyle     =   1  'Fixed Single
         Caption         =   "Displayed Unit Type"
         Height          =   315
         Left            =   120
         TabIndex        =   10
         Top             =   600
         Width           =   2415
      End
      Begin VB.Label Label1 
         Alignment       =   1  'Right Justify
         BorderStyle     =   1  'Fixed Single
         Caption         =   "Displayed Physical Channel"
         Height          =   315
         Left            =   120
         TabIndex        =   8
         Top             =   240
         Width           =   2415
      End
   End
   Begin VB.Frame Frame1 
      Caption         =   "Connection"
      Height          =   735
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   8655
      Begin VB.CheckBox CheckAutoDetect 
         Caption         =   "AutoDetect"
         Height          =   375
         Left            =   120
         TabIndex        =   4
         Top             =   240
         Width           =   1455
      End
      Begin VB.CommandButton CommandConnect 
         Caption         =   "&Connect"
         Height          =   372
         Left            =   5760
         TabIndex        =   3
         Top             =   240
         Width           =   1215
      End
      Begin VB.CommandButton CommandDisconnect 
         Caption         =   "&Disconnect"
         Height          =   372
         Left            =   7080
         TabIndex        =   2
         Top             =   240
         Width           =   1215
      End
      Begin VB.TextBox TextConnectionName 
         Height          =   372
         Left            =   1560
         TabIndex        =   1
         Text            =   "USB:0"
         Top             =   240
         Width           =   4095
      End
   End
   Begin TTLLiveCtrlLibCtl.TTLLive TTLLive 
      Left            =   8040
      OleObjectBlob   =   "Form1.frx":33760
      Top             =   360
   End
End
Attribute VB_Name = "FormMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Dim liDisplayedChannelHND As Long
Dim bRunning As Boolean
Dim DataBuffer As Variant
Dim liSamplesReceived As Long
Dim liFirstEncoderHND As Long
Dim liDataErrorCOunt As Long
Dim xpos As Integer
Dim ptLastY As Integer
Dim ptLastX As Integer

Dim RMS As CSimpleRMS
Dim Graph As CSimpleGraph
Dim RMS_Graph As CSimpleGraph

Const UT_DEFAULTS = 0
Const UT_COUNTS = 1
Const UT_SENS_VOLTS = 2
Const UT_SENS_MVOLTS = 3
Const UT_SENS_UVOLTS = 4

Private Sub ComboDisplayedPhysicalChannel_Click()
On Error GoTo Exception
  Dim liSensorID As Long
  
  liDisplayedChannelHND = ComboDisplayedPhysicalChannel.ListIndex
  liSensorID = TTLLive.SensorID(liDisplayedChannelHND)
  LabelSensorID.Caption = ToSensorTypeString(liSensorID)
  
  SetActiveChannel (liDisplayedChannelHND)
  
  Exit Sub
    
Exception:
  MsgBox Err.Description
End Sub

Private Sub ComboDisplayedUnitType_Click()
On Error GoTo Exception
  Select Case ComboDisplayedUnitType.ListIndex
    Case UT_COUNTS
      TTLLive.UnitType(liDisplayedChannelHND) = TTLAPI_UT_COUNT
      
    Case UT_DEFAULTS
      TTLLive.SensorType(liDisplayedChannelHND) = TTLLive.SensorID(liDisplayedChannelHND)
      
    Case UT_SENS_MVOLTS
      TTLLive.UnitType(liDisplayedChannelHND) = TTLAPI_UT_SENSMVOLT
      
    Case UT_SENS_UVOLTS
      TTLLive.UnitType(liDisplayedChannelHND) = TTLAPI_UT_SENSUVOLT
      
    Case UT_SENS_VOLTS
      TTLLive.UnitType(liDisplayedChannelHND) = TTLAPI_UT_SENSVOLT
  
  End Select
  
  Exit Sub
    
Exception:
  MsgBox Err.Description

End Sub

Private Sub CommandAutoSetupChannels_Click()
On Error GoTo Exception
  TTLLive1.AutoSetupChannels
  Exit Sub
    
Exception:
  MsgBox Err.Description
end1:

End Sub


Private Sub SetActiveChannel(liActiveChannelHND As Long)
On Error GoTo Exception
  Dim liChannelHND As Long
  
  liChannelHND = TTLLive.GetFirstChannelHND
  While liChannelHND >= 0
    TTLLive.ChannelActive(liChannelHND) = 0
    liChannelHND = TTLLive.GetNextChannelHND
  Wend
  
  TTLLive.ChannelActive(liActiveChannelHND) = 1
  
  Exit Sub

Exception:
  MsgBox Err.Description
End Sub


Private Sub CommandConnect_Click()
On Error GoTo Exception
  Dim liEncoderHND As Long
  Dim liChannelHND As Long
  Dim s As String
  Dim k As Integer
  
  MousePointer = 11
  
  liDataErrorCOunt = 0
  
  If CheckAutoDetect.Value = 1 Then
    TTLLive.OpenConnections TTLAPI_OCCMD_AUTODETECT, 1000, 0, 0
  Else
    liEncoderHND = TTLLive.OpenConnection(TextConnectionName.Text, 2000)
  End If
  
  If TTLLive.EncoderCount > 0 Then
    TTLLive.AutoSetupChannels
    liChannelHND = TTLLive.GetFirstChannelHND
    While liChannelHND >= 0
      k = Asc("A") + TTLLive.ChannelPhysicalIndex(liChannelHND)
      's = chr$(Int('A')+TTLLive.ChannelPhysicalIndex(liChannelHND))
      s = "Channel " + Chr$(k)
      ComboDisplayedPhysicalChannel.AddItem (s)
      TTLLive.ChannelActive(liChannelHND) = 0
      TTLLive.ForceSensor(liChannelHND) = 1
      liChannelHND = TTLLive.GetNextChannelHND
    Wend
    
    ComboDisplayedPhysicalChannel.ListIndex = TTLLive.GetFirstChannelHND
    ComboDisplayedUnitType.ListIndex = UT_COUNTS
    CommandStartData.Enabled = True
    CommandStopData.Enabled = True
    liFirstEncoderHND = TTLLive.GetFirstEncoderHND
    LabelProtocolType.Caption = TTLLive.ProtocolName(liFirstEncoderHND)
    LabelEncoderType.Caption = TTLLive.EncoderModelName(liFirstEncoderHND)
    TTLLive_EncoderStateChange liFirstEncoderHND, TTLLive.EncoderState(liFirstEncoderHND)
        
  Else
    MousePointer = 0
    MsgBox "No encoder could be found."
    LabelProtocolType.Caption = "---"
    LabelEncoderType.Caption = "---"
  End If
  MousePointer = 0
  Exit Sub

Exception:
  MousePointer = 0
  MsgBox Err.Description
End Sub

Private Sub CommandDisconnect_Click()
On Error GoTo Exception
  liFirstEncoderHND = -1
  CommandStopData_Click
  TTLLive.CloseConnections
  LabelProtocolType.Caption = "---"
  LabelEncoderType.Caption = "---"
  LabelEncoderStatus.Caption = "---"
  
  Exit Sub
    
Exception:
  MsgBox Err.Description

End Sub

Private Sub CommandStartData_Click()
On Error GoTo Exception
  TTLLive.StartChannels
  StartTimer (50)
  bRunning = True
  Exit Sub
    
Exception:
  MsgBox Err.Description

End Sub

Private Sub CommandStopData_Click()
On Error GoTo Exception
  bRunning = False
  EndTimer
  TTLLive.StopChannels
  Exit Sub
    
Exception:
  MsgBox Err.Description

End Sub

Private Sub Form_Load()
  ComboDisplayedUnitType.AddItem "Defaults", UT_DEFAULTS
  ComboDisplayedUnitType.AddItem "Counts", UT_COUNTS
  ComboDisplayedUnitType.AddItem "Sensor Volts", UT_SENS_VOLTS
  ComboDisplayedUnitType.AddItem "Sensor mVolts", UT_SENS_MVOLTS
  ComboDisplayedUnitType.AddItem "Sensor uVolts", UT_SENS_UVOLTS
  
  CommandStartData.Enabled = False
  CommandStopData.Enabled = False
  bRunning = False
  
  Set RMS = New CSimpleRMS
  Set Graph = New CSimpleGraph
  Set RMS_Graph = New CSimpleGraph
      
  RMS.AveragingSampleCount = 8
  RMS.Reset
  
  Graph.ScrollingGraph = False
  Graph.GraphArea = PictureChannelTop
  
  RMS_Graph.ScrollingGraph = True
  RMS_Graph.GraphArea = PictureChannelBottom
  
  FormMain.Caption = FormMain.Caption + "( Using TTLLive " + GetTTLLiveVersionStr(TTLLive) + ")"
    
  ' Simply activate all of them.
  TTLLive.NotificationMask = -1
    
End Sub

Public Sub Process()
On Error GoTo Exception
  Dim i As Integer
  Dim y As Integer
  Dim fSample As Single
    
  If bRunning = True Then
    liSamplesAvailable = TTLLive.SamplesAvailable(liDisplayedChannelHND)
    DataBuffer = TTLLive.ReadChannelDataVT(liDisplayedChannelHND, liSamplesAvailable)
    If IsArray(DataBuffer) Then
      liSamplesAvailable = UBound(DataBuffer)
      liSamplesReceived = liSamplesReceived + liSamplesAvailable
      LabelSamplesReceived.Caption = Str(liSamplesReceived)
      
      For i = 0 To (liSamplesAvailable - 1)
        fSample = DataBuffer(i)
        
        Graph.AddSample (fSample)
        
        If (RMS.AddSample(fSample)) Then
          RMS_Graph.AddSample (RMS.GetRMSValue)
        End If

      Next
    End If
    DataBuffer = Null
  End If
  
  Exit Sub
    
Exception:
  CommandStopData_Click
  MsgBox Err.Description
End Sub

Private Function ToSensorTypeString(liSensorType As Long) As String

  Select Case liSensorType
    Case TTLAPI_SENS_TYPE_EEG
      ToSensorTypeString = "EEG"
    Case TTLAPI_SENS_TYPE_EEG_Z
      ToSensorTypeString = "EEG-Z"
    Case TTLAPI_SENS_TYPE_ENCODER_OFFLINE
      ToSensorTypeString = "Offline"
    Case TTLAPI_SENS_TYPE_FORCE
      ToSensorTypeString = "Force"
    Case TTLAPI_SENS_TYPE_GONIOMETER
      ToSensorTypeString = "Goniometer"
    Case TTLAPI_SENS_TYPE_HR_BVP
      ToSensorTypeString = "HR BVP"
    Case TTLAPI_SENS_TYPE_MYOSCAN
      ToSensorTypeString = "Myo-Scan"
    Case TTLAPI_SENS_TYPE_MYOSCAN_PRO_1600W
      ToSensorTypeString = "Myo-Scan PRO (1600W)"
    Case TTLAPI_SENS_TYPE_MYOSCAN_PRO_400
      ToSensorTypeString = "Myo-Scan PRO (400W)"
    Case TTLAPI_SENS_TYPE_MYOSCAN_Z
      ToSensorTypeString = "Myo-Scan-Z"
    Case TTLAPI_SENS_TYPE_MYOSCAN_Z_HR
      ToSensorTypeString = "Myo-Scan-Z HR"
    Case TTLAPI_SENS_TYPE_MYOTRAC_INTERNAL
      ToSensorTypeString = "Myotrac Internal"
    Case TTLAPI_SENS_TYPE_RESPIRATION
      ToSensorTypeString = "Respiration"
    Case TTLAPI_SENS_TYPE_SKIN_CONDUCTANCE
      ToSensorTypeString = "Skin Conductance"
    Case TTLAPI_SENS_TYPE_TEMPERATURE
      ToSensorTypeString = "Temperature"
    Case TTLAPI_SENS_TYPE_UNKNOWN
      ToSensorTypeString = "Unknown"
    Case TTLAPI_SENS_TYPE_UNCONNECTED
      ToSensorTypeString = "Unconnected"
    Case TTLAPI_SENS_TYPE_VOLT_ISOLATOR
      ToSensorTypeString = "Voltage Isolator"
  End Select
End Function

Private Sub Form_Terminate()
  bRunning = False
  EndTimer
  TTLLive.StopChannels
  TTLLive.CloseConnections
  
  ' Dirty patch upon exiting application
  Sleep 500
  
  Set RMS = Nothing
  Set RMS_Graph = Nothing
  Set Graph = Nothing
End Sub

Private Sub TTLLive_DataError(ByVal liEncoderHND As Long, ByVal liReserved As Long)
  If liEncoderHND = liFirstEncoderHND Then
    liDataErrorCOunt = liDataErrorCOunt + 1
    LabelDataErrorCount.Caption = Str(liDataErrorCOunt)
  End If
  
End Sub

Private Sub TTLLive_EncoderStateChange(ByVal liEncoderHND As Long, ByVal liNewState As Long)
  If liEncoderHND = TTLLive.GetFirstEncoderHND Then
    Select Case liNewState
      Case TTLAPI_ENC_ST_DETECTING
        LabelEncoderStatus.Caption = "Detecting..."
        
      Case TTLAPI_ENC_ST_OFFLINE
        LabelEncoderStatus.Caption = "Offline"
        
      Case TTLAPI_ENC_ST_ONLINE
        LabelEncoderStatus.Caption = "Online"
        
    End Select
  End If
  
End Sub

Private Function GetTTLLiveVersionStr(ByRef aTTLLive As TTLLive) As String
  Dim liVersion As Long
  Dim Build As Long, Minor As Long, Major As Long
  
  If aTTLLive Is Nothing Then
    GetTTLLiveVersionStr = "---"
  Else
    liVersion = aTTLLive.Version
        
    Build = liVersion And ((2 ^ 16) - 1)
    Minor = liVersion / (2 ^ 16)
    Minor = Minor And 255
    Major = liVersion / (2 ^ 24)
    Major = Major And 255
        
    GetTTLLiveVersionStr = Str(Major) + " ." + Str(Minor) + " ." + Str(Build)

  End If
        
End Function


