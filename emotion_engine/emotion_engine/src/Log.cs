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
		public bool Enable = true;
        private String m_log_path =
            //System.IO.Directory.GetCurrentDirectory() + "\\log";
            "D:\\faham\\emophiz\\emophiz\\logs";
        private static DateTime msc_tick_start = System.DateTime.Now;

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

		public Log(string filname = "emotion.Log")
		{
            if (!System.IO.Directory.Exists(m_log_path))
            {
                System.IO.DirectoryInfo info = System.IO.Directory.CreateDirectory(m_log_path);
            }
            
            m_filename = filname;
            m_logfile = new System.IO.StreamWriter(m_log_path + "\\" + m_filename, false);
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
            // "yyyy-MM-dd-HH:mm:ss:ffff"
            return System.DateTime.Now.Subtract(msc_tick_start).Ticks.ToString();
		}

		private String getLogSignature(Details detail, Priority priority, String delim = ",")
		{
			String _out = "";
			switch (detail)
			{
				case Details.Raw:
					break;
				case Details.Short:
					_out = GetTimestamp();
					break;
				case Details.Standard:
					_out = GetTimestamp() + delim + System.Enum.GetName(typeof(Priority), priority);
					break;
				case Details.Descriptive:
				default:
					{
						System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(0, true);
						System.Diagnostics.StackFrame sf = st.GetFrame(1);
						string fl = sf.GetFileName();
						if (fl.Length > 30)
							fl = "..." + fl.Substring(fl.Length - 30);
						_out = GetTimestamp() 
                            + delim + fl 
                            + delim + sf.GetFileLineNumber() 
                            + delim + sf.GetMethod().Name
                            + delim + System.Enum.GetName(typeof(Priority), priority);
					}
					break;
			}

            return _out;
		}

		public void CSV(Details detail, Priority priority, params String[] csv_values)
		{
            String csv_record_str = "";
            String delim = ",";
            for (int i = 0; i < csv_values.Length; ++i)
            {
                if (csv_record_str != "")
                    csv_record_str += delim;
                csv_record_str += csv_values[i].ToString();
            }
            String sig = getLogSignature(detail, priority, delim);
            if (sig != "")
                csv_record_str = sig + delim + csv_record_str;
            m_messages.Enqueue(csv_record_str);
		}

		public void Message(String message, Details detail = Details.Standard, Priority priority = Priority.Information)
		{
			if (!Enable)
				return;

			String delim = "\t";
			String log_msg_str = getLogSignature(detail, priority, delim);
			m_messages.Enqueue(log_msg_str + delim + message);
		}
	}
}
