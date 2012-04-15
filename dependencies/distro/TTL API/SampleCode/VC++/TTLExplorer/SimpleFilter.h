// SimpleFilter.h
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_SIMPLEFILTER_H__1BA203D0_2088_44CF_B7D6_330D852512C6__INCLUDED_)
#define AFX_SIMPLEFILTER_H__1BA203D0_2088_44CF_B7D6_330D852512C6__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000

class CSimpleFilter
{
public:

	typedef double real_t;
	typedef float data_t;

	enum
	{ MAX_CHANNELS = 20
	, MAX_SECTIONS = 10
	};

	struct SECTION
	{
		// numerator quadratic
		real_t m_b1;
		real_t m_b2;
		// denominator quadratic
		real_t m_a1;
		real_t m_a2;
	};

	struct BIQUAD
	{
		real_t m_nGain;
		int m_cSections;
		SECTION m_aSections[ MAX_SECTIONS ];
	};

	CSimpleFilter()
	{
		ClearFilter();
	}

	void ClearFilter( void )
	{
		m_bq.m_nGain = 1;
		m_bq.m_cSections = 0;

		for ( int nChannel = 0; nChannel < MAX_CHANNELS; ++nChannel )
			for ( int s = 0; s < m_bq.m_cSections; ++s )
			{
				m_aT1[ nChannel ][ s ] = 0.0;
				m_aT2[ nChannel ][ s ] = 0.0;
			}
	}

	void SetFilter( const BIQUAD & rFilter )
	{
		ClearFilter();
		m_bq = rFilter;
	}

	void Filter( int nChannel, data_t* pData, int cData )
	{
		if ( nChannel < 0 || MAX_CHANNELS <= nChannel ) return;

		for ( int i = 0; i < cData; ++i )
		{
			real_t *pT1  = m_aT1[ nChannel ];
			real_t *pT2  = m_aT2[ nChannel ];
			SECTION *pS = m_bq.m_aSections;
			real_t y = pData[ i ] / m_bq.m_nGain;
			for ( int s = 0; s < m_bq.m_cSections; ++s, ++pS )
			{
				real_t x = y;
				y += *pT1;
				*pT1++ = pS->m_b1 * x - pS->m_a1 * y + *pT2;
				*pT2++ = pS->m_b2 * x - pS->m_a2 * y;
			}
			pData[ i ] = (data_t)( y );
		}
	}

private:

	BIQUAD m_bq;

	real_t m_aT1[ MAX_CHANNELS ][ MAX_SECTIONS ];
	real_t m_aT2[ MAX_CHANNELS ][ MAX_SECTIONS ];
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_SIMPLEFILTER_H__1BA203D0_2088_44CF_B7D6_330D852512C6__INCLUDED_)
