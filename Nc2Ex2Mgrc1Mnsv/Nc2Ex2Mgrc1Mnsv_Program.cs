using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nc2Dll;

namespace Mgrc1Test2Mnsv
{
	class Nc2Ex2Mgrc1Mnsv_Program
	{
		static NccpcSflMt1Simple1 gFlog = new NccpcSflMt1Simple1("Mgrc1Test2Mnsv", 0xFffff);
		static public void qv(string s1) { Console.WriteLine(s1); if (gFlog != null) { gFlog.write(s1); }  }

		public class TtMn : Mgrc1Mnsv
		{

			//public TtMn()
			//{
			//}

			public override void innLog(string s1) { Nc2Ex2Mgrc1Mnsv_Program.qv(s1); }
			public void qv(string s1) { innLog(s1); }

		}

		static void Main(string[] args)
		{
			NccpcNw1Cmn.stWsaStartup();

			string
				mnhost = Mgrc1Test2Mnsv.Properties.Settings.Default.MnsvHost, mnserv = Mgrc1Test2Mnsv.Properties.Settings.Default.MnsvServ;

			int buffersize = 0xffff,
				svomax = 50, uromax = Mgrc1Test2Mnsv.Properties.Settings.Default.UroMax;

			var mm = new NccpcMemmgr2Mgr();
			var tm = new NccpcTdMgr2();
			var mn = new TtMn();

			if (!mm.create()) { qv("mm create fail"); return; }
			if (!tm.create()) { qv("tm create fail"); return; }

			qv("Dbg Mnsv startup mnh:" + mnhost + " mns:" + mnserv + " UroMax:" + uromax);
			qv("Dbg key: Q = Quit, ");

			if (!mn.create(mnhost, mnserv, svomax, uromax))
			{ qv("Dbg Mn create fail"); return; }
			qv("Dbg Mn created");

			bool bWhile = true;
			while (bWhile)
			{
				if (Console.KeyAvailable)
				{
					ConsoleKeyInfo k = Console.ReadKey(false);

					switch (k.Key)
					{
					case ConsoleKey.Q:
						bWhile = false;
						qv("Dbg quit");
						break;
					case ConsoleKey.M:
						{
						}
						break;
					}
				}
				else
				{
					System.Threading.Thread.Sleep(100);
				}

				mn.framemove(100);
			}

			tm.iocptdWorkerClose();
			
			mn.release();
			tm.release();
			mm.release();

			NccpcNw1Cmn.stWsaCleanup();
			gFlog.onAppdestory();
		}
	}
}
