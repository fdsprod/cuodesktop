using System;
using System.Collections.Generic;
using System.Text;

namespace PatchUO
{
	public enum FileID : int
	{
		Map0_mul		= 0x00000000,
		StaIdx0_mul		= 0x00000001,
		Statics0_mul	= 0x00000002,
		ArtIdx_mul		= 0x00000003,
		Art_mul			= 0x00000004,
		Anim_idx		= 0x00000005,
		Anim_mul		= 0x00000006,
		SoundIdx_mul	= 0x00000007,
		Sound_mul		= 0x00000008,
		TexIdx_mul		= 0x00000009,
		TexMaps_mul		= 0x0000000A,
		GumpIdx_mul		= 0x0000000B,
		GumpArt_mul		= 0x0000000C,
		Multi_idx		= 0x0000000D,
		Multi_mul		= 0x0000000E,
		Skills_idx		= 0x0000000F,
		Skills_mul		= 0x00000010,
		TileData_mul	= 0x00000011,
		AnimData_mul	= 0x00000012,
		Hues_mul		= 0x00000013,
		Anim2_idx		= 0x00020005,
 		Anim2_mul		= 0x00020006,
 		Anim3_idx		= 0x00030005,
 		Anim3_mul		= 0x00030006,
 		Anim4_idx		= 0x00040005,
		Anim4_mul		= 0x00040006,
		Anim5_idx		= 0x00050005,
		Anim5_mul		= 0x00050006,
	}

	public enum ExtendedFileID : int//Fucking UOG had to go and fuck up the MUO
	{
		Map0_mul = 0x00000040,
		StaIdx0_mul = 0x00000041,
		Statics0_mul = 0x00000042,
		ArtIdx_mul = 0x00000043,
		Art_mul = 0x00000044,
		Anim_idx = 0x00000045,
		Anim_mul = 0x00000046,
		SoundIdx_mul = 0x00000047,
		Sound_mul = 0x00000048,
		TexIdx_mul = 0x00000049,
		TexMaps_mul = 0x0000004A,
		GumpIdx_mul = 0x0000004B,
		GumpArt_mul = 0x0000004C,
		Multi_idx = 0x0000004D,
		Multi_mul = 0x0000004E,
	}
}
