VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CSimpleGraph"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Attribute VB_Ext_KEY = "SavedWithClassBuilder6" ,"Yes"
Attribute VB_Ext_KEY = "Top_Level" ,"Yes"
Const SAMPLE_BUFFER_SIZE = 600

Dim mvarSampleBuffer(SAMPLE_BUFFER_SIZE - 1) As Single
'local variable(s) to hold property value(s)
Private mvarGraphArea As PictureBox 'local copy
'local variable(s) to hold property value(s)
Private mvarScrollingGraph As Boolean 'local copy
Private xpos As Integer
Private lastX As Integer
Private lastY As Integer
Private SampleCount As Integer


Public Property Let ScrollingGraph(ByVal vData As Boolean)
'used when assigning a value to the property, on the left side of an assignment.
'Syntax: X.ScrollingGraph = 5
    mvarScrollingGraph = vData
End Property


Public Property Get ScrollingGraph() As Boolean
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.ScrollingGraph
    ScrollingGraph = mvarScrollingGraph
End Property

Public Property Let GraphArea(ByVal vData As PictureBox)
'used when assigning an Object to the property, on the left side of a Set statement.
'Syntax: Set x.GraphArea = Form1
    Set mvarGraphArea = vData
End Property


Public Property Get GraphArea() As PictureBox
'used when retrieving value of a property, on the right side of an assignment.
'Syntax: Debug.Print X.GraphArea
    Set GraphArea = mvarGraphArea
End Property

Public Sub AddSamples(ByRef f32Samples As Single, nCount As Integer)
  Dim i As Integer
  For i = 0 To nCount - 1
    'f32Samples(i) = f32Samples(i) * f32Samples(i)
  Next
End Sub
Public Sub AddSample(ByVal f32Sample As Single)
  Dim y As Single
  Dim fSample As Single
  Dim x As Integer
  Dim midy As Integer
  
  Dim bSinus As Boolean
  
  bSinus = False
  bSinus = True
  
  If bSinus = True Then
    f32Sample = f32Sample
  End If
  
      
  If mvarScrollingGraph = True Then
    'ScrollDataBuffer (1)
    mvarSampleBuffer(0) = f32Sample
    ScrollDataBuffer (1)
    'ScrollDataBuffer (1)
    'ScrollDataBuffer (1)
    
    SampleCount = SampleCount + 1
    If SampleCount >= 1 Then
      SampleCount = 0
      mvarGraphArea.Cls
      DrawGrid
      midy = mvarGraphArea.ScaleHeight / 2
      lastY = midy
      lastX = 0
      For i = 0 To mvarGraphArea.ScaleWidth
        fSample = mvarSampleBuffer(i)
        y = midy + (fSample / 64)
        mvarGraphArea.Line (lastX, lastY)-(i, y), 65280
        lastY = y
        lastX = i
      Next
    End If
  Else
      'For i = 0 To (liSamplesAvailable - 1)
      '  fSample = DataBuffer(i)
      '  If (RMS.AddSample(fSample)) Then
      '
      '  End If
      '
      '  y = PictureChannelTop.ScaleHeight / 2
      '  y = y + (fSample / 64)
      '
      '  PictureChannelTop.Line (ptLastX, ptLastY)-(xpos, y), 65280
      '  ptLastY = y
      '  ptLastX = xpos
      '
      '  xpos = xpos + 1
      '  If xpos > PictureChannelTop.ScaleWidth Then
      '    xpos = 0
      '    ptLastY = y
      '    ptLastX = 0
      '    PictureChannelTop.Cls
      '    DrawGridTop
      '  End If
      'Next
    
      midy = mvarGraphArea.ScaleHeight / 2
      y = midy + (f32Sample / 64)
      
      mvarGraphArea.Line (lastX, lastY)-(xpos, y), 65280
      lastY = y
      lastX = xpos
      
      xpos = xpos + 1
      If xpos > mvarGraphArea.ScaleWidth Then
        xpos = 0
        lastY = y
        lastX = 0
        mvarGraphArea.Cls
        DrawGrid
      End If
  End If
End Sub

Public Sub DrawGrid()
  Dim x As Integer
  Dim y As Integer
  Dim ysize As Integer
  Dim xsize As Integer
  ysize = mvarGraphArea.ScaleHeight
  xsize = mvarGraphArea.ScaleWidth
  For x = 0 To xsize Step 10
    mvarGraphArea.Line (x, 0)-(x, ysize), 32768
  Next
  For y = 0 To ysize Step 10
    mvarGraphArea.Line (0, y)-(xsize, y), 32768
  Next
End Sub

Public Sub ScrollDataBuffer(ByVal nCount As Integer)
  Dim i As Integer
  Dim d As Integer ' Destination Index
  Dim s As Integer ' Source Index
  Dim itemsToScroll As Integer
    
  itemsToScroll = SAMPLE_BUFFER_SIZE - nCount - 1
  
  s = SAMPLE_BUFFER_SIZE - 1 - nCount
  d = SAMPLE_BUFFER_SIZE - 1
      
  For i = 0 To itemsToScroll
    mvarSampleBuffer(d) = mvarSampleBuffer(s)
    s = s - 1
    d = d - 1
  Next
End Sub
