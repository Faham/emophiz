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

#pragma comment(lib, "CEmotionEngine")


class CDirector : public CLogicalEntity
{
public:
	DECLARE_CLASS( CDirector, CLogicalEntity );

	DECLARE_DATADESC();
 
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
		m_ent_plyr_speed = NULL;
		m_ent_npc_zombie = NULL;
		m_nMaxAlive = 15;
		mp_player = NULL;
		m_zombie_speed.SetFloat(1.0f);
		m_player_speed.SetFloat(1.0f);
		m_emotion_engine = NULL;
	}
 
	// Input function
	void InputTickZombie( inputdata_t &inputData );
	void InputTickZombieDied( inputdata_t &inputData );
	void InputSetPlayerSpeed( inputdata_t &inputData );
	void InputStart( inputdata_t &inputData );
	void InputSetZombieSpeed( inputdata_t &data );
	void InputStartEmotionEngine( inputdata_t &data );
	void InputPrintEmotionValues( inputdata_t &data );
	
	void Think();

private:
	void set_zombie_speed(float val);
	void set_player_speed(float val);

	CBasePlayer *mp_player;
	variant_t m_zombie_speed;
	variant_t m_player_speed;
	int m_nThreshold; // Count at which to fire our output
	float m_nIncreasePower; // Increase Power
	int m_nCounter;   // Internal counter
	int m_nAlive; // number of alive enemies
	int m_nKilled; // number of killed enemies
	int m_nRound; // round number
	int m_nMaxAlive;
	CGameText * m_ent_gm_txt_killed;
	CGameText * m_ent_gm_txt_round;
	CPointTemplate * m_ent_pnt_spwn_zombie;
	CMovementSpeedMod * m_ent_plyr_speed;
	CNPC_BaseZombie* m_ent_npc_zombie;
 
	COutputEvent m_OnNextRound;	// Output event when the counter reaches the threshold
	emophiz::CEmotionEngine* m_emotion_engine;
};

LINK_ENTITY_TO_CLASS( director, CDirector );

// Start of our data description for the class
BEGIN_DATADESC( CDirector )
 
	// For save/load
	DEFINE_FIELD( m_nCounter, FIELD_INTEGER ),
 
	// As above, and also links our member variable to a Hammer keyvalue
	DEFINE_KEYFIELD( m_nThreshold, FIELD_INTEGER, "threshold" ),
 	DEFINE_KEYFIELD( m_nIncreasePower, FIELD_FLOAT, "increase_power" ),
 
	// Links our input name from Hammer to our input member function
	DEFINE_INPUTFUNC( FIELD_VOID, "TickZombieDied", InputTickZombieDied ),
 	DEFINE_INPUTFUNC( FIELD_FLOAT, "SetPlayerSpeed", InputSetPlayerSpeed ),
 	DEFINE_INPUTFUNC( FIELD_VOID, "Start", InputStart ),
 	DEFINE_INPUTFUNC( FIELD_FLOAT, "SetZombieSpeed", InputSetZombieSpeed ),
	DEFINE_INPUTFUNC( FIELD_VOID, "StartEmotionEngine", InputStartEmotionEngine ),
	DEFINE_INPUTFUNC( FIELD_VOID, "PrintEmotionValues", InputPrintEmotionValues ),

	// Links our output member variable to the output name used by Hammer
	DEFINE_OUTPUT( m_OnNextRound, "OnNextRound" ),

	DEFINE_THINKFUNC( Think ),
 
END_DATADESC()


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

	m_ent_gm_txt_killed = static_cast<CGameText*>(gEntList.FindEntityByName(NULL, "gm_txt_killed"));
	message.value.SetString(MAKE_STRING("Kills: 0"));
	m_ent_gm_txt_killed->InputDisplayText(message);

	m_ent_gm_txt_round = static_cast<CGameText*>(gEntList.FindEntityByName(NULL, "gm_txt_round"));
	message.value.SetString(MAKE_STRING("Round: 0"));
	m_ent_gm_txt_round->InputDisplayText(message);

	m_ent_pnt_spwn_zombie = static_cast<CPointTemplate*>   (gEntList.FindEntityByName(NULL, "pnt_spwn_zombie"));
	m_ent_plyr_speed      = static_cast<CMovementSpeedMod*>(gEntList.FindEntityByName(NULL, "plyr_speed"));
	m_ent_npc_zombie      = static_cast<CNPC_BaseZombie*>  (gEntList.FindEntityByName(NULL, "npc_zombie"));

	m_emotion_engine = new emophiz::CEmotionEngine();

	SetNextThink(gpGlobals->curtime); // Think now
}

