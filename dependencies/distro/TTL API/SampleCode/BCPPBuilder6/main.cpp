//---------------------------------------------------------------------------

#include <vcl.h>
#include <stdio.h>
#pragma hdrstop

#include "main.h"
//---------------------------------------------------------------------------
#pragma package(smart_init)
#pragma link "TTLLiveCtrlLib_OCX"
#pragma resource "*.dfm"
TFormMain *FormMain;
//---------------------------------------------------------------------------
__fastcall TFormMain::TFormMain(TComponent* Owner)
        : TForm(Owner)
{
}
//---------------------------------------------------------------------------
void __fastcall TFormMain::Button1Click(TObject *Sender)
{
        long command = 0;

        if( FormMain->CheckBoxUSB0->Checked )
                command+=TTLAPI_OCCMD_TTUSB0;
        if( FormMain->CheckBoxUSB1->Checked )
                command+=TTLAPI_OCCMD_TTUSB1;
        if( FormMain->CheckBoxAutodetect->Checked )
                command+=TTLAPI_OCCMD_AUTODETECT;
        if( FormMain->CheckBoxKeepExisting->Checked )
                command+=TTLAPI_OCCMD_KEEPEXISTING;

        TTLLive1->ControlInterface->OpenConnections(command,2000,NULL,NULL);
        //FormMain->StatusBarMain->Panels->Items.

        char s[1024]={0};
        sprintf(s,"Encoder count = %d",TTLLive1->ControlInterface->EncoderCount);
        FormMain->StatusBarMain->SimpleText = s;
}
//---------------------------------------------------------------------------
