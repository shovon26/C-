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
    public class GenericClassContent
    {
        public static string findContent(string content)
        {
            Stack<int> stack = new Stack<int>();
            int len = content.Length;
            bool ok = false;
            int from = -1;
            for (int i = 0; i < len;)
            {
                if (content[i] == '>')
                {
                    int idx = i + 1;
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

            string ret = "";
            for (int i = from; i < len; i++)
            {
                if (content[i] == '<') break;
                ret += content[i];
            }
            return ret;
        }
        public List<Tuple<string, string>> FindClassList(string address, Tuple<char, char, char, char, char, char> tag)
        {
            WebClient client = new WebClient();
            string reply = client.DownloadString(address);
            Console.WriteLine(reply);
            int len = reply.Length;
            //Console.WriteLine(len);
            List<Tuple<string, string>> classes = new List<Tuple<string, string>>();
            List<string> list = new List<string>();
            for (int i = 0; i + 2 < len;)
            {
                if (reply[i] == tag.Item1 && reply[i + 1] == tag.Item2 && reply[i + 2] == tag.Item3)
                {
                    string temp = "";
                    int idx = i + 3;
                    temp += reply[i];
                    temp += reply[i + 1];
                    temp += reply[i + 2];
                    while (idx + 2 < len)
                    {
                        if (reply[idx] == tag.Item4 && reply[idx + 1] == tag.Item5 && reply[idx + 2] == tag.Item6)
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
                    //Console.WriteLine(tag.Item1 + " " + tag.Item2 + " " + tag.Item3);
                    while (next_idx < len)
                    {
                        //Console.WriteLine(next_idx + " " + reply[next_idx]);
                        //Console.WriteLine("Value here : " + reply[next_idx] + " " + reply[next_idx+1] + " " + reply[next_idx+2]);
                        if (reply[next_idx] == tag.Item1 && reply[next_idx + 1] == tag.Item2 && reply[next_idx + 2] == tag.Item3)
                        {
                            break;
                        }
                        //if (reply[next_idx] == '$') Console.WriteLine("Possible:");
                        if (reply[next_idx] == '$')
                        {
                            int from = next_idx + 1;
                            have = true;
                            //Console.WriteLine("From previous : " + from);
                            while (true)
                            {
                                if (Char.IsDigit(reply, from) == true) break;
                                from++;
                            }
                            //Console.WriteLine("From next : " + from);
                            string current_price = "$";
                            int price_index = from;
                            while (true)
                            {
                                if (Char.IsDigit(reply, price_index) == false && reply[price_index] != '.') break;
                                current_price += reply[price_index];
                                price_index++;
                            }
                            price = current_price;
                        }
                        if(next_idx + 3 < len && reply[next_idx] == '&' && reply[next_idx + 1] == '#' && reply[next_idx + 2] == '3' && reply[next_idx + 3] == '6')
                        {
                            int from = next_idx + 4;
                            //Console.WriteLine("From previous : " + from);
                            while (true)
                            {
                                if (Char.IsDigit(reply, from) == true) break;
                                have = true;
                                from++;
                            }
                            //Console.WriteLine("From next : " + from);
                            string current_price = "$";
                            int price_index = from;
                            while (true)
                            {
                                if (Char.IsDigit(reply, price_index) == false && reply[price_index] != '.') break;
                                current_price += reply[price_index];
                                price_index++;
                            }
                            price = current_price;
                        }
                        next_idx++;
                    }
                    if (have == false) price = "$0";
                    i = idx + 2;
                    Tuple<string, string> tmptuple = new Tuple<string, string>(temp, price);
                    classes.Add(tmptuple);
                }
                else i++;
            }

            List < Tuple<string, string> > ans = new List < Tuple<string, string> >();
            Console.WriteLine("Class List and Their Price :\n\n");
            foreach (Tuple<string, string> item in classes)
            {
                string con1 = item.Item1;
                string price = item.Item2;
                string content = GenericClassContent.findContent(con1);
                Tuple<string, string> temp = new Tuple<string, string>(content, price);
                ans.Add(temp);
            }
            return ans;
        }
    }
    public class SearchContent
    {
        GenericClassContent genericClassContent = new GenericClassContent();
        public void forH1Tag(string address)
        {
            Tuple<char, char, char, char, char, char> tag = new Tuple<char, char, char, char, char, char>('<', 'h', '1', 'h', '1', '>');
            List<Tuple<string, string>> classList = genericClassContent.FindClassList(address, tag);

            foreach(Tuple<string, string> item in classList)
            {
                Console.WriteLine(item.Item1 + ",   " + item.Item2);
            }
        }

        public void forH2Tag(string address)
        {
            Tuple<char, char, char, char, char, char> tag = new Tuple<char, char, char, char, char, char>('<', 'h', '2', 'h', '2', '>');
            List<Tuple<string, string>> classList = genericClassContent.FindClassList(address, tag);

            foreach (Tuple<string, string> item in classList)
            {
                Console.WriteLine(item.Item1 + ",   " + item.Item2);
            }
        }

        public void forH3Tag(string address)
        {
            Tuple<char, char, char, char, char, char> tag = new Tuple<char, char, char, char, char, char>('<', 'h', '3', 'h', '3', '>');
            List<Tuple<string, string>> classList = genericClassContent.FindClassList(address, tag);

            foreach (Tuple<string, string> item in classList)
            {
                Console.WriteLine(item.Item1 + ",   " + item.Item2);
            }
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            string address = "https://gracefull.com/online-classes/"; //h2 tag
            //string address = "https://www.sutterhealth.org/classes-events-search?keywords=eclass&remove-default-affiliate-filter=true&attachQS=true"; //h2 tag
            //string address = "https://casa-natal.com/classes-groups/"; //h1 tag

            SearchContent searchContent = new SearchContent();
            searchContent.forH1Tag(address);
        }
    }
}
