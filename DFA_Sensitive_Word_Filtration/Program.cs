using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DFA_Sensitive_Word_Filtration {
    class Program {
        static void Main(string[] args) {
            List<string> filterMap = new List<string>();
            filterMap.Add("你是谁");
            filterMap.Add("你是狗吧");
            filterMap.Add("你妈");
            filterMap.Add("我是狗");
            filterMap.Add("我是工具人");
            filterMap.Add("我是你爸");
            filterMap.Add("我是你爸爸");
            filterMap.Add("我是你爹");
            filterMap.Add("你说什么");
            filterMap.Add("你说啥");
            filterMap.Add("你妈的为什么");
            filterMap.Add("你妈的");
            filterMap.Add("什么");
            filterMap.Add("什么破玩意");
            filterMap.Add("垃圾");
            filterMap.Add("哈哈哈");
            filterMap.Add("写的什么垃圾代码");
            InitFilter(filterMap);
            foreach (DictionaryEntry de in map) {
                Console.WriteLine(de.Key + " - " + de.Value);
            }
            Console.ReadLine();
        }

        //DFA构造敏感词树
        private static Hashtable map;
        private static void InitFilter(List<string> words) {
            map = new Hashtable(words.Count);
            for (int i = 0; i < words.Count; i++) {
                string word = words[i];
                Hashtable indexMap = map;
                for (int j = 0; j < word.Length; j++) {
                    char c = word[j];
                    if (indexMap.ContainsKey(c)) {
                        indexMap = (Hashtable)indexMap[c];
                    } else {
                        Hashtable newMap = new Hashtable();
                        newMap.Add("IsEnd", 0);
                        indexMap.Add(c, newMap);
                        indexMap = newMap;
                    }
                    if (j == word.Length - 1) {
                        if (indexMap.ContainsKey("IsEnd")) {
                            indexMap["IsEnd"] = 1;
                        } else {
                            indexMap.Add("IsEnd", 1);
                        }
                    }
                }
            }
        }
        //检测敏感词
        private int CheckFilterWord(string txt, int beginIndex) {
            bool flag = false;
            int len = 0;
            Hashtable curMap = map;
            for (int i = beginIndex; i < txt.Length; i++) {
                char c = txt[i];
                Hashtable temp = (Hashtable)curMap[c];
                if (temp != null) {
                    if ((int)temp["IsEnd"] == 1) {
                        flag = true;
                    } else {
                        curMap = temp;
                    }
                    len++;
                } else {
                    break;
                }
            }

            if (!flag) {
                len = 0;
            }
            return len;
        }
        //搜索敏感词并替换
        public string SerachFilterWordAndReplace(string txt) {
            int i = 0;
            StringBuilder sb = new StringBuilder(txt);
            while (i < txt.Length) {
                int len = CheckFilterWord(txt, i);
                if (len > 0) {
                    for (int j = 0; j < len; j++) {
                        sb[i + j] = '*';
                    }
                    i += len;
                } else {
                    ++i;
                }
            }
            return sb.ToString();
        }
    }
}
