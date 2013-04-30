// viewgraph.h : interface of the CViewGraph class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_VIEWGRAPH_H__1BA203D0_2088_44CF_B7D6_330D852512C6__INCLUDED_)
#define AFX_VIEWGRAPH_H__1BA203D0_2088_44CF_B7D6_330D852512C6__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000

class CViewGraph : public CWindowImpl<CViewGraph>
{
public:

	typedef CWindowImpl<CViewGraph> CWindowImpl_type;

	typedef float data_t;

	// We want to change the brush of the background, and not to a windows color,
	// so we have to expand explicitly the DECLARE_WND_CLASS...
	// DECLARE_WND_CLASS( T("WtlSample:TTLExplorer:GraphView") )
	static CWndClassInfo& GetWndClassInfo()
	{
		static CWndClassInfo wc =
		{
			{ sizeof( WNDCLASSEX )
			, CS_HREDRAW | CS_VREDRAW | CS_DBLCLKS
			, StartWindowProc
			, 0, 0, NULL, NULL, NULL
			, (HBRUSH)( ::GetStockObject( BLACK_BRUSH ) )
			, NULL
			, _T("WtlSample:TTLExplorer:ViewGraph")
			, NULL
			}
			, NULL, NULL, IDC_ARROW, TRUE, 0, _T("")
		};
		return wc;
	}

	bool OnStart( int nTickRate, int cLines, double nScale );
	void OnDataBegin( int cNewPoints );
	void OnData( int nLine, data_t* pPoints, int cPoints );
	void OnDataEnd( void );
	void OnStop( void );

protected:

	enum
	{
	  PERIOD = 1000 /*ms*/
	, INTEGER_SCALE = 1024 /* arbitrary units, about 0.5px */
	, MAX_LINES = 24
	, MAX_TICKRATE = 2048
	, MAX_POINTS = MAX_TICKRATE*PERIOD/1000
	};

	int m_xMax; // the integer x coordinate of the right edge
	int m_yMax; // the integer y coordinate of the top edge
	double m_nScale; // the floating point value of the top edge
	CString m_szScale; // string representation of m_nScale
	double m_nMultiplier; // m_yMax (==INTEGER_SCALE) == m_nScale * m_nMultiplier

	int m_cLines;
	int m_cToEnd;
	bool m_bWrap;
	CPen m_aPens[ MAX_LINES ];
	CPoint m_aPoints[ MAX_LINES ][ MAX_POINTS ];
	data_t m_nYs[ MAX_LINES ][ MAX_POINTS ];

	bool m_bCanUpdate;
	bool m_bCanAddPoints;
	int m_cNewPoints;
	int m_iNewStart;

	static COLORREF GetColor( int i )
	{
		static s_aColors[ MAX_LINES ] =
		{ RGB(0xff,0x00,0x00), RGB(0x00,0xff,0x00), RGB(0x00,0x00,0xff)
		, RGB(0x00,0xff,0xff), RGB(0xff,0x00,0xff), RGB(0xff,0xff,0x00)
		, RGB(0xff,0x80,0x00), RGB(0x80,0xff,0x00), RGB(0x80,0x00,0xff)
		, RGB(0xff,0x00,0x80), RGB(0x00,0xff,0x80), RGB(0x00,0x80,0xff)
		, RGB(0xff,0xc0,0x80), RGB(0xc0,0xff,0x80), RGB(0xc0,0x80,0xff)
		, RGB(0xff,0x80,0xc0), RGB(0x80,0xff,0xc0), RGB(0x80,0xc0,0xff)
		, RGB(0xff,0x80,0x80), RGB(0x80,0xff,0x80), RGB(0x80,0x80,0xff)
		, RGB(0x80,0xff,0xff), RGB(0xff,0x80,0xff), RGB(0xff,0xff,0x80)
		};
		return s_aColors[ i ];
	}

	BEGIN_MSG_MAP_EX( CViewGraph )
		MSG_WM_PAINT( OnPaint )
		MSG_WM_CREATE( OnCreate )
		MSG_WM_DESTROY( OnDestroy )
	END_MSG_MAP()

	void OnPaint( CDCHandle /*dcCtrl*/ );
	LRESULT OnCreate( LPCREATESTRUCT /*lpCreateStruct*/ );
	void OnDestroy( void );
};


/////////////////////////////////////////////////////////////////////////////

inline bool CViewGraph::OnStart( int nTickRate, int cLines, double nScale )
{
	if ( nTickRate < 10 || MAX_TICKRATE < nTickRate ) return false;
	if ( cLines < 0 || MAX_LINES < cLines ) return false;
	m_xMax = ::MulDiv( nTickRate, PERIOD, 1000 );
	m_yMax = INTEGER_SCALE;
	m_nScale = nScale;
	m_szScale.Format( _T("%.4f "), m_nScale );
	m_nMultiplier = m_yMax / m_nScale;
	m_cLines = cLines;
	m_cToEnd = 0;
	m_bWrap = false;
	for ( int L = 0; L < m_cLines; ++L )
	{
		for ( int i = 0; i < m_xMax; ++i )
		{
			m_aPoints[ L ][ i ].x = i;
			m_nYs[ L ][ i ] = 0.0;
		}
	}
	m_bCanUpdate = true;
	return true;
}