//-----------------------------------------------------------------------------

void CDirector::set_zombie_speed(float val)
{
	//inputdata_t data;
	//data.value.SetFloat(val);
	//
	//m_ent_npc_zombie = static_cast<CNPC_BaseZombie*>  (gEntList.FindEntityByName(NULL, "npc_zombie"));
	//if (m_ent_npc_zombie) {
	//	Msg("zombie speed changed.\n");
	//	m_ent_npc_zombie->InputSetMovementValue(data);
	//}
	mp_player = ToBasePlayer( UTIL_GetCommandClient() );
	if (!mp_player)
		return;

	m_zombie_speed.SetFloat(val);
	Msg("Zombie speed set to %f\n", val);
	g_EventQueue.AddEvent("npc_zombie", "setmovementvalue", m_zombie_speed, 0, mp_player, mp_player);
}

//-----------------------------------------------------------------------------

void CDirector::InputStartEmotionEngine( inputdata_t &data )
{
	m_emotion_engine->start();
}

//-----------------------------------------------------------------------------

void CDirector::InputPrintEmotionValues( inputdata_t &data )
{
	m_emotion_engine->connect();
	Msg("Arousal: %f, Valence: %f, GSR: %f, HR: %f, BVP: %f, EMGFrown: %f, EMGSmile: %f, Fun: %f, Boredom: %f, Excitement: %f\n",
		m_emotion_engine->readArousal   (),
		m_emotion_engine->readValence   (),
		m_emotion_engine->readGSR       (),
		m_emotion_engine->readHR        (),
		m_emotion_engine->readBVP       (),
		m_emotion_engine->readEMGFrown  (),
		m_emotion_engine->readEMGSmile  (),
		m_emotion_engine->readFun       (),
		m_emotion_engine->readBoredom   (),
		m_emotion_engine->readExcitement());
}

//-----------------------------------------------------------------------------

void CDirector::InputSetZombieSpeed( inputdata_t &data )
{
	set_zombie_speed(data.value.Float());
}

//-----------------------------------------------------------------------------

void CDirector::set_player_speed(float val)
{
	mp_player = ToBasePlayer( UTIL_GetCommandClient() );
	if (!mp_player)
		return;
	m_player_speed.SetFloat(val);
	Msg("Player speed set to %f\n", val);
	g_EventQueue.AddEvent("plyr_speed", "modifyspeed", m_player_speed, 0, mp_player, mp_player);
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
 
	if ( m_nCounter < m_nThreshold )
	{
		if (m_ent_pnt_spwn_zombie)
		{
			int _min = min(int(m_nThreshold * 0.3), m_nThreshold - m_nCounter);
			_min = min(_min, m_nMaxAlive - m_nAlive);
			for (int i = 0; i < _min; ++i)
			{
				++m_nCounter;
				++m_nAlive;
				m_ent_pnt_spwn_zombie->InputForceSpawnRandom(inputdata_t());
			}
		}
	}
	else if ( m_nCounter == m_nThreshold && m_nAlive == 0)
	{
		m_nCounter = 0;
		
		++m_nRound;
		if (m_ent_gm_txt_round)
		{
			inputdata_t message;
			CFmtStrN<50> formatter;
			message.value.SetString(MAKE_STRING(formatter.sprintf("Round: %d", m_nRound)));
			m_ent_gm_txt_round->InputDisplayText(message);
		}

		m_nThreshold = int(powf(m_nThreshold, m_nIncreasePower));
		//m_OnNextRound.FireOutput( inputData.pActivator, this );
	}
	SetNextThink( gpGlobals->curtime + 1 ); // Think again in 1 second
}

//-----------------------------------------------------------------------------
