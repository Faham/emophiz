object FormMain: TFormMain
  Left = 401
  Top = 122
  Width = 576
  Height = 274
  Caption = 'Simple Borland C++ Builder 6 TTLLive Client'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  PixelsPerInch = 96
  TextHeight = 13
  object Bevel1: TBevel
    Left = 200
    Top = 16
    Width = 241
    Height = 177
  end
  object Image1: TImage
    Left = 208
    Top = 24
    Width = 225
    Height = 161
  end
  object TTLLive1: TTTLLive
    Left = 469
    Top = 32
    Width = 32
    Height = 32
    ControlData = {00030000}
  end
  object StatusBarMain: TStatusBar
    Left = 0
    Top = 214
    Width = 568
    Height = 26
    Panels = <
      item
        Width = 150
      end
      item
        Width = 150
      end
      item
        Width = 50
      end>
    SimplePanel = False
  end
  object GroupBox1: TGroupBox
    Left = 8
    Top = 8
    Width = 185
    Height = 145
    Caption = 'Connection'
    TabOrder = 2
    object Button1: TButton
      Left = 24
      Top = 96
      Width = 145
      Height = 33
      Caption = 'OpenConnections'
      TabOrder = 0
      OnClick = Button1Click
    end
    object CheckBoxAutodetect: TCheckBox
      Left = 24
      Top = 56
      Width = 97
      Height = 17
      Caption = 'Autodetect'
      Checked = True
      State = cbChecked
      TabOrder = 1
    end
    object CheckBoxKeepExisting: TCheckBox
      Left = 24
      Top = 72
      Width = 97
      Height = 17
      Caption = 'Keep Existing'
      Checked = True
      State = cbChecked
      TabOrder = 2
    end
    object CheckBoxUSB0: TCheckBox
      Left = 24
      Top = 24
      Width = 97
      Height = 17
      Caption = 'USB:0'
      Checked = True
      State = cbChecked
      TabOrder = 3
    end
    object CheckBoxUSB1: TCheckBox
      Left = 24
      Top = 40
      Width = 97
      Height = 17
      Caption = 'USB:1'
      Checked = True
      State = cbChecked
      TabOrder = 4
    end
  end
end
