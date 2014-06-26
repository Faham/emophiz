#include "cbase.h"

#include <windows.h>
#include <mscoree.h>
#include <assert.h>
#include <stdio.h>
#include <tchar.h>
#include <math.h>
#include <maprules.h>
#include <fmtstr.h>
#include <point_template.h>
#include <player.h>
#include <npc_BaseZombie.h>
#include <eventqueue.h>
#include <CEmotionEngine.h>
#include <buttons.h>

#pragma comment(lib, "CEmotionEngine")


class CDirector : public CLogicalEntity
{
public:
	DECLARE_CLASS( CDirector, CLogicalEntity );

	DECLARE_DATADESC();
 
	enum ADAPTING_ELEMENT {
		AE_NONE = 0,
		AE_DEFAULT = 1,
		AE_PLAYER = 2,
		AE_NPC = 3,
		AE_ENVIRONMENT = 4,
	};

	// Constructor
	CDirector () 
	{
		m_nCounter = 0;
		m_nAlive = 0;
		m_nKilled = 0;
		m_nRound = 1;
		m_ent_gm_txt_killed = NULL;
		m_ent_gm_txt_round = NULL;
		m_ent_pnt_spwn_zombie = NULL;
		//m_ent_plyr_speed = NULL;
		//m_ent_npc_zombie = NULL;
		m_ent_funcbtn_healthpack = NULL;
		m_ent_funcbtn_grenade = NULL;
		m_nMaxAlive = 5;
		m_game_adapt_id = AE_NONE;
		m_adapting = false;
		mp_player = NULL;
		m_zombie_speed.SetFloat(1.0f);
		m_player_speed.SetFloat(1.0f);
		m_fog_end.SetFloat(1000.0f);
		m_fog_start.SetFloat(300.0f);
		m_grenade_regen_delay = 30;
		m_medic_regen_delay = 30;

		m_max_zombie_speed = 4.0f;
		m_min_zombie_speed = 1.0;
		m_max_player_speed = 2.0f;
		m_min_player_speed = 0.65f;
		m_min_fog_end = 500;
		m_max_fog_end = 1500;
		m_min_fog_start = 70;
		m_max_fog_start = 450;
		m_max_alive_adapted = m_nMaxAlive;

		m_nThreshold = 5;
		m_nIncreasePower = 1.3f;
		m_arousal = 0.0f;
		m_gsr_calibrating = false;
		m_hr_calibrating = false;
		m_calibrated = false;
		m_calibration_start_time = 0;
		m_gsr_calibration_duration = 60;
		m_hr_calibration_duration = 7;
		m_calibration_interval = 180;
		m_firstZombie = true;
	}
 
	// Input function
	void InputTickZombie( inputdata_t &inputData );
	void InputTickZombieDied( inputdata_t &inputData );
	void InputSetPlayerSpeed( inputdata_t &inputData );
	void InputStart( inputdata_t &inputData );
	void InputSetZombieSpeed( inputdata_t &data );
	void InputStartEmotionEngine( inputdata_t &data );
	void InputPrintEmotionValues( inputdata_t &data );
	void InputSetAdaptId( inputdata_t &data );
	
	void Think();

private:
	void set_zombie_speed(float val);
	void set_player_speed(float val);
	void set_fog_start_end(float val_s, float val_e);
	void force_spawn_random_zombie();
	void adapt_environment();
	void adapt_npc();
	void adapt_player();
	void readEmotions();
	void updateRound();
	void reset();
	void logEvent(int optcode);
	void logEvent(int optcode, float v);
	void logEvent(int optcode, float v1, float v2);
	void logMetrics();

