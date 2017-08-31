using System;
using System.Collections.Generic;
using System.Text;

namespace eidos.ml.test
{
    public class EidosTestBase
    {
        protected FloatingPointAssertions FloatingPointAssert()
        {
            return new FloatingPointAssertions();
        }
    }
}
