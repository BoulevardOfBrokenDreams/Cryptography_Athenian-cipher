using System.Text;

namespace CryptografyMurashko1
{
    public class MainClass
    {
        public static int alphabetLength = 30011;

        private const int M = 4453;
        private const int L = 5521;

        private static void main(string[] args)
        {
            StringBuilder text = new StringBuilder();
            text.Append("This textbook is intended for students who have already studied basic mathematics and need to study the methods of higher mathematics. It covers three content areas: Linear Algebra, Vector Algebra and Analytical Geometry. Each part contains basic mathematical conceptions and explains new mathematical terms. Many useful examples and exercises are presented in the textbook. Explained and illustrated by examples and exercises. The Linear Algebra topics include matrix operations, determinants and systems of linear equations. In the section “Vector Algebra”, a main attention is paid to the geometrical applications of vector operations. The vector approach is considered to be basic for discussion of classic problems of Analytical Geometry. The author welcomes reader’s suggestions for improvement of future editions of this textbook (V.V. Konev, LINEAR ALGEBRA, VECTOR ALGEBRA AND ANALYTICAL GEOMETRY).");

            StringBuilder encryptedText = Encrypt(text);
            Console.WriteLine(encryptedText);
            Console.WriteLine();

            StringBuilder decryptedText = Decrypt(encryptedText);
            //StringBuilder decryptedText = DecryptWithNoKey(encryptedText);
            Console.WriteLine(decryptedText);
        }

        //Поиск обратного элемента 
        public static int Obr(int m)
        {
            for (int i = 0; i < 30012; i++)
            {
                if (i * m % 30011 == 1)
                {
                    return i;
                }
            }
            return -1;
        }

        //Шифрование
        private static StringBuilder Encrypt(StringBuilder text)
        {
            StringBuilder ans = new StringBuilder();
            ans.Append(text);

            int length = ans.Length;
            for (int i = 0; i < length; i++)
            {
                ans[i] = (char)((ans[i] * M + L) % alphabetLength);
            }
            return ans;
        }

        //Дешифрование, зная ключ
        private static StringBuilder Decrypt(StringBuilder text, int m = M, int l = L)
        {
            int mObr = Obr(m);
            int length = text.Length;
            for (int i = 0; i < length; i++)
            {
                text[i] = (char)(text[i] * mObr % alphabetLength - l * mObr % alphabetLength);
            }
            return text;
        }

        //Дешифрование, не зная ключ
        private static StringBuilder DecryptWithNoKey(StringBuilder encryptedtext)
        {
            char mostCommonLetter1 = MostCommonLetter(encryptedtext, '0');
            char mostCommonLetter2 = MostCommonLetter(encryptedtext, mostCommonLetter1);

            char space = ' ';
            char e = 'e';

            int Y = (mostCommonLetter1 - mostCommonLetter2) % alphabetLength;
            if (Y < 0)
            {
                Y = alphabetLength + Y;
            }
            int X = (space - e) % alphabetLength;
            if (X < 0)
            {
                X = alphabetLength + X;
            }

            int revX = Obr(X);
            int m = Y * revX % alphabetLength;
            if (m < 0)
            {
                m = alphabetLength + m;
            }
            int l = (mostCommonLetter1 - m * space) % alphabetLength;
            if (l < 0)
            {
                l = alphabetLength + l;
            }

            return Decrypt(encryptedtext, m, l);
        }

        //Поиск самого частого символа в тексте
        public static char MostCommonLetter(StringBuilder text, char notCountedChar)
        {
            Dictionary<char, int> frequency = new Dictionary<char, int>();

            int max = 0;
            char mostCommonLetter = 'б';

            int length = text.Length;
            for (int i = 0; i < length; i++)
            {
                if (text[i] == notCountedChar)
                {
                    continue;
                }

                if (frequency.ContainsKey(text[i]))
                {
                    frequency[text[i]]++;
                }
                else
                {
                    frequency.Add(text[i], 1);
                }
                if (frequency[text[i]] > max)
                {
                    max = frequency[text[i]];
                    mostCommonLetter = text[i];
                }
            }
            return mostCommonLetter;
        }
    }


    public class PolinomEncryption
    {
        private static readonly Polinom K = new Polinom(0b11001011); //x^4 + x^3
        private static readonly Polinom N = new Polinom(0b1101100);  //x^3 + 1

        public static Polinom alphabetLength = new Polinom(0b100011011);

