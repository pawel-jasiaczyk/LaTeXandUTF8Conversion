﻿using System;
using System.Collections.Generic;

namespace LaTeXAndUtfConverter
{
	/// <summary>
	/// Klasa reprezentuje kolekcję liter do wykorzystania 
	/// przy konresji formatu LaTeX/BibTeX na UTF8.
	/// </summary>
	public class SingleAlphabet
	{
		#region Deklaracje

		private List<Letter> alphabet;

		#endregion

		#region Konstruktowy

		/// <summary>
		/// Initializes a new instance of the <see cref="LaTeXAndUtfConverter.SingleAlphabet"/> class.
		/// </summary>
		public SingleAlphabet ()
		{
			alphabet = new List<Letter> ();
		}

		#endregion

		#region Add

		/// <summary>
		/// Metoda doda obiekt Letter do kolekcji bez żadnych sprawdzeń
		/// </summary>
		/// <param name="letter">Letter to add.</param>
		public void AddLetter_Unsafe(Letter letter)
		{
			alphabet.Add (letter);
		}
			
		/// <summary>
		/// Metoda doda obiekt Letter do kolekcji bez żadnych sprawdzeń
		/// </summary>
		/// <param name="latex">Ciąg znaków w LaTeX</param>
		/// <param name="utf_dec">Dziesiętna wartość kodu UTF8</param>
		public void AddLetter_Unsafe(string latex, uint utf_dec)
		{
			alphabet.Add (new Letter{ UTF8_DEC = utf_dec, LaTeX = latex });
		}

		/// <summary>
		/// Metoda doda obiekt Letter do kolekcji bez żadnych sprawdzeń
		/// </summary>
		/// <param name="latex">Ciąg znaków w LaTeX</param>
		/// <param name="utf_hex">Heksadecymalna wartość kodu UTF8</param>
		public void AddLetter_Unsafe(string latex, string utf_hex)
		{
			alphabet.Add (new Letter{ UTF8_HEX = utf_hex, LaTeX = latex });
		}

		/// <summary>
		/// Metoda doda obiekt Letter do kolekcji bez żadnych sprawdzeń
		/// </summary>
		/// <param name="latex">Ciąg znaków w LaTeX</param>
		/// <param name="character">Znak UTF8</param>
		public void AddLetter_Unsafe(string latex, char character)
		{
			alphabet.Add (new Letter{ Character = character, LaTeX = latex });
		}

		/// <summary>
		/// Metoda doda obiekt Letter do kolekcji bez żadnych sprawdzeń
		/// </summary>
		/// <param name="latex">Ciąg znaków w LaTeX</param>
		/// <param name="utf_bytes">Bajty UTF8</param>
		public void AddLetter_Unsafe(string latex, byte[] utf_bytes)
		{
			alphabet.Add (new Letter{ LaTeX = latex, UTF8_bytes = utf_bytes });
		}

		/// <summary>
		/// Dodaje litere do alfabetu.
		/// Jeżeli któraś z wartości litery już występuje w alfabecie
		/// nowa litera nie zostanie dodana, a funkcja zwróci wartość
		/// AddLetterCode.EntryAlreadyExist.
		/// </summary>
		/// <returns>Informajca o skuteczności dodania.</returns>
		/// <param name="letter">Litera</param>
		public AddLetterCode AddLetter (Letter letter)
		{
			return AddLetter_Safe (letter);
		}

		/// <summary>
		/// Dodaje litere do alfabetu.
		/// Jeżeli któraś z wartości litery już występuje w alfabecie
		/// nowa litera nie zostanie dodana, a funkcja zwróci wartość
		/// AddLetterCode.EntryAlreadyExist.
		/// </summary>
		/// <returns>Informacja o skuteczności operacji</returns>
		/// <param name="latex">Ciąg znaków w LaTeX</param>
		/// <param name="utf_dec">Dziesiętna wartość kodu UTF8</param>
		public AddLetterCode AddLetter (string latex, uint utf_dec)
		{
			return AddLetter_Safe (new Letter{ LaTeX = latex, UTF8_DEC = utf_dec });
		}

		/// <summary>
		/// Dodaje litere do alfabetu.
		/// Jeżeli któraś z wartości litery już występuje w alfabecie
		/// nowa litera nie zostanie dodana, a funkcja zwróci wartość
		/// AddLetterCode.EntryAlreadyExist.
		/// </summary>
		/// <returns>Informacja oskuteczności operacji</returns>
		/// <param name="latex">Ciąg znaków LaTeX</param>
		/// <param name="utf_hex">Hexadecymalna wartość UTF8</param>
		public AddLetterCode AddLetter (string latex, string utf_hex)
		{
			return AddLetter_Safe (new Letter{ LaTeX = latex, UTF8_HEX = utf_hex });
		}