	bool m_firstZombie;
	bool m_gsr_calibrating;
	bool m_hr_calibrating;
	bool m_calibrated;
	float m_calibration_start_time;
	float m_gsr_calibration_duration;
	float m_hr_calibration_duration;
	float m_calibration_interval;
	CBasePlayer *mp_player;
	variant_t m_zombie_speed;
	variant_t m_player_speed;
	variant_t m_fog_end;
	variant_t m_fog_start;
	int m_min_fog_end;
	int m_max_fog_end;
	int m_min_fog_start;
	int m_max_fog_start;
	int m_nThreshold; // Count at which to fire our output
	float m_nIncreasePower; // Increase Power
	float m_max_zombie_speed;
	float m_min_zombie_speed;
	float m_max_player_speed;
	float m_min_player_speed;
	int m_nCounter;   // Internal counter
	int m_nAlive; // number of alive enemies
	int m_nKilled; // number of killed enemies
	int m_nRound; // round number
	int m_nMaxAlive;
	int m_game_adapt_id;
	bool m_adapting;
	CGameText * m_ent_gm_txt_killed;
	CGameText * m_ent_gm_txt_round;
	CBaseButton * m_ent_funcbtn_healthpack;
	CBaseButton * m_ent_funcbtn_grenade;
	CPointTemplate * m_ent_pnt_spwn_zombie;
	static bool ms_emotion_engine_started;
	static bool ms_emotion_engine_initialized;
	float m_arousal;
	float m_grenade_regen_delay;
	float m_medic_regen_delay;
	int m_max_alive_adapted;
 
	COutputEvent m_OnNextRound;	// Output event when the counter reaches the threshold
	static emophiz::CEmotionEngine* ms_emotion_engine;
};

//-----------------------------------------------------------------------------

LINK_ENTITY_TO_CLASS( director, CDirector );

// Start of our data description for the class
BEGIN_DATADESC( CDirector )
 
	// For save/load
	DEFINE_FIELD( m_nCounter, FIELD_INTEGER ),
 
	// As above, and also links our member variable to a Hammer keyvalue
	DEFINE_KEYFIELD( m_nThreshold, FIELD_INTEGER, "threshold" ),
 	DEFINE_KEYFIELD( m_nIncreasePower, FIELD_FLOAT, "increase_power" ),
 
	// Links our input name from Hammer to our input member function
	DEFINE_INPUTFUNC( FIELD_VOID   , "TickZombieDied"    , InputTickZombieDied     ),
 	DEFINE_INPUTFUNC( FIELD_FLOAT  , "SetPlayerSpeed"    , InputSetPlayerSpeed     ),
 	DEFINE_INPUTFUNC( FIELD_VOID   , "Start"             , InputStart              ),
 	DEFINE_INPUTFUNC( FIELD_FLOAT  , "SetZombieSpeed"    , InputSetZombieSpeed     ),
	DEFINE_INPUTFUNC( FIELD_VOID   , "StartEmotionEngine", InputStartEmotionEngine ),
	DEFINE_INPUTFUNC( FIELD_VOID   , "PrintEmotionValues", InputPrintEmotionValues ),
	DEFINE_INPUTFUNC( FIELD_INTEGER, "SetAdaptId"        , InputSetAdaptId         ),

	// Links our output member variable to the output name used by Hammer
	DEFINE_OUTPUT( m_OnNextRound, "OnNextRound" ),

	DEFINE_THINKFUNC( Think ),
 
END_DATADESC()

//-----------------------------------------------------------------------------

emophiz::CEmotionEngine* CDirector::ms_emotion_engine = new emophiz::CEmotionEngine();
bool CDirector::ms_emotion_engine_started = false;
bool CDirector::ms_emotion_engine_initialized = false;

//-----------------------------------------------------------------------------
// Purpose: Handle a tick input from another entity
//-----------------------------------------------------------------------------
void CDirector::InputTickZombieDied( inputdata_t &inputData )
{
	++m_nKilled;
	--m_nAlive;

	if (m_ent_gm_txt_killed)
	{
		inputdata_t message;
		CFmtStrN<50> formatter;
		message.value.SetString(MAKE_STRING(formatter.sprintf("Kills: %d", m_nKilled)));
		m_ent_gm_txt_killed->InputDisplayText(message);
	}
}

