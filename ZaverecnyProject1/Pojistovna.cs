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
		private List<Pojisteny> seznamPojistenych;
		private List<int> seznamIdSmazanych;		

		public Pojistovna()
		{
			seznamPojistenych = new List<Pojisteny>();
			seznamIdSmazanych = new List<int>();
		}

		/// <summary>
		/// Hlavní menu aplikace
		/// </summary>
		public void Menu()
		{
			LoadData();
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
					case 4:
						SmazPojisteneho();
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

		/// <summary>
		/// Uložení do souboru
		/// </summary>
		/// <param name="pojistenci">Seznam pojištěných</param>
		/// <param name="idSmazanych">Seznam id po smazaných pojištěných</param>
		private void SaveData(List<Pojisteny> pojistenci, List<int> idSmazanych)
		{
			string souborPojistenci = "Pojistenci.json";
			string souborIdSmazanych = "IdSmazanych.json";
			string jsonStringPojistenci = JsonSerializer.Serialize(pojistenci);
			string jsonStringIdSmazanych = JsonSerializer.Serialize(idSmazanych);
			File.WriteAllText(souborPojistenci, jsonStringPojistenci);
			File.WriteAllText(souborIdSmazanych, jsonStringIdSmazanych);
		}


		/// <summary>
		/// Načítání souborů pokud soubor existuje
		/// </summary>
		private void LoadData()
		{
			if (File.Exists(Environment.CurrentDirectory + @"\Pojistenci.json"))
			{
				string pojistenci = "Pojistenci.json";
				string jsonStringPojistenci = File.ReadAllText(pojistenci);
				seznamPojistenych = JsonSerializer.Deserialize<List<Pojisteny>>(jsonStringPojistenci)!;


			}
			if (File.Exists(Environment.CurrentDirectory + @"\IdSmazanych.json"))
			{
				string idSmazanych = "IdSmazanych.json";
				string jsonStringIdSmazanych = File.ReadAllText(idSmazanych);	
				seznamIdSmazanych = JsonSerializer.Deserialize<List<int>>(jsonStringIdSmazanych)!;			
			}

		}

		/// <summary>
		/// Vypisuje hlavičku aplikace, který je vždy stejný
		/// </summary>
		private void HlavickaProgramu()
		{
			Console.Clear();
			Console.WriteLine("________________________________________\n");
			Console.WriteLine("Evidence pojistenych");
			Console.WriteLine("________________________________________\n\n");
		}

		/// <summary>
		/// Vypisuje hlavičku aplikace u vyhledávání pojištěných
		/// </summary>
		private void HlavickaVyhledavani()
		{
			HlavickaProgramu();
			Console.WriteLine("Vyhledávání pojištěného.\n");
		}

		/// <summary>
		/// Vypisuje hlavičku aplikace u mazání pojištěného
		/// </summary>
		private void HlavickaMazani()
		{
			HlavickaProgramu();
			Console.WriteLine("Mazání pojištěného.\n");
		}

		/// <summary>
		/// Výběr akce v hlavní nabídce
		/// </summary>
		/// <returns>Číslo akce která se má provést</returns>
		private int VyberMenu()
		{
			int vybranaAkce;
			HlavickaProgramu();
			Console.WriteLine("Vyberte si akci:");
			Console.WriteLine("1 - Přidat nového pojištěného");
			Console.WriteLine("2 - Vypsat všechny pojištěné");
			Console.WriteLine("3 - Vyhledat pojištěného");
			Console.WriteLine("4 - Smazat pojištěného");
			Console.WriteLine("5 - Konec");
			//Zkontroluje jestli je napsaný text číslo od 1 do 5
			while (!(int.TryParse(Console.ReadLine(), out vybranaAkce) && vybranaAkce > 0 && vybranaAkce <= 5))
				Console.WriteLine("Zadejte číslo od 1 do 5 pro výběr akce.");
			return vybranaAkce;
		}

		/// <summary>
		/// Přídání nového pojištěného do evidence
		/// </summary>
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

			//Zajišťuje jedinečnost id
			if(seznamIdSmazanych.Count == 0)
			{
				seznamPojistenych.Add(new Pojisteny(jmeno, prijmeni, telefonniCislo, vek, seznamPojistenych.Count + 1));
			}
			else
			{
				seznamPojistenych.Add(new Pojisteny(jmeno, prijmeni, telefonniCislo, vek, seznamIdSmazanych[0]));
				seznamIdSmazanych.RemoveAt(0);
			}

			SaveData(seznamPojistenych, seznamIdSmazanych);
			Console.WriteLine("\nData byla ulozena. Pokračujte libovolnou klávesou...");
			Console.ReadKey();
		}


		/// <summary>
		/// Výpis všech pojištěných v evidenci
		/// </summary>
		private void VypisVsechny()
		{
			HlavickaProgramu();
			Console.WriteLine("Výpis všech uložených osob:\n");
			if (seznamPojistenych.Count != 0)
			{
				foreach (Pojisteny osoba in seznamPojistenych)
				{
					Console.WriteLine(osoba);
				}
			}
			else
				Console.WriteLine("Nejsou uložení žádní pojištění.");

			Console.WriteLine("\nPokračujte libovolnou klávesou...");
			Console.ReadKey();
		}

		/// <summary>
		/// Hledá pojištěné v evidenci podle zadaných kritérií uživatelem
		/// </summary>
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

		/// <summary>
		/// Vybere pojištěné k vymazání
		/// </summary>
		private void SmazPojisteneho()
		{
			List<Pojisteny> seznamNalezenych = new List<Pojisteny>();
			HlavickaMazani();
			Console.WriteLine("Zadejte jméno a příjmení pojištěného kterého chcete odstranit.");
			Console.Write("Zadejte jméno: ");
			string jmeno = Console.ReadLine();
			Console.Write("Zadejte příjmení: ");
			string prijmeni = Console.ReadLine();
			foreach (Pojisteny pojisteny in seznamPojistenych)
			{
				if (pojisteny.Jmeno.ToLower() == jmeno.ToLower() && pojisteny.Prijmeni.ToLower() == prijmeni.ToLower())
				{
					seznamNalezenych.Add(pojisteny);
				}
			}

			PocetPojistenychNaSmazani(seznamNalezenych);
		}

		/// <summary>
		/// Podle počtu uživatelů zvolí danou metodu na smazání
		/// </summary>
		/// <param name="seznamNalezenych">Seznam pojištěných, kterým se shoduje jméno a příjmení</param>
		private void PocetPojistenychNaSmazani(List<Pojisteny> seznamNalezenych)
		{
			HlavickaMazani();
			if (seznamNalezenych.Count == 0)
			{
				Console.WriteLine("V seznamu se tento pojištěný nenachází.");
				Console.WriteLine("\nPokračujte libovolnou klávesou...");
				Console.ReadKey();
			}
			else if (seznamNalezenych.Count == 1)
			{
				Pojisteny pojistenyNaSmazani = seznamNalezenych[0];
				MazaniPojisteneho(pojistenyNaSmazani);
			}
			else
			{
				List<int> idPojistenych = new List<int>();
				int napsaneId;
				Pojisteny pojistenyNaSmazani = seznamNalezenych[0];
				Console.WriteLine("Byli nalezeni tito pojištění:");
				foreach (Pojisteny pojisteny in seznamNalezenych)
				{
					Console.WriteLine(pojisteny.VypisUzivateleSId());
					idPojistenych.Add(pojisteny.Id);
				}
				Console.WriteLine("Napište Id pojištěného, kterého chcete smazat");
				while (!(int.TryParse(Console.ReadLine(), out napsaneId)) || !idPojistenych.Contains(napsaneId))
					Console.WriteLine("Napište číselně Id pojištěného, jehož chcecte smazat, které je napsáno víše.");
				foreach (Pojisteny pojisteny in seznamNalezenych)
				{
					if (pojisteny.Id == napsaneId)
						pojistenyNaSmazani = pojisteny;
				}
				MazaniPojisteneho(pojistenyNaSmazani);
			}
		}

		/// <summary>
		/// Ujištění před smazáním pojištěného
		/// </summary>
		/// <param name="pojisteny">Pojištěný, kterého chceme smazat</param>
		private void MazaniPojisteneho(Pojisteny pojisteny)
		{
			Console.WriteLine(pojisteny.VypisUzivateleSId());
			string odpoved = "";
			while (true)
			{
				Console.WriteLine("Opravdu chcete tohoto pojistného odstranit?[Ano/Ne]");
				odpoved = Console.ReadLine().Trim().ToLower();
				if (odpoved == "ano" || odpoved == "ne")
					break;
			}

			if (odpoved == "ano")
			{
				seznamIdSmazanych.Add(pojisteny.Id);
				seznamIdSmazanych.Sort();
				Console.WriteLine($"Pojištěný {pojisteny.Jmeno} {pojisteny.Prijmeni} s id {pojisteny.Id} byl smazán.");
				seznamPojistenych.Remove(pojisteny);
				SaveData(seznamPojistenych, seznamIdSmazanych);
				Console.WriteLine("\nPokračujte libovolnou klávesou...");
				Console.ReadKey();
			}
			else
			{
				Console.WriteLine($"Smazání pojištěného bylo zrušeno.");
				Console.WriteLine("\nPokračujte libovolnou klávesou...");
				Console.ReadKey();
			}
		}


		/// <summary>
		/// Určí podmínky pro hledání osob
		/// </summary>
		/// <param name="kriteria">Seznam požadavků podle kterých chceme vyhledávat</param>
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
			VypisOsob(jmeno, podleJmena, prijmeni, podlePrimeni, telefon, podleTelefonu, vek, podleVeku);			
		}

		/// <summary>
		/// Vypíše hledané osoby, podle zadaných kritérií
		/// </summary>
		/// <param name="jmeno">Vyhledává zadané jméno</param>
		/// <param name="podleJmena">Určuje jestli chceme hledat podle jména</param>
		/// <param name="prijmeni">Vykledává zadané příjmení</param>
		/// <param name="podlePrijmeni">Urřuje jestli chceme hledat podle příjmení</param>
		/// <param name="telefon">Vyhledává zadané telefoní číslo</param>
		/// <param name="podleTelefonu">Určuje jestli chceme hledat podle telefoního čísla</param>
		/// <param name="vek">Vyhledává zadaný věk</param>
		/// <param name="podleVeku">Určuje jestli chceme hledat podle věku</param>
		private void VypisOsob(string jmeno, bool podleJmena, string prijmeni, bool podlePrijmeni, string telefon, bool podleTelefonu, string vek, bool podleVeku)
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

		/// <summary>
		/// Menu pro hledani kriterií
		/// </summary>
		/// <returns>Vrátí seznam kritérií podle kterých ceme vyhledávat</returns>
		private List<int>  KriteriaHledani()
		{
			Console.WriteLine("Zadejte podle čeho chcete vyhledávat:");
			Console.WriteLine("Pokud chcete podle více kriterií oddělte je čárkou např: 1,3");
			Console.WriteLine("1 - Jmeno");
			Console.WriteLine("2 - Přijmení");
			Console.WriteLine("3 - Telefon");
			Console.WriteLine("4 - Věk");			 
			return SeznamKriterii();
		}

		/// <summary>
		/// Vrací list kriterií pro vyhledávání
		/// </summary>
		/// <returns>Vrátí seznam kritérií podle kterých ceme vyhledávat</returns>
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
