namespace CryptografyMurashko1
{
    public class First
    {
        public static readonly int _alphabet = 30012;
        public static readonly int _polinomialAlphabet = 0b100011011;
        public static readonly int mod = 30011;

        private static int varNumber;

        private static int M;
        private static int L;
        public static int Inverse;


        public static int Nx;
        public static int Kx;

        private static readonly string _text = "This textbook is intended for students who have already " +
            "studied basic mathematics and need to study the methods of higher mathematics. " +
            "It covers three content areas: Linear Algebra, Vector Algebra and Analytical Geometry. " +
            "Each part contains basic mathematical conceptions and explains new mathematical terms. " +
            "Many useful examples and exercises are presented in the textbook. " +
            "Explained and illustrated by examples and exercises. " +
            "The Linear Algebra topics include matrix operations, determinants and systems of linear equations. " +
            "In the section “Vector Algebra”, a main attention is paid to the geometrical applications of vector operations. " +
            "The vector approach is considered to be basic for discussion of classic problems of Analytical Geometry. " +
            "The author welcomes reader’s suggestions for improvement of future editions of this textbook " +
            "(V.V. Konev, LINEAR ALGEBRA, VECTOR ALGEBRA AND ANALYTICAL GEOMETRY).";

        public static void Main()
        {
            TextWriter tw = new StreamWriter(@"Out.txt");
            Console.Write("введите номер варианта:");
            varNumber = Convert.ToInt32(Console.ReadLine());

            FindM(); FindL(); FindNx(); FindKx(); Inverse = FindInverse(M);

            //для чисел
            tw.WriteLine("Работа с числами\nШифротеекст");
            //шифрование текста
            string closedText = Numbers.Encryption(_text, M, L);

            //запись в зашифрованном виде
            tw.WriteLine(closedText + "\n\nДешифрование с ключом");

            //запись в расшифрованном виде по ключу
            tw.WriteLine(Numbers.DecryptionWithKey(closedText, M, L) + "\n\nДешифрование без ключа");

            //запись в расшифрованном виде без ключа
            tw.WriteLine(Numbers.DecryptionWithoutKey(closedText));

            tw.WriteLine();

            //для многочленов
            tw.WriteLine("Работа с многочленами\nШифротекст");
            closedText = Polinomials.Encryption(_text, Kx, Nx);
            
            tw.WriteLine(closedText);

            //дешифрование с ключом
            tw.WriteLine("Дешифрование с ключом");
            tw.WriteLine(Polinomials.DecryptionWithKey(closedText, Kx, Nx));

            //Дешифрование без ключа
            tw.WriteLine("Дешифрование без ключа");
            tw.WriteLine(Polinomials.DecryptionWithoutKey(closedText));


            tw.Flush();
            tw.Close();
        }

        private static void FindL()
        {
            L = (int)(Math.Sqrt(30202020 + 20190 * varNumber) + 24 * varNumber);
        }

        private static void FindM()
        {
            M = (int)(Math.Sqrt(20192019 - 20200 * varNumber) - 38 * varNumber);
        }

        private static void FindNx()
        {
            Nx = 9 * varNumber % 256;
        }

        private static void FindKx()
        {
            Kx = 7 * (41 - varNumber) % 256;
        }

        public static int FindInverse(int N)
        {
            for (int i = 0; i < 30012; i++)
            {
                if (i * N % 30011 == 1)
                {
                    return i;
                }
            }
            return -1;
        }
    }

    internal static class Numbers
    {
        public static string Encryption(string text, int M, int L)
        {
            string Result = "";

            for (int i = 0; i < text.Length; i++)
            {
                Result += (char)((text[i] * M + L) % First.mod);
            }

            return Result;
        }

        public static string DecryptionWithKey(string text, int M, int L)
        {
            string Result = "";

            for (int i = 0; i < text.Length; i++)
            {
                Result += (char)(text[i] * First.Inverse % First.mod - L * First.Inverse % First.mod);
            }

            return Result;
        }

