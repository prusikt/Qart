﻿using NUnit.Framework;
using Qart.Core.Io;
using Qart.Testing.FileBased;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qart.Testing.Tests
{
    class TestSystemTests
    {
        TestSystem TestSystem = new TestSystem(new DataStore(PathUtils.ResolveRelative(@"TestData\TestCases")));

        [Test]
        public void GetTestCases()
        {
            var testCases = TestSystem.GetTestCases();
            Assert.That(testCases.Count(), Is.EqualTo(1));
            Assert.That(testCases.First().Id, Is.EqualTo("ATestCase"));
        }
    }
}