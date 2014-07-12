using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VDolgah.Tests
{
    [TestClass]
    public class FinanAlgTest
    {
        [TestMethod]
        public void EqualSaldo()
        {
            decimal[,] beforeGraph = new decimal[10, 10];
            int graphSize = beforeGraph.GetLength(0);
            decimal[,] afterGraph = new decimal[10, 10]; // вставить магию тут
            decimal[] beforeSaldo = new decimal[graphSize];
            decimal[] afterSaldo = new decimal[graphSize];

            for (int i = 0; i < graphSize; i++)
            {
                for (int j = 0; j < graphSize; j++)
                {
                    beforeSaldo[i] += beforeGraph[i, j];
                    beforeSaldo[i] -= beforeGraph[j, i];

                    afterSaldo[i] += afterGraph[i, j];
                    afterSaldo[i] -= afterGraph[j, i];


                }
            }
            CollectionAssert.AreEqual(beforeSaldo, afterSaldo);
        }
    }
}
