#include "cbase.h"

class CDirector : public CLogicalEntity
{
public:
	DECLARE_CLASS( CDirector, CLogicalEntity );

	DECLARE_DATADESC();
 
	// Constructor
	CDirector ()
	{
		m_nCounter = 0;
	}
 
	// Input function
	void InputTick( inputdata_t &inputData );

private:
	int	m_nThreshold; // Count at which to fire our output
	int	m_nCounter;   // Internal counter
 
	COutputEvent m_OnThreshold;	// Output event when the counter reaches the threshold
};

LINK_ENTITY_TO_CLASS( director, CDirector );

// Start of our data description for the class
BEGIN_DATADESC( CDirector )
 
	// For save/load
	DEFINE_FIELD( m_nCounter, FIELD_INTEGER ),
 
	// As above, and also links our member variable to a Hammer keyvalue
	DEFINE_KEYFIELD( m_nThreshold, FIELD_INTEGER, "threshold" ),
 
	// Links our input name from Hammer to our input member function
	DEFINE_INPUTFUNC( FIELD_VOID, "Tick", InputTick ),
 
	// Links our output member variable to the output name used by Hammer
	DEFINE_OUTPUT( m_OnThreshold, "OnThreshold" ),
 
END_DATADESC()

//-----------------------------------------------------------------------------
// Purpose: Handle a tick input from another entity
//-----------------------------------------------------------------------------
void CDirector::InputTick( inputdata_t &inputData )
{
	// Increment our counter
	++m_nCounter;
 
	// See if we've met or crossed our threshold value
	if ( m_nCounter >= m_nThreshold )
	{
		// Fire an output event
		m_OnThreshold.FireOutput( inputData.pActivator, this );
 
		// Reset our counter
		m_nCounter = 0;
	}
}
