using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nc2Dll;

namespace Mgrc1Test2Mcsv
{
	class Nc2Ex2Mgrc1Mcsv_Program
	{
		static NccpcSflMt1Simple1 gFlog = new NccpcSflMt1Simple1("Mgrc1Test2Mcsv", 0xFffff);
		static public void qv(string s1) { Console.WriteLine(s1); if (gFlog != null) { gFlog.write(s1); } }

		public class TtMc : Mgrc1Mcsv
		{
			//public TtMc(int transbuffersize)
			//    : base(transbuffersize, transbuffersize * 10)
			//{
			//}

			public override void innLog(string s1) { Nc2Ex2Mgrc1Mcsv_Program.qv(s1); }
			public void qv(string s1) { innLog(s1); }

			public override void onUroLoginComplete(ulong gwok)
			{
				base.onUroLoginComplete(gwok);

				qv("Dbg TtMc.onUroLoginComplete gwok:" + gwok);
			}

			public override void onUroLogoutComplete(ulong gwok)
			{
				base.onUroLogoutComplete(gwok);

				qv("Dbg TtMc.onUroLogoutComplete gwok:" + gwok);
			}

			public override void onUroChange(ulong gwokNew, ulong gwokOld)
			{
				base.onUroChange(gwokNew, gwokOld);

				qv("Dbg TtMc.onUroChange gwokNew: " + gwokNew + " gwokOld: " + gwokOld);
			}
		}

		static void Main(string[] args)
		{
			NccpcNw1Cmn.stWsaStartup();

			string
				mnhost = Mgrc1Test2Mcsv.Properties.Settings.Default.MnsvHost, mnserv = Mgrc1Test2Mcsv.Properties.Settings.Default.MnsvServ,
				mchost = Mgrc1Test2Mcsv.Properties.Settings.Default.McsvHost, mcserv = Mgrc1Test2Mcsv.Properties.Settings.Default.McsvServ;

			int buffersize = 0xffff,
				svomax = 50, uromax = Mgrc1Test2Mcsv.Properties.Settings.Default.UroMax;

			var mm = new NccpcMemmgr2Mgr();
			var tm = new NccpcTdMgr2();
			var mc = new TtMc();
			
			if (!mm.create()) { qv("mm create fail"); return; }
			if (!tm.create()) { qv("tm create fail"); return; }

			qv("Dbg Mcsv startup mnh:" + mnhost + " mns:" + mnserv + "mch:" + mchost + " mcs:" + mcserv + " UroMax:" + uromax);
			qv("Dbg key: Q = Quit, ");

			if (!mc.create(mnhost, mnserv, mchost, mcserv, uromax))
			{ qv("Dbg Mc create fail mcserv:" + mcserv); return; }
			qv("Dbg mc created");

			//mc.setMcctAliveTimeout(10);
			

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

				mc.framemove(100);
			}


			tm.iocptdWorkerClose();

			mc.release();
			tm.release();
			mm.release();

			NccpcNw1Cmn.stWsaCleanup();
			gFlog.onAppdestory();
		}

	}
}
