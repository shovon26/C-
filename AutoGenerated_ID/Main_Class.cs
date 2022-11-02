using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Main_class
    {
        static void Main(string[] args)
        {
            // DOB format = yyyymmdd
            User user1 = new User("Shovon", "19981120", "shsh@gmail.com", "0138329");
            User user2 = new User("Shakib", "20040606", "abc@gmail.com", "0138329876");
            User user3 = new User("Nafis", "19990610", "def@gmail.com", "01383298657");

            User user4 = new User(user1);

            user1.userInformation();
            user1.getAge(user1.DOB);
            Console.WriteLine();
            user2.userInformation();
            user2.getAge(user2.DOB);
            Console.WriteLine();
            user3.userInformation();
            user3.getAge(user3.DOB);
            Console.WriteLine();
            user4.userInformation();
            user4.getAge(user4.DOB);
        }
    }
}
