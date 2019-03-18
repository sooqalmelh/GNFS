﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Numerics;
using Newtonsoft.Json;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GNFSCore
{
	using Factors;
	using Interfaces;
	using IntegerMath;

	[DataContract]
	public partial class GNFS
	{
		#region Properties

		[DataMember]
		public BigInteger N { get; set; }
		[DataMember]
		public int PolynomialDegree { get; private set; }
		[DataMember]
		public BigInteger PolynomialBase { get; private set; }

		[JsonProperty(ItemConverterType = typeof(Serialization.PolynomialConverter))]
		public List<IPolynomial> PolynomialCollection { get; set; }
		public IPolynomial CurrentPolynomial { get; internal set; }

		[DataMember]
		public PolyRelationsSieveProgress CurrentRelationsProgress { get; set; }

		public CancellationToken CancelToken { get; set; }

		[DataMember]
		public FactorBase PrimeFactorBase { get; set; }

		/// <summary>
		/// Array of (p, m % p)
		/// </summary>
		public FactorPairCollection RationalFactorPairCollection { get; set; }

		/// <summary>
		/// Array of (p, r) where ƒ(r) % p == 0
		/// </summary>
		public FactorPairCollection AlgebraicFactorPairCollection { get; set; }
		public FactorPairCollection QuadradicFactorPairCollection { get; set; }

		public DirectoryLocations SaveLocations { get; internal set; }

		public LogMessageDelegate LogFunction { get; set; }

		public delegate void LogMessageDelegate(string message);

		#endregion

		#region Constructors 

		public GNFS()
		{
			PrimeFactorBase = new FactorBase();
			PolynomialCollection = new List<IPolynomial>();
			RationalFactorPairCollection = new FactorPairCollection();
			AlgebraicFactorPairCollection = new FactorPairCollection();
			QuadradicFactorPairCollection = new FactorPairCollection();
			CurrentRelationsProgress = new PolyRelationsSieveProgress();
		}

		public GNFS(CancellationToken cancelToken, LogMessageDelegate logFunction, BigInteger n, BigInteger polynomialBase, int polyDegree, BigInteger primeBound, int relationQuantity, int relationValueRange)
			: this()
		{
			CancelToken = cancelToken;
			LogFunction = logFunction;
			N = n;

			SaveLocations = new DirectoryLocations(N, polynomialBase, polyDegree);

			if (!Directory.Exists(SaveLocations.SaveDirectory))
			{
				// New GNFS instance
				Directory.CreateDirectory(SaveLocations.SaveDirectory);
				LogMessage($"Directory created.");

				if (polyDegree == -1)
				{
					this.PolynomialDegree = CalculateDegree(n);
				}
				else
				{
					this.PolynomialDegree = polyDegree;
				}

				this.PolynomialBase = polynomialBase;

				if (CancelToken.IsCancellationRequested) { return; }

				ConstructNewPolynomial(this.PolynomialBase, this.PolynomialDegree);
				LogMessage($"Polynomial constructed.");

				if (CancelToken.IsCancellationRequested) { return; }

				CaclulatePrimeBounds(primeBound);
				LogMessage($"Prime bounds calculated.");

				if (CancelToken.IsCancellationRequested) { return; }

				NewFactorBases();
				LogMessage($"Factor bases populated.");

				if (CancelToken.IsCancellationRequested) { return; }

				CurrentRelationsProgress = new PolyRelationsSieveProgress(this, relationQuantity, relationValueRange);
				LogMessage($"Relations container initialized.");

				Serialization.Save.Gnfs(this);
			}
		}

		#endregion

		public bool IsFactor(BigInteger toCheck)
		{
			return ((N % toCheck) == 0);
		}


		#region New Factorization

		// Values were obtained from the paper:
		// "Polynomial Selection for the Number Field Sieve Integer factorization Algorithm" - by Brian Antony Murphy
		// Table 3.1, page 44
		private static int CalculateDegree(BigInteger n)
		{
			int result = 2;
			int base10 = n.ToString().Length;

			if (base10 < 65)
			{
				result = 3;
			}
			else if (base10 >= 65 && base10 < 125)
			{
				result = 4;
			}
			else if (base10 >= 125 && base10 < 225)
			{
				result = 5;
			}
			else if (base10 >= 225 && base10 < 315)
			{
				result = 6;
			}
			else if (base10 >= 315)
			{
				result = 7;
			}

			return result;
		}

		private void CaclulatePrimeBounds()
		{
			BigInteger bound = new BigInteger();

			int base10 = N.ToString().Length; //N.NthRoot(10, ref remainder);
			if (base10 <= 10)
			{
				bound = 100;//(int)((int)N.NthRoot(_degree, ref remainder) * 1.5); // 60;
			}
			else if (base10 <= 18)
			{
				bound = base10 * 1000;//(int)((int)N.NthRoot(_degree, ref remainder) * 1.5); // 60;
			}
			else if (base10 <= 100)
			{
				bound = 100000;
			}
			else if (base10 > 100 && base10 <= 150)
			{
				bound = 250000;
			}
			else if (base10 > 150 && base10 <= 200)
			{
				bound = 125000000;
			}
			else if (base10 > 200)
			{
				bound = 250000000;
			}

			CaclulatePrimeBounds(bound);
		}

		private void CaclulatePrimeBounds(BigInteger bound)
		{
			PrimeFactorBase = new FactorBase();

			PrimeFactorBase.RationalFactorBaseMax = bound;
			PrimeFactorBase.AlgebraicFactorBaseMax = (PrimeFactorBase.RationalFactorBaseMax) * 3;

			PrimeFactorBase.QuadraticBaseCount = CalculateQuadraticBaseSize(PolynomialDegree);

			PrimeFactorBase.QuadraticFactorBaseMin = PrimeFactorBase.AlgebraicFactorBaseMax + 20;
			PrimeFactorBase.QuadraticFactorBaseMax = PrimeFactory.GetApproximateValueFromIndex((UInt64)(PrimeFactorBase.QuadraticFactorBaseMin + PrimeFactorBase.QuadraticBaseCount));

			Serialization.Save.Gnfs(this);

			LogMessage($"Constructing new prime bases (- of 3)...");

			PrimeFactory.IncreaseMaxValue(PrimeFactorBase.QuadraticFactorBaseMax);

			PrimeFactorBase.RationalFactorBase = PrimeFactory.GetPrimesTo(PrimeFactorBase.RationalFactorBaseMax).ToList();
			Serialization.Save.FactorBase.Rational(this);
			LogMessage($"Completed rational prime base (1 of 3).");

			PrimeFactorBase.AlgebraicFactorBase = PrimeFactory.GetPrimesTo(PrimeFactorBase.AlgebraicFactorBaseMax).ToList();
			Serialization.Save.FactorBase.Algebraic(this);
			LogMessage($"Completed algebraic prime base (2 of 3).");

			PrimeFactorBase.QuadraticFactorBase = PrimeFactory.GetPrimesFrom(PrimeFactorBase.QuadraticFactorBaseMin).Take(PrimeFactorBase.QuadraticBaseCount).ToList();
			Serialization.Save.FactorBase.Quadratic(this);
			LogMessage($"Completed quadratic prime base (3 of 3).");
		}

		private static int CalculateQuadraticBaseSize(int polyDegree)
		{
			int result = -1;

			if (polyDegree <= 3)
			{
				result = 10;
			}
			else if (polyDegree == 4)
			{
				result = 20;
			}
			else if (polyDegree == 5 || polyDegree == 6)
			{
				result = 40;
			}
			else if (polyDegree == 7)
			{
				result = 80;
			}
			else if (polyDegree >= 8)
			{
				result = 100;
			}

			return result;
		}

		private void ConstructNewPolynomial(BigInteger polynomialBase, int polyDegree)
		{
			CurrentPolynomial = new Polynomial(N, polynomialBase, polyDegree);
			PolynomialCollection.Add(CurrentPolynomial);
			Serialization.Save.Gnfs(this);
		}

		private void NewFactorBases()
		{
			LogMessage($"Constructing new factor bases (- of 3)...");

			if (!RationalFactorPairCollection.Any())
			{
				RationalFactorPairCollection = FactorPairCollection.Factory.BuildRationalFactorBase(this);
			}
			Serialization.Save.FactorPair.Rational(this);
			LogMessage($"Completed rational factor base (1 of 3).");


			if (CancelToken.IsCancellationRequested) { return; }
			if (!AlgebraicFactorPairCollection.Any())
			{
				AlgebraicFactorPairCollection = FactorPairCollection.Factory.BuildAlgebraicFactorBase(this);
			}
			Serialization.Save.FactorPair.Algebraic(this);
			LogMessage($"Completed algebraic factor base (2 of 3).");


			if (CancelToken.IsCancellationRequested) { return; }
			if (!QuadradicFactorPairCollection.Any())
			{
				QuadradicFactorPairCollection = FactorPairCollection.Factory.BuildQuadradicFactorBase(this);
			}
			Serialization.Save.FactorPair.Quadratic(this);
			LogMessage($"Completed quadratic factor base (3 of 3).");

			if (CancelToken.IsCancellationRequested) { return; }
		}

		#endregion

		public static List<Relation[]> GroupRoughNumbers(List<Relation> roughNumbers)
		{
			IEnumerable<Relation> input1 = roughNumbers.OrderBy(rp => rp.AlgebraicQuotient).ThenBy(rp => rp.RationalQuotient);
			//IEnumerable<Relation> input2 = roughNumbers.OrderBy(rp => rp.RationalQuotient).ThenBy(rp => rp.AlgebraicQuotient);

			Relation lastItem = null;
			List<Relation[]> results = new List<Relation[]>();
			foreach (Relation pair in input1)
			{
				if (lastItem == null)
				{
					lastItem = pair;
				}
				else if (pair.AlgebraicQuotient == lastItem.AlgebraicQuotient && pair.RationalQuotient == lastItem.RationalQuotient)
				{
					results.Add(new Relation[] { pair, lastItem });
					lastItem = null;
				}
				else
				{
					lastItem = pair;
				}
			}

			return results;
		}

		public static List<Relation> MultiplyLikeRoughNumbers(GNFS gnfs, List<Relation[]> likeRoughNumbersGroups)
		{
			List<Relation> result = new List<Relation>();

			foreach (Relation[] likePair in likeRoughNumbersGroups)
			{
				var As = likePair.Select(lp => lp.A).ToList();
				var Bs = likePair.Select(lp => lp.B).ToList();

				int a = (int)(As[0] + Bs[0]) * (int)(As[0] - Bs[0]);//(int)Math.Round(Math.Sqrt(As.Sum()));
				uint b = (uint)(As[1] + Bs[1]) * (uint)(As[1] - Bs[1]);//(int)Math.Round(Math.Sqrt(Bs.Sum()));

				if (a > 0 && b > 0)
				{
					result.Add(new Relation(gnfs, a, b));
				}
			}

			return result;
		}

		public void LogMessage(string message)
		{
			if (LogFunction != null)
			{
				LogFunction.Invoke(message);
			}
		}

		#region ToString

		public override string ToString()
		{
			StringBuilder result = new StringBuilder();

			result.AppendLine($"N = {N}");
			result.AppendLine();
			result.AppendLine($"Polynomial(degree: {PolynomialDegree}, base: {PolynomialBase}):");
			result.AppendLine("ƒ(m) = " + CurrentPolynomial.ToString());
			result.AppendLine();
			result.AppendLine("Prime Factor Base Bounds:");
			result.AppendLine($"RationalFactorBase : {PrimeFactorBase.RationalFactorBaseMax}");
			result.AppendLine($"AlgebraicFactorBase: {PrimeFactorBase.AlgebraicFactorBaseMax}");
			result.AppendLine($"QuadraticPrimeBase Range: {PrimeFactorBase.QuadraticFactorBaseMin} - {PrimeFactorBase.QuadraticFactorBaseMax}");
			result.AppendLine($"QuadraticPrimeBase Count: {PrimeFactorBase.QuadraticFactorBase.Count}");
			result.AppendLine();
			result.AppendLine($"RFB - Rational Factor Base - Count: {RationalFactorPairCollection.Count} - Array of (p, m % p) with prime p");
			result.AppendLine(RationalFactorPairCollection.ToString(200));
			result.AppendLine();
			result.AppendLine($"AFB - Algebraic Factor Base - Count: {AlgebraicFactorPairCollection.Count} - Array of (p, r) such that ƒ(r) ≡ 0 (mod p) and p is prime");
			result.AppendLine(AlgebraicFactorPairCollection.ToString(200));
			result.AppendLine();
			result.AppendLine($"QFB - Quadratic Factor Base - Count: {QuadradicFactorPairCollection.Count} - Array of (p, r) such that ƒ(r) ≡ 0 (mod p) and p is prime");
			result.AppendLine(QuadradicFactorPairCollection.ToString());
			result.AppendLine();

			result.AppendLine();
			result.AppendLine();

			return result.ToString();
		}

		#endregion
	}
}
