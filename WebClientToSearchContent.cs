using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
namespace WebClient_ConsoleApp
{
    public class SearchContent
    {
        public static string findContent(string content)
        {
            Stack<int> stack = new Stack<int>();
            int len = content.Length;
            bool ok = false;
            int from = -1;
            for(int i=0; i<len;)
            {
                if (content[i] == '>')
                {
                    int idx = i+1;
                    //Console.WriteLine(idx);
                    if (content[idx] == '<')
                    {
                        i++;
                        continue;
                    }
                    while (true)
                    {
                        if (content[idx] == '<') break;
                        if (content[idx] != ' ')
                        {
                            ok = true;
                            break;
                        }
                        idx++;
                    }
                    if (ok == true)
                    {
                        from = i + 1;
                        break;
                    }
                }
                else i++;
            }
           // Console.WriteLine("From : " + from);
            string ret = "";
            for(int i=from; i<len; i++)
            {
                if (content[i] == '<') break; 
                ret += content[i];
            }
            return ret;
        }
        public static bool findHeading(char a, char b, char c)
        {
            return (a == '<' && b == 'h' && (c >= '1' && c <= '6')) ;
        }
        public static void DownloadString(string address)
        {
            WebClient client = new WebClient();
            string reply = client.DownloadString(address);
            int len = reply.Length;
            Console.WriteLine(len);
            //Console.WriteLine(reply);
            Console.WriteLine("\n\n\n New line from here:::\n\n\n");
            List<Tuple<string, string>> classes= new List<Tuple<string, string>>();
            List<string> list = new List<string>();
            for (int i=0; i+2<len;)
            {
                if (reply[i] == '<' && reply[i + 1] == 'h' && reply[i + 2] == '1')
                {
                    string temp = "";
                    int idx = i + 3;
                    temp += reply[i];
                    temp += reply[i+1];
                    temp += reply[i+2];
                    while (idx+2 < len)
                    {
                        if (reply[idx] == 'h' && reply[idx + 1] == '1' && reply[idx + 2] == '>')
                        {
                            temp += reply[idx];
                            temp += reply[idx + 1];
                            temp += reply[idx + 2];
                            break;
                        }
                        temp += reply[idx];
                        idx++;
                    }
                    int next_idx = idx;

                    bool have = false;
                    string price = "";
                    while(next_idx < len)
                    {
                        if (reply[next_idx] == '<' && reply[next_idx + 1] == 'h' && reply[next_idx+2] == '1')
                        {
                            break;
                        }
                        if (reply[next_idx] == '$')
                        {
                            if(next_idx+1 < len && (reply[next_idx+1] >= '0' && reply[next_idx+1] <= '9')) {
                                have = true;
                                string current_price = "$";
                                int price_index = next_idx + 1;
                                while (true)
                                {
                                    if (Char.IsDigit(reply, price_index) == false) break;
                                    current_price += reply[price_index];
                                    price_index++;
                                }
                                price = current_price;
                            }
                        }
                        next_idx++;
                    }
                    if (have == false) price = "$0";
                    i = idx+2;
                    /*list.Add(temp);*/
                    Tuple<string, string> tmptuple = new Tuple<string, string>(temp, price);

                    classes.Add(tmptuple);
                }
                else i++;
            }
            Console.WriteLine("Total Class : " + classes.Count + "\n\n");

            foreach (Tuple<string, string> item in classes)
            {
                Console.WriteLine(item.Item1 + "   " + item.Item2);
            }
            Console.WriteLine("\n\nClass Name      |        Price");
            foreach(Tuple<string, string> item in classes)
            {
                string con1 = item.Item1;
                string price = item.Item2;
                string content = SearchContent.findContent(con1);
                Console.WriteLine(content + " |   " + price);
            }
        }
    }

    public class Program
    {
        static  void Main(string[] args)
        {
            //string address = "https://www.providence.org/locations/socal/mission-hospital-mission-viejo/classes-and-events";
            string address = "https://casa-natal.com/classes-groups/";
            SearchContent.DownloadString(address);
        }
    }
}
