/******************************************************************************
:                                                                             :
: Filename       : TTLAPIDEF.H                                                :
: Author         : Nicolas Montmarquette - Thought Technology Ltd.            :
: Target         : N/A                                                        :
: Project        : TTLAPI                                                     :
: Description    : DEFINES and MACROS used in TTLAPI.                         :
: Comments       : This file is intended both for internal and outside        :
:                  developpers usage. Is to be sent with TTLAPI.DLL           :
:                                                                             :
:----------------  Updates  --------------------------------------------------:
:                                                                             :
: Date,        Name,         Comments.                                        :
:                                                                             :
: aug-09-2001, Nicolas M.,   Initial Writing.                                 :
: feb-07-2002, Nicolas M.,   TTLLive 0.7.x.x                                  :
: mar-27-2002, Nicolas M.,   TTLLive 0.8.0.42                                 :
: may-10-2002, Nicolas M.,   TTLLive 0.9.0.62                                 :
: Jul-23-2002, Nicolas M.,   TTLLive 0.9.0.69+                                :
: Aug-19-2002, Nicolas M.,   TTLLive 0.9.0.119+ /w ITTLSessionMgr stuff.      :
: Aug-19-2002, Nicolas M.,   TTLLive 0.9.0.200+                               :
: sep-22-2002, Nicolas M.,   TTLLive 0.9.0.400+, enums imbedded in type lib.  :
: dec-17-2002, Nicolas M.,   TTLLive 0.9.0.450+, enums for decryption states  : 
:                            added OCCMD macros with (1<<x) and for 4 USB port:
: may-01-2003, Nicolas M.,   TTLLive 0.9.0.546+, Added new enums for encoder  :
:                            synchronization modes.                           :
: may-27-2003, Valery F.,    TTLLive 0.9.0.XXX+, added Myotrac Infiniti into  : 
:                            encoder type enum                                :
: jun-20-2003, Valery F.,    TTLLive 0.9.0.XXX+, added tuples for Myotrac     :
:                            scripts description                              : 
: sep-24-2003, Nicolas M.,   TTLLive 2.0.0.XXX, smalls fixups                 :
******************************************************************************/

#ifndef __TTLAPIDEF_H_
#define __TTLAPIDEF_H_ 


#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#pragma message( "\n\n***** Including TTLAPIDEFS.H *****" )

// _ATL_ATTRIBUTES define is new to .NET environnement
#ifdef _ATL_ATTRIBUTES
#pragma message( "\n\n***** _ATL_ATTRIBUTES set *****" )
#define TYPEDEF_ENUM_STATEMENT typedef[export,library_block,v1_enum]
#define TYPEDEF_STATEMENT [public,library_block,export]typedef
#else
#pragma message( "\n\n***** _ATL_ATTRIBUTES not set *****" )
#define TYPEDEF_STATEMENT typedef
#define TYPEDEF_ENUM_STATEMENT typedef
#endif

#ifndef WM_APP
#define WM_APP 0x8000
#define DELETE_WM_APP 
#endif

// Channel enumerator
TYPEDEF_ENUM_STATEMENT enum _TTLAPI_DATA_CHANNELS {
  /* 00 */TTLAPI_CHANNEL_A = 0,
  /* 01 */TTLAPI_CHANNEL_B,
  /* 02 */TTLAPI_CHANNEL_C,
  /* 03 */TTLAPI_CHANNEL_D,
  /* 04 */TTLAPI_CHANNEL_E,
  /* 05 */TTLAPI_CHANNEL_F,
  /* 06 */TTLAPI_CHANNEL_G,
  /* 07 */TTLAPI_CHANNEL_H,
  /* 08 */TTLAPI_CHANNEL_I,
  /* 09 */TTLAPI_CHANNEL_J,
} TTLAPI_DATA_CHANNELS;

// Notification messages
TYPEDEF_ENUM_STATEMENT enum _TTLAPI_WM_MESSAGES {
  TTLAPI_WM_ON_CHANNEL_DATA = (WM_APP+101),
  TTLAPI_WM_ON_EVENT_SWITCH,
  TTLAPI_WM_ON_SENSOR_STATE_CHANGE,
  TTLAPI_WM_ON_ENCODER_STATE_CHANGE,
  TTLAPI_WM_ON_DATA_OVERFLOW,
  TTLAPI_WM_ON_DATA_ERROR,
  TTLAPI_WM_ON_DATA_TIMEOUT,
  TTLAPI_WM_ON_IMP_CHECK_RESULTS
}TTLAPI_WM_MESSAGES;

