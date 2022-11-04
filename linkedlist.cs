using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp1
{
    public class Node
    {
        public int _Data;
        public Node _Next;
        public Node() { }
        public Node(int data)
        {
            _Data = data;
            _Next = null;
        }
    }

    public class ListNode
    {
        public Node head;
        public ListNode()
        {
            head = null;
        }
        
        public void AddAtStart(ListNode list, int new_data)
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

        public void AddAfter(Node prev_node, int new_data)
        {
            if(prev_node == null)
            {
                Console.WriteLine("Previous node can't be null");
                return;
            }

            Node new_node = new Node(new_data);
            new_node._Next = prev_node._Next;
            prev_node._Next = new_node;
        }

        public void AddAfterNodeNumber(ListNode list, int position, int new_data)
        {
            Node new_node = new Node(new_data);
            if (list.head == null)
            {
                list.head = new_node;
                return ;
            }
            Node temp = list.head;
            int cnt = 0;
            Node prev_node = null;
            while(temp != null)
            {
                cnt++;
                if(cnt == position)
                {
                    prev_node = temp;
                    break;
                }
                temp = temp._Next;
            }
            if(prev_node._Next == null)
            {
                prev_node._Next = new_node;
                return;
            }
            new_node._Next = prev_node._Next;
            prev_node._Next = new_node;
        }

        public void Append(ListNode list, int new_data)
        {
            Node new_node = new Node(new_data);
            if(list.head == null)
            {
                list.head = new_node;
                return;
            }
            Node temp = list.head;
            while(temp._Next != null)
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
            while(temp._Next._Next != null)
            {
                temp = temp._Next;
            }
            temp._Next = null;
            return;
        }

        public void RemoveFromPosition(ListNode list, int position)
        {
            if (list.head == null)return;
            int total = SizeOfList(list);
            if(position == total)
            {
                RemoveEnd(list);   
                return;
            }
            if(position == 1)
            {
                RemoveStart(list);
                return;
            }
            int cnt = 0;
            Node temp = list.head;
            Node prev_node = null;
            while(temp != null)
            {
                cnt++;
                if(cnt+1 == position)
                {
                    prev_node = temp;
                    break;
                }
                temp = temp._Next;
            }
            prev_node._Next = temp._Next._Next;
            return;
        }

        public bool SearchElement(ListNode list, int val)
        {
            Node node = list.head;
            while(node != null)
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
            while(node != null)
            {
                Console.Write(node._Data + " ");
                node = node._Next;
            }
            Console.WriteLine();
        }

        public int SizeOfList(ListNode list)
        {
            Node node = list.head;
            int cnt = 0;
            while (node != null)
            {
                cnt++;
                node = node._Next;
            }
            return cnt;
        }

        public void AddArrayAtLast(ListNode list, int[] arr)
        {
            for(int i=0; i < arr.Length; i++)
            {
                Append(list, arr[i]);
            }
            return;
        }
        public void AddArrayAtStart(ListNode list, int[] arr)
        {
            for(int i=0; i<arr.Length; i++)
            {
                AddAtStart(list, arr[i]);
            }
            return;
        }

        public void ConvertListToArray(ListNode list, int[] arr)
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

    public class Main_class
    {
        static void Main(string[] args)
        {
            ListNode listNode = new ListNode();

            listNode.AddAtStart(listNode, 10);
            listNode.AddAtStart(listNode, 20);
            listNode.AddAtStart(listNode, 20);
            listNode.AddAtStart(listNode, 30);
            listNode.AddAfter(listNode.head, 40);
            listNode.Append(listNode, 50);
            listNode.Append(listNode, 70);
            listNode.Append(listNode, 80);
            listNode.AddAfterNodeNumber(listNode, 1, 100);
            listNode.RemoveFromPosition(listNode, 2);
            listNode.RemoveStart(listNode);
            listNode.RemoveEnd(listNode);
            int[] arr = new int[] { 5, 4, 6 };
            listNode.AddArrayAtLast(listNode, arr);
            listNode.AddArrayAtStart(listNode, arr);
            int listLength = listNode.SizeOfList(listNode);
            Console.WriteLine("Size of current list : " + listLength);
            listNode.PrintList(listNode);
            bool found = listNode.SearchElement(listNode, 30);
            Console.WriteLine((found == true) ? "Found" : "Not Found");
            int[] A = new int[listLength];
            listNode.ConvertListToArray(listNode, A);
            Console.Write("Array element : ");
            for (int i = 0; i < A.Length; i++)
            {
                Console.Write(A[i] + " ");
            }
            Console.WriteLine();
        }
    }
}
