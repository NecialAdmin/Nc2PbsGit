using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nc2Dll;

namespace Nc2ExNao2Con1
{
	class Nc2ExNao2Con1_Test3
	{
		static NccpcSflMt1Simple1 gFlog = new NccpcSflMt1Simple1("Nc2DExNao2Con1", 0xffff);
		static public void qv(string s1) { Console.WriteLine(s1); } //gFlog.write(s1); }

		static public volatile bool gbWhile = true;

		public class Nm : Nao2MgrCpc
		{
			public override void innLog(string s1) { Nc2ExNao2Con1_Test3.qv(s1); }
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

					if (mWc % 1000000 == 0)
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

			public Nit getNit() { return (Nit)mNaiType; }
		}

		public static void Main1()
		{
			while (true)
			{
				gbWhile = true;

				Nm nm = new Nm();
				nm.create();

				No no = new No(nm);

				no.create();

				qv("naiWrite");
				no.naiWrite(new Ni(Nit.Test1));
				no.naiWrite(new Ni(Nit.Test1));
				no.naiWrite(new Ni(Nit.Test1));
				no.naiWrite(new Ni(Nit.Test1));
				no.naiWrite(new Ni(Nit.Test1));
				no.naiWrite(new Ni(Nit.Test1));
				no.naiWrite(new Ni(Nit.Test1));

				int wc = 0;

				while (gbWhile)
				{
					//wc++;

					//qv("while wc:" + wc);
					//no.naiWrite(new Ni(Nit.Test1));
					//if (wc == 50) { no.naiWrite(new Ni(Nit.Test2)); }
					System.Threading.Thread.Sleep(50);
				}

				qv("end1");
				no.release();
				qv("end2");
				nm.release();
				qv("end3");
			}
		}
	}
}
