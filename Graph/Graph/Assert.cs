using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    static class Assert
    {
        public static void IsTrue(bool value)
        {
            if (!value)
                throw new AssertException("Expected true.");
        }

        public static void IsFalse(bool value)
        {
            if (value)
                throw new Exception("Expected false.");
        }

        public static void AreEqual(object expected, object given)
        {
            if (!object.Equals(expected, given))
                throw new Exception($"Expected '{expected}' but got '{given}'.");
        }
    }


    [Serializable]
    public class AssertException : Exception
    {
        public AssertException(string message = null)
            : base(message)
        { }
    }
}
