using System;
using System.Text;
using System.Xml.Serialization;

namespace LaTeXAndUtfConverter
{
	/// <summary>
	/// Klasa reprezentuje litery diakrytyzowane. 
	/// Ich kod UTF-8 oraz string reprezentujący je w systemie LaTeX(UTF-8)
	/// </summary>
	public class Letter
	{
		#region Deklaracje
		private byte[] utf8;

		[XmlIgnore]
		/// <summary>
		/// Liczbowa reprezentacja kodu UTF-8 litery
		/// </summary>
		/// <value>Kod UTF-8</value>
		public uint UTF8
		{ 
			get { return BitConverter.ToUInt32 (utf8, 0); }
			set { utf8 = BitConverter.GetBytes (value);	}
		}
		/// <summary>
		/// String reprezentujący szestnastkową reprezentację kodu UTF-8 litery.
		/// </summary>
		/// <value>UTF-8 HEX</value>
		public string UTF8_HEX
		{ 
			get{ return BitConverter.ToUInt32(utf8,0).ToString ("X"); } 
			set{
				try{
					utf8 = BitConverter.GetBytes(
						uint.Parse(value, 
							System.Globalization.NumberStyles.HexNumber));
				}
				catch
				{
					throw;
				}
			}
		}
		[XmlIgnore]
		/// <summary>
		/// Litera w postaci znaku char.
		/// </summary>
		/// <value>Litera</value>
		public char Character { 
			get{
				try {
					byte[] temp = new byte[4];
					Array.Copy(utf8, temp, 4);
					Array.Reverse(temp);
					return Encoding.UTF8.GetChars (temp) [0]; // błąd
				} catch {
					throw;
				}
			}
			set{
				char[] t = { value };
				utf8 = (Encoding.UTF8.GetBytes(t));
				Array.Reverse (utf8);
				} 
		}
		[XmlIgnore]
		public byte[] UTF8_bytes{ get { return utf8; } set { utf8 = value; } }
		/// <summary>
		/// String reprezentujący zapis litery diakrytyzowanej w LaTeX'u.
		/// </summary>
		/// <value>Kod LaTeX.</value>
		public string LaTeX{ get; set; }

		#endregion

		public Letter ()
		{
		}
	}
}

