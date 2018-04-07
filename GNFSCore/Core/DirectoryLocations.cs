﻿using System;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Threading;

using GNFSCore.Factors;
using GNFSCore.Polynomial;
using GNFSCore.IntegerMath;

namespace GNFSCore
{
	public class DirectoryLocations
	{
		private string _polynomial = null;
		private string _saveDirectory = null;
		private string _parameters = "_GNFS.Parameters";

		private static readonly int showDigits = 22;
		private static readonly string elipse = "[...]";
		private static readonly string saveRootDirectory = "C:\\GNFS";

		public string SaveDirectory { get { return _saveDirectory; } /*set { _saveDirectory = value; }*/ }

		public string GnfsParameters_SaveFile { get { return Path.Combine(SaveDirectory, _parameters); } }

		public string Polynomial_Filename { get { return "_Polynomial.Parameters"; } }
		public string Polynomial_SaveDirectory { get { return _polynomial ?? GetPolynomialPath(); } }

		public string Polynomial_SaveFile { get { return Path.Combine(Polynomial_SaveDirectory, Polynomial_Filename); } }
		public string RationalFactorBase_SaveFile { get { return Path.Combine(Polynomial_SaveDirectory, "Rational.FactorBase"); } }
		public string AlgebraicFactorBase_SaveFile { get { return Path.Combine(Polynomial_SaveDirectory, "Algebraic.FactorBase"); } }
		public string QuadradicFactorBase_SaveFile { get { return Path.Combine(Polynomial_SaveDirectory, "Quadradic.FactorBase"); } }

		public DirectoryLocations(string baseDirectory)
		{
			_saveDirectory = baseDirectory;
		}

		public DirectoryLocations(BigInteger n)
		{
			_saveDirectory = GenerateSaveDirectory(n);
		}

		public string GetPolynomialPath()
		{
			IEnumerable<string> polyDirectoriesrelationFiles = Directory.EnumerateDirectories(SaveDirectory, "Poly_B[*", SearchOption.TopDirectoryOnly);

			_polynomial = polyDirectoriesrelationFiles.FirstOrDefault() ?? SaveDirectory;
			return _polynomial;
		}

		public void SetPolynomialPath(IPolynomial poly)
		{
			_polynomial = Path.Combine(SaveDirectory, $"Poly_B[{poly.Base}]_D[{poly.Degree}]");
		}
		
		public static string GenerateSaveDirectory(BigInteger n)
		{
			string directoryFilename = GenerateFileNameFromBigInteger(n);
			return Path.Combine(saveRootDirectory, directoryFilename);
		}

		public static string GenerateFileNameFromBigInteger(BigInteger n)
		{
			string result = n.ToString();

			if (result.Length >= (showDigits * 2) + elipse.Length)
			{
				result = result.Substring(0, showDigits) + elipse + result.Substring(result.Length - showDigits, showDigits);
			}

			return result;
		}
	}
}
