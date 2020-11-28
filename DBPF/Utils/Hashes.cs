/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   This program is free software; you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by  *
 *   the Free Software Foundation; either version 2 of the License, or     *
 *   (at your option) any later version.                                   *
 *                                                                         *
 *   This program is distributed in the hope that it will be useful,       *
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of        *
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the         *
 *   GNU General Public License for more details.                          *
 *                                                                         *
 *   You should have received a copy of the GNU General Public License     *
 *   along with this program; if not, write to the                         *
 *   Free Software Foundation, Inc.,                                       *
 *   59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.             *
 ***************************************************************************/
using Classless.Hasher;

namespace SimPe
{
	public class Hashes
	{
		static CRC crc24 = new CRC(CRCParameters.GetParameters(CRCStandard.CRC24));
		static CRC crc32 = new CRC(new Classless.Hasher.CRCParameters(32, 0x04C11DB7, 0xffffffff, 0, false));

		public static ulong ToLong(byte[] input)
		{
			ulong ret = 0;
			foreach (byte b in input)
			{
				ret = ret << 8;
				ret += b;
			}

			return ret;
		}

		public static uint GroupHash(string name)
		{
			name = name.Trim().ToLower();
			byte[] rt = crc24.ComputeHash(Helper.ToBytes(name, 0));//CRC24Seed, CRC24Poly, filename.ToCharArray());

			return (uint)(ToLong(rt) | 0x7f000000);
		}
	}
}