//-----------------------------------------------------------------------------
// Purpose: Handle the start
//-----------------------------------------------------------------------------
void CDirector::InputStart( inputdata_t &inputData )
{
	inputdata_t message;

	if (NULL == m_ent_gm_txt_killed)
		m_ent_gm_txt_killed = static_cast<CGameText*>(gEntList.FindEntityByName(NULL, "gm_txt_killed"));
	message.value.SetString(MAKE_STRING("Kills: 0"));
	m_ent_gm_txt_killed->InputDisplayText(message);

	if (NULL == m_ent_gm_txt_round)
		m_ent_gm_txt_round = static_cast<CGameText*>(gEntList.FindEntityByName(NULL, "gm_txt_round"));
	message.value.SetString(MAKE_STRING("Round: 0"));
	m_ent_gm_txt_round->InputDisplayText(message);


	if (NULL == m_ent_funcbtn_healthpack)
		m_ent_funcbtn_healthpack = static_cast<CBaseButton*>(gEntList.FindEntityByName(NULL, "funcbtn_healthpack"));

	if (NULL == m_ent_funcbtn_grenade)
		m_ent_funcbtn_grenade = static_cast<CBaseButton*>(gEntList.FindEntityByName(NULL, "funcbtn_grenade"));

	if (NULL == m_ent_pnt_spwn_zombie)
		m_ent_pnt_spwn_zombie = static_cast<CPointTemplate*>   (gEntList.FindEntityByName(NULL, "pnt_spwn_zombie"));

	reset();

	if (!ms_emotion_engine_started) {
		ms_emotion_engine->start();
		ms_emotion_engine_started = true;
	}

	SetNextThink(gpGlobals->curtime + 5); // Think 5 seconds later to also connect the emotion engine
}

//-----------------------------------------------------------------------------

void CDirector::reset()
{
	set_zombie_speed(1.0f);
	set_player_speed(1.0f);
	set_fog_start_end(300, 1000);
	m_nCounter  = 0;
	m_nAlive    = 0;
	m_nKilled   = 0;
	m_nRound    = 1;
	m_nMaxAlive = 5;
	m_calibrated = false;
	m_gsr_calibrating = false;
	m_hr_calibrating = false;
	m_game_adapt_id = AE_NONE;
	m_adapting = false;
}

//-----------------------------------------------------------------------------

void CDirector::logEvent(int optcode) {
	ms_emotion_engine->logGameEvent(optcode);
}

void CDirector::logEvent(int optcode, float v) {
	ms_emotion_engine->logGameEvent(optcode, v);
}

void CDirector::logEvent(int optcode, float v1, float v2) {
	ms_emotion_engine->logGameEvent(optcode, v1, v2);
}

void CDirector::logMetrics() {

	if (!ms_emotion_engine_initialized)
		return;

	ms_emotion_engine->logGameMetrics(
		m_arousal,                 // arousal
		m_player_speed.Float(),    // player_speed
		m_zombie_speed.Float(),    // zombie_speed
		m_fog_start.Float(),       // fog_start_dist
		m_fog_end.Float(),         // fog_end_dist
		m_nRound,                  // current_round
		m_nThreshold,              // zombie_threshold
		m_nIncreasePower,          // zombie_increase_power
		m_nMaxAlive,               // max_zombie_alive
		m_nAlive,                  // number of alive zombies
		m_nKilled,                 // number of killed zombies
		m_grenade_regen_delay,     // grenade_regen_delay
		m_medic_regen_delay,       // medic_regen_delay
		m_gsr_calibrating,         // calibrating
		m_game_adapt_id            // adaptation_condition
	);
}

//-----------------------------------------------------------------------------

void CDirector::set_zombie_speed(float val)
{
	mp_player = UTIL_GetLocalPlayer();
	if (!mp_player)
		return;

	m_zombie_speed.SetFloat(val);
	Msg("Zombie speed set to %f\n", val);
	g_EventQueue.AddEvent("npc_zombie", "setmovementvalue", m_zombie_speed, 0, mp_player, mp_player);
	logMetrics();
}

//-----------------------------------------------------------------------------

void CDirector::InputStartEmotionEngine( inputdata_t &data )
{
	// NOTE: after starting the engine first you need to connect to the encoder through the ui before doing any other interaction with the engine.
	ms_emotion_engine->start();
}

//-----------------------------------------------------------------------------

void CDirector::InputSetAdaptId( inputdata_t &data )
{
	reset();
	m_game_adapt_id = data.value.Int();
	m_adapting = (m_game_adapt_id != AE_NONE && m_game_adapt_id != AE_DEFAULT);
	Msg("Adaption id is set to %d\n", m_game_adapt_id);
}

//-----------------------------------------------------------------------------

