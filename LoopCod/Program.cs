using System;
using System.Collections.Generic;
using System.Linq;

namespace LoopCode
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(" Циклический код(7,4) ");
                Console.WriteLine(new string('-', 25));
                Console.WriteLine("Введите число, которое нужно передать: ");
                var str = Console.ReadLine();
                str = getStringBinary(str);


                LoopCode CyclicCode = new LoopCode(str);
                var code = CyclicCode.encode();

                Console.WriteLine("Кодовое слово: {0}", code);
                var temp = code.ToArray();
                var randBit = Rabdomizer(0, 7);
                temp[randBit] = '1';

                CyclicCode.Current = new string(temp);
                var check = CyclicCode.decoding();
                if (check.Length <= 4)
                {
                    Console.WriteLine("Принятое слово: {0}\n", CyclicCode.Current);
                    CyclicCode.SerachErrors(check);
                }
                else
                {
                    Console.WriteLine("Принятое слово: {0}\n", CyclicCode.Current);
                    Console.WriteLine(check);
                }
                CyclicCode.BinaryinDec();
                Console.ReadKey();
            }
        }

        public static int Rabdomizer(int start, int end)
        {
            Random rand = new Random();
            return rand.Next(start, end);

        }

        static string getStringBinary(string str)
        {
            int y = 0;

            int.TryParse(str, out y);

            var binarychar = Convert.ToString(y, 2).PadLeft(4, '0').ToArray();
            return new string(binarychar);

        }

    }

    public class LoopCode
    {
        /// <summary>
        /// Класс Циклического кода
        /// обычное выполнение(задаётся 4 битное кодовое слово, как параметр)
        /// 
        /// </summary>
        protected string _str_cyclicCode;
        private string _str_GPolinome;

        public LoopCode(string codedata)
        {
            _str_cyclicCode = codedata;
            _str_cyclicCode += addThreeZero(_str_cyclicCode);//добавка 3 нулей
            _str_GPolinome = "1011";//g(x)=x^3+x+1   
        }
        private string addThreeZero(string str)
        {
            if (str.Length == 4)
            {
                return "000";
            }
            else
            {
                return str;
            }
        }


        public string Current
        {
            get
            {
                return _str_cyclicCode;
            }
            set
            {
                _str_cyclicCode = value;
            }
        }
        public string encode()
        {
            var tmp = division(_str_cyclicCode, _str_GPolinome);
            _str_cyclicCode = _str_cyclicCode.Remove(_str_cyclicCode.Length - 3, 3);
            _str_cyclicCode += tmp;
            return _str_cyclicCode;
        }
        private string division(string divident, string divisor)
        {
            //Деление 2 полиномов           
            var result = "";//резульятат деления, он нахуй никому не нужен хыыыы
            int part = divisor.Length;
            var tmp = divident.Substring(0, part);

            while (part < divident.Length)
            {
                if (tmp[0] == '1')
                {
                    tmp = xor(divisor, tmp) + divident[part];
                    result += "1";
                }
                else
                {
                    var t = help(part);
                    tmp = xor(t, tmp) + divident[part];
                    result += "0";
                }
                part++;
            }
            if (tmp[0] == '1')
            {
                tmp = xor(divisor, tmp);
                result += "1";
            }
            else
            {
                var t = help(part);
                tmp = xor(t, tmp);
                result += "0";
            }

            return tmp;

        }
        public string decoding()
        {
            var errorbit = division(_str_cyclicCode, _str_GPolinome);
            if (errorbit == "000")
            {
                return "Сообщение передано без ошибки!";
            }
            else
            {
                return errorbit;
            }

        }
        private string help(int count)
        {
            var stroke = "";
            while (count > 0)
            {
                stroke += "0";
                count--;
            }
            return stroke;
        }
        private string xor(string a, string b)
        {
            var res = "";
            for (int i = 1; i < b.Length; i++)
            {
                if (a[i] == b[i])
                {
                    res += '0';
                }
                else
                {
                    res += '1';
                }
            }
            return res;
        }
        public string SerachErrors(string err)
        {
            //в узду эти матрицы :) 101=1;          
            var syndrome = new List<string>(new string[] { "101", "111", "110", "011", "100", "010", "001" });
            var t = syndrome.FindIndex(x => x == err);
            Console.WriteLine("Произошла ошибка в {0} бите", t + 1);
            var charmm = _str_cyclicCode.ToArray();
            if (charmm[t] == '1')
            {
                charmm[t] = '0';
            }
            else
            {
                charmm[t] = '1';
            }
            _str_cyclicCode = new string(charmm);
            Show(t);

            return _str_cyclicCode;


        }


        public void BinaryinDec()
        {

            var r = _str_cyclicCode.Remove(4);
            var dec = Convert.ToInt32(r, 2);
            Console.WriteLine(new string('-', 25));
            Console.WriteLine("\nПередаваемое число - {0}", dec);

        }
        private void Show(int ind)
        {
            for (int i = 0; i < _str_cyclicCode.Length; i++)
            {
                if (i == ind)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(_str_cyclicCode[i]);
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(_str_cyclicCode[i]);
                }
            }
            Console.WriteLine();
        }
    }


}
