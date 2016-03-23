using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nc2Dll;

namespace Mgrc1Test2Gwsv
{
	class Nc2Ex2Mgrc1Gwsv_Program
	{
		static NccpcSflMt1Simple1 gFlog = new NccpcSflMt1Simple1("Mgrc1Test2Gwsv", 0xFffff);
		static public void qv(string s1) { Console.WriteLine(s1); if (gFlog != null) { gFlog.write(s1); } }

		public class TtGw : Mgrc1GwsvCpc
		{
			//public TtGw(int transbuffersize)
			//    : base(transbuffersize, transbuffersize * 10)
			//{
			//}

			public override void innLog(string s1) { Nc2Ex2Mgrc1Gwsv_Program.qv(s1); }
			public void qv(string s1) { innLog(s1); }

			public override void onUroLoginAllowOnTdwork(ulong gwok)
			{
				base.onUroLoginAllowOnTdwork(gwok);

				qv("Dbg TtGw.onUroLoginAllowOnTdwork gwok:" + gwok + " and send LgiCpReq");
				uroLoginCompleteRequest(gwok);
			}

			public override void onUroLoginComplete(ulong gwok, NccpcNw1Pk2 ncpk)
			{
				base.onUroLoginComplete(gwok, ncpk);

				qv("Dbg TtGw.onUroLoginComplete gwok:" + gwok + " ncpk len:" + ncpk.getDataLen());
			}

			public override void onUroLogoutAllowOnTdwork(ulong gwok)
			{
				base.onUroLogoutAllowOnTdwork(gwok);

				uroLogoutCompleteRequest(gwok);
			}
		}

		static void Main(string[] args)
		{
			NccpcNw1Cmn.stWsaStartup();

			string
				mnhost = Mgrc1Test2Gwsv.Properties.Settings.Default.MnsvHost, mnserv = Mgrc1Test2Gwsv.Properties.Settings.Default.MnsvServ,
				gwhost = Mgrc1Test2Gwsv.Properties.Settings.Default.GwsvHost, gwserv = Mgrc1Test2Gwsv.Properties.Settings.Default.GwsvServ;

			int buffersize = 0xffff,
				svomax = 50, uromax = Mgrc1Test2Gwsv.Properties.Settings.Default.UroMax;

			var mm = new NccpcMemmgr2Mgr();
			var tm = new NccpcTdMgr2();
			var gw = new TtGw();
			
			if (!mm.create()) { qv("mm create fail"); return; }
			if (!tm.create()) { qv("tm create fail"); return; }

			qv("Dbg Gwsv startup mnh:" + mnhost + " mns:" + mnserv + "gwh:" + gwhost + " gws:" + gwserv + " UroMax:" + uromax);
			qv("Dbg key: Q = Quit, ");

			if (!gw.create(mnhost, mnserv, gwhost, gwserv, uromax))
			{ qv("Dbg Gw create fail gwserv:" + gwserv); return; }
			qv("Dbg gw created");

			gw.setGwctAliveTimeout(10);
			

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

				gw.framemove(100);
			}


			tm.iocptdWorkerClose();

			gw.release();
			tm.release();
			mm.release();

			NccpcNw1Cmn.stWsaCleanup();
			gFlog.onAppdestory();
		}

	}
}
