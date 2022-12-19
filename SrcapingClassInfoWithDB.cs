using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
namespace WebClient_ConsoleApp
{
    public class Medical
    {
        public int MedicalID { get; set; }
        public string MedicalUrl { get; set; }
        public string ClassTag { get; set; }
        public DateTime dateTime { get; set; }

        public List<Class> classData = new List<Class>();

        public Medical(int iD, string medicalUrl, string classTag)
        {
            MedicalID = iD;
            MedicalUrl = medicalUrl;
            ClassTag = classTag;
        }
    }

    public class Class
    {
        public int ClassID { get; set; }
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
        public string ClassPrice { get; set; }

        public int MedicalID { get; set; }

        public Class(int iD, string className, string classDescription, string classPrice, int medicalID)
        {
            ClassID = iD;
            ClassName = className;
            ClassDescription = classDescription;
            ClassPrice = classPrice;
            MedicalID = medicalID;
        }

        public Class(string className, string classDescription, string classPrice)
        {
            ClassName = className;
            ClassDescription = classDescription;
            ClassPrice = classPrice;
        }
    }
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
            for (int i = 0; i < len;)
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
            string ret = "";
            for(int i=0; i<result.Length; i++)
            {
                if((result[i] >= 'a' && result[i] <= 'z') || (result[i] >= 'A' && result[i] <= 'Z') ||
                    result[i] == ' ' || result[i] == '.' || result[i] == ',' || result[i] == '$' || result[i] == ':'
                    || result[i] == ';' || result[i] == '(' || result[i] == ')')
                {
                    ret += result[i];
                }
            }