        private static void main(string[] args)
        {
            StringBuilder text = new StringBuilder();
            text.Append("This textbook is intended for students who have already " +
                "studied basic mathematics and need to study the methods of higher mathematics. " +
                "It covers three content areas: Linear Algebra, Vector Algebra and Analytical Geometry. " +
                "Each part contains basic mathematical conceptions and explains new mathematical terms. " +
                "Many useful examples and exercises are presented in the textbook. " +
                "Explained and illustrated by examples and exercises. " +
                "The Linear Algebra topics include matrix operations, determinants and systems of linear equations. " +
                "In the section “Vector Algebra”, a main attention is paid to the geometrical applications of vector operations. " +
                "The vector approach is considered to be basic for discussion of classic problems of Analytical Geometry. " +
                "The author welcomes reader’s suggestions for improvement of future editions of this textbook " +
                "(V.V. Konev, LINEAR ALGEBRA, VECTOR ALGEBRA AND ANALYTICAL GEOMETRY).");

            StringBuilder encryptedText = Encrypt(text, K, N);
            Console.WriteLine(encryptedText);
            Console.WriteLine();

            StringBuilder decryptedText = Decrypt(encryptedText, K, N);
            //StringBuilder decryptedText = DecryptWithNoKey(encryptedText);
            Console.WriteLine(decryptedText);
        }

        //Поиск обратного элемента 
        public static Polinom Obr(Polinom k)
        {
            for (int i = 0; i < 10000; i++)
            {
                if (((new Polinom(i) * k) % alphabetLength).Value == 1)
                {
                    return new Polinom(i);
                }
            }
            return new Polinom(-1);
        }

        //Шифрование
        public static StringBuilder Encrypt(StringBuilder text, Polinom k, Polinom n)
        {
            StringBuilder ans = new StringBuilder();
            ans.Append(text);

            int length = ans.Length;
            for (int i = 0; i < length; i++)
            {
                ans[i] = (char)((new Polinom(ans[i]) * k % alphabetLength + n) % alphabetLength).Value;
            }
            return ans;
        }

        //Дешифрование, зная ключ
        public static StringBuilder Decrypt(StringBuilder text, Polinom k, Polinom n)
        {
            Polinom obr = Obr(k);

            int length = text.Length;
            for (int i = 0; i < length; i++)
            {
                text[i] = (char)((new Polinom(text[i]) * obr % alphabetLength + n * obr % alphabetLength) % alphabetLength).Value;
            }
            return text;
        }

        //Дешифрование, не зная ключ
        public static StringBuilder DecryptWithNoKey(StringBuilder encryptedtext)
        {
            Polinom mostCommonLetter1 = new Polinom(MainClass.MostCommonLetter(encryptedtext, ' '));
            Polinom mostCommonLetter2 = new Polinom(MainClass.MostCommonLetter(encryptedtext, (char)mostCommonLetter1.Value));

            Polinom space = new Polinom(' ');
            Polinom e = new Polinom('e');

            Polinom Y = (mostCommonLetter1 + mostCommonLetter2) % alphabetLength;
            Polinom X = (space + e) % alphabetLength;

            Polinom revX = Obr(X);
            Polinom k = Y * revX % alphabetLength;
            Polinom n = (mostCommonLetter1 + k * space) % alphabetLength;

            return Decrypt(encryptedtext, k, n);
        }
    }


    public class Polinom
    {
        public int Value { get; set; }
        public Polinom(int value = 0)
        {
            Value = value;
        }

        public static Polinom operator +(Polinom pol1, Polinom pol2)
        {
            return new Polinom { Value = pol1.Value ^ pol2.Value };
        }

        public static Polinom operator *(Polinom pol1, Polinom pol2)
        {
            Polinom ans = new Polinom();
            int temp = pol2.Value;
            for (int i = 8; i > 0; i--, temp <<= 1)
            {
                if ((temp & 128) == 128)
                {
                    ans += Mul(pol1, i - 1);
                }
            }
            return ans;
        }
        public static Polinom operator %(Polinom pol1, Polinom pol2)
        {
            int x = GetLastOne(pol1) - GetLastOne(pol2);
            while (x >= 0)
            {
                pol1.Value ^= (pol2.Value << x);
                x = GetLastOne(pol1) - GetLastOne(pol2);
            }
            return pol1;
        }

        public static Polinom Mul(Polinom pol, int a)
        {
            Polinom ans = new Polinom();
            int value = pol.Value;
            ans.Value = value << a;
            Console.WriteLine(ans.Value);
            return ans;
        }

        public static int GetLastOne(Polinom pol)
        {
            int q = pol.Value;
            int i;
            long last = 0b10000000000000000000000000000000;
            for (i = 32; (q & last) == 0 && i > 0; i--, q <<= 1)
            {
                ;
            }

            return i;
        }
    }


}
