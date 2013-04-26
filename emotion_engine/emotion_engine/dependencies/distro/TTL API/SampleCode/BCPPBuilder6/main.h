//---------------------------------------------------------------------------

#ifndef mainH
#define mainH
//---------------------------------------------------------------------------
#include <Classes.hpp>
#include <Controls.hpp>
#include <StdCtrls.hpp>
#include <Forms.hpp>
#include "TTLLiveCtrlLib_OCX.h"
#include <ComCtrls.hpp>
#include <OleCtrls.hpp>
#include <ExtCtrls.hpp>
//---------------------------------------------------------------------------
class TFormMain : public TForm
{
__published:	// IDE-managed Components
        TTTLLive *TTLLive1;
        TStatusBar *StatusBarMain;
        TGroupBox *GroupBox1;
        TButton *Button1;
        TCheckBox *CheckBoxAutodetect;
        TCheckBox *CheckBoxKeepExisting;
        TCheckBox *CheckBoxUSB0;
        TCheckBox *CheckBoxUSB1;
        TBevel *Bevel1;
        TImage *Image1;
        void __fastcall Button1Click(TObject *Sender);
private:	// User declarations
public:		// User declarations
        __fastcall TFormMain(TComponent* Owner);
};
//---------------------------------------------------------------------------
extern PACKAGE TFormMain *FormMain;
//---------------------------------------------------------------------------
#endif
