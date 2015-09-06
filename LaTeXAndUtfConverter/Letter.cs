using System;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;

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
		private string latex;

		#endregion

		#region Właściwości

		/// <summary>
		/// Liczbowa reprezentacja dziesiętna kodu UTF-8 litery
		/// </summary>
		/// <value>Kod UTF-8</value>
		[XmlIgnore]
		public uint UTF8_DEC{
			get{
				uint result = 0;
				for (int i = 0; i < utf8.Length; i++)			// Ręczna transformacja uint na tablicę
					result += 									// byte[] spowodowana niezgodnościami
						(utf8 [i] * (uint)Math.Pow 				// pomiędzy kolejnością bajtów BitConvertera
							(256, (utf8.Length - i -1)));		// oraz kodowania UTF8
				return result;
			}
			set {
				List<byte> bytes = new List<byte> ();			// BitConverter i tak korzystał by
				while (value > 0) {								// z kolekcji. Moj kod wygląda mniej
					bytes.Add ((byte)(value % 256));			// elegancko, ale pozwala lepiej kontrolowac
					value /= 256;								// konwersje
				}
				utf8 = new byte[bytes.Count];					// ta wyglądająca na dłuższą wersja
				for (int i = 0; i < utf8.Length; i++) {			// wydaje mi się efektywniejsza
					utf8 [i] = bytes [utf8.Length - 1 - i];		// od razu uzupełnia tablicę właściwymi
				}												// liczbami, bez odwracania
				// utf8 = bytes.ToArray ();
				// Array.Reverse (utf8);
			}
		}

		/// <summary>
		/// String reprezentujący szestnastkową reprezentację kodu UTF-8 litery.
		/// </summary>
		/// <value>UTF-8 HEX</value>
		[XmlAttribute]
		public string UTF8_HEX
		{ 
			get{ return UTF8_DEC.ToString ("X"); } 						// odniesienie przez już
			set{														// skonfigurowaną właściwość
				try{													// UTF8_DEC
					UTF8_DEC = uint.Parse(value, 
							System.Globalization.NumberStyles.HexNumber);
				}
				catch
				{
					throw;
				}
			}
		}
			
		/// <summary>
		/// Litera w postaci znaku char.
		/// </summary>
		/// <value>Litera</value>
		[XmlIgnore]
		public char Character {
			get { return Encoding.UTF8.GetChars (utf8) [0]; }
			set { utf8 = Encoding.UTF8.GetBytes (new char[] { value }); }
		}

		/// <summary>
		/// Bitowa postać bajtów UTF8.
		/// </summary>
		/// <value>The UT f8 bytes.</value>
		[XmlIgnore]
		public byte[] UTF8_bytes{ get { return utf8; } set { utf8 = value; } }

		/// <summary>
		/// String reprezentujący zapis litery diakrytyzowanej w LaTeX'u.
		/// </summary>
		/// <value>Kod LaTeX.</value>
		[XmlAttribute]
		public string LaTeX{ get{return @latex;} set{latex = @value;} }

		#endregion

		#region Konstruktory

		/// <summary>
		/// Initializes a new instance of the <see cref="LaTeXAndUtfConverter.Letter"/> class.
		/// </summary>
		public Letter ()
		{
		}

		#endregion

		#region Publiczne Funkcje Pomocnicze

		/// <summary>
		/// Gets the bytes as bits as string.
		/// </summary>
		/// <returns>The bytes as bits as string.</returns>
		public string GetBytesAsBitsString()
		{
			string result = "";
			for (int i = 0; i < utf8.Length; i++) {
				result += GetBitsFromByte (utf8 [i]);
				if (i < utf8.Length - 1)
					result += " ";
			}
			return result;
		}
			
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="LaTeXAndUtfConverter.Letter"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="LaTeXAndUtfConverter.Letter"/>.</returns>
		public override string ToString ()
		{
			return string.Format ("" +
				"[Letter: " +
				"UTF8_DEC={0}, " +
				"UTF8_HEX={1}, " +
				"Character={2}, " +
				"LaTeX={3}]", 
				UTF8_DEC, 
				UTF8_HEX, 
				Character, 
				LaTeX);
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="LaTeXAndUtfConverter.Letter"/>.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="LaTeXAndUtfConverter.Letter"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="LaTeXAndUtfConverter.Letter"/>; otherwise, <c>false</c>.</returns>
		public override bool Equals (object obj)
		{
			if (obj == null)
				return false;
			if (!(obj is Letter))
				return false;
			else
				if((this.UTF8_DEC == ((Letter)obj).UTF8_DEC) &&
					this.LaTeX == ((Letter)obj).LaTeX) return true;
			return false;
		}

		#endregion

		#region Prywatne funkcje pomocnicze

		// Zwraca bajt w postaci zero-jedynkowego stringa. Kolejność od bitów najbardziej znaczących.
		private string GetBitsFromByte(byte input)
		{
			byte temp = input;
			int dziel = 0;
			string result = "";
			for (int i = 0; i < 8; i++) {
				dziel = (int)(Math.Pow (2, 7 - i));
				result += (temp / dziel).ToString ();
				temp = (byte)(temp % dziel);
			}
			return result;
		}

		#endregion
	}
}