#ifdef DELETE_WM_APP 
#undef DELETE_WM_APP 
#undef WM_APP
#endif

// Notification messages activation bits
#define TTLAPI_ON_CHANNEL_DATA_ACTIVE           (1<<4)
#define TTLAPI_ON_EVENT_SWITCH_ACTIVE           (1<<5)
#define TTLAPI_ON_SENSOR_STATE_CHANGE_ACTIVE    (1<<6)
#define TTLAPI_ON_ENCODER_STATE_CHANGE_ACTIVE   (1<<7)
#define TTLAPI_ON_DATA_OVERFLOW_ACTIVE          (1<<8)
#define TTLAPI_ON_DATA_ERROR_ACTIVE             (1<<9)
#define TTLAPI_ON_DATA_TIMEOUT_ACTIVE           (1<<10)
#define TTLAPI_ON_IMP_CHECK_RESULTS_ACTIVE      (1<<11)

// Short Read Mask Bits for the ShortReadMask property
#define TTLAPI_SRM_EVENT_SWITCH_ACTIVE          (1<<0)
#define TTLAPI_SRM_SENSOR_REMOVED_ACTIVE        (1<<1)
#define TTLAPI_SRM_SENSOR_PLUGGED_ACTIVE        (1<<2)
#define TTLAPI_SRM_CAL_IN_PROGRESS_ACTIVE       (1<<3)
#define TTLAPI_SRM_IC_IN_PROGRESS_ACTIVE        (1<<4)
#define TTLAPI_SRM_ENCODER_OFFLINE_ACTIVE       (1<<5)
#define TTLAPI_SRM_DATA_TIMEOUT_ACTIVE          (1<<6)
#define TTLAPI_SRM_DATA_ERROR_ACTIVE            (1<<7)
#define TTLAPI_SRM_IMP_CHECK_RESULTS_ACTIVE     (1<<8)

#define TTLAPI_EI_EVENT_SWITCH_ACTIVE           (1<<0)
#define TTLAPI_EI_SENSOR_REMOVED_ACTIVE         (1<<1)
#define TTLAPI_EI_SENSOR_PLUGGED_ACTIVE         (1<<2)
#define TTLAPI_EI_CAL_IN_PROGRESS_ACTIVE        (1<<3)
#define TTLAPI_EI_IC_IN_PROGRESS_ACTIVE         (1<<4)
#define TTLAPI_EI_ENCODER_OFFLINE_ACTIVE        (1<<5)
#define TTLAPI_EI_DATA_TIMEOUT_ACTIVE           (1<<6)
#define TTLAPI_EI_DATA_ERROR_ACTIVE             (1<<7)
#define TTLAPI_EI_IMP_CHECK_RESULTS_ACTIVE      (1<<8)

// Defines for user model channel states
TYPEDEF_ENUM_STATEMENT enum _TTLAPI_SENSOR_STATES {
  TTLAPI_SENS_ST_ENCODER_OFFLINE = -1,    
  TTLAPI_SENS_ST_UNCONNECTED,             
  TTLAPI_SENS_ST_NORMAL,                  
  TTLAPI_SENS_ST_CAL_IN_PROGRESS,         
  TTLAPI_SENS_ST_IC_IN_PROGRESS 
}TTLAPI_SENSOR_STATES;

 // Encoder synchronization modes.
TYPEDEF_ENUM_STATEMENT enum _TTLAPI_ESM {
  TTLAPI_ESM_NONE = 0,    
  TTLAPI_ESM_SELF,             
  TTLAPI_ESM_EXTERNAL
}TTLAPI_ESM;

