﻿using System;
using System.Collections.Generic;
using System.IO;
using Data.Entities;
using Data.States;

namespace UtilitiesTesting.Fixtures
{
	partial class FixtureData
	{
		private  const string _xmlPayLoadLocation = "DockyardTest\\Content\\DocusignXmlPayload.xml";
        
        public static ContainerDO TestContainer1()
		{
            var containerDO = new ContainerDO();
            containerDO.Id = TestContainer_Id_49();
            containerDO.RouteId = TestRoute2().Id;
            containerDO.ContainerState = 1;
            containerDO.ProcessNodes.Add(TestProcessNode1());
            return containerDO;
		}

        public static ContainerDO TestHealthDemoContainer1()
        {
            var containerDO = new ContainerDO();
            containerDO.Id = TestContainer_Id_49();
            containerDO.RouteId = TestRoute2().Id;
            containerDO.ContainerState = ContainerState.Executing;
            containerDO.ProcessNodes.Add(TestProcessNode1());
            return containerDO;
        }

        public static Guid TestContainer_Id_1()
        {
            // Previous ID value: 1.
            return new Guid("811D3B85-3A79-446E-8FD0-135A3D45AA94");
        }

        public static Guid TestContainer_Id_2()
        {
            // Previous ID value: 2.
            return new Guid("BEC8657E-CFB9-437C-A306-DE5FA8FB946F");
        }

        public static Guid TestContainer_Id_3()
        {
            // Previous ID value: 3.
            return new Guid("3D038766-527F-4DED-934A-5042D2910EE6");
        }

        public static Guid TestContainer_Id_4()
        {
            // Previous ID value: 4.
            return new Guid("206DAAED-C6CE-40C5-8326-46CFCA1FE873");
        }

        public static Guid TestContainer_Id_49()
        {
            // Previous ID value: 49.
            return new Guid("221242A7-371F-43B5-9CE1-A2B302CAD428");
        }

        public static Guid TestContainer_Id_55()
        {
            // Previous ID value: 55.
            return new Guid("4002ADA2-DCA3-424F-885B-3E7658512150");
        }

        public static IList<ContainerDO> GetContainers()
		{
            IList<ContainerDO> containeList = new List<ContainerDO>();
            var routeId = TestRoute5().Id;
            containeList.Add(new ContainerDO()
			{
                Id = TestContainer_Id_1(),
				Name = "Container 1",
                RouteId = routeId,
                // Fr8AccountId = "testuser",
				ContainerState = ContainerState.Executing
			});

            containeList.Add(new ContainerDO()
			{
                Id = TestContainer_Id_2(),
                Name = "Container 2",
                RouteId = routeId,
                // Fr8AccountId = "testuser",
				ContainerState = ContainerState.Executing
			});

            containeList.Add(new ContainerDO()
			{
                Id = TestContainer_Id_3(),
                Name = "Container 3",
                RouteId = routeId,
               // Fr8AccountId = "testuser",
				ContainerState = ContainerState.Unstarted
			});

            containeList.Add(new ContainerDO()
			{
                Id = TestContainer_Id_4(),
                Name = "Container 4",
                RouteId = routeId,
                // Fr8AccountId = "anotheruser",
				ContainerState = ContainerState.Unstarted
			});

            return containeList;
		}

        public static IList<ContainerDO> TestControllerContainersByUser()
        {
            IList<ContainerDO> containerList = new List<ContainerDO>();
            var routeId = TestRoute4().Id;
            containerList.Add(new ContainerDO()
            {
                Id = TestContainer_Id_1(),
                Name = "Container 1",
                RouteId = routeId,
                ContainerState = ContainerState.Executing
            });

            containerList.Add(new ContainerDO()
            {
                Id = TestContainer_Id_2(),
                Name = "Container 2",
                RouteId = routeId,
                ContainerState = ContainerState.Executing
            });

            containerList.Add(new ContainerDO()
            {
                Id = TestContainer_Id_3(),
                Name = "Container 3",
                RouteId = routeId,
                ContainerState = ContainerState.Unstarted
            });

            containerList.Add(new ContainerDO()
            {
                Id = TestContainer_Id_4(),
                Name = "Container 4",
                RouteId = routeId,
                ContainerState = ContainerState.Unstarted
            });

            return containerList;
        }

