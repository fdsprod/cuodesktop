
#ifdef ENCPATCHER_EXPORTS
#define DLLFUNCTION __declspec(dllexport)
#else
#define DLLFUNCTION __declspec(dllimport)
#endif

#pragma pack(1)

typedef int (PASCAL *NetIOFunc)(SOCKET, char *, int, int);
typedef int (PASCAL *ConnFunc)(SOCKET, const sockaddr *, int);
typedef int (PASCAL *CLSFunc)(SOCKET);
typedef HANDLE (WINAPI *CreateFileAFunc)(LPCTSTR,DWORD,DWORD,LPSECURITY_ATTRIBUTES,DWORD,DWORD,HANDLE);
//typedef HFILE (WINAPI *_lopenFunc)(LPCTSTR,int);
//typedef BOOL (WINAPI *ReadFileFunc)( HANDLE, LPVOID, DWORD, LPDWORD, LPOVERLAPPED );

bool HookFunction( const char *Dll, const char *FuncName, int Ordinal, unsigned long NewAddr, unsigned long *OldAddr, unsigned long *PatchAddr );
void MemoryPatch( unsigned long Address, unsigned long value );
void MemoryPatch( unsigned long Address, int value, int numBytes );
void MemoryPatch( unsigned long Address, const void *value, int length );

//Hooks:
int PASCAL HookRecv( SOCKET, char *, int, int );
int PASCAL HookSend( SOCKET, char *, int, int );
int PASCAL HookConnect( SOCKET, const sockaddr *, int );
int PASCAL HookCloseSocket( SOCKET );
HANDLE WINAPI CreateFileAHook( LPCTSTR, DWORD, DWORD, LPSECURITY_ATTRIBUTES, DWORD, DWORD, HANDLE );
//HFILE WINAPI _lopenHook( LPCTSTR, int);
//BOOL WINAPI ReadFileHook( HANDLE, LPVOID, DWORD, LPDWORD, LPOVERLAPPED );

typedef struct
{
	bool PatchEncryption;
	char RedirectMap[256];
}  DllParameters;