inline void CViewGraph::OnDataBegin( int cNewPoints )
{
	if ( ! m_bCanUpdate ) return;
	m_cNewPoints = cNewPoints % m_xMax;
	m_iNewStart = m_cToEnd;
	m_bCanAddPoints = true;
}

inline void CViewGraph::OnData( int nLine, data_t* pPoints, int cPoints )
{
	if ( ! m_bCanAddPoints ) return;
	if ( nLine < 0 || m_cLines <= nLine ) return;
	data_t* pYs = m_nYs[ nLine ];
	if ( m_cNewPoints < cPoints ) cPoints = m_cNewPoints; // drop extra points
	int x = m_iNewStart;
	data_t y = 0;
	for ( int i = 0; i < cPoints; ++i )
	{
		y = pYs[ x ] = pPoints[ i ];
		++x; if ( m_xMax <= x ) x = 0;
	}
	for ( /**/; i < m_cNewPoints; ++i )
	{
		pYs[ x ] = y;
		++x; if ( m_xMax <= x ) x = 0;
	}
}

inline void CViewGraph::OnDataEnd( void )
{
	if ( ! m_bCanAddPoints ) return;
	int x = m_iNewStart;
	for ( int i = 0; i < m_cNewPoints; ++i )
	{
		for ( int L = 0; L < m_cLines; ++L )
		{
			m_aPoints[ L ][ x ].y = (int)( m_nYs[ L ][ x ] * m_nMultiplier );
		}
		++x;
		if ( x < m_xMax ) continue;
		x = 0;
		m_bWrap = true;
	}
	m_cToEnd = x; // == ( m_iNewStart + m_cNewPoints ) % m_xMax
	m_bCanAddPoints = false;
	Invalidate();
}

inline void CViewGraph::OnStop( void )
{
	m_bCanAddPoints = false;
	m_bCanUpdate = false;
}


/////////////////////////////////////////////////////////////////////////////

inline LRESULT CViewGraph::OnCreate( LPCREATESTRUCT /*lpCreateStruct*/ )
{
	for ( int L = 0; L < MAX_LINES; ++L )
		m_aPens[ L ].CreatePen( PS_SOLID, 0, GetColor( L ) );

	bool bOk =
	OnStart( MAX_TICKRATE, 0, 1.0 );
	OnDataBegin( 0 );
	OnDataEnd();
	OnStop();

	bOk; ATLASSERT( bOk );

	return 0;
}

inline void CViewGraph::OnDestroy( void )
{
	for ( int L = 0; L < MAX_LINES; ++L )
		m_aPens[ L ].DeleteObject();
	SetMsgHandled( FALSE );
}

inline void CViewGraph::OnPaint( CDCHandle /*dcCtrl*/ )
{
	CPaintDC dc( m_hWnd );

	CRect rc;
	GetClientRect( rc );

	// write scale in upper right
	HFONT holdFont =
	dc.SelectStockFont( DEFAULT_GUI_FONT );
	dc.SetBkMode( TRANSPARENT );
	dc.SetTextColor( RGB(0xFF,0xFF,0xFF) );
	dc.DrawText( m_szScale, -1, rc, DT_TOP | DT_RIGHT | DT_SINGLELINE | DT_NOPREFIX | DT_NOCLIP );

	// set map mode for normal plotting
	dc.SetMapMode( MM_ANISOTROPIC );
	dc.SetViewportOrg( rc.left, ( rc.top + rc.bottom ) / 2 );
	dc.SetWindowExt( m_xMax, m_yMax );
	dc.SetViewportExt( rc.Width(), -rc.Height()/2 );

	// draw axis
	HPEN holdPen =
	dc.SelectStockPen( WHITE_PEN );
	dc.MoveTo( 0, 0 ); dc.LineTo( m_xMax, 0 );
	dc.MoveTo( m_cToEnd, -m_yMax ); dc.LineTo( m_cToEnd, +m_yMax );
	// note: with pixel round-offs etc., the line could be shorter by one pixel

	// draw lines
	for ( int L = 0; L < m_cLines; ++L )
	{
		dc.SelectPen( m_aPens[ L ] );
		dc.Polyline( m_aPoints[ L ], m_cToEnd );
		if ( ! m_bWrap ) continue;
		dc.Polyline( m_aPoints[ L ] + m_cToEnd, m_xMax - m_cToEnd );
	}

	dc.SelectPen( holdPen );
	dc.SelectFont( holdFont );
}


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_VIEWGRAPH_H__1BA203D0_2088_44CF_B7D6_330D852512C6__INCLUDED_)
