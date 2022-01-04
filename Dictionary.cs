using System;
using System.Collections.Generic;
namespace HelloProject {
	
	class Program {
		static void Main(string[] args) {
			Dictionary<int, int> dct = new Dictionary<int, int>();
			dct.Add(1, 380);
			dct.Add(2, 381);
			dct.Add(3, 382);
			dct.Add(4, 383);
			dct.Add(5, 385);
			dct.Add(6, 386);

			foreach(var x in dct){
				Console.Write(x.Key + " " + x.Value);
				Console.Write('\n');
			}
			
			Dictionary<int, List<string> > dc = new Dictionary<int, List<string> >();
			dc.Add(1, new List<string>(){"Arif", "dfh", "sdfd", "SDF"});
			dc.Add(2, new List<string>(){"Hade"});
			dc.Add(3, new List<string>(){"Bijoy"});
			dc.Add(4, new List<string>(){"Shovon"});
			dc.Add(5, new List<string>(){"Murad"});
			dc.Add(6, new List<string>(){"Asif"});
			foreach(var x in dc){
				Console.Write(x.Key + " ");
				//foreach(string s in x.Value) Console.Write(s + " ");
				for(int i=0; i<x.Value.Count; i++) Console.Write(x.Value[i] + " ");
				Console.Write('\n');
			}
		}
	}
}