TYPEDEF_ENUM_STATEMENT enum _TTLAPI_SENSOR_TYPES {
  TTLAPI_SENS_TYPE_ENCODER_OFFLINE  =-1,
  TTLAPI_SENS_TYPE_UNKNOWN          =0,
  TTLAPI_SENS_TYPE_UNCONNECTED      =1,
  TTLAPI_SENS_TYPE_EEG              =2,
  TTLAPI_SENS_TYPE_HR_BVP           =4,
  TTLAPI_SENS_TYPE_SKIN_CONDUCTANCE =5,
  TTLAPI_SENS_TYPE_EKG_Z            =6,
  TTLAPI_SENS_TYPE_EEG_Z            =7,
  TTLAPI_SENS_TYPE_MYOSCAN          =8,
  TTLAPI_SENS_TYPE_MYOSCAN_PRO_400  =9,
  TTLAPI_SENS_TYPE_RESPIRATION      =10,
  TTLAPI_SENS_TYPE_MYOSCAN_Z_HR     =11,
  TTLAPI_SENS_TYPE_MYOSCAN_Z        =12,
  TTLAPI_SENS_TYPE_GONIOMETER       =13,
  TTLAPI_SENS_TYPE_FORCE            =14,
  TTLAPI_SENS_TYPE_MYOSCAN_PRO_1600W=15,
  TTLAPI_SENS_TYPE_VOLT_ISOLATOR    =16,
  TTLAPI_SENS_TYPE_TEMPERATURE      =17
}TTLAPI_SENSOR_TYPES;

// Impedance check calculation types
TYPEDEF_ENUM_STATEMENT enum _TTLAPI_IMPCHECK_CALC_TYPES {
  TTLAPI_IMPCHECK_TYPE_RESISTIVE =0,
  TTLAPI_IMPCHECK_TYPE_TOTAL,
}TTLAPI_IMPCHECK_CALC_TYPES;

TYPEDEF_ENUM_STATEMENT enum _TTLAPI_IMPCHECK_ELECTRODES {
  /* 00 */TTLAPI_IMPCHECK_ELECTRODE_PLUS=0,
  /* 01 */TTLAPI_IMPCHECK_ELECTRODE_MINUS,
  /* 02 */TTLAPI_IMPCHECK_ELECTRODE_REF,
  /* 03 */TTLAPI_IMPCHECK_PAIRS_PLUS_REF,
  /* 04 */TTLAPI_IMPCHECK_PAIRS_MINUS_REF,
  /* 05 */TTLAPI_IMPCHECK_PAIRS_PLUS_MINUS
}TTLAPI_IMPCHECK_ELECTRODES;

// Impedance check other defines
#define TTLAPI_IMPCHECK_MAX_RESULT              6

// Defines for encryption state (Procomp2)
TYPEDEF_ENUM_STATEMENT enum _TTL_ENCRYPTION_STATES {
  /* 00 */TTL_ENCRYPTION_STATE_SYNCHRONIZING=0,
  /* 01 */TTL_ENCRYPTION_STATE_PASS_THROUGH,
  /* 02 */TTL_ENCRYPTION_STATE_CANT_DECRYPT,
  /* 03 */TTL_ENCRYPTION_STATE_DECRYPTING,
  /* 04 */TTL_ENCRYPTION_STATE_NOT_DECRYPTING
} TTL_ENCRYPTION_STATES;

// Defines for encoder states
TYPEDEF_ENUM_STATEMENT enum _TTLAPI_ENCODER_STATES {
  TTLAPI_ENC_ST_OFFLINE=-1,
  TTLAPI_ENC_ST_DETECTING=1,
  TTLAPI_ENC_ST_ONLINE=2
}TTLAPI_ENCODER_STATES;

// Defines for Thought Technology Encoder connection types
TYPEDEF_ENUM_STATEMENT enum _TTLAPI_CONNECTIONS_TYPES {
  /* 00 */TTLAPI_CT_UNKNOWN=0,
  /* 01 */TTLAPI_CT_COM,
  /* 02 */TTLAPI_CT_USB,
  /* 03 */TTLAPI_CT_TELEMETRY,
  /* 04 */TTLAPI_CT_UDP,
  /* 05 */TTLAPI_CT_TCP,
  /* 06 */TTLAPI_CT_FILE,
  /* 07 */TTLAPI_CT_SSF_FILE,
  /* 08 */TTLAPI_CT_TTCMD_DLL
}TTLAPI_CONNECTIONS_TYPES;

// Defines for Thought Technology Protocol Types
TYPEDEF_ENUM_STATEMENT enum _TTLAPI_PROTOCOL_TYPES {
  /* 00 */TTLAPI_PROT_UNKNOWN =0,
  /* 01 */TTLAPI_PROT_ADAPTIVE,
  /* 02 */TTLAPI_PROT_BIOGRAPH,
  /* 03 */TTLAPI_PROT_BIORESEARCH,
  /* 04 */TTLAPI_PROT_EEG_SPECTRUM,
  /* 05 */TTLAPI_PROT_MYOTRAC3,
  /* 06 */TTLAPI_PROT_INFINITI_PRO,
  /* 07 */TTLAPI_PROT_INFINITI_FLEX,
  /* 08 */TTLAPI_PROT_INFINITI_CF_RECORDED,
  /* 09 */TTLAPI_PROT_INFINITI_MYOTRAC
}TTLAPI_PROTOCOL_TYPES;

