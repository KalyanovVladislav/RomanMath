using NUnit.Framework;
using RomanMath.Impl;
using System;

namespace RomanMath.Tests
{
	public class Tests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void Test1()
		{
			var result = Service.Evaluate("IV+II/V");
			Assert.AreEqual(4, result);
		}
		[Test]
		public void NullExpressionTest()
        {
			Assert.Throws<ArgumentNullException>(() => Service.Evaluate(null));
		}
		[Test]
		public void InvalidOperatorsTest()
		{
			Assert.Throws<ArgumentException>(() => Service.Evaluate("I*V"));
		}
		[Test]
		public void InvalidLettersTest()
		{
			Assert.Throws<ArgumentException>(() => Service.Evaluate("B-V"));
		}
		[Test]
		public void NotAllowedCharactersTest()
		{
			Assert.Throws<ArgumentException>(() => Service.Evaluate("IV+II/V="));
		}
		[Test]
		public void WhitespacesTest()
		{
			Assert.Throws<ArgumentException>(() => Service.Evaluate("IV + II       / V"));
		}
		[Test]
		public void MultylineExpressionTest()
		{
			Assert.Throws<ArgumentException>(() => Service.Evaluate("IV + II\n/ V"));
		}
		[Test]
		public void InvalidExpressionTest()
		{
			Assert.Throws<ArgumentException>(() => Service.Evaluate("IV++II/V"));
		}
	}
}