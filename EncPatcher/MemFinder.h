#pragma pack(1)
#pragma once

#include <vector>
using std::vector;

class MemFinder
{
public:
	static DWORD Find( const void *data, int length, DWORD hint = 0x00400000, DWORD maxAddress = 0x00FFFFFF );

	MemFinder();
	~MemFinder();

	void AddEntry( const void *data, int length, short maxResults, DWORD hint );
	void AddEntry( const void *data, int length, DWORD hint = 0x00400000 ) { AddEntry( data, length, 1, hint ); }

	DWORD GetPosition( const void *data, int length, unsigned int idx = 0 );

	void Execute();

	void Clear();

private:
	struct Entry
	{
		char *Data;
		int Length;
		DWORD PositionHint;
		int MaxResults;
		vector<DWORD> Results;
	};

	vector<Entry> _Entries;
	bool _Executed;
	DWORD _StartPos;
};