const int TTLAPI_PROT_INFINITY_PRO = TTLAPI_PROT_INFINITI_PRO;
const int TTLAPI_PROT_INFINITY_FLEX = TTLAPI_PROT_INFINITI_FLEX;

// Defines for Thought Technology Encoder Types
TYPEDEF_ENUM_STATEMENT enum _TTLAPI_ENCODER_MODEL_TYPES {
  TTLAPI_ET_UNKNOWN =0,
  TTLAPI_ET_MYOTRAC3,
  TTLAPI_ET_MYOTRAC3G,
  TTLAPI_ET_PROCOMPPLUS,
  TTLAPI_ET_PROCOMP2,
  TTLAPI_ET_INFINITI_PRO, 
  TTLAPI_ET_INFINITI_FLEX,
  TTLAPI_ET_INFINITI_MYOTRAC
}TTLAPI_ENCODER_MODEL_TYPES;

const int TTLAPI_ET_INFINITY_PRO = TTLAPI_ET_INFINITI_PRO;
const int TTLAPI_ET_INFINITY_FLEX = TTLAPI_ET_INFINITI_FLEX;

// Defines for Thought Technology TTLAPI Unit Types
TYPEDEF_ENUM_STATEMENT enum _TTLAPI_UNIT_TYPES {
  TTLAPI_UT_PERCENT=2, // Unitless quantity (e.g. BVP, respirometer), 
  TTLAPI_UT_SENSVOLT,  // volts presented at the input of a sensor
  TTLAPI_UT_VRMS,      // volts, converted according to an RMS-type function
  TTLAPI_UT_DEGC,      // Degrees Centigrade (Celsius)
  TTLAPI_UT_DEGF,      // Degrees Fahrenheit
  TTLAPI_UT_DEGREE,    // Degrees (angle)
  TTLAPI_UT_SIEMENS,   // Siemens (conductance)
  TTLAPI_UT_COUNT,     // Encoder counts(raw integers).
  TTLAPI_UT_ENCVOLT,   // voltage as input to the encoder (after any sensor preamplifier). 
  TTLAPI_UT_PU,        // arbitrary unit, monotonically but not very linearly related to 
                       // the amount of force detected on a resistive-type force sensor
  TTLAPI_UT_SENSMVOLT, // millivolts presented at the input of a sensor
  TTLAPI_UT_SENSUVOLT, // microvolts presented at the input of a sensor
  TTLAPI_UT_MVRMS,     // millivolts, converted according to an RMS-type function
  TTLAPI_UT_UVRMS,     // microvolts, converted according to an RMS-type function
  TTLAPI_UT_USIEMENS   // microSiemens (conductance).
}TTLAPI_UNIT_TYPES;

// Defines for Thought Technology TTLAPI OpenAllConnections 
// libmCommand parameter
#define TTLAPI_OCCMD_AUTODETECT                 (1<<0)
#define TTLAPI_OCCMD_KEEPEXISTING               (1<<1)
#define TTLAPI_OCCMD_COM1                       (1<<2)
#define TTLAPI_OCCMD_COM2                       (1<<3)
#define TTLAPI_OCCMD_COM3                       (1<<4)
#define TTLAPI_OCCMD_COM4                       (1<<5)
#define TTLAPI_OCCMD_COM5                       (1<<6)
#define TTLAPI_OCCMD_COM6                       (1<<7)
#define TTLAPI_OCCMD_COM7                       (1<<8)
#define TTLAPI_OCCMD_COM8                       (1<<9)
#define TTLAPI_OCCMD_TTUSB0                     (1<<10)
#define TTLAPI_OCCMD_TTUSB1                     (1<<11)
#define TTLAPI_OCCMD_TTUSB2                     (1<<12)
#define TTLAPI_OCCMD_TTUSB3                     (1<<13)
#define TTLAPI_OCCMD_SIMULATOR                  (1<<14)
#define TTLAPI_OCCMD_UDP                        (1<<15)
#define TTLAPI_OCCMD_TCP                        (1<<16)

