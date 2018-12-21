﻿using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagCloud
{
    class Tests
    {
        private CloudLayouter cloud;
        private TestContext currentContext;
        private const string FilePath = "DebugFile.bmp";

        [SetUp]
        public void SetUp()
        {
            cloud = new CloudLayouter(new Point(0, 0), 0.5, new ArchimedesSpiralPointsMaker());
            currentContext = TestContext.CurrentContext;
        }

        [TearDown]
        public void TearDown()
        {
            if (currentContext.Result.FailCount != 0)
            {
                var visualizer = new Visualizer(new Size(600, 600), "#FFFFFF", "#000000");
                visualizer.RenderCurrentConfig(cloud, FilePath);
                Console.WriteLine("Tag cloud visualization saved to file " + FilePath);
            }
        }

        [Test]
        public void TagCloudConstructor_ShouldMakeCenter()
        {
            cloud.Center.Should().Be(new Point(0, 0));
        }


        [Test]
        public void FirstRectangle_ShouldBeNearCenter()
        {
            var rect = cloud.PutNextRectangle(new Size(10, 10));
            rect.GetValueOrThrow().Contains(cloud.Center).Should().BeTrue();
        }

        [TestCase(-1, 1, TestName = "With Negative Width")]
        [TestCase(0, 1, TestName = "With Zero Width")]
        [TestCase(1, -1, TestName = "With Negative Height")]
        [TestCase(1, 0, TestName = "With Zero Height")]
        public void PutNextRectangleShouldThrowArgumentException(int width, int height)
        {
            var size = new Size(width, height);
            Assert.Throws<ArgumentException>(() => cloud.PutNextRectangle(size));
        }
        [Test]
        public void FirstRectangle_ShouldBeInCenter()
        {
            var rect = cloud.PutNextRectangle(new Size(10, 10));
            rect.GetValueOrThrow().Location.Should().Be(new Point(0, 0));
        }
        [Test]
        public void ArchimedesSpiralPointsMaker_ShouldReturnCenterAtFirst()
        {
            var value = new ArchimedesSpiralPointsMaker()
                .GenerateNextPoint(new Point(0, 0), 2).First();
            value.Should().Be(new Point(0, 0));
        }
    }
}