void CDirector::InputPrintEmotionValues( inputdata_t &data )
{
	if (ms_emotion_engine->isConnected())
		Msg("GSR: %f\n", ms_emotion_engine->readGSR());
		//Msg("Arousal: %f\nValence: %f\nGSR: %f\nHR: %f\nBVP: %f\nEMGFrown: %f\nEMGSmile: %f\nFun: %f\nBoredom: %f\nExcitement: %f\n",
			//ms_emotion_engine->readArousal   (),
			//ms_emotion_engine->readValence   (),
			//ms_emotion_engine->readGSR       (),
			//ms_emotion_engine->readHR        (),
			//ms_emotion_engine->readBVP       (),
			//ms_emotion_engine->readEMGFrown  (),
			//ms_emotion_engine->readEMGSmile  (),
			//ms_emotion_engine->readFun       (),
			//ms_emotion_engine->readBoredom   (),
			//ms_emotion_engine->readExcitement());
	else
		Msg("Engine is not connected");
}

//-----------------------------------------------------------------------------

void CDirector::InputSetZombieSpeed( inputdata_t &data )
{
	set_zombie_speed(data.value.Float());
}

//-----------------------------------------------------------------------------

void CDirector::set_player_speed(float val)
{
	mp_player = UTIL_GetLocalPlayer();
	if (!mp_player)
		return;

	m_player_speed.SetFloat(val);
	//g_EventQueue.AddEvent("plyr_speed", "modifyspeed", m_player_speed, 0, mp_player, mp_player);
	mp_player->SetLaggedMovementValue( m_player_speed.Float() );
	Msg("Player speed set to %f\n", val);
	logMetrics();
}

//-----------------------------------------------------------------------------

void CDirector::set_fog_start_end(float val_s, float val_e)
{
	mp_player = UTIL_GetLocalPlayer();
	if (!mp_player)
		return;

	//ent_fire env_fog_ctrl setstartdist 2
	m_fog_start.SetFloat(val_s);
	g_EventQueue.AddEvent("env_fog_ctrl", "setstartdist", m_fog_start, 0, mp_player, mp_player);

	//ent_fire env_fog_ctrl setenddist 500
	m_fog_end.SetFloat(val_e);
	g_EventQueue.AddEvent("env_fog_ctrl", "setenddist", m_fog_end, 0, mp_player, mp_player);
	logMetrics();
}

//-----------------------------------------------------------------------------

void CDirector::force_spawn_random_zombie()
{
	mp_player = UTIL_GetLocalPlayer();
	if (!mp_player)
		return;
	g_EventQueue.AddEvent("pnt_spwn_zombie", "ForceSpawnRandom", variant_t(), 0, mp_player, mp_player);

	if (m_firstZombie) {
		set_zombie_speed(1.0f);
		set_zombie_speed(1.0f);
		set_zombie_speed(1.0f);
		m_firstZombie = false;
	}
}

//-----------------------------------------------------------------------------

void CDirector::InputSetPlayerSpeed( inputdata_t &data )
{
	set_player_speed(data.value.Float());
}

//-----------------------------------------------------------------------------