		/// <summary>
		/// Dodaje litere do alfabetu.
		/// Jeżeli któraś z wartości litery już występuje w alfabecie
		/// nowa litera nie zostanie dodana, a funkcja zwróci wartość
		/// AddLetterCode.EntryAlreadyExist.
		/// </summary>
		/// <returns>Informacja o skuczeności operacji</returns>
		/// <param name="latex">Ciąg znaków LaTeX</param>
		/// <param name="character">Znak</param>
		public AddLetterCode AddLetter (string latex, char character)
		{
			return AddLetter_Safe (new Letter{ LaTeX = latex, Character = character });
		}

		/// <summary>
		/// Dodaje litere do alfabetu.
		/// Jeżeli któraś z wartości litery już występuje w alfabecie
		/// nowa litera nie zostanie dodana, a funkcja zwróci wartość
		/// AddLetterCode.EntryAlreadyExist.
		/// </summary>
		/// <returns>Informacja o skuczeności operacji</returns>
		/// <param name="latex">Ciąg znaków LaTeX</param>
		/// <param name="utf_bytes">Utf8 bytes.</param>
		public AddLetterCode AddLetter (string latex, byte[] utf_bytes)
		{
			byte[] temp = new byte[utf_bytes.Length];
			Array.Copy (utf_bytes, temp, utf_bytes.Length);
			return AddLetter_Safe (new Letter{ LaTeX = latex, UTF8_bytes = temp });
		}

		private AddLetterCode AddLetter_Safe(Letter letter)
		{
			AddLetterCode resultCode = AddLetterCode.OK;
			foreach (Letter l in alphabet) 
				if (l.LaTeX == letter.LaTeX || l.UTF8_DEC == letter.UTF8_DEC)
					resultCode = AddLetterCode.EntryAlreadyExist;
			if (resultCode == AddLetterCode.OK)
				alphabet.Add (letter);
			return resultCode;
		}

		#endregion

		#region Gettery

		/// <summary>
		/// Zwraca tablicę wszystkich obiektów Letter z wartością
		/// pola LaTeX == latex.
		/// </summary>
		/// <returns>Tablica liter</returns>
		/// <param name="latex">Poszukiwany format LaTeX</param>
		public Letter[] GetLettersByLatexString(string latex)
		{
			List<Letter> result = new List<Letter> ();
			foreach (Letter letter in alphabet) 
				if (letter.LaTeX == latex)
					result.Add (letter);
			return result.ToArray ();
		}

		/// <summary>
		/// Zwraca tablicę wszystkich obiektów Letter dal których
		/// pole LaTeX zawiera string latex
		/// </summary>
		/// <returns>The all Letters inlcuding latex string.</returns>
		/// <param name="latex">Poszukiwany format LaTeX</param>
		/// <description> Metoda zwróci wszystkie wpisy liter zawierające
		/// poszukiwany string.
		/// W przeciwieństwie do metody <code>GetLettersByLatexString(string latex)</code>
		/// zwróci również obiekty Letter dla których poszukiwany string
		/// jest jedynie częśćią zawartości pola LaTeX
		/// </description>
		public Letter[] GetAllLettersInlcudingLatexString(string latex)
		{
			List<Letter> result = new List<Letter> ();
			foreach (Letter letter in alphabet)
				if (letter.LaTeX.Contains (latex))
					result.Add (letter);
			return result.ToArray();
		}

		#endregion

		#region Delete

		/// <summary>
		/// Usuwa literę z kolekcji
		/// </summary>
		/// <param name="toDelete">To delete.</param>
		public void DeleteLetter(Letter toDelete)
		{
			alphabet.Remove (toDelete);
		}

		#endregion

		#region STATYCZNE FUNKCJE

		#endregion
	}
	/// <summary>
	/// Kod zakończenia działania funkcji bezpiecznego dodawania litery do kolekcji
	/// </summary>
	public enum AddLetterCode
	{
		/// <summary>
		/// Funkcja dodawania litery zakończyła działanie pomyślnie
		/// </summary>
		OK,
		/// <summary>
		/// W kolekcji znajduje się już litera zawierająca przynajmniej jedną
		/// z wartości ustawionych dla dodawanej litery.
		/// </summary>
		EntryAlreadyExist
	}
}

