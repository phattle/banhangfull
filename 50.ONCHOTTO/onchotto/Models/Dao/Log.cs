
//create by LOGITEM 2014  
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
namespace OnChotto.Models.Dao
{
	public class Log
	{
		//define varible
		public static bool disable { set; get; }
		public const string splitFolder = @"\";
		public static string tempFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
		public const string logFolder = "Log";

		//end define variable
		//Write log with exception
		public static string Write(Exception ex)
		{
			string log_file = tempFolder + splitFolder + logFolder;
			log_file += splitFolder + DateTime.Now.ToString("yyyyMMdd") + ".txt";
			if (System.IO.Directory.Exists(tempFolder + splitFolder + logFolder) == false)
			{
				System.IO.Directory.CreateDirectory(tempFolder + splitFolder + logFolder);
			}
            
			DateTime ctime = DateTime.Now;
			System.IO.StreamWriter sw = System.IO.File.AppendText(log_file);
			sw.WriteLine("|<<----------------Error:" + ctime.ToString("yyyy/MM/dd hh:mm:ss") + "----------------");
			sw.WriteLine("==>WKSNAME     : " + Environment.MachineName);
			sw.WriteLine("==>OS Version  : " + Environment.OSVersion);
			sw.WriteLine("==>Product_Name : " + ex.Message);
			sw.WriteLine("==>DETAIL      : ");
			sw.WriteLine(ex.ToString());
			sw.WriteLine("-------------------Error:" + ctime.ToString("yyyy/MM/dd hh:mm:ss") + "---------------->>|");
			sw.WriteLine("");            
			sw.Close();
            return log_file;
		}
		//Write log with string message
		public static void Write(string message)
		{
			if (Log.disable) return;
			string log_file = tempFolder + splitFolder + logFolder;
			log_file += splitFolder + DateTime.Now.ToString("yyyyMMdd") + ".txt";
			if (System.IO.Directory.Exists(tempFolder + splitFolder + logFolder) == false)
			{
				System.IO.Directory.CreateDirectory(tempFolder + splitFolder + logFolder);
			}
			DateTime ctime = DateTime.Now;
			System.IO.StreamWriter sw = System.IO.File.AppendText(log_file);
			sw.WriteLine("|<<----------------Error:" + ctime.ToString("yyyy/MM/dd hh:mm:ss") + "----------------");
			sw.WriteLine("==>WKSNAME     : " + Environment.MachineName);
			sw.WriteLine("==>OS Version  : " + Environment.OSVersion);
            sw.WriteLine("==>User name  : " + Environment.UserName);
			sw.WriteLine("==>Product_Name : " + message);
			sw.WriteLine("==>DETAIL      : ");
			sw.WriteLine("-------------------Error:" + ctime.ToString("yyyy/MM/dd hh:mm:ss") + "---------------->>|");
			sw.WriteLine();
			sw.Close();
		}
		public static void WriteMessage(string message)
		{
			if (Log.disable) return;
			string log_file = tempFolder + splitFolder + logFolder;
			log_file += splitFolder + DateTime.Now.ToString("yyyyMMdd") + ".txt";
			if (System.IO.Directory.Exists(tempFolder + splitFolder + logFolder) == false)
			{
				System.IO.Directory.CreateDirectory(tempFolder + splitFolder + logFolder);
			}
			DateTime ctime = DateTime.Now;
			System.IO.StreamWriter sw = System.IO.File.AppendText(log_file);
			sw.WriteLine("|<<----------------" + ctime.ToString("yyyy/MM/dd hh:mm:ss") + "----------------");
            sw.WriteLine("==>WKSNAME     : " + Environment.MachineName);
            sw.WriteLine("==>OS Version  : " + Environment.OSVersion);
            sw.WriteLine("==>User name  : " + Environment.UserName);
			sw.WriteLine("==>Content: " + message);
			sw.WriteLine("-------------------" + ctime.ToString("yyyy/MM/dd hh:mm:ss") + "---------------->>|");
			sw.WriteLine();
			sw.Close();
		}
        
		public static void WritePlain(string message)
		{
			if (Log.disable) return;
			string log_file = tempFolder + splitFolder + logFolder;
			log_file += splitFolder + DateTime.Now.ToString("yyyyMMdd") + "bak.txt";
			if (System.IO.Directory.Exists(tempFolder + splitFolder + logFolder) == false)
			{
				System.IO.Directory.CreateDirectory(tempFolder + splitFolder + logFolder);
			}
			System.IO.StreamWriter sw = System.IO.File.AppendText(log_file);
			sw.WriteLine(message);
			sw.WriteLine();
			sw.Close();
		}
		public static void WritePlain(string message, string ext)
		{
			if (Log.disable) return;
			string log_file = tempFolder + splitFolder + logFolder;
			log_file += splitFolder + DateTime.Now.ToString("yyyyMMdd") + ext;
			if (System.IO.Directory.Exists(tempFolder + splitFolder + logFolder) == false)
			{
				System.IO.Directory.CreateDirectory(tempFolder + splitFolder + logFolder);
			}
			System.IO.StreamWriter sw = System.IO.File.AppendText(log_file);
			sw.WriteLine(message);
			sw.WriteLine();
			sw.Close();
		}
	}
}