		/// <summary>
		/// Determines physical location of XML file with test data contents 
		/// </summary>
		/// <param name="physLocation"></param>
		/// <returns></returns>
		public static string FindXmlPayloadFullPath(string physLocation, string filepath="DockyardTest\\Content\\DocusignXmlPayload.xml")
		{
			if (string.IsNullOrEmpty(physLocation))
				return string.Empty;

			string path = Path.Combine(physLocation, filepath);
			if (!File.Exists(path))
				path = FindXmlPayloadFullPath(UpNLevels(physLocation, 1), filepath);
			return path;
		}

		/// <summary>
		/// Given a directory path, returns an upper level path by the specified number of levels up.
		/// </summary>
		private static string UpNLevels(string path, int levels)
		{
			int index = path.LastIndexOf('\\', path.Length - 1, path.Length);
			if (index <= 3)
				return string.Empty;
			string result = path.Substring(0, index);
			if (levels > 1)
			{
				result = UpNLevels(result, levels - 1);
			}
			return result;
		}

        public static ContainerDO TestContainerWithCurrentActivityAndNextActivity()
        {
            var container = new ContainerDO();
            container.Id = TestContainer_Id_49();
				container.Route = TestRoute2();
            container.RouteId = TestRoute2().Id;
            container.ContainerState = 1;
            container.ProcessNodes.Add(TestProcessNode1());
            container.CurrentRouteNode = FixtureData.TestAction7();
				container.NextRouteNode = FixtureData.TestAction10();
            return container;
        }

        public static ContainerDO TestContainerCurrentActivityNULL()
        {
            var container = new ContainerDO();
            container.Id = TestContainer_Id_49();
            container.RouteId = TestRoute2().Id;
            container.ContainerState = 1;
            container.ProcessNodes.Add(TestProcessNode1());
            container.CurrentRouteNode = null;
            return container;
        }

        public static ContainerDO TestContainerWithCurrentActivityAndNextActivityTheSame()
        {
            var container = new ContainerDO();
            container.Id = TestContainer_Id_49();
            container.RouteId = TestRoute2().Id;
            container.ContainerState = 1;
            container.ProcessNodes.Add(TestProcessNode1());
            container.CurrentRouteNode = FixtureData.TestAction7();
            container.NextRouteNode = FixtureData.TestAction7();
            return container;
        }

        public static ContainerDO TestContainerSetNextActivity()
        {
            var container = new ContainerDO();
            container.Id = TestContainer_Id_49();
            container.RouteId = TestRoute2().Id;
            container.ContainerState = 1;
            container.ProcessNodes.Add(TestProcessNode1());
            container.CurrentRouteNode = FixtureData.TestAction7();
            container.NextRouteNode = null;
            return container;
        }

        public static ContainerDO TestContainerUpdateNextActivity()
        {
            var container = new ContainerDO();
            container.Id = TestContainer_Id_49();
            container.RouteId = TestRoute2().Id;
            container.ContainerState = 1;
            container.ProcessNodes.Add(TestProcessNode1());
            container.CurrentRouteNode = FixtureData.TestAction8(null);
            container.NextRouteNode = null;
            return container;
        }

        public static ContainerDO TestContainerExecute()
        {
            var containerDO = new ContainerDO();
            containerDO.Id = TestContainer_Id_49();
            containerDO.Route = FixtureData.TestRoute2();
            containerDO.RouteId = containerDO.Route.Id;
            containerDO.ContainerState = 1;
            containerDO.ProcessNodes.Add(FixtureData.TestProcessNode1());
            return containerDO;
        }
	}
}