using System;
using System.Collections.Generic;

namespace ТЯПиК
{
    class Program
    {
        static MainCore mainCore = new MainCore();

        static void Main(string[] args)
        {
            mainCore.alg8_2();
        }
    }

    public class MainCore
    {
        // Обявляю NEPS
        public List<string> NoTerminals = new List<string>(); //нетерминалы
        public List<string> Terminals = new List<string>(); //терминалы
        public List<List<string>> Rules = new List<List<string>>(); //правила
        public string Axioma; //начальный символ

        public List<string> V = new List<string>();//множество достижимых символов
        List<string> V_start = new List<string>();
        bool begin_ = true;//

        public List<string> NoTerminals_V = new List<string>(); //нетерминалы вывода 
        public List<string> Terminals_V = new List<string>(); //терминалы вывода
        public List<List<string>> Rules_V = new List<List<string>>(); //правила вывода


        public void alg8_2()
        {
            if (begin_ == true)//если только начало алгоритма, то 
            V.Add(Axioma);//v_0={s}
            //если не начало, то пропускается этот шаг
            int i;

            V_start = V;//это необходимо для сравнения V_i-1 = V_i, если равны, то алгоритм продолжается, а если нет - то рекурсия 
            for (i =0; i < Rules.Count; i++)
            {
                    for (int k = 0; k < V.Count; k++)
                    {
                        if (Rules[i][0] == V[k])//аксиома правила принадлежит ли множеству достижимых символов?
                        {
                            for(int j = 0; j < Rules[i].Count; j++)//добавляет множество символов, которые принадлежат правилу и аксиома которых уже добавлена в множестве достижимых символов
                            {
                                string rule = Rules[i][j];//символ правила 
                                 if (!V.Exists(x => x == rule))//проверяет, существует ли символ в множестве достижимых символов 
                                 {
                                    V.Add(rule);//если не существует, то добавляет его в  V(множ-во достиж-ых сим-лов)
                                 }
                            } 
                            
                            if (V_start != V)
                            {
                                begin_ = false;
                                alg8_2();//рекурсия 
                            }
                            else
                            {
                                End_alg8_2();//создает то что будет на выводе
                            }
                        }
                    }
            }
        }

        void End_alg8_2()
        {
            NoTerminals_V = Together(NoTerminals, V);
            Terminals_V = Together(Terminals, V);

            for (int i = 0; i < Rules.Count; i++)//проходимся по всем правилам
            {
                bool prov = true;

                for (int j = 0; j < Rules[i].Count; j++)
                {
                    if (!V.Exists(x => x == Rules[i][j]))//если символ правила не содержится в V, то правило не добавляется в итоговоый вывод
                    {
                        prov = false;
                    }
                }

                if (prov == true)
                {
                    Rules_V.Add(Rules[i]);//итоговый вывод правил
                }
            }
        }

        List<string> Together(List<string> first, List<string> second)//пересечение двух объектов 
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