            // Remove extra space
            int idx = 0;
            int N = ret.Length;
            string answer = "";
            while (ret[idx] == ' ') idx++;
            while(idx < N)
            {
                if (idx < N && ret[idx] == ' ')
                {
                    answer += ret[idx];
                    idx++;
                    while (idx < N && ret[idx] == ' ') idx++;
                }
                else
                {
                    if(idx < N) answer += ret[idx];
                    idx++;
                }
            }
            return answer;
        }

        private static int convertPrice(string price)
        {
            string tmp = "";
            for (int i = 1; i < price.Length; i++)
            {
                if (price[i] >= '0' && price[i] <= '9') tmp += price[i];
                else break;
            }
            int sum = 0;
            int mul = 1;
            for (int i = tmp.Length - 1; i >= 0; i--)
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

        private static bool checkClass(string content, string tag = "<img")
        {
            int len = content.Length;
            bool ok = true;
            for(int i=0; i<len; i++)
            {
                if(i+4 < len && content.Substring(i, 4) == tag)
                {
                    ok = false;
                    break;
                }
            }
            return ok;
        }
        public List<Class> findClassListByBig(string address, string tag)
        {
            WebClient client = new WebClient();
            //client.Headers["Content-Type"] = "text/html";
            client.Headers["user-agent"] = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Mobile Safari/537.36";

            string reply = client.DownloadString(address);
            int len = reply.Length;
            List<Class> classes = new List<Class>();
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
                            Class classNow = new Class(temp, add, classPrice);
                            classes.Add(classNow);
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
                        Class currentClass = new Class(temp, delta, classPrice);
                        classes.Add(currentClass);

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

            List<Class> classList = new List<Class>();
            foreach (Class item in classes)
            {
                string con1 = item.ClassName;
                string con2 = item.ClassDescription;
                string className = GenericClassContent.findContent(con1);
                string classDescription = GenericClassContent.removeDescriptionTag(con2);
                string classPrice = item.ClassPrice;
                Class currentClass = new Class(className, classDescription, classPrice);
                classList.Add(currentClass);
            }
            return classList;
        }

        public List<Class> findClassListByHeading(string address, Tuple<char, char, char, char, char, char> tag)
        {
            WebClient client = new WebClient();
            client.Headers["user-agent"] = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Mobile Safari/537.36";
            string reply = client.DownloadString(address);
            int len = reply.Length;
            //Console.WriteLine("Total Length : " + len);
            //Console.WriteLine("html full : " + reply);
            List<Class> classes = new List<Class>();
            for (int i = 0; i + 2 < len;)
            {
                if (i + 2 < len && reply[i] == tag.Item1 && reply[i + 1] == tag.Item2 && reply[i + 2] == tag.Item3)
                {
                    string temp = "";
                    int idx = i + 3;
                    temp += reply[i];
                    temp += reply[i + 1];
                    temp += reply[i + 2];
                    while (idx + 2 < len)
                    {
                        if (idx+2 < len && reply[idx] == tag.Item4 && reply[idx + 1] == tag.Item5 && reply[idx + 2] == tag.Item6)
                        {
                            temp += reply[idx];
                            temp += reply[idx + 1];
                            temp += reply[idx + 2];
                            break;
                        }
                        temp += reply[idx];
                        idx++;
                    }
                    bool containImage = GenericClassContent.checkClass(temp);

                    //Console.WriteLine("Class Name : " + temp);

                    if (temp.Substring(4, 5) == "Class" || temp.Substring(4, 7) == "Classes" || containImage == false)
                    {
                        i++;
                        continue;
                    }

                    string classPrice = "";


                    // Finding class Price
                    try
                    {
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

                        if (from == -1) from = idx + 3;

                        // find Ending paragraph
                        int nextClassIndex = from + 1;
                        bool ok1 = false;
                        while (nextClassIndex < len)
                        {
                            if (nextClassIndex + 3 < len && reply[nextClassIndex] == tag.Item1 && reply[nextClassIndex + 1] == tag.Item2 && reply[nextClassIndex + 2] == tag.Item3)
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
                            for (int rep = from + 3; rep < len; rep++)
                            {
                                if (rep + 2 < len && reply[rep] == '/' && reply[rep + 1] == 'p' && reply[rep + 2] == '>')
                                {
                                    add += reply[rep];
                                    add += reply[rep + 1];
                                    add += reply[rep + 2];
                                    break;
                                }
                                add += reply[rep];
                            }
                            Class tempClass = new Class(temp, add, classPrice);
                            classes.Add(tempClass);
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
                        Class currentClass = new Class(temp, delta, classPrice);
                        classes.Add(currentClass);

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

            List<Class> classList = new List<Class>();

            foreach (Class item in classes)
            {
                string con1 = item.ClassName;
                string con2 = item.ClassDescription;
                string className = GenericClassContent.findContent(con1);
                string classDescription = GenericClassContent.removeDescriptionTag(con2);
                string classPrice = item.ClassPrice;
                Class currentClass = new Class(className, classDescription, classPrice);
                classList.Add(currentClass);
            }
            return classList;
        }

        public static void findHTML(string address)
        {
            WebClient client = new WebClient();
            //client.Headers["accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            client.Headers["user-agent"] = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Mobile Safari/537.36";
            string reply = client.DownloadString(address);
            Console.WriteLine(reply);
        }

        public List<Class> findClassListByParagraph(string address, Tuple<char, char, char, char, char, char> tag, int fontSize = 32, string match = "font-size:")
        {
            WebClient client = new WebClient();
            //client.Headers["accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            client.Headers["user-agent"] = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Mobile Safari/537.36";
            string reply = client.DownloadString(address);

            int len = reply.Length;
            List<Class> classes = new List<Class>();
            for(int i=0; i + 2 < len;)
            {
                if (i + 2 < len && reply[i] == tag.Item1 && reply[i + 1] == tag.Item2 && reply[i + 2] == tag.Item3) // find possible class 
                {
                    int idx = i + 3;
                    //Console.WriteLine("Test 1 : " + reply.Substring(idx, 10));
                    bool possibleClass = false;
                    while (idx < len)
                    {
                        if (idx + 3 < len && reply[idx] == '<' && reply[idx + 1] == tag.Item4 && reply[idx + 2] == tag.Item5 && reply[idx + 3] == tag.Item6) break;
                        if (reply.Substring(idx, 10) == match)
                        {
                            int from = idx + 11;
                            while (true)
                            {
                                if (Char.IsDigit(reply, from) == true) break;
                                from++;
                            }
                            string num = "";
                            while (from < len)
                            {
                                if (Char.IsDigit(reply, from) == false) break;
                                num += reply[from];
                                from++;
                            }
                            int size = Int32.Parse(num);
                            if(size == fontSize) possibleClass = true;
                            break;
                        }
                        idx++;
                    }
                    if (possibleClass == true)
                    {
                        string temp = "";
                        temp += reply.Substring(i, 3);
                        idx = i + 3;
                        while (idx < len)
                        {
                            if (idx+3 < len && reply[idx] == '<' && reply[idx + 1] == tag.Item4 && reply[idx + 2] == tag.Item5 && reply[idx + 3] == tag.Item6)
                            {
                                temp += reply.Substring(idx, 4);
                                break;
                            }
                            else temp += reply[idx];
                            idx++;
                        }

                        // find class price
                        string classPrice = "";
                        try
                        {
                            int next_idx = idx+6;
                            bool have = false;
                            //string price = "";
                            while (next_idx < len)
                            {
                                if (next_idx+2 < len && reply[next_idx] == tag.Item1 && reply[next_idx + 1] == tag.Item2 && reply[next_idx + 2] == tag.Item3)
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
                            //find starting paragraph
                            int next_idx = idx+4;

                            int from = next_idx;

                            string description = "";

                            //Console.WriteLine("Test : " + reply.Substring(from, 20));

                            while(from < len)
                            {
                                if (from+3 < len && reply[from] == '<' && reply[from + 1] == tag.Item4 && reply[from + 2] == tag.Item5 && reply[from + 3] == tag.Item6) break;
                                description+= reply[from];
                                from++;
                            }
                           // Console.WriteLine("Class Price : " + classPrice + "\n\n");
                            
                            Class currentClass = new Class(temp, description, classPrice);
                            classes.Add(currentClass);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error2 : " + ex.Message);
                            break;
                        }
                    }
                    i++;
                }
                else i++;
            }

            List<Class> classList = new List<Class>();

            foreach (Class item in classes)
            {
                string con1 = item.ClassName;
                string con2 = item.ClassDescription;
                string className = GenericClassContent.findContent(con1);
                string classDescription = GenericClassContent.removeDescriptionTag(con2);
                string classPrice = item.ClassPrice;
                Class currentClass = new Class(className, classDescription, classPrice);
                //Console.WriteLine(currentClass.ClassName + "\n" + currentClass.ClassDescription+ "\n" + currentClass.ClassPrice);
                classList.Add(currentClass);
            }
            return classList;
        }
    }


    public class SearchContent
    {
        GenericClassContent genericClassContent = new GenericClassContent();
        public void forH1Tag(string address)
        {
            Tuple<char, char, char, char, char, char> tag = new Tuple<char, char, char, char, char, char>('<', 'h', '1', 'h', '1', '>');
            List<Class> classList = genericClassContent.findClassListByHeading(address, tag);
            foreach (Class item in classList)
            {
                Console.WriteLine("\n\n" + item.ClassName + ", \n\n " + item.ClassDescription + " \n\n" + item.ClassPrice);
            }
            Console.WriteLine("\n\n");
        }

        public void forH2Tag(string address)
        {
            Tuple<char, char, char, char, char, char> tag = new Tuple<char, char, char, char, char, char>('<', 'h', '2', 'h', '2', '>');
            List<Class> classList = genericClassContent.findClassListByHeading(address, tag);
            foreach (Class item in classList)
            {
                Console.WriteLine("\n\n" + item.ClassName + ", \n\n " + item.ClassDescription + " \n\n" + item.ClassPrice);
            }
            Console.WriteLine("\n\n");
        }

        public void forH3Tag(string address)
        {
            Tuple<char, char, char, char, char, char> tag = new Tuple<char, char, char, char, char, char>('<', 'h', '3', 'h', '3', '>');
            List<Class> classList = genericClassContent.findClassListByHeading(address, tag);
            foreach (Class item in classList)
            {
                Console.WriteLine("\n\n" + item.ClassName + ", \n\n " + item.ClassDescription + " \n\n" + item.ClassPrice);
            }
            Console.WriteLine("\n\n");
        }
        public void forH4Tag(string address)
        {
            Tuple<char, char, char, char, char, char> tag = new Tuple<char, char, char, char, char, char>('<', 'h', '4', 'h', '4', '>');
            List<Class> classList = genericClassContent.findClassListByHeading(address, tag);
            foreach (Class item in classList)
            {
                Console.WriteLine("\n\n" + item.ClassName + ", \n\n " + item.ClassDescription + " \n\n" + item.ClassPrice);
            }
            Console.WriteLine("\n\n");
        }
        public void forH5Tag(string address)
        {
            Tuple<char, char, char, char, char, char> tag = new Tuple<char, char, char, char, char, char>('<', 'h', '5', 'h', '5', '>');
            List<Class> classList = genericClassContent.findClassListByHeading(address, tag);
            foreach (Class item in classList)
            {
                Console.WriteLine("\n\n" + item.ClassName + ", \n\n " + item.ClassDescription + " \n\n" + item.ClassPrice);
            }
            Console.WriteLine("\n\n");
        }
        public void forH6Tag(string address)
        {
            Tuple<char, char, char, char, char, char> tag = new Tuple<char, char, char, char, char, char>('<', 'h', '6', 'h', '6', '>');
            List<Class> classList = genericClassContent.findClassListByHeading(address, tag);
            foreach (Class item in classList)
            {
                Console.WriteLine("\n\n" + item.ClassName + ", \n\n " + item.ClassDescription + " \n\n" + item.ClassPrice);
            }
            Console.WriteLine("\n\n");
        }

        public void forBigTag(string address)
        {
            string tag = "<BIG></BIG>";   // in webclient big tag is automatically converted to upper case
            List<Class> classList = genericClassContent.findClassListByBig(address, tag);
            foreach (Class item in classList)
            {
                Console.WriteLine("\n\n" + item.ClassName + ", \n\n " + item.ClassDescription + " \n\n" + item.ClassPrice);
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
                    Console.WriteLine("Error here : " + ex.Message);
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
            else if (tag == "big")
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
            else
            {
                Console.WriteLine("Provide Proper tag");
            }
        }
    }

    public class DBHelper
    {
        GenericClassContent genericClassContent = new GenericClassContent();

        //Execute All of common query for different html tag which is helping us to extract class information
        private static void executeDatabaseQuery(Medical medical, List<Class> classListCurrent, List<Class> classListDB)
        {
            string connectionString = "Data Source=DESKTOP-U2K0EKM;Initial Catalog=SearchContentDB;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand();
            List<Medical> medicalListDB = DBHelper.StaticMedicalTableList();
            //Case 1 : Current Medical is completely new to database

            bool have = false;
            foreach (Medical item in medicalListDB)
            {
                // Console.WriteLine(medical.MedicalID + " : " + item.MedicalID);
                if (medical.MedicalID == item.MedicalID)
                {
                    have = true;
                    break;
                }
            }
            //Console.WriteLine("Have value here : " + have);
            if (have == false)
            {
                foreach (Class item in classListCurrent)
                {
                    string className = item.ClassName;
                    string classDescription = item.ClassDescription;
                    string classPrice = item.ClassPrice;

                    try
                    {
                        string Query2 = "insert into Class(ClassName, ClassDescription, ClassPrice, MedicalID, ClassStatus) values('" + className + "', '" + classDescription + "', '" + classPrice + "', '" + medical.MedicalID + "', '" + true + "');";
                        command = new SqlCommand(Query2, connection);
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Database Error : " + ex.Message);
                    }
                }
                connection.Close();
                return;
            }

            //Case 2 : Present in Database and current Extracted class information || Present in current extracted class information but not in database
            foreach (Class itemCurrent in classListCurrent)
            {
                bool present = false;
                int id = -1;
                foreach (Class itemDB in classListDB)
                {
                    if (itemDB.ClassName == itemCurrent.ClassName)
                    {
                        id = itemDB.ClassID;
                        present = true;
                        break;
                    }
                }
                //Console.WriteLine("Present 1  here : " + present);
                if (present == true)
                {
                    try
                    {
                        String Query = "UPDATE Class set ClassName = '" + itemCurrent.ClassName + "', ClassDescription = '" + itemCurrent.ClassDescription + "', ClassPrice = '" + itemCurrent.ClassPrice + "', DateAndTime = CURRENT_TIMESTAMP, ClassStatus = '" + true + "' where ID = '" + id + "';";
                        //Console.WriteLine("Medical ID : " + medical.MedicalID + "\n\n Query : " + Query);
                        command = new SqlCommand(Query, connection);
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error Message : " + ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        string Query = "insert into Class(ClassName, ClassDescription, ClassPrice, MedicalID, ClassStatus) values('" + itemCurrent.ClassName + "', '" + itemCurrent.ClassDescription + "', '" + itemCurrent.ClassPrice + "', '" + medical.MedicalID + "', '" + true + "');";
                        command = new SqlCommand(Query, connection);
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error Message : " + ex.Message);
                    }
                }
            }

            //Case 3 : Present in Database but not present in Current class information
            foreach (Class itemDB in classListDB)
            {
                bool isPresent = true;
                foreach (Class itemCurrent in classListCurrent)
                {
                    if (itemDB.ClassName == itemCurrent.ClassName)
                    {
                        isPresent = false;
                        break;
                    }
                }
                //Console.WriteLine("Present 2  here : " + isPresent);
                if (isPresent == true)
                {
                    try
                    {
                        string Query = "update Class set ClassStatus = '" + false + "' where ID = '" + itemDB.ClassID + "'";
                        //Console.WriteLine("2nd Query : " + Query);
                        command = new SqlCommand(Query, connection);
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error Message : " + ex.Message);
                    }
                }
            }
            connection.Close();
        }

        public void updateClassTableForHeading(Medical medical, Tuple<char, char, char, char, char, char> Tag)
        {
            List<Class> classListCurrent = genericClassContent.findClassListByHeading(medical.MedicalUrl, Tag);
            List<Class> classListDB = DBHelper.existingClassFromDB(medical.MedicalID); 
            DBHelper.executeDatabaseQuery(medical, classListCurrent, classListDB); //Will execute all of the required query for this current heading tag
        }

        public void updateClassTableForParagraph(Medical medical, Tuple<char, char, char, char, char, char> Tag)
        {
            //Always you have to provide the fontSize in findClassListByParagraph(medical.MedicalUrl, Tag)
            List<Class> classListCurrent = genericClassContent.findClassListByParagraph(medical.MedicalUrl, Tag); 
            List<Class> classListDB = DBHelper.existingClassFromDB(medical.MedicalID);
            DBHelper.executeDatabaseQuery(medical, classListCurrent, classListDB); //Will execute all of the required query for this current paragraph tag
        }

        public void updateClassTableForBig(Medical medical, string Tag)
        {
            List<Class> classListCurrent = genericClassContent.findClassListByBig(medical.MedicalUrl, Tag);
            List<Class> classListDB = DBHelper.existingClassFromDB(medical.MedicalID);
            DBHelper.executeDatabaseQuery(medical, classListCurrent, classListDB); //Will execute all of the required query for this current Big tag
        }
        public static List<Class> existingClassFromDB(int medicalID)
        {
            string connectionString = "Data Source=DESKTOP-U2K0EKM;Initial Catalog=SearchContentDB;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlDataReader sqlDataReader;
            SqlCommand command = new SqlCommand();

            command = new SqlCommand("select ID, ClassName, ClassDescription, ClassPrice, MedicalID from Class;", connection);
            sqlDataReader = command.ExecuteReader();
            List<Class> classes = new List<Class>();
            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    Class currentClass = new Class(sqlDataReader.GetInt32(0), sqlDataReader.GetString(1), sqlDataReader.GetString(2),
                                                    sqlDataReader.GetString(3), sqlDataReader.GetInt32(4));
                    if(currentClass.MedicalID == medicalID) classes.Add(currentClass);
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            return classes;
        }

        public static List<Medical> StaticMedicalTableList()
        {
            List<Medical> medicalList = new List<Medical>();
            string connectionString = "Data Source=DESKTOP-U2K0EKM;Initial Catalog=SearchContentDB;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlDataReader sqlDataReader;
            SqlCommand command = new SqlCommand();

            command = new SqlCommand("update Medical set DateAndTime =\tCURRENT_TIMESTAMP;", connection);
            command.ExecuteNonQuery();
            command = new SqlCommand("select ID, MedicalUrl, ClassTag from Medical;", connection);
            sqlDataReader = command.ExecuteReader();

            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    Medical currentMedical = new Medical(sqlDataReader.GetInt32(0), sqlDataReader.GetString(1), sqlDataReader.GetString(2));
                    medicalList.Add(currentMedical);
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            return medicalList;
        }

        public List<Medical> MedicalTableList()
        {
            List<Medical> medicalList = new List<Medical>();
            string connectionString = "Data Source=DESKTOP-U2K0EKM;Initial Catalog=SearchContentDB;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlDataReader sqlDataReader;
            SqlCommand command = new SqlCommand();

            command = new SqlCommand("update Medical set DateAndTime =\tCURRENT_TIMESTAMP;", connection);
            command.ExecuteNonQuery();
            command = new SqlCommand("select ID, MedicalUrl, ClassTag from Medical;", connection);
            sqlDataReader = command.ExecuteReader();

            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    Medical currentMedical = new Medical(sqlDataReader.GetInt32(0), sqlDataReader.GetString(1), sqlDataReader.GetString(2));
                    medicalList.Add(currentMedical);
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            return medicalList;
        }
    }

    public class DatabaseUI
    {
        DBHelper dBHelper= new DBHelper();
        
        public void ClassTable()
        {  
            List<Medical> medicalList = dBHelper.MedicalTableList();
            //Console.WriteLine("Total Medical : " + medicalList.Count);
            foreach(Medical medical in medicalList)
            {
                int medicalID = medical.MedicalID;
                string url = medical.MedicalUrl;
                string tag = medical.ClassTag;
                Medical executeCurrentMedical = new Medical(medicalID, url, tag);
                if(tag == "h1")
                {
                    Tuple<char, char, char, char, char, char> Tag = new Tuple<char, char, char, char, char, char>('<', 'h', '1', 'h', '1', '>');
                    dBHelper.updateClassTableForHeading(executeCurrentMedical, Tag);
                }
                else if(tag == "h2")
                {
                    Tuple<char, char, char, char, char, char> Tag = new Tuple<char, char, char, char, char, char>('<', 'h', '2', 'h', '2', '>');
                    dBHelper.updateClassTableForHeading(executeCurrentMedical, Tag);
                }
                else if (tag == "h3")
                {
                    Tuple<char, char, char, char, char, char> Tag = new Tuple<char, char, char, char, char, char>('<', 'h', '3', 'h', '3', '>');
                    dBHelper.updateClassTableForHeading(executeCurrentMedical, Tag);
                }
                else if (tag == "h4")
                {
                    Tuple<char, char, char, char, char, char> Tag = new Tuple<char, char, char, char, char, char>('<', 'h', '4', 'h', '4', '>');
                    dBHelper.updateClassTableForHeading(executeCurrentMedical, Tag);
                }
                else if (tag == "h5")
                {
                    Tuple<char, char, char, char, char, char> Tag = new Tuple<char, char, char, char, char, char>('<', 'h', '5', 'h', '5', '>');
                    dBHelper.updateClassTableForHeading(executeCurrentMedical, Tag);
                }
                else if (tag == "h6")
                {
                    Tuple<char, char, char, char, char, char> Tag = new Tuple<char, char, char, char, char, char>('<', 'h', '6', 'h', '6', '>');
                    dBHelper.updateClassTableForHeading(executeCurrentMedical, Tag);
                }
                else if(tag == "p")
                {
                    Tuple<char, char, char, char, char, char> Tag = new Tuple<char, char, char, char, char, char>('<', 'p', '>', '/', 'p', '>');
                    dBHelper.updateClassTableForParagraph(executeCurrentMedical, Tag);
                }
                else if(tag == "big")
                {
                    string Tag = "<BIG></BIG>";
                    dBHelper.updateClassTableForBig(executeCurrentMedical, Tag);
                }
            }
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            // string address = "https://gracefull.com/online-classes/"; //h2 tag
            //string address = "https://www.sutterhealth.org/classes-events-search?keywords=eclass&remove-default-affiliate-filter=true&attachQS=true"; //h2 tag
            //string address = "https://casa-natal.com/classes-groups/"; //h1 tag
            //string address = "https://www.scripps.org/events";  //h4 tag
            //string address = "https://southcoastmidwifery.com/childbirth-education/"; //p tag
            //string address = "https://psjhcrmwebsites.microsoftcrmportals.com/home?Region=CAOC&Ministry=%7B585D078B-8967-4D24-BCF0-FF2C9D4A52F2%7D"; //h1 tag
            string address = "https://dominicanhospital.digitalsignup.com/Class/137-balance-bones-and-strength";
            //GenericClassContent.findHTML(address);

            SearchContentUI searchContentUI = new SearchContentUI();
            searchContentUI.inputOutput();

            DatabaseUI databaseUI = new DatabaseUI();
            //databaseUI.ClassTable();

        }
    }
}
