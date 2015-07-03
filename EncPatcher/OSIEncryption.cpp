#include "stdafx.h"

#include "EncPatcher.h"
#include "OSIEncryption.h"
#include "TwoFish.h"

OSIEncryption::OSIEncryption()
{
	memset( &m_Key, 0, sizeof(m_Key) );
	memset( &m_Cipher, 0, sizeof(m_Cipher) );
	memset( m_TFTable, 0, 256 );
	memset( m_XORTable, 0, 16 );

	m_XORPos = m_TFPos = 0;
}

void OSIEncryption::Initialize( DWORD Seed )
{
	memset( &m_Key, 0, sizeof(m_Key) );
	memset( &m_Cipher, 0, sizeof(m_Cipher) );
	memset( m_TFTable, 0, 256 );

	makeKey( &m_Key, DIR_DECRYPT, 0x80, NULL );
	m_Key.key32[0] = m_Key.key32[1] = m_Key.key32[2] = m_Key.key32[3] = Seed;
	reKey( &m_Key );
	cipherInit( &m_Cipher, MODE_ECB, NULL );

	for(int i=0;i<256;i++)
		m_TFTable[i] = i;
	ReinitTFTable();

	MD5( m_TFTable, 256, m_XORTable );
	m_XORPos = 0;
}

void OSIEncryption::ReinitTFTable()
{
	unsigned char tmpBuff[256];
	blockEncrypt( &m_Cipher, &m_Key, m_TFTable, 256*8, tmpBuff );
	memcpy( m_TFTable, tmpBuff, 256 );
	m_TFPos = 0;
}

//going TO the client uses rotating XOR, going FROM the client uses TwoFish
void OSIEncryption::XORCrypt( const BYTE *in, BYTE *out, int len )
{
	for (int i=0;i<len;i++)
	{
		out[i] = in[i] ^ m_XORTable[m_XORPos&0x0F];
		m_XORPos++;
	}
}

void OSIEncryption::TwoFishCrypt( const BYTE *in, BYTE *out, int len )
{
	for(int i=0;i<len;i++)
	{
		if( m_TFPos >= 256 )
			ReinitTFTable();
		out[i] = in[i] ^ m_TFTable[m_TFPos++];
	}
}

//server -> us
void OSIEncryption::DecryptFromServer( const BYTE *in, BYTE *out, int len )
{
	XORCrypt( in, out, len );
}

//us -> client
void OSIEncryption::EncryptForClient( const BYTE *in, BYTE *out, int len )
{
	XORCrypt( in, out, len );
}

//us -> server
void OSIEncryption::EncryptForServer( const BYTE *in, BYTE *out, int len )
{
	TwoFishCrypt( in, out, len );
}

//client -> us
void OSIEncryption::DecryptFromClient( const BYTE *in, BYTE *out, int len )
{
	TwoFishCrypt( in, out, len );
}


