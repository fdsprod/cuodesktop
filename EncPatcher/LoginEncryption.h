#pragma once

class LoginEncryption
{
public:
	LoginEncryption();

	void Initialize( const BYTE *pSeed );

	static void SetKeys( const DWORD *k1, const DWORD *k2 );

	void Encrypt( const BYTE *in, BYTE *out, int len );
	void Decrypt( const BYTE *in, BYTE *out, int len );

	BYTE Test( BYTE );

	static DWORD GenerateBadSeed( DWORD oldSeed );
private:
	static const DWORD *Key1, *Key2;

	BYTE Crypt( BYTE );

	DWORD m_Table[2];
};

// login encryption... search disassembly for
//XX XX XX XX C1 E2 1F D1 E8 D1 E9 0B C6 0B CA 35 XX XX XX XX 81 F1 XX XX XX XX 4D
//static key1 -- -- -- -- -- -- -- -- -- -- -- -- static key2 -- -- dynamic key --   
#define CRYPT_KEY_STR "\xC1\xE2\x1F\xD1\xE8\xD1\xE9\x0B\xC6\x0B\xCA\x35"
#define CRYPT_KEY_LEN 12

//static key1 D1 E8 0B C6 C1 E2 1F 35 static key2 D1 E9 89 83 F0 00 42 00 8B 45 08 0B CA 81 F1 dynamic key 48
#define CRYPT_KEY_STR_3D "\xD1\xE8\x0B\xC6\xC1\xE2\x1F\x35"
#define CRYPT_KEY_3D_LEN 8

/*
.text:0041AA2F C1 E6 1F                          shl     esi, 31
.text:0041AA32 D1 E8                             shr     eax, 1
.text:0041AA34 0B C6                             or      eax, esi
.text:0041AA36 47                                inc     edi
.text:0041AA37 33 05 BC 29 6B 00                 xor     eax, LoginKey_2
.text:0041AA3D C1 E2 1F                          shl     edx, 1Fh
.text:0041AA40 89 83 F8 00 0A 00                 mov     [ebx+0A00F8h], eax
.text:0041AA46 D1 E8                             shr     eax, 1
.text:0041AA48 0B C6                             or      eax, esi
.text:0041AA4A 8B 35 BC 29 6B 00                 mov     esi, LoginKey_2
.text:0041AA50 33 C6                             xor     eax, esi
.text:0041AA52 D1 E9                             shr     ecx, 1
.text:0041AA54 89 83 F8 00 0A 00                 mov     [ebx+0A00F8h], eax
.text:0041AA5A 0B CA                             or      ecx, edx
.text:0041AA5C 8B 15 B8 29 6B 00                 mov     edx, LoginKey_1
*/
// -- -- -- -- -- --
// 1F D1 E8 0B C6 47 33 05 memoryloc_2 C1 E2 1F 89 83 F8 00 0A 00 D1 E8 0B C6 8B 35 memoryloc_2 33 C6 D1 E9 89 83 F8 00 0A 00 0B Ca 8b 15 memoryloc_1 33 CA
#define CRYPT_KEY_STR_NEW "\x1F\xD1\xE8\x0B\xC6\x47\x33\x05"
#define CRYPT_KEY_NEW_LEN 8
