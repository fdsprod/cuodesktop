#include "stdafx.h"
#include "MemFinder.h"
#include "EncPatcher.h"
#include "OSIEncryption.h"
#include "LoginEncryption.h"

unsigned long OldRecv, OldSend, OldConnect, OldCloseSocket, OldCreateFileA/*, /*OldReadFileOld_lopen*/;
unsigned long RecvAddress, SendAddress, ConnectAddress, CloseSocketAddress, CreateFileAAddress/*, ReadFileAddress, _lopenAddress*/;

bool FirstSend = true, Seeded = false, LoginServer = false, Forwarding = false, Forwarded = false;
DWORD CryptSeed = 0;

SOCKET CurrentConnection = 0;

OSIEncryption *GameCrypt = NULL;
LoginEncryption *LoginCrypt = NULL;

HMODULE hInstance = NULL;

bool Unencrypted = true;
map<string,string> RedirectMap;

BOOL APIENTRY DllMain( HANDLE hModule, DWORD dwReason, LPVOID lpReserved )
{
	hInstance = (HMODULE)hModule;
	switch ( dwReason )
	{
	case DLL_PROCESS_ATTACH:
		DisableThreadLibraryCalls( hInstance );
		break;
	
	case DLL_PROCESS_DETACH:
		break;

	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
		break;
	}

	return TRUE;
}

