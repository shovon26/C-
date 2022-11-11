using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp1
{
    public class Node
    {
        public object _Data;
        public Node _Next;
        public Node() { }
        public Node(object data)
        {
            _Data = data;
            _Next = null;
        }
    }

    public class ListNode : IEnumerable, IEnumerator
    {
        private int position = -1;
        public Node head;
        public Node current = null;
        public ListNode()
        {
            head = null;
        }

        //Enumerator Operation
        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)this;
        }
        public bool MoveNext()
        {
            if (position + 1 < SizeOfList(head))
            {
                position++;
                return true;
            }
            return false;
        }
        public void Reset()
        {
            position = -1;
        }
        public object Current
        {
            get
            {
                int cnt = 0;
                object val = -1;
                current = head;
                while (current != null)
                {
                    if (cnt == position)
                    {
                        val = current._Data;
                        break;
                    }
                    current = current._Next;
                    cnt++;
                }
                return val;
            }
        }

        public object this[int index]   //Indexer for finding ith node using index
        {
            get
            {
                int cnt = 0;
                object val = -1;
                current = head;
                while (current != null)
                {
                    if (cnt == index)
                    {
                        val = current._Data;
                        break;
                    }
                    current = current._Next;
                    cnt++;
                }
                return val;
            }
        }

        public void AddAtStart(ListNode list, object new_data)
        {
            Node new_node = new Node(new_data);
            if (list.head == null)
            {
                list.head = new_node;
                return;
            }
            new_node._Next = list.head;
            list.head = new_node;
            return;
        }

        public void AddAfter(Node prev_node, object new_data)
        {
            if (prev_node == null)
            {
                Console.WriteLine("Previous node can't be null");
                return;
            }

            Node new_node = new Node(new_data);
            new_node._Next = prev_node._Next;
            prev_node._Next = new_node;
        }

        public void AddAfterNodeNumber(ListNode list, int position, object new_data)
        {
            Node new_node = new Node(new_data);
            if (list.head == null)
            {
                list.head = new_node;
                return;
            }
            Node temp = list.head;
            int cnt = 0;
            Node prev_node = null;
            while (temp != null)
            {
                cnt++;
                if (cnt == position)
                {
                    prev_node = temp;
                    break;
                }
                temp = temp._Next;
            }
            if (prev_node._Next == null)
            {
                prev_node._Next = new_node;
                return;
            }
            new_node._Next = prev_node._Next;
            prev_node._Next = new_node;
        }

        public void Append(ListNode list, object new_data)
        {
            Node new_node = new Node(new_data);
            if (list.head == null)
            {
                list.head = new_node;
                return;
            }
            Node temp = list.head;
            while (temp._Next != null)
            {
                temp = temp._Next;
            }
            temp._Next = new_node;
        }

        public void RemoveStart(ListNode list)
        {
            Node temp = list.head;
            list.head = list.head._Next;
            return;
        }

        public void RemoveEnd(ListNode list)
        {
            Node temp = list.head;
            while (temp._Next._Next != null)
            {
                temp = temp._Next;
            }
            temp._Next = null;
            return;
        }

        public void RemoveFromPosition(ListNode list, int position)
        {
            if (list.head == null) return;
            int total = SizeOfList(list.head);
            if (position == total)
            {
                RemoveEnd(list);
                return;
            }
            if (position == 1)
            {
                RemoveStart(list);
                return;
            }
            int cnt = 0;
            Node temp = list.head;
            Node prev_node = null;
            while (temp != null)
            {
                cnt++;
                if (cnt + 1 == position)
                {
                    prev_node = temp;
                    break;
                }
                temp = temp._Next;
            }
            prev_node._Next = temp._Next._Next;
            return;
        }

        public void UpdateFromPosition(ListNode list, int position, object new_data)
        {
            list.AddAfterNodeNumber(list, position, new_data);
            list.RemoveFromPosition(list, position);
            return;
        }

        public bool SearchElement(ListNode list, object val)
        {
            Node node = list.head;
            while (node != null)
            {
                if (node._Data == val) return true;
                node = node._Next;
            }
            return false;
        }

        public void PrintList(ListNode list)
        {
            Node node = list.head;
            Console.Write("List Data : ");
            while (node != null)
            {
                Console.Write(node._Data + " ");
                node = node._Next;
            }
            Console.WriteLine();
        }

        public int SizeOfList(Node node)
        {
            int cnt = 0;
            while (node != null)
            {
                cnt++;
                node = node._Next;
            }
            return cnt;
        }

        public void AddArrayAtLast(ListNode list, object[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                Append(list, arr[i]);
            }
            return;
        }
        public void AddArrayAtStart(ListNode list, object[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                AddAtStart(list, arr[i]);
            }
            return;
        }

        public void ConvertListToArray(ListNode list, object[] arr)
        {
            Node temp = list.head;
            int id = 0;
            while (temp != null)
            {
                arr[id++] = temp._Data;
                temp = temp._Next;
            }
        }
    }

    public class User
    {
        public static int increment = 1;
        private int _ID;
        private string _Name;
        private string _Email;
        private string _DOB;

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
        public string DOB
        {
            get { return _DOB; }
            set { _DOB = value; }
        }

        public User() { }
        public User(string name, string email, string dOB)
        {
            _ID = User.increment++;
            _Name = name;
            _Email = email;
            _DOB = dOB;
        }

        public User(User user)
        {
            this._ID = user._ID;
            this._Name = user._Name;
            this._Email = user._Email;
            this._DOB = user._DOB;
        }

        ListNode users = new ListNode();
        public void AddUser(User user)
        {
            users.Append(users, user);
            return;
        }

        public void RemoveUserbyId(int id)
        {
            int cnt = 0;
            foreach (User user in users)
            {
                cnt++;
                if(user.ID == id)
                {
                    users.RemoveFromPosition(users, cnt);
                    break;
                }
            }
            users.Reset();
            return;
        }
        public int FindUserIDByName(string name)
        {
            int temp_Id = -1;
            foreach (User user in users)
            {
                if (user.Name == name)
                {
                    temp_Id = user.ID;
                    break;
                }
            }
            users.Reset();
            return temp_Id;
        }
        public bool IsPresentUserById(int id)
        {
            bool ok = false;
            foreach(User user in users)
            {
                if(user.ID == id)
                {
                    ok = true;
                    break;
                }
            }
            users.Reset();
            return ok;
        }
        public bool IsPresentUserByName(string name)
        {
            bool ok = false;
            foreach (User user in users)
            {
                if (user.Name == name)
                {
                    ok = true;
                    break;
                }
            }
            return ok;
        }
        public void UpdateUser(int id, User new_user)
        {
            int cnt = 0;
            foreach (User user in users)
            {
                cnt++;
                if (user.ID == id)
                {
                    users.UpdateFromPosition(users, cnt, new_user);
                    break;
                }
            }
            users.Reset();
            return;
        }

        
        public void Run()
        {
            while (true)
            {
                Console.WriteLine("Here is the options are provided : ");
                Console.WriteLine("Press 1 for Add a user : ");
                Console.WriteLine("Press 2 for Remove a  user : ");
                Console.WriteLine("Press 3 for Update a user : ");
                Console.WriteLine("Press 4 to show the user list ");
                Console.WriteLine("Press any other number to exit the operation");
                Console.WriteLine("Thank you");
                Console.Write("Enter operation number : ");
                int input = Convert.ToInt32(Console.ReadLine());
                if (input == 1)
                {
                    /*Console.Write("Enter ID : ");
                    int id = Convert.ToInt32(Console.ReadLine());*/
                    Console.Write("Enter Name : ");
                    string name = Console.ReadLine();
                    Console.Write("Enter Email : ");
                    string email = Console.ReadLine();
                    Console.Write("Enter DOB : ");
                    string dob = Console.ReadLine();

                    User temp = new User(name, email, dob);
                    AddUser(temp);
                    Console.Clear();
                } 
                else if(input == 2)
                {
                    Console.Write("Enter way to remove :\n1 for remove by id\n2 for remove by name\n");
                    int n = Convert.ToInt32(Console.ReadLine());
                    if (n == 1)
                    {
                        Console.Write("Enter ID to remove : ");
                        int id = Convert.ToInt32(Console.ReadLine());
                        if (IsPresentUserById(id) == true) RemoveUserbyId(id);
                        else Console.WriteLine("There is no user of this id");
                    }
                    else if(n == 2)
                    {
                        Console.Write("Enter Name to remove : ");
                        string name = Console.ReadLine();
                        Console.WriteLine("Entered Name : " + name);
                        int IdByName = FindUserIDByName(name);
                        if (IdByName != -1) RemoveUserbyId(IdByName);
                        else Console.WriteLine("There is no user of this name");
                    }
                    Console.Clear();
                }
                else if(input == 3)
                {
                    Console.Write("Enter ID of user to update :");
                    int id = Convert.ToInt32(Console.ReadLine());
                    if (IsPresentUserById(id) == true)
                    {
                        Console.Write("Enter new name : ");
                        string new_name = Console.ReadLine();
                        Console.Write("Enter new email : ");
                        string new_email = Console.ReadLine();
                        Console.Write("Enter new DOB : ");
                        string new_dob = Console.ReadLine();

                        User new_user = new User(new_name, new_email, new_dob);
                        UpdateUser(id, new_user);
                    }
                    else Console.WriteLine("There is no user of this id");
                    Console.Clear();
                }

                else if (input == 4)
                {
                    FindAll();
                }
                else break;
            }
        }       
        public void FindAll()
        {
            int total = users.SizeOfList(users.head);
            if (total == 0)
            {
                Console.WriteLine("There is no user in current Database");
            }
            else
            {
                Console.WriteLine("User List : ");
                foreach (User user in users)
                {
                    Console.WriteLine(user.ID + " " + user.Name + " " + user.Email + " " + user.DOB);
                }
                Console.WriteLine("\nThank you\n");
                users.Reset();
            }
        }
    }

    public class Main_class
    {
        static void Main(string[] args)
        {
            ListNode listNode = new ListNode();
            
            //Sample User
            User user1 = new User("Shovon", "shah@gmail.com", "06-12-1998");
            User user2 = new User("Shakib", "abc@gmail.com", "06-12-2004");
            User user3 = new User("Ashik", "def@gmail.com", "10-01-1999");

            User users = new User();
            User.increment = 1;
            users.Run();
        }
    }
}