// Top 4 bits are used 
#define TTLAPI_OCCMD_FORCE_PROT(x)              ((x)<<28)

TYPEDEF_ENUM_STATEMENT enum _TTLAPI_TRANSCRIBE_MODE {
  TTLAPI_TSM_NORMAL = 0,
  TTLAPI_TSM_OVERWRITE,
  TTLAPI_TSM_SKIP,
}TTLAPI_TRANSCRIBE_MODE;

TYPEDEF_ENUM_STATEMENT enum _TTLAPI_DATA_SOURCE_TYPE {
  TTLAPI_DST_INVALID = -1,
  TTLAPI_DST_UNKNOWN,
  TTLAPI_DST_DIRECTORY,
  TTLAPI_DST_MSF,
  TTLAPI_DST_MYOTRAC_INFINITY
}TTLAPI_DATA_SOURCE_TYPE;

// Defines for Thought Technology TTLAPI .SSF play back modes
TYPEDEF_ENUM_STATEMENT enum _TTLAPI_PLAYBACK_MODES {
  TTLAPI_PBM_STOP =0,
  TTLAPI_PBM_ONCE,
  TTLAPI_PBM_LOOP,
}TTLAPI_PLAYBACK_MODES;

// Defines for Thought Technology TTLAPI .SSF play back modes
TYPEDEF_ENUM_STATEMENT enum _TTLAPI_MSF_REBUILD_MODES {
  TTLAPI_RBM_QUICK =0,
  TTLAPI_RBM_FULL,
}TTLAPI_MSF_REBUILD_MODES;

/*
#ifdef _ATL_ATTRIBUTES
typedef [ export,v1_enum ] enum _TTLAPI_RTC_UPDATE_MODES {
#define TYPEDEF_STATEMENT [public,library_block,export]typedef
#else
typedef enum _TTLAPI_RTC_UPDATE_MODES {
#endif
  TTL_RTCUM_NONE = 0,  // No action is performed or operation completed
  TTL_RTCUM_ABSOLUTE,  // RTC is updated with Current time 
  TTL_RTCUM_RELATIVE,
} TTLAPI_RTC_UPDATE_MODES;

*/
TYPEDEF_ENUM_STATEMENT enum _TTLAPI_RTC_UPDATE_MODES {
  TTL_RTCUM_NONE = 0,  // No action is performed or operation completed
  TTL_RTCUM_ABSOLUTE,  // RTC is updated with Current time 
  TTL_RTCUM_RELATIVE,
}TTLAPI_RTC_UPDATE_MODES;