__declspec(dllexport) void __stdcall Attach( DllParameters *params, int paramsLen )
{
	DWORD pos, pid = GetCurrentProcessId();
	MemFinder mf;

	if ( paramsLen == sizeof(DllParameters) && !IsBadReadPtr( params, paramsLen ) )
	{
		Unencrypted = params->PatchEncryption;

		params->RedirectMap[255] = '\0';

		ifstream fin( params->RedirectMap );

		while ( fin )
		{
			string line;

			getline( fin, line );

			if ( line.length() < 1 || line[0] == '#' )
				continue;

			size_t mid = line.find( '=' );

			if ( mid > 0 && mid < line.length() )
			{
				line = _strlwr( (char*)line.c_str() );
				RedirectMap.insert( map<string,string>::value_type( line.substr( 0, mid ), line.substr( mid+1 ) ) );
			}
		}
	}

	mf.AddEntry( "UoClientApp", 12, 0x00500000 );
	mf.AddEntry( "report\0", 8, 0x00500000 );
	mf.AddEntry( "Another copy of ", 16, 0x00500000 );
	mf.AddEntry( "\x00\x68\x88\x13\x00\x00\x56\xE8", 8 ); // (end of a push offset), push 5000, push esi
	if ( Unencrypted )
	{
		mf.AddEntry( CRYPT_KEY_STR, CRYPT_KEY_LEN );
		mf.AddEntry( CRYPT_KEY_STR_NEW, CRYPT_KEY_NEW_LEN );
	}

	mf.Execute();

	// Multi UO
	pos = mf.GetPosition( "UoClientApp", 12 );
	if ( pos )
	{
		DWORD old;
		VirtualProtect( (void*)pos, 12, PAGE_READWRITE, &old );
		_snprintf( (char*)pos, 12, "UoApp%d", pid );
		VirtualProtect( (void*)pos, 12, old, &old );
	}

	pos = mf.GetPosition( "Another copy of ", 16 );
	if ( pos )
	{
		char buff[5];
		
		buff[0] = 0x68; // push
		*((DWORD*)(&buff[1])) = pos;

		pos = 0x00400000;
		do {
			pos = MemFinder::Find( buff, 5, pos, 0x00600000 );
			if ( pos )
			{
				if ( (*((unsigned char*)(pos - 5))) == 0x74 ) // jz?
					MemoryPatch( pos-5, 0xEB, 1 ); // change to jmp
				pos += 5; // skip ahead to find the next instance
			}
		} while ( pos > 0 && pos < 0x00600000 );
	}

	pos = mf.GetPosition( "report\0", 8 );
	if ( pos )
	{
		DWORD old;
		VirtualProtect( (void*)pos, 12, PAGE_READWRITE, &old );
		_snprintf( (char*)pos, 8, "r%d", pid );
		VirtualProtect( (void*)pos, 12, old, &old );
	}

	// Splash screen crap:
	pos = mf.GetPosition( "\x00\x68\x88\x13\x00\x00\x56\xE8", 8 );
	if ( pos )
		MemoryPatch( pos+2, 0x00000005 ); // change 5000ms to 5ms

	// Login Encryption Keys:
	if ( Unencrypted )
	{
		pos = mf.GetPosition( CRYPT_KEY_STR, CRYPT_KEY_LEN );
		if ( !pos )
		{
			pos = mf.GetPosition( CRYPT_KEY_STR_NEW, CRYPT_KEY_NEW_LEN );

			if ( !pos )
			{
				pos = MemFinder::Find( CRYPT_KEY_STR_3D, CRYPT_KEY_3D_LEN );
				if ( pos )
					LoginEncryption::SetKeys( (const DWORD*)(pos + CRYPT_KEY_3D_LEN), (const DWORD*)(pos + CRYPT_KEY_3D_LEN + 19) );
			}
			else
			{
				pos += CRYPT_KEY_NEW_LEN;

				const DWORD *pKey1 = *((DWORD**)pos);
				const DWORD *pKey2 = pKey1 - 1;
				if ( !IsBadReadPtr( pKey2, 4 ) && !IsBadReadPtr( pKey1, 4 ) )
					LoginEncryption::SetKeys( pKey1, pKey2 );
			}
		}
		else
		{
			LoginEncryption::SetKeys( (const DWORD*)(pos + CRYPT_KEY_LEN), (const DWORD*)(pos + CRYPT_KEY_LEN + 6) );
		}
	}
	/*{
		pos = mf.GetPosition( CRYPT_KEY_STR, CRYPT_KEY_LEN );
		if ( pos )
		{
			LoginEncryption::SetKeys( (const BYTE*)(pos - 4), (const BYTE*)(pos + CRYPT_KEY_LEN), (const BYTE*)(pos + CRYPT_KEY_LEN + 6) );
		}
		else
		{
			pos = MemFinder::Find( CRYPT_KEY_STR_3D, CRYPT_KEY_3D_LEN );
			if ( pos )
				LoginEncryption::SetKeys( (const BYTE*)(pos - 4), (const BYTE*)(pos + CRYPT_KEY_3D_LEN), (const BYTE*)(pos + CRYPT_KEY_3D_LEN + 19) );
		}
	}*/

	// Encryption patching
	if ( Unencrypted )
	{
		HookFunction( "wsock32.dll", "closesocket", 3, (unsigned long)HookCloseSocket, &OldCloseSocket, &CloseSocketAddress );
		HookFunction( "wsock32.dll", "connect", 4, (unsigned long)HookConnect, &OldConnect, &ConnectAddress );
		HookFunction( "wsock32.dll", "recv", 16, (unsigned long)HookRecv, &OldRecv, &RecvAddress );
		HookFunction( "wsock32.dll", "send", 19, (unsigned long)HookSend, &OldSend, &SendAddress );
	}

	//HookFunction( "kernel32.dll", "_lopen", 0, (unsigned long)_lopenHook, &Old_lopen, &_lopenAddress );
	HookFunction( "kernel32.dll", "CreateFileA", 0, (unsigned long)CreateFileAHook, &OldCreateFileA, &CreateFileAAddress );
	//HookFunction( "kernel32.dll", "ReadFile", 0, (unsigned long)ReadFileHook, &OldReadFile, &ReadFileAddress );
}

/*HFILE WINAPI _lopenHook( LPCTSTR lpFileName, int iReadWrite )
{
	MessageBox( NULL, lpFileName, "_lopenHook", 0 );

	return ((_lopenFunc)Old_lopen)( lpFileName, iReadWrite );
}*/

/*BOOL WINAPI ReadFileHook( HANDLE hFile, LPVOID lpBuffer, DWORD nNumberOfBytesToRead, LPDWORD lpNumberOfBytesRead, LPOVERLAPPED lpOverlapped )
{
	return ((ReadFileFunc)OldReadFile)(hFile, lpBuffer, nNumberOfBytesToRead, lpNumberOfBytesRead, lpOverlapped);
}*/

void CreateEncryption()
{
	delete GameCrypt;
	delete LoginCrypt;

	GameCrypt = new OSIEncryption();
	LoginCrypt = new LoginEncryption();
}

