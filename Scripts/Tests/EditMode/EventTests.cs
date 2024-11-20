using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Universal.Events;

namespace Tests.EditMode
{
    public class EventTests
    {
        [Test]
        public void T1ActionRecorder()
        {
            ActionRecorder ar = new(0);
            TestClass t1 = new();
            TestClass t2 = new();
            Action a1 = delegate { t1.a = 1; };
            Action a2 = delegate { t1.a = 2; };
            Action a3 = delegate { t2.c = "d"; };
            Assert.IsTrue(ar.TryRecord(a1));
            Assert.IsTrue(ar.TryRecord(a2));
            Assert.IsTrue(ar.TryRecord(a3));
            t2.c = "f";
            Assert.IsTrue(t2.c.Equals("f"));
            ar.InvokeLast();
            Assert.IsTrue(t2.c.Equals("d"));
            ar.InvokeLast();
            Assert.IsTrue(t1.a == 2);
            ar.InvokeLast();
            Assert.IsTrue(t1.a == 1);

            ar.InvokeLast();//
            Assert.IsTrue(t1.a == 1);
        }
        [Test]
        public void T2ActionRecorder()
        {
            ActionRecorder ar = new(2);
            TestClass t1 = new();
            TestClass t2 = new();
            Action a1 = delegate { t1.a = 1; };
            Action a2 = delegate { t1.a = 2; };
            Action a3 = delegate { t2.c = "d"; };
            Assert.IsTrue(ar.TryRecord(a1));
            Assert.IsTrue(ar.TryRecord(a2));
            Assert.IsFalse(ar.TryRecord(a3));
            ar.ReplaceRecord(a3);
            ar.InvokeLast();
            Assert.IsTrue(t2.c.Equals("d"));
            ar.InvokeLast();
            Assert.IsTrue(t1.a == 2);

            ar.InvokeLast();//
            Assert.IsTrue(t1.a == 2);
        }

        [Test]
        public void T1ActionRequest()
        {
            bool value = false;
            ActionRequest ar = new(delegate { TestAction(ref value); });
            Assert.AreEqual(ar.CanExecute(0), true);
            Assert.AreEqual(ar.CanExecute(int.MinValue), true);
            Assert.AreEqual(ar.CanExecute(int.MaxValue), true);
            ar.TryExecute(0);
            Assert.AreEqual(value, true);
        }

        [Test]
        public void T2ActionRequest()
        {
            bool value = false;
            ActionRequest ar = new(delegate { TestAction(ref value); });
            ar.AddBlockLevel(1);
            ar.AddBlockLevel(1);
            ar.AddBlockLevel(1);

            ar.TryExecute(0);
            Assert.AreEqual(value, false);

            ar.TryExecute(1);
            Assert.AreEqual(value, false);

            ar.TryExecute(2);
            Assert.AreEqual(value, true);
            value = false;

            ar.RemoveBlockLevel(1);
            ar.TryExecute(1);
            Assert.AreEqual(value, false);

            ar.RemoveBlockLevel(1);
            ar.TryExecute(1);
            ar.TryExecute(0);
            Assert.AreEqual(value, false);

            ar.RemoveBlockLevel(1);
            ar.TryExecute(-1);
            Assert.AreEqual(value, true);
            value = false;

            ar.AddBlockLevel(1);
            ar.RemoveBlockLevel(2);
            ar.RemoveBlockLevel(0);
            ar.RemoveBlockLevel(-1);
            ar.TryExecute(1);
            Assert.AreEqual(value, false);
        }

        private void TestAction(ref bool value)
        {
            value = true;
        }

        private class TestClass
        {
            public int a = 0;
            public int b = 0;
            public string c = "c";
            public string d = "d";
        }
    }
}