void CDirector::Think()
{
	BaseClass::Think(); // Always do this if you override Think()
 
	if (!ms_emotion_engine_initialized) {
		ms_emotion_engine->connect();
		ms_emotion_engine_initialized = true;
	}

	if ( m_nCounter < m_nThreshold )
	{
		int _min = min(int(m_nThreshold * 0.3), m_nThreshold - m_nCounter);
		_min = min(_min, m_max_alive_adapted - m_nAlive);
		for (int i = 0; i < _min; ++i)
		{
			++m_nCounter;
			++m_nAlive;
			force_spawn_random_zombie();
			logMetrics();
		}
		SetNextThink(gpGlobals->curtime + 1);
	}
	else if ( m_nCounter == m_nThreshold && m_nAlive == 0)
	{
		m_nCounter = 0;
		++m_nRound;
		logMetrics();

		if (m_ent_gm_txt_round)
		{
			inputdata_t message;
			CFmtStrN<50> formatter;
			message.value.SetString(MAKE_STRING(formatter.sprintf("Round: %d", m_nRound)));
			m_ent_gm_txt_round->InputDisplayText(message);
		}

		mp_player = UTIL_GetLocalPlayer();
		if (mp_player) {
			variant_t msg;
			msg.SetString(MAKE_STRING("Round Complete"));
			g_EventQueue.AddEvent("gm_txt_message", "DisplayText", msg, 0, mp_player, mp_player);
		}

		updateRound();

		m_nThreshold = int(powf(m_nThreshold, m_nIncreasePower));
		logMetrics();

		SetNextThink(gpGlobals->curtime + 10);
	} else {
		SetNextThink(gpGlobals->curtime + 1);
	}


	if (m_adapting && ms_emotion_engine->isConnected()) {
		if (!m_gsr_calibrating && !m_calibrated) {

			ms_emotion_engine->calibrateGSR(true);
			ms_emotion_engine->calibrateBVP(true);
			
			m_calibration_start_time = gpGlobals->curtime;
			m_gsr_calibrating = true;
			m_hr_calibrating = true;
			logMetrics();
			Msg("Calibrating...\n");
		}
		
		if (m_hr_calibrating && m_calibration_start_time + m_hr_calibration_duration < gpGlobals->curtime) {
			
			ms_emotion_engine->calibrateBVP(false);
			m_hr_calibrating = false;
			logMetrics();

		}
		
		if (m_gsr_calibrating && m_calibration_start_time + m_gsr_calibration_duration < gpGlobals->curtime) {
			
			ms_emotion_engine->calibrateGSR(false);
			m_gsr_calibrating = false;
			m_calibrated = true;
			logMetrics();
			Msg("Calibration finished.\n");

		}
		
		if (m_calibrated) {
			readEmotions();
			if (m_game_adapt_id == AE_PLAYER)      adapt_player();
			if (m_game_adapt_id == AE_NPC)         adapt_npc();
			if (m_game_adapt_id == AE_ENVIRONMENT) adapt_environment();
		}

		if (m_calibration_start_time + m_gsr_calibration_duration + m_calibration_interval < gpGlobals->curtime)
			m_calibrated = false;
	}
}

//-----------------------------------------------------------------------------

void CDirector::readEmotions() {
	if (ms_emotion_engine->isConnected()) {
		m_arousal = ms_emotion_engine->readGSR() / 100.0f;
		//m_arousal = ms_emotion_engine->readArousal() / 100.0f;
	}
}

//-----------------------------------------------------------------------------

void CDirector::updateRound() {
	// deciding new max alive zombies
	m_nMaxAlive = m_nMaxAlive * 1.50;
	m_max_alive_adapted = m_nMaxAlive;

	// decide zombie increase rate
	m_nIncreasePower = 1.3f;
	logMetrics();
}

//-----------------------------------------------------------------------------

void CDirector::adapt_environment() {
	// set fog distant
	int fg_start = m_min_fog_start + m_arousal * (m_max_fog_start - m_min_fog_start);
	int fg_end = m_min_fog_end + m_arousal * (m_max_fog_end - m_min_fog_end);

	Msg("Fog distance set to (start, end) %f, %f\n", fg_start, fg_end);
	set_fog_start_end(fg_start, fg_end);

	// set healthpack rate
	m_medic_regen_delay = 100 - (m_arousal * 60); // interpolates arousal from 0.0~1.0 to 90.0~30.0
	m_ent_funcbtn_healthpack->SetDelay(m_medic_regen_delay);
	logMetrics();
}

//-----------------------------------------------------------------------------

void CDirector::adapt_npc() {
	// deciding new zombie speed
	float next_zombie_speed = 1 / (0.30f + m_arousal); //m_min_zombie_speed + (m_arousal * (m_max_zombie_speed - m_min_zombie_speed));
	set_zombie_speed(next_zombie_speed);

	//Todo: add zombie numbers change adaptively
	m_max_alive_adapted = m_nMaxAlive - (int)(m_arousal * (m_nMaxAlive * 0.5) - (m_nMaxAlive * 0.25));
}

//-----------------------------------------------------------------------------

void CDirector::adapt_player() {
	// set player speed
	float next_player_speed = m_min_player_speed + (m_arousal * (m_max_player_speed - m_min_player_speed));
	set_player_speed(next_player_speed);

	// grenade rate
	m_grenade_regen_delay = 40 - (m_arousal * 20); // interpolates arousal from 0.0~1.0 to 40.0~20.0
	m_ent_funcbtn_grenade->SetDelay(m_grenade_regen_delay);
	logMetrics();
}

//-----------------------------------------------------------------------------