int PASCAL HookRecv( SOCKET sock, char *buff, int len, int flags )
{
	int ackLen = (*(NetIOFunc)OldRecv)( sock, buff, len, flags );

 	if ( sock == CurrentConnection && len > 0 )
	{
		if ( LoginServer )
		{
			if ( ((BYTE)buff[0]) == ((BYTE)0x8C) )
				LoginServer = false;

			if ( Forwarding )
				Seeded = Forwarded = true;
		}
		else
		{
			GameCrypt->EncryptForClient( (BYTE*)buff, (BYTE*)buff, ackLen );
		}
	}

	return ackLen;
}

int PASCAL HookSend( SOCKET sock, char *buff, int len, int flags )
{
	if ( sock == CurrentConnection && CurrentConnection && len > 0 )
	{
		if ( !Seeded )
		{
			Seeded = true;

			if ( len >= 4 )
			{
				CryptSeed = *((DWORD*)buff);
				GameCrypt->Initialize( CryptSeed );
				LoginCrypt->Initialize( (BYTE*)&CryptSeed );
			}
		}
		else
		{
			if ( FirstSend )
			{
				FirstSend = false;

				LoginServer = LoginCrypt->Test( (BYTE)buff[0] ) == ((BYTE)0x80);
			}

			if ( Forwarded )
			{
				CryptSeed = LoginEncryption::GenerateBadSeed( CryptSeed );

				GameCrypt->Initialize( CryptSeed );

				GameCrypt->DecryptFromClient( (BYTE*)buff, (BYTE*)buff, len );
				LoginCrypt->Decrypt( (BYTE*)buff, (BYTE*)buff, len );

				LoginServer = false;
				Forwarding = false;
				Forwarded = false;
			}
			else
			{
				if ( LoginServer )
				{
					LoginCrypt->Decrypt( (BYTE*)buff, (BYTE*)buff, len );
					
					//char temp[256];
					//sprintf( temp, "Packet: %02X", (BYTE)buff[0] );
					//MessageBox( NULL, temp, "blah", 0 );
					
					if ( ((BYTE)buff[0]) == 0xA0 )
						Forwarding = true;
				}
				else
				{
					GameCrypt->DecryptFromClient( (BYTE*)buff, (BYTE*)buff, len );
				}
			}
		}
	}
	
	return (*(NetIOFunc)OldSend)( sock, buff, len, flags );
}

int PASCAL HookConnect( SOCKET sock, const sockaddr *addr, int addrlen )
{
	int retVal = (*(ConnFunc)OldConnect)( sock, addr, addrlen );

	if ( retVal != SOCKET_ERROR )
	{
		CreateEncryption();

		Seeded = false;
		LoginServer = false;
		FirstSend = true;
		Forwarding = false;
		Forwarded = false;

		CurrentConnection = sock;
	}

	return retVal;
}

int PASCAL HookCloseSocket( SOCKET sock )
{
	int retVal = (*(CLSFunc)OldCloseSocket)( sock );

	if ( sock == CurrentConnection && sock != 0 )
		CurrentConnection = 0;

	return retVal;
}

HANDLE WINAPI CreateFileAHook( LPCTSTR lpFileName, DWORD dwDesiredAccess, DWORD dwShareMode, LPSECURITY_ATTRIBUTES lpSecurityAttributes, DWORD dwCreationDisposition, DWORD dwFlagsAndAttributes, HANDLE hTemplateFile )
{
	if ( RedirectMap.size() > 0 )
	{
		string toOpen = _strlwr( (char*)lpFileName );
		size_t slash = toOpen.find_last_of( '\\' );

		if ( slash > 0 && slash < toOpen.length() )
			toOpen = toOpen.substr( slash + 1 );
		slash = toOpen.find_last_of( '/' );
		if ( slash > 0 && slash < toOpen.length() )
			toOpen = toOpen.substr( slash + 1 );

		map<string,string>::iterator iter = RedirectMap.find( toOpen );

		/*if(iter != RedirectMap.end())
		{
			string redirect = _strlwr( (char*)iter->second.c_str() );			

			if ( toOpen == "body.def" )
			{
				MessageBox( NULL, lpFileName, "EncPatcher - Original", 0 );
				MessageBox( NULL, redirect.c_str(), "EncPatcher - Redirect", 0 );
				MessageBox( NULL, iter->first.c_str(), "EncPatcher - First", 0 );
			}
		}*/

		if ( iter != RedirectMap.end() )
			return ((CreateFileAFunc)OldCreateFileA)( iter->second.c_str(), dwDesiredAccess, dwShareMode, lpSecurityAttributes, dwCreationDisposition, dwFlagsAndAttributes, hTemplateFile );
	}
	
	return ((CreateFileAFunc)OldCreateFileA)( lpFileName, dwDesiredAccess, dwShareMode, lpSecurityAttributes, dwCreationDisposition, dwFlagsAndAttributes, hTemplateFile );
}


