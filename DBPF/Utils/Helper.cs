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
using System;
using System.IO;

namespace SimPe
{
	/// <summary>
	/// Some Helper Functions frequently used in the handlers
	/// </summary>
	public static class Helper
	{
		public const string lbr = "\r\n";
		public const string tab = "    ";

		/// <summary>
		/// Creates a HexString (with Leading 0) of the given Length
		/// </summary>
		/// <param name="input">The HexFormated String with arbitrary Length</param>
		/// <param name="length">The min. Length for the String</param>
		/// <returns>The input String with added zeros.</returns>
		public static string MinStrLength(string input, int length)
		{
			while (input.Length<length) input = "0"+input;
			return input;
		}

		/// <summary>
		/// Returns the Value as HexString
		/// </summary>
		/// <param name="input">the Input Value</param>
		/// <returns>value as HexString (allways 8 Chars long)</returns>
		public static string HexString(uint input)
		{
			return (MinStrLength(input.ToString("X"), 8));
		}

		/// <summary>
		/// Returns the Value as HexString
		/// </summary>
		/// <param name="input">the Input Value</param>
		/// <returns>value as HexString (allways 4 Chars long)</returns>
		public static string HexString(byte input)
		{
			return (MinStrLength(input.ToString("X"), 2));
		}

		/// <summary>
		/// Creates a String from a byte Array
		/// </summary>
		/// <param name="data">The Byte Array</param>
		/// <returns>the String Representation</returns>
		public static string ToString(byte[] data) 
		{
			if (data==null) return "";

			string text = "";
			BinaryReader ms = new BinaryReader(new MemoryStream(data));
			try 
			{
				while (ms.BaseStream.Position<ms.BaseStream.Length) 
				{
					if (ms.PeekChar()==0) break;
					if (ms.PeekChar()==-1) break;
					text += ms.ReadChar();
				}
			} 
			catch ( Exception) {}	
	
			return text;
		}

		/// <summary>
		/// Returns the passed String as a Byte Array of the given Length
		/// </summary>
		/// <param name="str">The String to Convert</param>
		/// <param name="len">Length of the Array (the returned Array will have this exact Length)</param>
		/// <returns>A Byte Array of the given Length (filled with 0)</returns>
		public static byte[] ToBytes(string str, int len) 
		{
			byte[] ret = null;
			if (len!=0) 
			{
				ret = new byte[len];
				System.Text.Encoding.ASCII.GetBytes(str, 0, Math.Min(len, str.Length), ret, 0);
			}
			else ret = System.Text.Encoding.ASCII.GetBytes(str);

			return ret;
		}

		/// <summary>
		/// Copy a Complete directory
		/// </summary>
		/// <param name="sourcePath"></param>
		/// <param name="destinationPath"></param>
		/// <param name="recurse"></param>
		/// <remarks>created by Mark (daviesma@qca.org.uk)</remarks>
		public static void CopyDirectory(string sourcePath, string destinationPath, bool recurse)
		{
			String[] files;
			if (destinationPath[destinationPath.Length-1] != Path.DirectorySeparatorChar)
				destinationPath+=Path.DirectorySeparatorChar;
			if(!Directory.Exists(destinationPath)) Directory.CreateDirectory(destinationPath);
			files = Directory.GetFileSystemEntries (sourcePath);
			foreach(string element in files)
			{
				if (recurse)
				{
					// copy sub directories (recursively)
					if(Directory.Exists(element))
						CopyDirectory(element,destinationPath+Path.GetFileName(element), recurse);
						// copy files in directory
					else
						File.Copy(element,destinationPath+Path.GetFileName(element),true);
				}
				else
				{
					// only copy files in directory
					if(!Directory.Exists(element))
						File.Copy(element,destinationPath+Path.GetFileName(element),true);
				}
			}
		} 
	}
}
