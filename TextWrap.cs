using System;
using System.Collections.Generic;
namespace HelloProject {
	
	class Program {
		static void Main(string[] args) {
		   string s = "abc defg xyz c pq";
		   int mxLength = 5;
		   List<string> list = new List<string>();
		   int len = s.Length;
		   string tmp = "";
		   for(int i=0; i<len; i++){
			   if(i+1 == len){
				   tmp += s[i];
				   list.Add(tmp);
				   tmp = "";
			   }
			   if(s[i] == ' '){
				   list.Add(tmp);
				   tmp = "";
				   continue;
			   }
			   tmp += s[i];
		   }
		   string ans = "";
		   ans += list[0];
		   int cnt = list[0].Length;
		   if(list.Count > 1){
			   ans += ' ';
			   cnt++;
		   }
		   for(int i=1; i<list.Count; i++){
			   cnt += list[i].Length;
			   if(cnt > mxLength){
				   ans += '\r';
				   ans += '\n';
				   cnt = 0;
				   i--;
			   }
			   else{
				   ans += list[i];
				   if(i+1 < list.Count){
					   ans += ' ';
					   cnt++;
				   }
			   }
		   }
		   Console.WriteLine(ans);
		}
	}
}