void MemoryPatch( unsigned long Address, unsigned long value )
{
	MemoryPatch( Address, &value, 4 ); // sizeof(int)
}

void MemoryPatch( unsigned long Address, int value, int numBytes )
{
	MemoryPatch( Address, &value, numBytes ); 
}

void MemoryPatch( unsigned long Address, const void *value, int length )
{
	DWORD OldProtect;
	if ( !VirtualProtect( (void *)Address, length, PAGE_READWRITE, &OldProtect ) )
		return;

	memcpy( (void *)Address, value, length );

	VirtualProtect( (void *)Address, length, OldProtect, &OldProtect );
}

bool HookFunction( const char *Dll, const char *FuncName, int Ordinal, unsigned long NewAddr,
						unsigned long *OldAddr, unsigned long *PatchAddr )
{
    DWORD image_base = (DWORD)GetModuleHandle(NULL);
	if ( !image_base ) 
		return false;

	DWORD dwOldFuncFound = NULL;

    IMAGE_DOS_HEADER *idh = (IMAGE_DOS_HEADER *)image_base;

    IMAGE_FILE_HEADER *ifh = (IMAGE_FILE_HEADER *)(image_base + idh->e_lfanew + sizeof(DWORD));

    IMAGE_OPTIONAL_HEADER *ioh = (IMAGE_OPTIONAL_HEADER *)((DWORD)(ifh) + sizeof(IMAGE_FILE_HEADER));

    IMAGE_IMPORT_DESCRIPTOR *iid = (IMAGE_IMPORT_DESCRIPTOR *)(image_base + ioh->DataDirectory[IMAGE_DIRECTORY_ENTRY_IMPORT].VirtualAddress);

	if ( Ordinal <= 0 )
	{
		HANDLE hMod = GetModuleHandle( Dll );
	
		if ( hMod != NULL && FuncName != NULL )
			dwOldFuncFound = (DWORD)GetProcAddress( (HMODULE)hMod, FuncName );
	}

    while( iid->Name )
    {
        if( _stricmp( Dll, (char *)(image_base + iid->Name) ) == 0 )
        {
            IMAGE_THUNK_DATA * pThunk = (IMAGE_THUNK_DATA *)((DWORD)iid->OriginalFirstThunk + image_base);
            IMAGE_THUNK_DATA * pThunk2 = (IMAGE_THUNK_DATA *)((DWORD)iid->FirstThunk + image_base);

            while( pThunk->u1.AddressOfData )
            {
                char *name = NULL;
                int ord;

                if( pThunk->u1.Ordinal & 0x80000000 )
				{
					// Imported by ordinal only:
                    ord = pThunk->u1.Ordinal & 0xFFFF;
				}
                else
                {
					// Imported by name (with ordinal hint)
                    IMAGE_IMPORT_BY_NAME * pName = (IMAGE_IMPORT_BY_NAME *)((DWORD)pThunk->u1.AddressOfData + image_base);
                    ord = pName->Hint;
                    name = (char *)pName->Name;
                }

                if ( ord == Ordinal ||
					( name && FuncName && !strcmp( name, FuncName ) ) || 
					( dwOldFuncFound != NULL && pThunk2->u1.Function == dwOldFuncFound ) )
                {
					*OldAddr = (unsigned long)pThunk2->u1.Function;
					*PatchAddr = (unsigned long)(&pThunk2->u1.Function);
					MemoryPatch( *PatchAddr, NewAddr );

					return true;
                }

				pThunk++;
                pThunk2++;
            }
		}
        iid++;
    }

	return false;
}

