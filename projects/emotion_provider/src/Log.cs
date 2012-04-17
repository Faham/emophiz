using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace emophiz
{
	public class Log
	{
		private string m_filename;
		private System.IO.StreamWriter m_logfile;
		private System.Collections.Concurrent.ConcurrentQueue<string> m_messages;
		private System.Timers.Timer m_timer = new System.Timers.Timer(100);

		public enum Priority
		{
			Information,
			Warning,
			Error,
			Fatal,
		};

		public enum Details
		{
			Raw,
			Short,
			Standard,
			Descriptive,
		};

		public Log(string filname = "Emotion.Log")
		{
			m_filename = filname;
			m_logfile = new System.IO.StreamWriter(m_filename, false);
			m_logfile.AutoFlush = true;
			m_messages = new System.Collections.Concurrent.ConcurrentQueue<string>();
			m_timer.Elapsed += new System.Timers.ElapsedEventHandler(this.TimerElapsed);
			m_timer.Start();
		}

		private void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			dumpMessagesToFile();
		}

		private void dumpMessagesToFile()
		{
			while (!m_messages.IsEmpty)
			{
				string temp;
				if (m_messages.TryDequeue(out temp))
					m_logfile.WriteLine(temp);
			}
		}

		~Log()
		{
			dumpMessagesToFile();
		}

		public static String GetTimestamp()
		{
			return System.DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss:ffff");
		}

		public void Message(String message, Details detail = Details.Standard, Priority priority = Priority.Information)
		{
			switch (detail)
			{
				case Details.Raw:
					m_messages.Enqueue(message);
					break;
				case Details.Short:
					m_messages.Enqueue(GetTimestamp() + "\t" + message);
					break;
				case Details.Standard:
					{
						System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(0, true);
						System.Diagnostics.StackFrame sf = st.GetFrame(1);
						string fl = sf.GetFileName();
						if (fl.Length > 30)
							fl = "..." + fl.Substring(fl.Length - 30);
						m_messages.Enqueue(GetTimestamp()
							+ "\t" + "Priority: " + System.Enum.GetName(typeof(Priority), priority)
							+ "\t" + "Message: " + message);
					}
					break;
				case Details.Descriptive:
				default:
					{
						System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(0, true);
						System.Diagnostics.StackFrame sf = st.GetFrame(1);
						string fl = sf.GetFileName();
						if (fl.Length > 30)
							fl = "..." + fl.Substring(fl.Length - 30);
						m_messages.Enqueue(GetTimestamp()
							+ "\t" + "File: " + fl
							+ "\t" + "Line: " + sf.GetFileLineNumber()
							+ "\t" + "Function: " + sf.GetMethod().Name
							+ "\t" + "Priority: " + System.Enum.GetName(typeof(Priority), priority)
							+ "\t" + "Message: " + message);
					}
					break;
			}
		}
	}
}