        public static string DecryptionWithoutKey(string Text)
        {
            char firstchar = FindMostCommonChar(Text, 'ф');
            char secondChar = FindMostCommonChar(Text, firstchar);

            char space = ' ';
            char e = 'e';

            int T1 = (firstchar - secondChar) % First.mod;
            if (T1 < 0)
            {
                T1 = First.mod + T1;
            }

            int X = (space - e) % First.mod;
            if (X < 0)
            {
                X = First.mod + X;
            }

            int revX = First.FindInverse(X);
            int M = T1 * revX % First.mod;
            if (M < 0)
            {
                M = First.mod + M;
            }
            int L = (firstchar - M * space) % First.mod;
            if (L < 0)
            {
                L = First.mod + L;
            }

            return DecryptionWithKey(Text, M, L);
        }

        public static char FindMostCommonChar(string text, char notCountedChar)
        {
            Dictionary<char, int> keyValues = new Dictionary<char, int>();

            int max = 0;
            char necessarychar = 'я';

            int length = text.Length;
            for (int i = 0; i < length; i++)
            {
                if (text[i] == notCountedChar)
                {
                    continue;
                }

                if (keyValues.ContainsKey(text[i]))
                {
                    keyValues[text[i]]++;
                }
                else
                {
                    keyValues.Add(text[i], 1);
                }
                if (keyValues[text[i]] > max)
                {
                    max = keyValues[text[i]];
                    necessarychar = text[i];
                }
            }
            return necessarychar;
        }
    }


    //TODO прекратить страдать фигней и сделать перегрузку операторов
    //создать отдельный класс для перегрузки
    internal static class Polinomials
    {
        public static int FindInverse(int Kx)
        {
            for (int i = 0; i < 10000; i++)
            {
                if (DivR(Imul(i, Kx), First._polinomialAlphabet) == 1)
                {
                    return i;
                }
            }
            return -1;
        }

        public static int Sum(int firstPolinomial, int secondPolinomial)
        {
            return firstPolinomial ^ secondPolinomial;
        }

        public static int Imul(int firstPolinomial, int secondPolinomial)
        {
            int temp = secondPolinomial;
            int result = 0;

            for (int i = 8; i > 0; i--, temp <<= 1)
            {
                if ((temp & 128) == 128)
                {
                    result ^= (firstPolinomial << (i - 1));
                }
            }
            return result;
        }

        public static int DivR(int resultingPolinomial, int secondPolinomial)
        {
            int check = Lastbit(resultingPolinomial) - Lastbit(secondPolinomial);
            while (check >= 0)
            {
                resultingPolinomial ^= (secondPolinomial << check);
                check = Lastbit(resultingPolinomial) - Lastbit(secondPolinomial);
            }
            return resultingPolinomial;
        }

        public static int Lastbit(int polinomial)
        {
            int i;
            long last = 0b10000000000000;
            for (i = 32; (polinomial & last) == 0 && i > 0; i--, polinomial <<= 1)
            {
                ;
            }

            return i;
        }

        public static string Encryption(string text, int Kx, int Nx)
        {
            string result = "";

            for (int i = 0; i < text.Length; i++)
            {
                result += (char)(DivR(Sum(DivR(Imul(text[i], Kx), First._polinomialAlphabet), Nx), First._polinomialAlphabet));
            }
            return result;
        }

        public static string DecryptionWithKey(string text, int Kx, int Nx)
        {
            string result = "";
            int Inverse = FindInverse(Kx);

            for (int i = 0; i < text.Length; i++)
            {
                result += (char)DivR(Sum(DivR(Imul(text[i], Inverse), First._polinomialAlphabet), DivR(Imul(Nx, Inverse), First._polinomialAlphabet)), First._polinomialAlphabet);
            }

            return result;
        }

        public static string DecryptionWithoutKey(string text)
        {
            int firstChar = Numbers.FindMostCommonChar(text, ' ');
            int secondChar = Numbers.FindMostCommonChar(text, (char)firstChar);

            int space = ' ';
            int e = 'e';

            int Y = DivR(Sum(firstChar, secondChar), First._polinomialAlphabet);
            int X = DivR(Sum(space, e), First._polinomialAlphabet);

            int inverseX = FindInverse(X);
            int k = DivR(Imul(Y, inverseX), First._polinomialAlphabet);
            int n = DivR(Sum(firstChar, Imul(k, space)), First._polinomialAlphabet);

            return DecryptionWithKey(text, k, n);
        }
    }
}
