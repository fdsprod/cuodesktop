#pragma once

#include "TwoFish.h"

void MD5( const BYTE* data, int dataLen, BYTE *hash );

class OSIEncryption
{
public:
	OSIEncryption();

	void Initialize( DWORD dwSeed );

	void DecryptFromServer( const BYTE *in, BYTE *out, int len );
	void DecryptFromClient( const BYTE *in, BYTE *out, int len );

	void EncryptForServer( const BYTE *in, BYTE *out, int len );
	void EncryptForClient( const BYTE *in, BYTE *out, int len );

private:
	void TwoFishCrypt( const BYTE *in, BYTE *out, int len );
	void ReinitTFTable();

	void XORCrypt( const BYTE *in, BYTE *out, int len );
	void InitializeXORTable( const BYTE* data, int dataLen );
	void Hash( BYTE *field, const BYTE *param ) const;
	void CallHash( BYTE *key, const BYTE *challenge, int len ) const;
	void CalcResponse( BYTE *result, BYTE *field ) const;

	keyInstance m_Key; 
	cipherInstance m_Cipher; 
	unsigned int m_TFPos;
	unsigned char m_XORPos;
	BYTE m_TFTable[256];
	BYTE m_XORTable[16];
};

