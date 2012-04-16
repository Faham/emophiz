using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace emophiz
{
	public class Log
	{
		private string m_filename;
		private System.IO.StreamWriter m_logfile;
		private System.Collections.Concurrent.ConcurrentQueue<string> m_messages;

		public enum Priority
		{
			Information,
			Warning,
			Error,
			Fatal
		};

		public Log(string filname = "Emotion.Log")
		{
			m_filename = filname;
			m_messages = new System.Collections.Concurrent.ConcurrentQueue<string>();
		}

		~Log()
		{
			//m_logfile.Flush();
			m_logfile = new System.IO.StreamWriter(m_filename, false);
			for (System.Collections.IEnumerator itr = m_messages.GetEnumerator(); itr.MoveNext();)
				m_logfile.WriteLine(itr.Current);
			m_logfile.Close();
		}

		public static String GetTimestamp()
		{
			return System.DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss:ffff");
		}

		public void Message(String message, Priority priority = Priority.Information)
		{
			m_messages.Enqueue(GetTimestamp() 
				+ "\t" + System.Enum.GetName(typeof(Priority), priority) 
				+ "\t" + message);
		}
	}
}
