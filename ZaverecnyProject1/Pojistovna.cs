using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZaverecnyProject1
{
	internal class Pojistovna
	{
		List<Pojisteny> seznamPojistenych;
		

		public Pojistovna()
		{
			seznamPojistenych = new List<Pojisteny>();
		}

		//Hlavní menu aplikace
		public void Menu()
		{
			LoadData(seznamPojistenych);
			while (true)
			{
				bool konec = false;
				switch (VyberMenu())
				{
					case 1:
						Pridej();
						break;
					case 2:
						VypisVsechny();
						break;
					case 3:
						VyhledejOsobu();
						break;
					default:
						konec = true;
						break;
				}
				if (konec)
				{
					break;
				}

			}
		}

		//Uložení pojištěných
		private void SaveData(List<Pojisteny> pojistenci)
		{
			string fileName = "Pojistenci.json";
			string jsonString = JsonSerializer.Serialize(pojistenci);
			File.WriteAllText(fileName, jsonString);
		}

		//Načítání pojištěných pokud soubor existuje
		private void LoadData(List<Pojisteny> pojistenci)
		{
			if (File.Exists(Environment.CurrentDirectory + @"\Pojistenci.json"))
			{
				string fileName = "Pojistenci.json";
				string jsonString = File.ReadAllText(fileName);
				seznamPojistenych = JsonSerializer.Deserialize<List<Pojisteny>>(jsonString)!;
			}

		}

		//Vypisuje vrh aplikace, který je vždy stejný
		private void HlavickaProgramu()
		{
			Console.Clear();
			Console.WriteLine("________________________________________\n");
			Console.WriteLine("Evidence pojistenych");
			Console.WriteLine("________________________________________\n\n");
		}

		//Vypisuje vrh aplikace u vyhledávání pojištěných
		private void HlavickaVyhledavani()
		{
			HlavickaProgramu();
			Console.WriteLine("Vyhledávání pojištěného.\n");
		}

		//Výběr akce v hlavní nabídce
		private int VyberMenu()
		{
			int vybranaAkce;
			HlavickaProgramu();
			Console.WriteLine("Vyberte si akci:");
			Console.WriteLine("1 - Přidat nového pojištěného");
			Console.WriteLine("2 - Vypsat všechny pojištěné");
			Console.WriteLine("3 - Vyhledat pojištěného");
			Console.WriteLine("4 - Konec");
			//Zkontroluje jestli je napsaný text číslo od 1 do 4
			while (!(int.TryParse(Console.ReadLine(), out vybranaAkce) && vybranaAkce > 0 && vybranaAkce <= 4))
				Console.WriteLine("Zadejte číslo od 1 do 4 pro výběr akce.");
			return vybranaAkce;
		}

		//Přídání nového pojištěného do evidence
		private void Pridej()
		{
			HlavickaProgramu();
			Console.WriteLine("Vytvoření pojištěného.\n");
			Console.WriteLine("Zadejte jméno pojistěného:");
			string jmeno = Console.ReadLine().Trim();
			Console.WriteLine("Zadejte příjmení:");
			string prijmeni = Console.ReadLine().Trim();
			Console.WriteLine("Zadejte telefonní číslo:");
			string telefonniCislo = Console.ReadLine().Trim();
			Console.WriteLine("Zadejte věk:");
			string vek = Console.ReadLine().Trim();

			seznamPojistenych.Add(new Pojisteny(jmeno, prijmeni, telefonniCislo, vek));
			SaveData(seznamPojistenych);
			Console.WriteLine("\nData byla ulozena. Pokračujte libovolnou klávesou...");
			Console.ReadKey();
		}


		//Výpis všech pojištěných v evidenci
		private void VypisVsechny()
		{
			HlavickaProgramu();
			Console.WriteLine("Výpis všech uložených osob:\n");
			foreach (Pojisteny osoba in seznamPojistenych)
			{
				Console.WriteLine(osoba);
			}
			Console.WriteLine("\nPokračujte libovolnou klávesou...");
			Console.ReadKey();
		}


		//Hledá pojištěné v evidenci podle zadaných kritérií uživatelem
		private void VyhledejOsobu()
		{
			HlavickaVyhledavani();

			List<int> seznamKriterii = KriteriaHledani();
			if (seznamKriterii.Count != 0)
				VyhledavaniOsoby(seznamKriterii);
			else
			{
				Console.WriteLine("Nebylo zadané kritérium pro vyhledávání");
				Console.WriteLine("\nPokračujte libovolnou klávesou...");
				Console.ReadKey();
			}
		}

		//Určí podmínky pro hledání osob
		private void VyhledavaniOsoby(List<int> kriteria)
		{
			HlavickaVyhledavani();

			string jmeno = "";
			string prijmeni = "";
			string telefon = "";
			string vek = "";

			bool podleJmena = false;
			bool podlePrimeni = false;
			bool podleTelefonu = false;
			bool podleVeku = false;

			if (kriteria.Contains(1))
			{
				Console.WriteLine("Zadejte jméno pojištěného:");
				jmeno = Console.ReadLine().Trim();
				podleJmena = true;
			}
			if (kriteria.Contains(2))
			{
				Console.WriteLine("Zadejte přijmení pojištěného:");
				prijmeni = Console.ReadLine().Trim();
				podlePrimeni = true;
			}
			if (kriteria.Contains(3))
			{
				Console.WriteLine("Zadejte Telefon pojištěného:");
				telefon = Console.ReadLine().Trim();
				podleTelefonu = true;
			}
			if (kriteria.Contains(4))
			{
				Console.WriteLine("Zadejte věk pojištěného:");
				vek = Console.ReadLine().Trim();
				podleVeku = true;
			}
			Console.WriteLine();
			VipisOsob(jmeno, podleJmena, prijmeni, podlePrimeni, telefon, podleTelefonu, vek, podleVeku);			
		}

		//Vypíše hledané osoby, podle zadaných kritérií
		private void VipisOsob(string jmeno, bool podleJmena, string prijmeni, bool podlePrijmeni, string telefon, bool podleTelefonu, string vek, bool podleVeku)
		{
			bool jeVEvidenci = false;
			foreach (Pojisteny pojisteny in seznamPojistenych)
			{
				bool odpovidaJmeno = true;
				bool odpovidaPrijmeni = true;
				bool odpovidaTelefon = true;
				bool odpovidaVek = true;

				if (podleJmena)
				{
					odpovidaJmeno = ((pojisteny.Jmeno).ToLower() == jmeno.ToLower()) ? true : false;
				}
				if (podlePrijmeni)
				{
					odpovidaPrijmeni = (pojisteny.Prijmeni.ToLower() == prijmeni.ToLower()) ? true : false;
				}
				if (podleTelefonu)
				{
					odpovidaTelefon = (pojisteny.TelefonniCislo.ToLower() == telefon.ToLower()) ? true : false;
				}
				if (podleVeku)
				{
					odpovidaVek = (pojisteny.Vek.ToLower() == vek.ToLower()) ? true : false;
				}
				if (odpovidaJmeno && odpovidaPrijmeni && odpovidaTelefon && odpovidaVek)
				{
					jeVEvidenci = true;
					Console.WriteLine(pojisteny);
				}
			}
			if (!jeVEvidenci)
				Console.WriteLine("V evidenci nikdo takový není.\n");

			Console.WriteLine("\nPokračujte libovolnou klávesou...");
			Console.ReadKey();
		}

		//Menu pro hledani kriterií
		private List<int>  KriteriaHledani()
		{
			Console.WriteLine("Zadejte podle čeho chcete vyhledávat:");
			Console.WriteLine("Pokud chcete podle více kriterií oddělte je čárkou např: 1,3");
			Console.WriteLine("1 - Jmeno");
			Console.WriteLine("2 - Přijmení");
			Console.WriteLine("3 - Telefon");
			Console.WriteLine("4 - Věk");
			
			List<int> seznamKriterii = SeznamKriterii();
			return seznamKriterii;

		}

		//Vrací list kriterií pro vyhledávání
		private List<int> SeznamKriterii()
		{
			string kriteria = Console.ReadLine().Trim();
			string[] stringKriterii = kriteria.Split(',', options: StringSplitOptions.RemoveEmptyEntries);
			List<int> seznamKriterii = new List<int>();
			foreach (string kriterium in stringKriterii)
			{
				int cislo;
				//Kontroluje jestli je zadaný text číslo od 1 do 4 a není ještě obsaženo v seznamu
				if (
					int.TryParse(kriterium, out cislo) &&
					cislo > 0 &&
					cislo <= 4 &&
					!seznamKriterii.Contains(cislo)
					)
				{
					seznamKriterii.Add(cislo);
				}
			}
			seznamKriterii.Sort();
			return seznamKriterii;
		}

	}
}
