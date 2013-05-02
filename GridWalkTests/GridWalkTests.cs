using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace GridWalkTests
{
    [TestClass]
    public class GridWalkTests
    {
        PrivateObject program;
        
        [TestInitialize]
        public void Init()
        {
            program = new PrivateObject(typeof(GridWalker));
        }

        private Dictionary<Tuple<int,int>, bool> GetAccessiblePoints()
        {
            return (Dictionary<Tuple<int,int>, bool>)program.GetFieldOrProperty("accessiblePoints");
        }

        private List<Tuple<int,int>> GetOrderedPoints()
        {
            return (List<Tuple<int,int>>)program.GetFieldOrProperty("orderedPoints");
        }

        [TestMethod]
        public void IsPointValidTest()
        {
            bool actual = (bool)program.Invoke("LimitCheck", new object[] { 59, 79, 19 });
            Assert.IsFalse(actual);

            actual = (bool)program.Invoke("LimitCheck", new object[] { -5, -7, 19 });
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void AddIfPointValidPointOutOfRange()
        {
            program.Invoke("AddPointIfValid", new object[] { 59, 79 });
            var accessiblePoints = GetAccessiblePoints();
            var orderedPoints = GetOrderedPoints();
            Assert.IsTrue(accessiblePoints.Count == 1 && orderedPoints.Count == 1);
        }

        [TestMethod]
        public void AddIfPointValidPointAlreadyVisited()
        {
            program.Invoke("AddPointIfValid", new object[] { 0, 0 });
            var accessiblePoints = GetAccessiblePoints();
            var orderedPoints = GetOrderedPoints();
            Assert.IsTrue(accessiblePoints.Count == 1 && orderedPoints.Count == 1);
        }

        [TestMethod]
        public void AddIfPointValidAdded()
        {
            program.Invoke("AddPointIfValid", new object[] { 0, 1 });
            var accessiblePoints = GetAccessiblePoints();
            var orderedPoints = GetOrderedPoints();
            Assert.IsTrue(accessiblePoints.Count == 2 && orderedPoints.Count == 2);

            program.Invoke("AddPointIfValid", new object[] { -5, -7 });
            Assert.IsTrue(accessiblePoints.Count == 3 && orderedPoints.Count == 3);
        }

        [TestMethod]
        public void GetNumberOfAccessiblePointsTest()
        {
            var gridWalker = new GridWalker();
            var rtn = gridWalker.GetNumberOfAccessiblePoints();
            Assert.IsTrue(rtn == 0, "Number of accessible points is: " + rtn.ToString());
        }

        [TestMethod]
        public void GetNumberOfAccessiblePointsTestLimited()
        {
            //1 == 5
            //2 == 13
            //3 == 25
            var gridWalker = new GridWalker(3);
            var rtn = gridWalker.GetNumberOfAccessiblePoints();
            Assert.IsTrue(rtn == 25, "Number of accessible points is: " + rtn.ToString());
        }
    }
}
