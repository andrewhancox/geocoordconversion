﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TDPG.GeoCoordConversion.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class TestSuite
    {
        private List<GeoTestDataSet> testData;

        [TestInitialize]
        public void Setup()
        {
            // Following values come from http://www.nearby.org.uk/coord.cgi?p=BN1+6PJ&f=conv            

            testData = new List<GeoTestDataSet>
                {
                    new GeoTestDataSet(
                        "Brighton",
                        new PolarGeoCoordinate(50.84609, -0.1424094, 0, AngleUnit.Degrees, CoordinateSystems.OSGB36),
                        new PolarGeoCoordinate(50.84668, -0.1439875, 0, AngleUnit.Degrees, CoordinateSystems.WGS84),
                        new GridReference(530760, 106880)),
                    new GeoTestDataSet(
                        "Newcastle",
                        new PolarGeoCoordinate(54.979808, -1.584025, 0, AngleUnit.Degrees, CoordinateSystems.OSGB36),
                        new PolarGeoCoordinate(54.979889, -1.585609, 0, AngleUnit.Degrees, CoordinateSystems.WGS84),
                        new GridReference(426620, 565110)),
                    new GeoTestDataSet(
                        "Truro",
                        new PolarGeoCoordinate(50.262067, -5.052743, 0, AngleUnit.Degrees, CoordinateSystems.OSGB36),
                        new PolarGeoCoordinate(50.262655, -5.053748, 0, AngleUnit.Degrees, CoordinateSystems.WGS84),
                        new GridReference(182450, 044760)),
                    //I can't get sufficiently good quality data for belfast to include this
                    //{new GeoTestDataSet(
                    //    "Belfast",
                    //    //OSGB36 is calculated using this tool since I can't any osgb36 coords for NI
                    // PolarGeoCoordinate.ChangeCoordinateSystem(
                    //     new PolarGeoCoordinate(54.579254, -5.934520, 0, AngleUnit.Degrees, CoordinateSystems.WGS84),
                    //     CoordinateSystems.OSGB36),
                    //    new PolarGeoCoordinate(54.579254, -5.934520, 0, AngleUnit.Degrees, CoordinateSystems.WGS84),
                    //    new GridReference(145849, 527567))},
                    new GeoTestDataSet(
                        "John O'Groats",
                        new PolarGeoCoordinate(58.639451, -3.069178, 0, AngleUnit.Degrees, CoordinateSystems.OSGB36),
                        new PolarGeoCoordinate(58.639073, -3.070747, 0, AngleUnit.Degrees, CoordinateSystems.WGS84),
                        new GridReference(337940, 972850))
                };
        }


        public TestSuite()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion       

        [TestMethod]
        public void TestDegreesToRadians()
        {
            PolarGeoCoordinate deg = new PolarGeoCoordinate(180, 180, 0, AngleUnit.Degrees, CoordinateSystems.OSGB36);
            PolarGeoCoordinate rad = new PolarGeoCoordinate(Math.PI, Math.PI, 0, AngleUnit.Radians, CoordinateSystems.OSGB36);

            Assert.IsTrue(rad.IsTheSameAs(PolarGeoCoordinate.ChangeUnits(deg, AngleUnit.Radians)));
        }

        [TestMethod]
        public void TestRadiansToDegrees()
        {
            PolarGeoCoordinate deg = new PolarGeoCoordinate(180, 180, 0, AngleUnit.Degrees, CoordinateSystems.OSGB36);
            PolarGeoCoordinate rad = new PolarGeoCoordinate(Math.PI, Math.PI, 0, AngleUnit.Radians, CoordinateSystems.OSGB36);

            Assert.IsTrue(deg.IsTheSameAs(PolarGeoCoordinate.ChangeUnits(rad, AngleUnit.Degrees)));
        }

        [TestMethod]
        public void TestPolarGeoCoordComparison()
        {
            Assert.IsTrue(
                testData[0].OSGB36.IsTheSameAs(testData[0].OSGB36)
                && !testData[0].OSGB36.IsTheSameAs(testData[0].WGS84)
                && testData[0].OSGB36.IsTheSameAs(testData[0].OSGB36,true,true)
                && !testData[0].OSGB36.IsTheSameAs(testData[0].WGS84,true,true));
        }

        [TestMethod]
        public void TestGridReferenceComparison()
        {
            Assert.IsTrue(testData[0].NE.IsTheSameAs(testData[0].NE)
               && !testData[0].NE.IsTheSameAs(testData[1].NE)
               && testData[0].NE.IsTheSameAs(testData[0].NE,true)
               && !testData[0].NE.IsTheSameAs(testData[1].NE, true));
        }

        [TestMethod]
        public void TestWGS84ToOSGB36TestOSGB36ToWGS84()
        {
            StringBuilder sb = new StringBuilder();

            foreach (GeoTestDataSet item in testData)
            {
                PolarGeoCoordinate converted = PolarGeoCoordinate.ChangeCoordinateSystem(item.WGS84, CoordinateSystems.OSGB36);

                if (!item.OSGB36.IsTheSameAs(converted, true, true))
                    sb.AppendLine(item.City);
            }

            if (sb.Length > 0)
            {
                sb.AppendLine("failed out of" + testData.Count.ToString());
                Assert.Fail(sb.ToString());
            }
        }

        [TestMethod]
        public void TestOSGB36ToWGS84()
        {
            StringBuilder sb = new StringBuilder();

            foreach (GeoTestDataSet item in testData)
            {
                PolarGeoCoordinate converted = PolarGeoCoordinate.ChangeCoordinateSystem(item.OSGB36, CoordinateSystems.WGS84);
                
                if (!item.WGS84.IsTheSameAs(converted, true, true))
                    sb.AppendLine(item.City);
            }

            if (sb.Length > 0)
            {
                sb.AppendLine("failed out of" + testData.Count.ToString());
                Assert.Fail(sb.ToString());
            }
        }

        [TestMethod]
        public void TestOSGB36PolarGeoToGridref()
        {
            StringBuilder sb = new StringBuilder();

            foreach (GeoTestDataSet item in testData)
            {
                GridReference converted = PolarGeoCoordinate.ChangeToGridReference(item.OSGB36);
                
                if (!item.NE.IsTheSameAs(converted, true))
                    sb.AppendLine(item.City);
            }

            if (sb.Length > 0)
            {
                sb.AppendLine("failed out of" + testData.Count.ToString());
                Assert.Fail(sb.ToString());
            }
        }

        [TestMethod]
        public void TestWGS84PolarGeoToGridref()
        {
            StringBuilder sb = new StringBuilder();

            foreach (GeoTestDataSet item in testData)
            {
                GridReference converted = PolarGeoCoordinate.ChangeToGridReference(item.WGS84);

                if (!item.NE.IsTheSameAs(converted, true))
                    sb.AppendLine(item.City);
            }

            if (sb.Length > 0)
            {
                sb.AppendLine("failed out of" + testData.Count.ToString());
                Assert.Fail(sb.ToString());
            }
        }

        [TestMethod]
        public void TestGridrefToPolarGeo()
        {
            StringBuilder sb = new StringBuilder();

            foreach (GeoTestDataSet item in testData)
            {
                PolarGeoCoordinate converted = GridReference.ChangeToPolarGeo(item.NE);                

                if (!item.OSGB36.IsTheSameAs(converted, true, true))
                    sb.AppendLine(item.City);
            }

            if (sb.Length > 0)
            {
                sb.AppendLine("failed out of" + testData.Count.ToString());
                Assert.Fail(sb.ToString());
            }
        }
    }
}