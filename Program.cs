using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ConsoleApp1
{
	class Program
	{
		static void Main(string[] args)
		{
			Grammar a = new Grammar();

			a.Algorithm_2();

			a.getGrammar();
		}
	}
}

namespace ConsoleApp1
{
	class Grammar
	{
		private List<string> noterminals = new List<string>(); //нетерминалы
		private List<string> terminals = new List<string>(); //терминалы
		private List<List<string>> rules = new List<List<string>>(); //правила
		private String initial;
		private List<string> goodSimvols = new List<string>(); //хорошие нетерминалы (алг 1)

		private bool termWord = false;          // Если false - то язык пуст
		private bool isProgrammEnd = false;     // Если true - то все рекурсивные вызовы завершаются

		public List<string> V = new List<string>();//множество достижимых символов (алг 2)
		List<string> V_start = new List<string>();
		bool begin_ = true;//

		public List<string> noterminals_V = new List<string>(); //нетерминалы вывода 
		public List<string> terminals_V = new List<string>(); //терминалы вывода
		public List<List<string>> rules_V = new List<List<string>>(); //правила вывода

		private void ReadFile()
		{
			string curfile = @"Grammar.txt";

			if (File.Exists(curfile)) //проверяем открыт ли файл
			{
				StreamReader file = new StreamReader("Grammar.txt"); // говорим, что переменная file теперь ссылается на наш файл

				string value; //терминал или нетерминал
				value = ""; //нужно, чтобы коректно работал код			
				int j = 0; //для поиска *
				string finish = "^"; // стопсивол
				string litter = "*"; //разделитель
				bool theend = true; //чтобы цикл while останавливался
				List<string> expressions; //каждое новое правило сначала заноситься в промежуточный лист

				initial = file.ReadLine(); //первая строка начальный символ
				string buff = file.ReadLine(); //вторая строка файла нетерминалы

				while (theend) //пока не нашли ^ и не дошли до конца
				{

					while (buff[j] != litter[0] && buff[j] != finish[0]) j++; //отмеряем размер строки до *
					if (buff[j] == litter[0])
					{
						value = buff.Substring(0, j); //копируем строку до * в отдельную переменную
						noterminals.Add(value); //добавляет строку в лист
						buff = buff.Remove(0, j + 1); //удаляем строку вместе с *
						j = 0; //начинаем с начала строки
						theend = true;

					}
					else if (buff[j] == finish[0]) //нашли стопсимвол и выходим из цикла
					{
						theend = false;
					}
					else
					{
						Console.WriteLine("Something went wrong while reading the first line");
						theend = false; // в любой непонятной ситуайии выходим из цикла
					}


				}
				int size_buff = buff.Length; //осталась последняя строка
				value = buff.Substring(0, size_buff - 1); //копируем последнюю строку
				noterminals.Add(value); //добавляем строку в лист

				buff = file.ReadLine(); //читаем третью строку файла. терминалы
				theend = true; //запускаем цикл

				while (theend) //пока не нашли ^ и не дошли до конца
				{

					while (buff[j] != litter[0] && buff[j] != finish[0]) j++; //отмеряем размер строки до *
					if (buff[j] == litter[0])
					{
						value = buff.Substring(0, j); //копируем строку до * в отдельную переменную
						terminals.Add(value); //добавляет строку в лист
						buff = buff.Remove(0, j + 1); //удаляем строку вместе с *
						j = 0; //начинаем с начала строки
						theend = true;

					}
					else if (buff[j] == finish[0]) //нашли стопсимвол и выходим из цикла
					{
						theend = false;
					}
					else
					{
						Console.WriteLine("Something went wrong while reading the second line");
						theend = false; // в любой непонятной ситуайии выходим из цикла
					}
				}
				size_buff = buff.Length; //осталась последняя строка
				value = buff.Substring(0, size_buff - 1); //копируем последнюю строку
				terminals.Add(value); //добавляем строку в лист

				while (!file.EndOfStream) //пока не конец файла
				{
					buff = file.ReadLine(); //читаем строку. правиа
					theend = true; //запускаем цикл
					expressions = new List<string>(); //создаем новую строку таблицы листов


					while (theend) //пока не нашли ^
					{
						while (buff[j] != litter[0] && buff[j] != finish[0]) j++; //отмеряем размер строки до *
						if (buff[j] == litter[0]) //нашли разделяющий символ
						{
							value = buff.Substring(0, j); //копируем строку до * в отдельную переменную
							expressions.Add(value); //добавляет строку в лист
							buff = buff.Remove(0, j + 1); //удаляем строку вместе с *							
							theend = true;

						}
						else if (buff[j] == finish[0]) //нашли стопсимвол и выходим из цикла
						{
							theend = false;
						}
						else
						{
							Console.WriteLine("something went wrong while reading the rules");
							theend = false; // в любой непонятной ситуайии выходим из цикла
						}
						j = 0; //начинаем с начала строки
					}
					size_buff = buff.Length; //осталась последняя строка
					value = buff.Substring(0, size_buff - 1); //копируем последнюю строку
					expressions.Add(value); //добавляем строку в лист
					rules.Add(expressions);
				}

				file.Close(); //закрываем файл
			}
		}
		// Находит все терминалы в правилах (только односимвольные), и отправляет их в Finder		
		private void CheckIsHaveTermOnrules()
		{
			for (int i = 0; i < rules.Count; i++)
			{
				for (int j = 0; j < rules[i].Count; j++) // Смотрим по всем правилам
				{
					for (int k = 0; k < terminals.Count; k++) // По всем терминалам
					{
						int size_rules = rules[i][j].Length;
						String buff = rules[i][j];
						String buffer = terminals[k];
						bool q = true;
						for (int t = 0; t < size_rules; t++)
						{
							if (buff[t] == buffer[0] && q == true) // Сравниваем каждый элемент
							{
								q = false;
								// Нашли, что в каком-то правиле есть терминал		
								//Console.Write(rules[i][0] + "  -  ");	
								int size_good = goodSimvols.Count;
								int e = 0;
								bool add_good = true;
								while (e < size_good)
								{
									if (goodSimvols[e] == rules[i][0]) add_good = false;
									e++;
								}
								if (add_good) this.goodSimvols.Add(rules[i][0]);//хорошие не терминалы								 
																				//Console.WriteLine("При начальном обходе нашли терминал в правилах, это: " + rules[i][j]);
								Finder(rules[i][0]); // Отправляем его в Finder
							}
						}

					}
				}
			}
		}
		// Получает на вход лексему, и ищет существует ли правило, из которого эту лексему можно получить
		// Если такое правило есть, то рекурсивно отправляет в самого себя это правило, в качестве входного значения
		// Если это найденное правило является аксиомой - то язык не пуст
		private void Finder(string findCell)
		{
			if (isProgrammEnd == false)
			{
				//Console.WriteLine("Ищем элемент " + findCell + " во всех правилах");
				for (int i = 0; i < rules.Count; i++)
				{
					for (int j = 1; j < rules[i].Count; j++)
					{
						int size_rules = rules[i][j].Length;
						String buff = rules[i][j];
						bool q = true;
						for (int t = 0; t < size_rules; t++)
						{
							if (buff[t] == findCell[0] && q == true)
							{
								q = false;
								//Console.Write("Нашли. ");
								//Console.WriteLine("Начало правила: " + rules[i][0] + ". Теперь ищем правило, из которого этот элемент получался бы");
								//Console.Write(rules[i][0] + "  +  ");
								int size_good = goodSimvols.Count;
								int k = 0;
								bool add_good = true;
								while (k < size_good)
								{
									if (goodSimvols[k] == rules[i][0]) add_good = false;
									k++;
								}
								if (add_good) this.goodSimvols.Add(rules[i][0]);
								Finder(rules[i][0]);
							}
						}

					}
				}
			}
		}
		// Ищет во множестве хороших смиволов начальный символ
		private void Search_for_original_symbol()
		{
			for (int i = 0; i < goodSimvols.Count; i++)
			{
				Console.Write(goodSimvols[i] + "  +  ");
				if (goodSimvols[i] == initial)
				{
					//Console.WriteLine("В множестве хороших смиволов нашли аксиому. Значит язык не пуст");
					termWord = true;
					break;
				}
			}
		}
		private void printCanBelanguage()
		{
			Console.WriteLine(" ");
			Search_for_original_symbol();
			Console.WriteLine(" ");
			Console.WriteLine(goodSimvols.Count);
			if (termWord == true) Console.WriteLine("Language is not empty");
			else Console.WriteLine("Language is empty");
			Console.WriteLine(" ");
		}
		public Grammar()
		{
			ReadFile();
		}
		public void getGrammar()
		{
			
			for (int i = 0; i < noterminals.Count; i++)
			{
				if (i > 0) Console.Write(", ");
				Console.Write(noterminals[i]);
			}
			Console.WriteLine();

			for (int i = 0; i < terminals.Count; i++)
			{
				if (i > 0) Console.Write(", ");
				Console.Write(terminals[i]);
			}
			Console.WriteLine();
			for (int i = 0; i < rules.Count; i++) // Чуть сложного кода для карсивого вывода)
			{
				Console.Write("    " + rules[i][0] + " -> ");
				for (int j = 1; j < rules[i].Count; j++) // Проверить на ошибки при выводе 
				{
					if (j > 1) Console.Write(", ");
					Console.Write(rules[i][j]);
				}
				Console.WriteLine(" ");
			}
			Console.WriteLine(initial);

			Console.WriteLine(this.goodSimvols.Count);
			for (int i = 0; i < this.goodSimvols.Count; i++)
			{
				if (i > 0) Console.Write(", ");
				Console.Write(this.goodSimvols[i]);
			}
			Console.WriteLine();

			Console.WriteLine("Множество достижимых символов");//вывод алгоритма_2
			for (int i = 0; i < V.Count; i++)
			{
				if (i > 0) Console.Write(", ");
				Console.Write(V[i]);
			}
			Console.WriteLine();


			for (int i = 0; i < noterminals_V.Count; i++)
			{
				if (i > 0) Console.Write(", ");
				Console.Write(noterminals_V[i]);
			}
			Console.WriteLine("  ;");
			for (int i = 0; i < terminals_V.Count; i++)
			{
				if (i > 0) Console.Write(", ");
				Console.Write(terminals_V[i]);
			}
			Console.WriteLine("  ;");

			for (int i = 0; i < rules_V.Count; i++) // Чуть сложного кода для карсивого вывода)
			{
				Console.Write("    " + rules_V[i][0] + " -> ");
				for (int j = 1; j < rules_V[i].Count; j++) // Проверить на ошибки при выводе 
				{
					if (j > 1) Console.Write(", ");
					Console.Write(rules_V[i][j]);
				}
				Console.WriteLine(" ");
			}
		}
		public void Algorithm_1()
		{
			CheckIsHaveTermOnrules();
			printCanBelanguage();
		}
		public void Algorithm_2()
		{
			if (begin_ == true)//если только начало алгоритма, то 
				V.Add(initial);//v_0={s}
							   //если не начало, то пропускается этот шаг
			int i;
			V_start = V;//это необходимо для сравнения V_i-1 = V_i, если равны, то алгоритм продолжается, а если нет - то рекурсия 
			for (i = 0; i < rules.Count; i++)
			{
				for (int k = 0; k < V.Count; k++)
				{
					if (rules[i][0] == V[k])//аксиома правила принадлежит ли множеству достижимых символов?
					{
						for (int j = 0; j < rules[i].Count; j++)//добавляет множество символов, которые принадлежат правилу и аксиома которых уже добавлена в множестве достижимых символов
						{
							string rule = rules[i][j];//с правила 
							for (int t = 0; t < rule.Length; t++)
							{
								string w = Convert.ToString(rule[t]);
								if (!V.Exists(x => x == w))//проверяет, существует ли символ в множестве достижимых символов 
								{
									V.Add(w);//если не существует, то добавляет его в  V(множ-во достиж-ых сим-лов)
											 
									if (V_start != V)
									{
										begin_ = false;
										Algorithm_2();//рекурсия 
									}
									else
									{
										End_alg8_2();//создает то что будет на выводе
									}
								}
                            }
							
						}
						
					}
				}
			}
		}
		private void End_alg8_2()
		{
			noterminals_V = Together(noterminals, V);
			terminals_V = Together(terminals, V);

			for (int i = 0; i < rules.Count; i++)//проходимся по всем правилам
			{
				bool prov = true;
				for (int j = 0; j < rules[i].Count; j++)
				{
					string rule = rules[i][j];//с правила 
					for (int t = 0; t < rule.Length; t++)
					{
						string w = Convert.ToString(rule[t]);
						if (!V.Exists(x => x == w))//проверяет, существует ли символ в множестве достижимых символов 
						{
							prov = false;
						}
					}
				}
				if (prov == true && !rules_V.Exists(x=>x==rules[i]))
				{
					rules_V.Add(rules[i]);//итоговый вывод правил
				}
			}
		}
		private List<string> Together(List<string> first, List<string> second)//пересечение двух объектов 
		{
			List<string> itog = new List<string>();
			for (int i = 0; i < first.Count; i++)
			{
				for (int j = 0; j < second.Count; j++)
				{
					if (first[i] == second[j])
					{
						string res = first[i];
						itog.Add(res);
					}
				}
			}
			return itog;
		}
	}
}