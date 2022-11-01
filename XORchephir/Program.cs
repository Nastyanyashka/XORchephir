using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace XORchephir
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = String.Empty;
            string input_text = String.Empty;
            Console.WriteLine("Укажите путь для создания файла/записи в файл");
            path = Console.ReadLine();
            StreamReader? reader = null;
            bool type;

            Console.WriteLine("Нажмите 1 если хотите написать/перезаписать сообщение в файл\n " +
                "Нажмите 2 если хотите прочитать из файла сообщение и записать его в файл");
            char flag = Console.ReadKey().KeyChar;
            if (flag == '1')
            {
                Console.Write("\nВведите сообщение которое вы хотите зашифровать/дешифровать: ");
                input_text = Console.ReadLine();
                type = false;
            }
            else if(flag =='2')
            {
                reader = new StreamReader(path);
                input_text = reader.ReadToEnd();//Считываем из файла текст
                type = true;
                reader.Close();
            }
            else {
                Console.Write("Что-то пошло не так");
                return;
            }
            StreamWriter writer = new StreamWriter(path, type);

            string key = String.Empty;
           
            Console.Write("\nВведите ключ: ");
            key = Console.ReadLine();
            string final_text = EncryptDecrypt(input_text,key);//Шифруем/Дешифруем текст
            writer.Write(input_text);//Записываем входные данные
            writer.Write(final_text);//Записываем выходные данные
            Console.WriteLine(final_text);

            writer.Close();

            string EncryptDecrypt(string input_text,string key)
            { 
            string coded_key = string.Empty;
            string coded_text = String.Empty;
            string output_text = String.Empty;
                int code_length;
                for (int i = 0; i < input_text.Length; i++)
                {
                    string code = Convert.ToString(input_text[i], 2);//переводим в двоичный код каждый символ из текста
                    if (code.Length != 8)
                    {
                        code_length = code.Length;
                        for (int j = 0; j < 8 - code_length; j++)
                        {
                            code = code.Insert(0, "0");
                        }
                    }
                    coded_text += code;//добавляем каждый символ к общему тексту
                }
                for (int i = 0; i < key.Length; i++)
                {
                    string code = Convert.ToString(key[i], 2);//переводим в двоичный код каждый символ из ключа
                    if (code.Length != 8)
                    {
                        code_length = code.Length;
                        for (int j = 0; j < 8 - code_length; j++)
                        {
                            code = code.Insert(0, "0");
                        }
                    }
                    coded_key += code;//добавляем каждый символ к ключу
                }
                //8
                for (int i = 0; i < input_text.Length; i++)
                {
                    for (int j = i * 8; j < (i + 1) * 8; j++)
                    {
                        output_text += (coded_text[j] ^ coded_key[j % coded_key.Length]);//шифруем методом XOR
                        
                    }
                    if (i != input_text.Length - 1)
                    {
                        output_text += " ";
                    }
                }

                var bytes = output_text.Split(' ').Select(x => Convert.ToByte(x, 2)).ToArray();
                return Encoding.ASCII.GetString(bytes);//Возращаем зашифрованную строку
            }
        }
    }
}
