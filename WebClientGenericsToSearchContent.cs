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
        private static string findContent(string content) //remove tag and return only class name
        {
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

        private static string removeDescriptionTag(string content)  //remove description tag and return only raw description
        {
            string result = "";

            int len = content.Length;
            bool space = false;
            for(int i=0; i < len;)
            {
                if (content[i] == '<')
                {
                    while (i < len)
                    {
                        if (content[i] == '>')
                        {
                            i++;
                            if (!space) space = true;
                            else result += ' ';
                            break;
                        }
                        i++;
                    }
                }
                else
                {
                    result += content[i];
                    i++;
                }
            }

            return result;
        }

        private static int convertPrice(string price)
        {
            string tmp = "";
            for(int i=1; i<price.Length; i++)
            {
                if (price[i] >= '0' && price[i] <= '9') tmp += price[i];
                else break;
            }
            int sum = 0;
            int mul = 1;
            for(int i=tmp.Length-1; i>=0; i--)
            {
                sum += (tmp[i] - '0') * mul;
                mul *= 10;
            }
            return sum;
        }

        private static string findClassTag(string address, Tuple<char, char, char, char, char, char> tag) //tag which is started by every class under a specific birth center
        {
            WebClient client = new WebClient();
            string reply = client.DownloadString(address);

            int len = reply.Length;

            List<Tuple<string, string, string>> classes = new List<Tuple<string, string, string>>();
            string className = "";
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

                    if (temp.Substring(4, 5) == "Class" || temp.Substring(4, 7) == "Classes")
                    {
                        i++;
                        continue;
                    }
                    else
                    {
                        className = temp;
                        break;
                    }
                }
                else i++;
            }
            return className.Substring(1, 2);
        }


        public List<Tuple<string, string, string>> findClassListByBig(string address, string tag)
        {
            WebClient client = new WebClient();
            client.Headers["Content-Type"] = "text/html";
            string reply = client.DownloadString(address);
            int len = reply.Length;
            List<Tuple<string, string, string>> classes = new List<Tuple<string, string, string>>();
            for(int i=0; i < len;)
            {
                if (i + 5 < len && reply.Substring(i, 5) == tag.Substring(0, 5))
                {
                    string temp = reply.Substring(i, 5);

                    int idx = i + 5;
                    while(idx+6 < len)
                    {
                        if(idx+6 < len && reply.Substring(idx, 6) == tag.Substring(5, 6))
                        {
                            temp += reply.Substring(idx, 6);
                            break;
                        }
                        temp += reply[idx];
                        idx++;
                    }

                    string classPrice = "";

                    // Finding class Price
                    try
                    {
                        int next_idx = idx+7;
                        bool have = false;
                        //string price = "";
                        while (next_idx < len)
                        {
                            if (i + 5 < len && reply.Substring(next_idx, 5) == tag.Substring(0, 5))
                            {
                                break;
                            }
                            if (reply[next_idx] == '$')
                            {
                                int from = next_idx + 1;
                                have = true;
                                while (true)
                                {
                                    if (Char.IsDigit(reply, from) == true) break;
                                    from++;
                                }
                                string current_price = "$";
                                int price_index = from;
                                while (true)
                                {
                                    if (Char.IsDigit(reply, price_index) == false && reply[price_index] != '.') break;
                                    current_price += reply[price_index];
                                    price_index++;
                                }
                                //Console.WriteLine("Current Price : " + current_price);

                                int P = GenericClassContent.convertPrice(current_price);
                                if (P >= 10 && P <= 500)
                                {
                                    classPrice = current_price;
                                    break;
                                }
                                else
                                {
                                    classPrice = "$0";
                                }
                                next_idx++;
                            }
                            else if (next_idx + 3 < len && reply[next_idx] == '&' && reply[next_idx + 1] == '#' && reply[next_idx + 2] == '3' && reply[next_idx + 3] == '6')
                            {
                                //Console.WriteLine("Block 2");
                                int from = next_idx + 4;
                                while (true)
                                {
                                    if (Char.IsDigit(reply, from) == true) break;
                                    have = true;
                                    from++;
                                }
                                string current_price = "$";
                                int price_index = from;
                                while (true)
                                {
                                    if (Char.IsDigit(reply, price_index) == false && reply[price_index] != '.') break;
                                    current_price += reply[price_index];
                                    price_index++;
                                }

                                int P = GenericClassContent.convertPrice(current_price);
                                if (P >= 10 && P <= 500)
                                {
                                    classPrice = current_price;
                                    break;
                                }
                                else
                                {
                                    classPrice = "$0";
                                }
                                next_idx++;
                            }
                            else next_idx++;
                        }
                        if (have == false) classPrice = "$0";
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error1 : " + ex.Message);
                    }


                    // Finding class Description
                    try
                    {
                        int from = idx;

                        // find Ending paragraph
                        int nextClassIndex = from + 1;
                        bool ok = false;
                        while (nextClassIndex < len)
                        {
                            if (nextClassIndex + 5 < len)
                            {
                                if (reply.Substring(nextClassIndex, 5) == tag.Substring(0, 5))
                                {
                                    ok = true;
                                    break;
                                }
                            }
                            nextClassIndex++;
                        }
                        if (ok == false)
                        {
                            string add = "";
                            add += reply[from];
                            add += reply[from + 1];
                            add += reply[from + 2];
                            for (int rep = from + 3; rep < len; rep++)
                            {
                                if (rep + 2 < len && reply[rep] == '<' && reply[rep + 1] == 'H' && reply[rep + 2] == 'R')
                                {
                                    add += reply[rep];
                                    add += reply[rep + 1];
                                    add += reply[rep + 2];
                                    break;
                                }
                                add += reply[rep];
                            }
                            //Console.WriteLine("Add string : " + add);
                            Tuple<string, string, string> tupleNow = new Tuple<string, string, string>(temp, add, classPrice);
                            classes.Add(tupleNow);
                            break;
                        }

                        int to = -1;
                        while (nextClassIndex > from)
                        {
                            if (nextClassIndex-2 >= 0 && reply[nextClassIndex] == 'R' && reply[nextClassIndex - 1] == 'H' && reply[nextClassIndex - 2] == '<')
                            {
                                to = nextClassIndex;
                                break;
                            }
                            nextClassIndex--;
                        }
                        if (to == -1) to = len - 1;
                        string delta = "";
                        for (int rep = from; rep <= to; rep++) delta += reply[rep];
                        Tuple<string, string, string> tmp = new Tuple<string, string, string>(temp, delta, classPrice);
                        classes.Add(tmp);

                        i = to;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error2 : " + ex.Message);
                        break;
                    }

                    
                }
                else i++;
            }

            List<Tuple<string, string, string>> ans = new List<Tuple<string, string, string>>();
            Console.WriteLine("\nClass List and Their Price :\n");
            foreach (Tuple<string, string, string> item in classes)
            {
                string con1 = item.Item1;
                string con2 = item.Item2;
                string className = GenericClassContent.findContent(con1);
                string description = GenericClassContent.removeDescriptionTag(con2);
                Tuple<string, string, string> temp = new Tuple<string, string, string>(className, description, item.Item3);
                ans.Add(temp);
            }
            return ans;
        }
        public List<Tuple<string, string, string>> findClassListByHeading(string address, Tuple<char, char, char, char, char, char> tag)
        {
            WebClient client = new WebClient();
            client.Headers["Content-Type"] = "text/html";
            string reply = client.DownloadString(address);
            int len = reply.Length;
            List<Tuple<string, string, string>> classes = new List<Tuple<string, string, string>>();
            for (int i = 0; i+2 < len;)
            {
                if (i+2 < len && reply[i] == tag.Item1 && reply[i + 1] == tag.Item2 && reply[i + 2] == tag.Item3)
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

                    if(temp.Substring(4, 5) == "Class" || temp.Substring(4, 7) == "Classes")
                    {
                        i++;
                        continue;
                    }

                    string classPrice = "";


                    // Finding class Price
                    try {
                        int next_idx = idx;
                        bool have = false;
                        //string price = "";
                        while (next_idx < len)
                        {
                            if (reply[next_idx] == tag.Item1 && reply[next_idx + 1] == tag.Item2 && reply[next_idx + 2] == tag.Item3)
                            {
                                break;
                            }
                            if (reply[next_idx] == '$')
                            {
                                //Console.WriteLine("Block 1");
                                int from = next_idx + 1;
                                have = true;
                                while (true)
                                {
                                    if (Char.IsDigit(reply, from) == true) break;
                                    from++;
                                }
                                string current_price = "$";
                                int price_index = from;
                                while (true)
                                {
                                    if (Char.IsDigit(reply, price_index) == false && reply[price_index] != '.') break;
                                    current_price += reply[price_index];
                                    price_index++;
                                }
                                //Console.WriteLine("Current Price : " + current_price);
                                
                                int P = GenericClassContent.convertPrice(current_price);
                                if(P >= 10 && P <= 500)
                                {
                                    classPrice = current_price;
                                    break;
                                }
                                else
                                {
                                    classPrice = "$0";
                                }
                                next_idx++;
                            }
                            else if (next_idx + 3 < len && reply[next_idx] == '&' && reply[next_idx + 1] == '#' && reply[next_idx + 2] == '3' && reply[next_idx + 3] == '6')
                            {
                                //Console.WriteLine("Block 2");
                                int from = next_idx + 4;
                                while (true)
                                {
                                    if (Char.IsDigit(reply, from) == true) break;
                                    have = true;
                                    from++;
                                }
                                string current_price = "$";
                                int price_index = from;
                                while (true)
                                {
                                    if (Char.IsDigit(reply, price_index) == false && reply[price_index] != '.') break;
                                    current_price += reply[price_index];
                                    price_index++;
                                }

                                int P = GenericClassContent.convertPrice(current_price);
                                if (P >= 10 && P <= 500)
                                {
                                    classPrice = current_price;
                                    break;
                                }
                                else
                                {
                                    classPrice = "$0";
                                }
                                next_idx++;
                            }
                            else next_idx++;
                        }
                        if (have == false) classPrice = "$0";
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Error1 : " + ex.Message);
                    }


                    // Finding class Description
                    try
                    {
                        //find starting paragraph
                        int next_idx = idx;
                        int from = -1;
                        while (next_idx < len)
                        {
                            if (next_idx + 1 < len && reply[next_idx] == '<' && reply[next_idx + 1] == 'p')
                            {
                                from = next_idx;
                                break;
                            }
                            next_idx++;
                        }
                        
                        //Console.WriteLine("Char : " + reply.Substring(from, 50));

                        if (from == -1) from = idx+3;
                       // Console.WriteLine("Paragraph start : " + from);

                        // find Ending paragraph
                        int nextClassIndex = from + 1;
                        bool ok1 = false;
                        while (nextClassIndex < len)
                        {
                            if (nextClassIndex+3 < len && reply[nextClassIndex] == tag.Item1 && reply[nextClassIndex + 1] == tag.Item2 && reply[nextClassIndex + 2] == tag.Item3)
                            {
                                ok1 = true;
                                break;
                            }
                            nextClassIndex++;
                        }
                        if (ok1 == false)
                        {
                            string add = "";
                            add += reply[from];
                            add += reply[from + 1];
                            add += reply[from + 2];
                            for(int rep=from+3; rep<len; rep++)
                            {
                                if(rep+2 < len && reply[rep] == '/' && reply[rep+1] == 'p' && reply[rep+2] == '>')
                                {
                                    add += reply[rep];
                                    add += reply[rep + 1];
                                    add += reply[rep + 2];
                                    break;
                                }
                                add += reply[rep];
                            }
                            //Console.WriteLine("Add string : " + add);
                            Tuple<string, string, string> tupleNow = new Tuple<string, string, string>(temp, add, classPrice);
                            classes.Add(tupleNow);
                            break;
                        }
                        
                        int to = -1;
                        while (nextClassIndex > from)
                        {
                            if (reply[nextClassIndex] == '>' && reply[nextClassIndex - 1] == 'p' && reply[nextClassIndex - 2] == '/')
                            {
                                to = nextClassIndex;
                                break;
                            }
                            nextClassIndex--;
                        }
                        string delta = "";
                        for (int rep = from; rep <= to; rep++) delta += reply[rep];
                        Tuple<string, string, string> tmp = new Tuple<string, string, string>(temp, delta, classPrice);
                        classes.Add(tmp);

                        i = to;
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Error2 : " + ex.Message);
                        break;
                    }
                }
                else i++;
            }

            List < Tuple<string, string, string> > ans = new List < Tuple<string, string, string> >();
            Console.WriteLine("\nClass List and Their Price :\n");
            foreach (Tuple<string, string, string> item in classes)
            {
                string con1 = item.Item1;
                string con2 = item.Item2;
                string className = GenericClassContent.findContent(con1);
                string description = GenericClassContent.removeDescriptionTag(con2);
                Tuple<string, string, string> temp = new Tuple<string, string, string>(className, description, item.Item3);
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
            List<Tuple<string, string, string>> classList = genericClassContent.findClassListByHeading(address, tag);

            foreach(Tuple<string, string, string> item in classList)
            {
                Console.WriteLine("\n\n" + item.Item1 + ", \n\n " + item.Item2 + " \n\n" + item.Item3);
            }
            Console.WriteLine("\n\n");
        }

        public void forH2Tag(string address)
        {
            Tuple<char, char, char, char, char, char> tag = new Tuple<char, char, char, char, char, char>('<', 'h', '2', 'h', '2', '>');
            List<Tuple<string, string, string>> classList = genericClassContent.findClassListByHeading(address, tag);

            foreach (Tuple<string, string, string> item in classList)
            {
                Console.WriteLine("\n\n" + item.Item1 + ", \n\n " + item.Item2 + " \n\n" + item.Item3);
            }
            Console.WriteLine("\n\n");
        }

        public void forH3Tag(string address)
        {
            Tuple<char, char, char, char, char, char> tag = new Tuple<char, char, char, char, char, char>('<', 'h', '3', 'h', '3', '>');
            List<Tuple<string, string, string>> classList = genericClassContent.findClassListByHeading(address, tag);

            foreach (Tuple<string, string, string> item in classList)
            {
                Console.WriteLine("\n\n" + item.Item1 + ", \n\n " + item.Item2 + " \n\n" + item.Item3);
            }
            Console.WriteLine("\n\n");
        }
        public void forH4Tag(string address)
        {
            Tuple<char, char, char, char, char, char> tag = new Tuple<char, char, char, char, char, char>('<', 'h', '4', 'h', '4', '>');
            List<Tuple<string, string, string>> classList = genericClassContent.findClassListByHeading(address, tag);

            foreach (Tuple<string, string, string> item in classList)
            {
                Console.WriteLine("\n\n" + item.Item1 + ", \n\n " + item.Item2 + " \n\n" + item.Item3);
            }
            Console.WriteLine("\n\n");
        }
        public void forH5Tag(string address)
        {
            Tuple<char, char, char, char, char, char> tag = new Tuple<char, char, char, char, char, char>('<', 'h', '5', 'h', '5', '>');
            List<Tuple<string, string, string>> classList = genericClassContent.findClassListByHeading(address, tag);

            foreach (Tuple<string, string, string> item in classList)
            {
                Console.WriteLine("\n\n" + item.Item1 + ", \n\n " + item.Item2 + " \n\n" + item.Item3);
            }
            Console.WriteLine("\n\n");
        }
        public void forH6Tag(string address)
        {
            Tuple<char, char, char, char, char, char> tag = new Tuple<char, char, char, char, char, char>('<', 'h', '6', 'h', '6', '>');
            List<Tuple<string, string, string>> classList = genericClassContent.findClassListByHeading(address, tag);

            foreach (Tuple<string, string, string> item in classList)
            {
                Console.WriteLine("\n\n" + item.Item1 + ", \n\n " + item.Item2 + " \n\n" + item.Item3);
            }
            Console.WriteLine("\n\n");
        }

        public void forBigTag(string address)
        {
            string tag = "<BIG></BIG>";
            List<Tuple<string, string, string>> classList = genericClassContent.findClassListByBig(address, tag);

            foreach (Tuple<string, string, string> item in classList)
            {
                Console.WriteLine("\n\n" + item.Item1 + ", \n\n " + item.Item2 + " \n\n" + item.Item3);
            }
            Console.WriteLine("\n\n");
        }
    }

    public class SearchContentUI
    {
        SearchContent searchContent = new SearchContent();
        public void inputOutput()
        {
            Console.Write("Enter birthcenter URL : ");
            string address = Console.ReadLine();
            Console.Write("Enter class starting tag : ");
            string tag = Console.ReadLine();

            if (tag == "h1")
            {
                try
                {
                    searchContent.forH1Tag(address);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error : " + ex.Message);
                }
            }
            else if (tag == "h2")
            {
                try
                {
                    searchContent.forH2Tag(address);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error : " + ex.Message);
                }
            }
            else if (tag == "h3")
            {
                try
                {
                    searchContent.forH3Tag(address);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error : " + ex.Message);
                }
            }
            else if (tag == "h4")
            {
                try
                {
                    searchContent.forH4Tag(address);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error : " + ex.Message);
                }
            }
            else if (tag == "h5")
            {
                try
                {
                    searchContent.forH5Tag(address);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error : " + ex.Message);
                }
            }
            else if (tag == "h6")
            {
                try
                {
                    searchContent.forH6Tag(address);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error : " + ex.Message);
                }
            }
            else if(tag == "big")
            {
                try
                {
                    searchContent.forBigTag(address);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error : " + ex.Message);
                }
            }
        }
    }
     
    public class Program
    {
        static void Main(string[] args)
        {
            //string address = "https://gracefull.com/online-classes/"; //h2 tag
            //string address = "https://www.sutterhealth.org/classes-events-search?keywords=eclass&remove-default-affiliate-filter=true&attachQS=true"; //h2 tag
            //string address = "https://casa-natal.com/classes-groups/"; //h1 tag

            SearchContentUI searchContentUI = new SearchContentUI();
            searchContentUI.inputOutput();
        }
    }
}
