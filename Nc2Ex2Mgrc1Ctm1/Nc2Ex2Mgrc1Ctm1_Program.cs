using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nc2Dll;

namespace Mgrc1Test2Ctm1
{
	class Nc2Ex2Mgrc1Ctm1_Program
	{
		static NccpcSflMt1Simple1 gFlog = new NccpcSflMt1Simple1("Mgrc1Test2Ctm1", 0xffff);

		static public void qv(string s1) { Console.WriteLine(s1); } //gFlog.write(s1); }

		public class TtCt : Mgrc1CttmCpc
		{
			public TtCt(NccpcMemmgr2Mgr mm, NccpcTdMgr2 tm)
				: base(mm, tm)
			{

			}

			public override void innLog(string s1) { Nc2Ex2Mgrc1Ctm1_Program.qv(s1); }
			public void qv(string s1) { innLog(s1); }
		}

		static void Main(string[] args)
		{
			NccpcNw1Cmn.stWsaStartup();

			string
				gwhost = Mgrc1Test2Ctm1.Properties.Settings.Default.GwsvHost, gwserv = Mgrc1Test2Ctm1.Properties.Settings.Default.GwsvServ;

			const int buffersize = 0xffff,
				svomax = 50, uromax = 500;

			var mm = new NccpcMemmgr2Mgr();
			var tm = new NccpcTdMgr2();
			var ct1i = DateTime.Now.Ticks % uromax;
			var ct1 = new TtCt(mm, tm);

			if (!mm.create()) { qv("mm create fail"); return; }
			if (!tm.create()) { qv("tm create fail"); return; }

			qv("Dbg Ctm1 startup gwh:" + gwhost + " gws:" + gwserv);
			qv("Dbg key: Q = Quit, ");


			qv("Dbg ct:" + ct1i + " ptr:" + ct1 + " add gwhost:" + gwhost + " gwserv:" + gwserv);

			if (!ct1.create(gwhost, gwserv.ToString()))
			{ qv("Dbg ct create fail"); return ; }
			qv("Dbg ct created");

			ct1.urdLoginReqSend(ct1i);
			

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

				//ct1.framemove(100);
			}


			ct1.release();
			tm.release();
			mm.release();

			NccpcNw1Cmn.stWsaCleanup();
		}
	}
}
