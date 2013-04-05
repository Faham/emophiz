//========= Copyright © 1996-2005, Valve Corporation, All rights reserved. ============//
//
// Purpose: 
//
// $NoKeywords: $
//
//=============================================================================//

#ifndef MAPRULES_H
#define MAPRULES_H

#ifdef CLIENT_DLL

	#include "c_baseentity.h"
	
	#define CGameRules C_GameRules
	#define CGameRulesProxy C_GameRulesProxy

#else
	
	#include "baseentity.h"
	#include "recipientfilter.h"

#endif

class CRuleEntity : public CBaseEntity
{
public:
	DECLARE_CLASS( CRuleEntity, CBaseEntity );

	void	Spawn( void );

	DECLARE_DATADESC();

	void	SetMaster( string_t iszMaster ) { m_iszMaster = iszMaster; }

protected:
	bool	CanFireForActivator( CBaseEntity *pActivator );

private:
	string_t	m_iszMaster;
};

// 
// CRulePointEntity -- base class for all rule "point" entities (not brushes)
//
class CRulePointEntity : public CRuleEntity
{
public:
	DECLARE_DATADESC();
	DECLARE_CLASS( CRulePointEntity, CRuleEntity );

	int		m_Score;
	void		Spawn( void );
};

//
// CGameText / game_text	-- NON-Localized HUD Message (use env_message to display a titles.txt message)
//	Flag: All players					SF_ENVTEXT_ALLPLAYERS
//
#define SF_ENVTEXT_ALLPLAYERS			0x0001

class CGameText : public CRulePointEntity
{
public:
	DECLARE_CLASS( CGameText, CRulePointEntity );

	bool	KeyValue( const char *szKeyName, const char *szValue );

	DECLARE_DATADESC();

	inline	bool	MessageToAll( void ) { return (m_spawnflags & SF_ENVTEXT_ALLPLAYERS); }
	inline	void	MessageSet( const char *pMessage ) { m_iszMessage = AllocPooledString(pMessage); }
	inline	const char *MessageGet( void )	{ return STRING( m_iszMessage ); }

	void InputDisplay( inputdata_t &inputdata );
	void InputDisplayText( inputdata_t &inputdata );
	void Display( CBaseEntity *pActivator );

	void Use( CBaseEntity *pActivator, CBaseEntity *pCaller, USE_TYPE useType, float value )
	{
		Display( pActivator );
	}

private:

	string_t m_iszMessage;
	hudtextparms_t	m_textParms;
};

#endif		// MAPRULES_H