TYPEDEF_ENUM_STATEMENT enum _TTALPI_TUPLE_ID {
  TTL_TID_DW_G_SESSHDR_VER = 0x1000,    //  Session header version
  TTL_TID_SZ_G_COPYRIGHT,               //  TTL Copyright String
  TTL_TID_DW_G_FW_VER,                  //* Recording encoder firmware version
  TTL_TID_DW_G_HW_VER,                  //*
  TTL_TID_DT_G_DTTMREC,                 //*
  TTL_TID_DT_G_DTTMTRN,                 //
  TTL_TID_SZ_G_COMMENT,                 //
  TTL_TID_BY_G_PROTTYPE,                //*
  TTL_TID_WO_G_CHANNELSTATUS,           //* Bitfield of recorded channels
  TTL_TID_WO_G_CHANNELSTATUSCHANGED,    //* Bitfield of changed channel status during recording
  TTL_TID_SZ_G_ENCSERIAL,               //* 
  TTL_TID_BY_G_ENCTYPE,                 //* Encoder Type
  TTL_TID_WO_G_ENCID,                   //  Encoder ID (legacy protocol only)
  TTL_TID_WO_G_ENCCHANCOUNT,            //* Encoder number of channels
  TTL_TID_WO_G_CHANCOUNT,               //* 0x0d Number of recorded channels
  TTL_TID_WO_G_CITLEN,                  //* Item count in the CIT 
  TTL_TID_FL_G_TICKRATE,                //* 
  TTL_TID_WO_G_DATA_OFFSET,             //* Offset in byte in the starting sector
  // These are part of the channel descritor
  TTL_TID_WO_CH_PHYSCHAN,               //* Physical channel number on encoder
  TTL_TID_WO_CH_CITPOS,                 //* Item position in CIT
  TTL_TID_FL_CH_BANDWIDTH,              //*  
  TTL_TID_XX_CH_SAMPDELAY,              //* 
  TTL_TID_BY_CH_SENSID,                 //*   
  TTL_TID_WO_CH_CHANTYPE,               //* Type of the recorded channel (see T_TTL_ChannelType)
  TTL_TID_ST_CIT_ENTRY,                 //* Carry a structure of T_CIT_Entry type
  // These describe interleaved items 
  // (now they are imbedded in the struct carried by  TTL_TID_ST_CIT_ENTRY tuple
  TTL_TID_BY_CIT_TYPE,                  //* Interleaved item type (see _T_TTL_CIT_ChannelType )
  TTL_TID_BY_CIT_SIZE,                  //* Size in byte of the interleaved item (usually 1 or 2 )
  TTL_TID_WO_CIT_FREQRATIO,             //* Ratio by which occurence frequency is lower than base rate
  TTL_TID_WO_CIT_INITCOUNT,             //* Initial tick count for Interleaved item
  TTL_TID_DW_G_SESSION_SIZE,            //  Size of session data in ticks
  TTL_TID_WO_G_BATT_LVL,                //  Battery voltage in mV 
  TTL_TID_WO_G_PATIENTNUMBER,           //  Patient ID
  TTL_TID_SZ_G_PATIENTINITIALS,         //  Patient Initials
  TTL_TID_WO_SCRIPT_SCRIPTTYPE,         //  script type (EMG or EMG triggered)
  TTL_TID_WO_SCRIPT_SCRIPTSOURCE,       //  script source (preset or custom)
  TTL_TID_DW_SCRIPT_ACTIVITYNUM,        //  number of activities in the script
  TTL_TID_WO_SCRIPT_ACTIVITYTYPE,       //  activity type in the script
  TTL_TID_WO_SCRIPT_ACTIVITYCYCLENUM,   //  number of cycles in the script activity
  TTL_TID_WO_SCRIPT_ACTIVITYSTEPNUM,    //  number of steps in the script activity
  TTL_TID_DW_SCRIPT_ACTIVITYMIN,        //  channel RMS min in the script activity
  TTL_TID_DW_SCRIPT_ACTIVITYMAX,        //  channel RMS max in the script activity
  TTL_TID_FL_SCRIPT_ACTIVITYMEAN,       //  channel RMS mean in the script activity
  TTL_TID_FL_SCRIPT_ACTIVITYSDEV,       //  channel RMS sdev in the script activity
  TTL_TID_DW_SCRIPT_ACTIVITYZEROCROSS,  //  channel zero crossing counter in the script activity
  TTL_TID_DW_SCRIPT_STIMRAMPTIME_MS,    //  channel stim ramp time
  TTL_TID_DW_SCRIPT_STIMPULSEPERIOD_MS, //  channel stim pulse period
  TTL_TID_DW_SCRIPT_STIMPULSEWIDTH_US,  //  channel stim pulse width
  TTL_TID_DW_SCRIPT_GRAPHTYPE,          //  graph type to use (MyoTrac screen)
  TTL_TID_WO_SCRIPT_STEPTYPE,           //  step type in the script activity
  TTL_TID_DW_SCRIPT_STEPDURATION_MS,    //  step duration (ms) in the script activity
  TTL_TID_DW_EMGTRIG_THRESHOLD_US,      //  threshold (us) for emg trigged
  TTL_TID_DW_G_MARKER = 0x2000          //  Start of MSD specific User generated marker
} TTALPI_TUPLE_ID;

// Perform the selected gap sync mode on the bit selected items.
TYPEDEF_ENUM_STATEMENT enum _TTLAPI_GAP_SYNC_MODES {
  TTLAPI_GSM_NONE = 0,
  TTLAPI_GSM_FILL,
  TTLAPI_GSM_LATE_FILL
} TTLAPI_GAP_SYNC_MODES;

// Perform the selected gap sync mode on the bit selected items.
TYPEDEF_ENUM_STATEMENT enum _TTLAPI_GAP_SYNC_MASK {
  TTLAPI_GFMSK_ALL      = -1,
  TTLAPI_GFMSK_ENCODER  = 1,
  TTLAPI_GFMSK_CHANNEL  = 2  
} TTLAPI_GAP_FILL_MODES;

#undef TYPEDEF_STATEMENT
#endif //__TTLAPIDEF_H_

