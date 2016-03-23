using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nc2Dll;

namespace Nc2ExNao2Con1
{
	class Nc2ExNao2Con1_Test2
	{
		static NccpcSflMt1Simple1 gFlog = new NccpcSflMt1Simple1("Nc2DExNao2Con1", 0xffff);
		static public void qv(string s1) { Console.WriteLine(s1); } //gFlog.write(s1); }

		static public volatile bool gbWhile = true;

		public class Nm : Nao2MgrCpc
		{
			public override void innLog(string s1) { Nc2ExNao2Con1_Test2.qv(s1); }
		}

		public enum Nit
		{
			None,
			Test1,
			Test2,
		}

		public class No : Nao2ObjBaseCpc
		{
			public long mWc = 0;

			public No(Nm nm) : base(nm) { }

			public override void naiOnRead(Nao2IptBaseCpc nai)
			{
				using (var ni = (Ni)nai)
				{
					var nit = ni.getNit();

					mWc++;

					if (mWc % 100000 == 0)
					{ qv("naiOnRead nit:" + nit + " wc:" + mWc); }

					switch (nit)
					{
						case Nit.Test1:
							naiWrite(new Ni(Nit.Test1));
							break;
						case Nit.Test2:
							//System.Threading.Thread.Sleep(1000);
							gbWhile = false;
							break;
					}

				}
			}
		}
		public class Ni : Nao2IptBaseCpc
		{
			public Ni(Nit nit)
			{
				mNaiType = (long)nit;
			}

			//protected override void Dispose(bool v1)
			//{
			//	qv("Dispose");
			//}

			public Nit getNit() { return (Nit)mNaiType; }
		}

		public static void Main1()
		{
			Nm nm = new Nm();
			nm.create();

			while (true)
			{
				qv("loop start");
				gbWhile = true;

				var nos = new List<No>();


				for (int i = 0; i < 10; i++)
				{
					No no = new No(nm);
					no.create();
					nos.Add(no);
				}
				
				foreach(var no in nos)
				{
					no.naiWrite(new Ni(Nit.Test1));
					no.naiWrite(new Ni(Nit.Test1));
					no.naiWrite(new Ni(Nit.Test1));
					no.naiWrite(new Ni(Nit.Test1));
				}

				int wc = 0;

				while (gbWhile)
				{
					wc++;

					//qv("while wc:" + wc);
					//no.naiWrite(new Ni(Nit.Test1));
					//if (wc == 20) { nos.First().naiWrite(new Ni(Nit.Test2)); }
					System.Threading.Thread.Sleep(100);
				}

				qv("end1 - no.release()");
				foreach (var no in nos)
				{
					no.release();
				}
			}
			qv("end2 - nm.release()");
			nm.release();
			qv("end3 - app");
		}
	}
}
