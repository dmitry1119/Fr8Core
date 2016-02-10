﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http.Results;
using NUnit.Framework;
using StructureMap;
using StructureMap.AutoMocking;
using Data.Entities;
using Data.Infrastructure.StructureMap;
using Data.Interfaces;
using Data.Interfaces.DataTransferObjects;
using Data.States;
using DockyardTest.Controllers.Api;
using Hub.Interfaces;
using HubWeb.Controllers;
using UtilitiesTesting;
using UtilitiesTesting.Fixtures;


namespace DockyardTest.Controllers
{
    [TestFixture]
    [Category("ContainerControllerTests")]
    class ContainerControllerTests : ApiControllerTestBase
    {
        private Fr8AccountDO _testUserAccount;

        private Hub.Interfaces.IContainer _containerService;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _testUserAccount = FixtureData.TestDockyardAccount5();
            _containerService = ObjectFactory.GetInstance<Hub.Interfaces.IContainer>();

            using (var unitOfWork = ObjectFactory.GetInstance<IUnitOfWork>())
            {
                //uow.UserRepository.Add(_testUserAccount);
                var plan = FixtureData.TestRoute4();
                unitOfWork.UserRepository.Add(plan.Fr8Account);
                // This will Add a user as well as a plan for creating Containers
                unitOfWork.PlanRepository.Add(plan);
                unitOfWork.AspNetUserRolesRepository.AssignRoleToUser(Roles.Admin, _testUserAccount.Id);
                unitOfWork.SaveChanges();

                ObjectFactory.GetInstance<ISecurityServices>().Login(unitOfWork, _testUserAccount);
            }
        }

        [TearDown]
        public void TearDown()
        {
            using (var uow = ObjectFactory.GetInstance<IUnitOfWork>())
            {
                var curUser = uow.UserRepository.GetQuery()
                    .SingleOrDefault(x => x.Id == _testUserAccount.Id);

                ObjectFactory.GetInstance<ISecurityServices>().Logout();

                uow.UserRepository.Remove(curUser);
                uow.SaveChanges();
            }
        }

        [Test]
        public void ContainerController_ShouldHaveHMACOnGetPayloadMethod()
        {
            ShouldHaveFr8HMACAuthorizeOnFunction(typeof(ContainersController), "GetPayload");
        }

        [Test]
        public void ContainerController_ShouldHaveFr8ApiAuthorizeOnGetPayloadMethod()
        {
            ShouldHaveFr8ApiAuthorizeOnFunction(typeof(ContainersController), "GetPayload");
        }

        [Test]
        public void ContainerController_ShouldHaveFr8ApiAuthorizeOnGetIdsByNameMethod()
        {
            ShouldHaveFr8ApiAuthorizeOnFunction(typeof(ContainersController), "GetIdsByName");
        }

        [Test]
        public void ContainerController_ShouldHaveFr8ApiAuthorizeOnGetMethod()
        {
            ShouldHaveFr8ApiAuthorizeOnFunction(typeof(ContainersController), "Get");
        }

        [Test]
        public void ContainerController_Will_ReturnEmptyOkResult_If_No_Container_Found()
        {
            //Act
            var containerController = CreateContainerController();
            Addcontainer();
            Guid? id = FixtureData.TestContainer_Id_55();
            var actionResult = containerController.Get(id);
            //Assert
            Assert.IsNull(actionResult as OkNegotiatedContentResult<ContainerDO>);
        }

        [Test]
        public void ContainerController_Will_Return_All_UserContainers_When_Get_Invoked_With_Null()
        {
            //Arrange
            var containerController = CreateContainerController();
            Addcontainer();

            //Act
            Guid? id = null;
            var actionResult = containerController.Get(id) as OkNegotiatedContentResult<IEnumerable<ContainerDTO>>;

            ////Assert
            Assert.NotNull(actionResult);
            Assert.AreEqual(4, actionResult.Content.Count());
        }

        [Test]
        public void ContainerController_Will_Return_Single_Container_When_Get_Invoked_With_Id()
        {
            //Arrange
            var containerController = CreateContainerController();
            Addcontainer();

            //Act
            Guid? id = FixtureData.TestContainer_Id_1();
            var actionResult = containerController.Get(id) as OkNegotiatedContentResult<ContainerDTO>;

            ////Assert
            Assert.NotNull(actionResult);
        }

        private void Addcontainer()
        {
            //Arrange 
            using (var unitOfWork = ObjectFactory.GetInstance<IUnitOfWork>())
            {
                foreach (var container in FixtureData.TestControllerContainersByUser())
                {
                    unitOfWork.ContainerRepository.Add(container);
                }
                unitOfWork.SaveChanges();
            }
        }

        private static ContainersController CreateContainerController()
        {
            return new ContainersController();
        }

    }
}
