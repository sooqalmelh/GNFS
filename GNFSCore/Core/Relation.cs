﻿using System;
using System.Linq;
using System.Text;
using System.Numerics;
using GNFSCore.IntegerMath;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GNFSCore
{
	using FactorBase;
	using Polynomial;
	using IntegerMath;
	using ExtendedNumerics;
	using PrimeSignature;
	using System.Xml.Serialization;
	using System.Xml;
	using System.Xml.Schema;

	public class Relation : IXmlSerializable
	{
		public int A { get; set; }
		public int B { get; set; }
		public BigInteger C;
		public BigInteger D;
		public BigInteger E;
		public BigInteger F;
		public BigInteger G;
		public BigInteger AlgebraicNorm { get; set; }
		public BigInteger RationalNorm { get; set; }

		[NonSerialized]
		private GNFS _gnfs;
		private BigInteger AlgebraicQuotient { get; set; }
		private BigInteger RationalQuotient { get; set; }

		public bool IsSmooth
		{
			get
			{
				return BigInteger.Abs(AlgebraicQuotient) == 1 && BigInteger.Abs(RationalQuotient) == 1;
			}
		}

		public Relation()
		{ }

		public Relation(int a, int b, GNFS gnfs)
		{
			A = a;
			B = b;
			_gnfs = gnfs;

			AlgebraicNorm = Normal.Algebraic(a, b, _gnfs.CurrentPolynomial); // b^deg * f( a/b )
			RationalNorm = Normal.Rational(a, b, _gnfs.CurrentPolynomial.Base); // a + bm

			AlgebraicQuotient = AlgebraicNorm;
			RationalQuotient = RationalNorm;

			BigInteger rationalEval = _gnfs.CurrentPolynomial.Evaluate(RationalNorm);
			C = rationalEval % _gnfs.N;
			D = rationalEval % B;
			E = _gnfs.CurrentPolynomial.Evaluate(AlgebraicNorm);
			F = E % _gnfs.N;
			G = E % B;
		}

		public void Sieve()
		{
			AlgebraicQuotient = Factor(_gnfs.AlgebraicPrimeBase, AlgebraicNorm, AlgebraicQuotient);
			RationalQuotient = Factor(_gnfs.RationalPrimeBase, RationalNorm, RationalQuotient);
		}

		private static BigInteger Factor(IEnumerable<int> factors, BigInteger norm, BigInteger quotient)
		{
			BigInteger sqrt = BigInteger.Abs(norm).SquareRoot();

			BigInteger result = quotient;
			foreach (int factor in factors)
			{
				if (result == 0 || result == -1 || result == 1 || factor > sqrt)
				{
					break;
				}
				while (result % factor == 0 && result != 1 && result != -1)
				{
					result /= factor;

					BigInteger absResult = BigInteger.Abs(result);
					if (absResult > 1 && absResult < int.MaxValue - 1)
					{
						int intValue = (int)absResult;
						if (factors.Contains(intValue))
						{
							result = 1;
						}
					}
				}
			}
			return result;
		}

		public Tuple<BitVector, BitVector> GetMatrixRowVector()
		{
			BitVector rationalBitVector = new BitVector(RationalNorm, _gnfs.RationalFactorBase);
			BitVector algebraicBitVector = new BitVector(AlgebraicNorm, _gnfs.AlgebraicFactorBase);
			//bool[] quadraticBitVector = QuadraticResidue.GetQuadraticCharacters(this, _gnfs.QFB);
			//List<bool> combinedVector = new List<bool>();
			//combinedVector.AddRange(rationalBitVector.Elements);
			//combinedVector.AddRange(algebraicBitVector.Elements);
			//combinedVector.AddRange(quadraticBitVector);
			//return new BitVector(RationalNorm, combinedVector.ToArray());
			return new Tuple<BitVector, BitVector>(rationalBitVector, algebraicBitVector);
		}

		public override string ToString()
		{
			return
				$"(a:{A.ToString().PadLeft(4)}, b:{B.ToString().PadLeft(2)})\t" +
				$"[ƒ(b) ≡ 0 mod a:{AlgebraicNorm.ToString().PadLeft(10)} ({AlgebraicNorm.IsSquare()}),\ta+b*m={RationalNorm.ToString().PadLeft(4)} ({RationalNorm.IsSquare()})]\t" +
				$"ƒ({RationalNorm}) =".PadRight(8) + $"{C.ToString().PadLeft(6)}" + $"% B = ".PadLeft(16).PadRight(26) + $"{D.ToString().PadLeft(6).PadRight(12)}" +
				$"ƒ({AlgebraicNorm}) =".PadLeft(6).PadRight(14) + $" {E.ToString().PadLeft(6)} % N =".PadRight(62) + $"{F.ToString().PadLeft(12)} % B =".PadRight(12) + $"{G.ToString().PadLeft(6)}";

			//+"\t QUOTIENT(Alg): {AlgebraicQuotient} \t QUOTIENT(Rat): {RationalQuotient}";
		}

		public static void Serialize(string filePath, Relation relation)
		{
			Serializer.Serialize($"{filePath}\\{relation.A}_{relation.B}.relation", relation);
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteElementString("A", A.ToString());
			writer.WriteElementString("B", B.ToString());
			writer.WriteElementString("C", C.ToString());
			writer.WriteElementString("D", D.ToString());
			writer.WriteElementString("E", E.ToString());
			writer.WriteElementString("F", F.ToString());
			writer.WriteElementString("G", G.ToString());
			writer.WriteElementString("AlgebraicNorm", AlgebraicNorm.ToString());
			writer.WriteElementString("RationalNorm", RationalNorm.ToString());
		}

		public void ReadXml(XmlReader reader)
		{
			reader.MoveToContent();
			reader.ReadStartElement();
			A = int.Parse(reader.ReadElementString("A"));
			B = int.Parse(reader.ReadElementString("B"));
			C = BigInteger.Parse(reader.ReadElementString("C"));
			D = BigInteger.Parse(reader.ReadElementString("D"));
			E = BigInteger.Parse(reader.ReadElementString("E"));
			F = BigInteger.Parse(reader.ReadElementString("F"));
			G = BigInteger.Parse(reader.ReadElementString("G"));
			AlgebraicNorm = BigInteger.Parse(reader.ReadElementString("AlgebraicNorm"));
			RationalNorm = BigInteger.Parse(reader.ReadElementString("RationalNorm"));
			reader.ReadEndElement();
		}

		public XmlSchema GetSchema() { return null; }
	}
}
