// SimpleRecorder.h : interface of the CSimpleRecorder class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_SIMPLERECORDER_H__1BA203D0_2088_44CF_B7D6_330D852512C6__INCLUDED_)
#define AFX_SIMPLERECORDER_H__1BA203D0_2088_44CF_B7D6_330D852512C6__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000

class CSimpleRecorder
{
public:

	typedef float data_t;

	CSimpleRecorder()
		: m_pFile( NULL )
		, m_cChannels( 0 )
		, m_cChannelData( 0 )
	{
	}

	~CSimpleRecorder()
	{
		ATLASSERT( NULL == m_pFile );
	}

	void OnStart( bool bRecord, int cChannels, LPCTSTR szFile = _T("record.csv") )
	{
		OnStop();
		if ( ! bRecord ) return;
		ATLASSERT( 0 <= cChannels && cChannels <= MAX_CHANNELS );
		if ( 1 > cChannels ) return;
		if ( MAX_CHANNELS < cChannels ) cChannels = MAX_CHANNELS;
		m_cChannels = cChannels;
		m_pFile = fopen( szFile, _T("wt") );
		m_nSample = 0;
	}

	void OnStop( void )
	{
		if ( NULL == m_pFile ) return;
		fclose( m_pFile );
		m_pFile = NULL;
	}

	void OnDataBegin( int cData )
	{
		if ( NULL == m_pFile ) return;
		ATLASSERT( 0 <= cData && cData <= MAX_CHANNEL_BUFFER );
		if ( 1 > cData ) return;
		if ( MAX_CHANNEL_BUFFER < cData ) cData = MAX_CHANNEL_BUFFER;
		m_cChannelData = cData;
		for ( int c = 0; c < m_cChannels; ++c )
		{
			::ZeroMemory( m_aChannelData[ c ], sizeof(data_t)*m_cChannelData );
		}
	}

	void OnData( int nChannel, data_t* pData, int cData )
	{
		if ( NULL == m_pFile ) return;
		if ( nChannel < 0 || m_cChannels < nChannel ) return;
		if ( 1 > cData ) return;
		if ( m_cChannelData < cData ) cData = m_cChannelData;
		::CopyMemory( m_aChannelData[ nChannel ], pData, sizeof(data_t)*cData );
	}

	void OnDataEnd( void )
	{
		if ( NULL == m_pFile ) return;
		for ( int i = 0; i < m_cChannelData; ++i )
		{
			fprintf( m_pFile, _T("%ld"), m_nSample + i );
			for ( int c = 0; c < m_cChannels; ++c )
			{
				fprintf( m_pFile, _T(",%g"), m_aChannelData[ c ][ i ] );
			}
			fprintf( m_pFile, _T("\n") );
		}
		fflush( m_pFile );
		m_nSample += m_cChannelData;
		m_cChannelData = 0;
	}

protected:

	enum
	{
		MAX_ENCODERS = 2
	,	MAX_CHANNELS_PER_ENCODER = 10
	,	MAX_CHANNELS = MAX_ENCODERS * MAX_CHANNELS_PER_ENCODER
	,	MAX_CHANNEL_BUFFER = 2048
	};

	FILE * m_pFile;

	int m_cChannels;
	int m_cChannelData;
	data_t m_aChannelData[ MAX_CHANNELS ][ MAX_CHANNEL_BUFFER ];

	long m_nSample;
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_SIMPLERECORDER_H__1BA203D0_2088_44CF_B7D6_330D852512C6__INCLUDED_)
