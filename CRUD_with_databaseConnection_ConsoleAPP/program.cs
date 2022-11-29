using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ConsoleApp
{
    public class Node<T>
    {
        public T _Data;
        public Node<T> _Next;
        public Node() { }
        public Node(T data)
        {
            _Data = data;
            _Next = null;
        }
    }

    public class ListNode<T> : IEnumerable, IEnumerator
    {
        private int position = -1;
        public Node<T> head;
        public Node<T> current = null;
        public ListNode()
        {
            head = null;
        }
        private bool Compare<T>(T x, T y)  //Compare Node data whether they are equal or not
        {
            return EqualityComparer<T>.Default.Equals(x, y);
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

        public void AddAtStart(ListNode<T> list, T new_data)
        {
            Node<T> new_node = new Node<T>(new_data);
            if (list.head == null)
            {
                list.head = new_node;
                return;
            }
            new_node._Next = list.head;
            list.head = new_node;
            return;
        }

        public void AddAfter(Node<T> prev_node, T new_data)
        {
            if (prev_node == null)
            {
                Console.WriteLine("Previous node can't be null");
                return;
            }

            Node<T> new_node = new Node<T>(new_data);
            new_node._Next = prev_node._Next;
            prev_node._Next = new_node;
        }

        public void AddAfterNodeNumber(ListNode<T> list, int position, T new_data)
        {
            Node<T> new_node = new Node<T>(new_data);
            if (list.head == null)
            {
                list.head = new_node;
                return;
            }
            Node<T> temp = list.head;
            int cnt = 0;
            Node<T> prev_node = null;
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

        public void Append(ListNode<T> list, T new_data)
        {
            Node<T> new_node = new Node<T>(new_data);
            if (list.head == null)
            {
                list.head = new_node;
                return;
            }
            Node<T> temp = list.head;
            while (temp._Next != null)
            {
                temp = temp._Next;
            }
            temp._Next = new_node;
        }

        public void RemoveStart(ListNode<T> list)
        {
            Node<T> temp = list.head;
            list.head = list.head._Next;
            return;
        }

        public void RemoveEnd(ListNode<T> list)
        {
            Node<T> temp = list.head;
            while (temp._Next._Next != null)
            {
                temp = temp._Next;
            }
            temp._Next = null;
            return;
        }

        public void RemoveFromPosition(ListNode<T> list, int position)
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
            Node<T> temp = list.head;
            Node<T> prev_node = null;
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

        public void UpdateFromPosition(ListNode<T> list, int position, T new_data)
        {
            list.AddAfterNodeNumber(list, position, new_data);
            list.RemoveFromPosition(list, position);
            return;
        }

        public bool SearchElement(ListNode<T> list, T val)
        {
            Node<T> node = list.head;
            while (node != null)
            {
                //if (node._Data == val) return true;
                if (Compare<T>(node._Data, val) == true) return true;
                node = node._Next;
            }
            return false;
        }

        public void PrintList(ListNode<T> list)
        {
            Node<T> node = list.head;
            Console.Write("List Data : ");
            while (node != null)
            {
                Console.Write(node._Data + " ");
                node = node._Next;
            }
            Console.WriteLine();
        }

        public int SizeOfList(Node<T> node)
        {
            int cnt = 0;
            while (node != null)
            {
                cnt++;
                node = node._Next;
            }
            return cnt;
        }

        public void AddArrayAtLast(ListNode<T> list, T[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                Append(list, arr[i]);
            }
            return;
        }
        public void AddArrayAtStart(ListNode<T> list, T[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                AddAtStart(list, arr[i]);
            }
            return;
        }

        public void ConvertListToArray(ListNode<T> list, T[] arr)
        {
            Node<T> temp = list.head;
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

        public User(int iD, string name, string email, string dOB)
        {
            _ID = iD;
            _Name = name;
            _Email = email;
            _DOB = dOB;
        }

        public User(string name, string email, string dOB)
        {
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
    }

    public class UserUI
    {
        UserManager userManager = new UserManager();
        DatabaseHandler databaseHandler= new DatabaseHandler();
        public void Run()
        {
            userManager.addAllUserFromDatabase();
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
                    Console.Write("Enter Name : ");
                    string name = Console.ReadLine();
                    Console.Write("Enter Email : ");
                    string email = Console.ReadLine();
                    Console.Write("Enter DOB : ");
                    string dob = Console.ReadLine();

                    User temp = new User(name, email, dob);
                    databaseHandler.createUser(temp);   // Update database and Add a new User in UserDB database

                    userManager.AddUser(temp);
                    Console.Clear();
                }
                else if (input == 2)
                {
                    Console.Write("Enter ID to remove : ");
                    int id = Convert.ToInt32(Console.ReadLine());
                    databaseHandler.removeUser(id);
                    if (userManager.IsPresentUserById(id) == true)
                    {
                        userManager.RemoveUserbyId(id);
                    }
                    else Console.WriteLine("There is no user of this id");
                    Console.Clear();
                }
                else if (input == 3)
                {
                    Console.Write("Enter ID of user to update :");
                    int id = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Enter new name : ");
                    string new_name = Console.ReadLine();
                    Console.Write("Enter new email : ");
                    string new_email = Console.ReadLine();
                    Console.Write("Enter new DOB : ");
                    string new_dob = Console.ReadLine();

                    User new_user = new User(id, new_name, new_email, new_dob);
                    userManager.UpdateUser(id, new_user);
                    databaseHandler.updateUser(new_user);
                    Console.Clear();
                }

                else if (input == 4)
                {
                    databaseHandler.readUser();
                }
                else break;
            }
        }
    }
    public class UserManager
    {
        ListNode<User> users = new ListNode<User>();
        DatabaseHandler databaseHandler= new DatabaseHandler();

        public void addAllUserFromDatabase()
        {
            //connectionString will be changed depending on database connection and authentication
            string connectionString = "Data Source=DESKTOP-U2K0EKM;Initial Catalog=UserDB;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlDataReader reader;
            connection = new SqlConnection(Properties.Settings.Default.connectionStr);
            connection.Open();
            reader = new SqlCommand("select ID, Name, Email, DOB from Users order by ID ASC", connection).ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    User temp_User = new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
                    users.Append(users, temp_User);
                }
            }
        }
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
                if (user.ID == id)
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
            foreach (User user in users)
            {
                if (user.ID == id)
                {
                    ok = true;
                    break;
                }
            }
            users.Reset();
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

    public class DatabaseHandler
    {
        public DatabaseHandler() { }
        public void createUser(User user)
        {
            string connectionString = "Data Source=DESKTOP-U2K0EKM;Initial Catalog=UserDB;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string Query = "insert into Users(Name, Email, DOB) values('" + user.Name + "', '" + user.Email + "', '" + user.DOB + "')";
            SqlCommand cmd = new SqlCommand(Query, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        public void readUser()
        {
            string connectionString = "Data Source=DESKTOP-U2K0EKM;Initial Catalog=UserDB;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlDataReader reader;
            connection = new SqlConnection(Properties.Settings.Default.connectionStr);
            connection.Open();
            reader = new SqlCommand("select ID, Name, Email, DOB from Users order by ID ASC", connection).ExecuteReader();
            Console.WriteLine("Users List :");
            if (reader.HasRows)
            {
                Console.WriteLine("UserID, Name, Email, DOB");
                while (reader.Read())
                {
                    Console.WriteLine("{0}, {1}, {2}, {3}", reader.GetInt32(0),
                    reader.GetString(1), reader.GetString(2), reader.GetString(3));
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            Console.WriteLine("\nThank You\n");
        }

        public void updateUser(User user)
        {
            string connectionString = "Data Source=DESKTOP-U2K0EKM;Initial Catalog=UserDB;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string Query = "UPDATE Users SET Name = '"+user.Name+"', Email = '"+user.Email+"', DOB = '"+user.DOB+"' WHERE ID =" + user.ID;
           // Console.WriteLine("Update Query : " + Query);
            SqlCommand cmd = new SqlCommand(Query, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        public void removeUser(int id)
        {
            string connectionString = "Data Source=DESKTOP-U2K0EKM;Initial Catalog=UserDB;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string Query = "DELETE FROM Users where ID="+ id;
            Console.WriteLine("here : " + Query);
            SqlCommand cmd = new SqlCommand(Query, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }
    public class Program
    {
        static void Main(string[] args)
        {
            UserUI userUI = new UserUI();
            userUI.Run();
        }
    }
}
