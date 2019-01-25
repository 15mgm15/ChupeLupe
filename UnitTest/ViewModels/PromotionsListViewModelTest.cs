using System;
using NUnit.Framework;
using Xamarin.Forms;
using Xamarin.Forms.Mocks;
using ChupeLupe.Services;
using Moq;
using ChupeLupe.ViewModels;
using ChupeLupe.Models;
using System.Collections.Generic;
using AutoFixture;
using UnitTest.Helpers;

namespace UnitTest.ViewModels
{
    [TestFixture]
    public class PromotionsListViewModelTest
    {
        Fixture Fixture { get; set; }
        DependencyServiceStub DependencyService { get; set; }
        Mock<IWebServicesApi> ServerSideDataMock { get; set; }
        Mock<INavigation> NavigationMock { get; set; }

        [SetUp]
        public void SetUp()
        {
            MockForms.Init();

            Fixture = new Fixture();
            DependencyService = new DependencyServiceStub();

            ServerSideDataMock = new Mock<IWebServicesApi>();
            DependencyService.Register<IWebServicesApi>(ServerSideDataMock.Object);

            NavigationMock = new Mock<INavigation>();
            DependencyService.Register<INavigation>(NavigationMock.Object);
        }

        [Test]
        public void GetPromotionsCommandIsSuccessful()
        {
            // Arrange
            var vm = new PromotionsListViewModel(NavigationMock.Object, DependencyService);
            List<Promotion> list = new List<Promotion>
            {
                new Promotion
                {
                    Id = Fixture.Create<int>(),
                    Title = Fixture.Create<string>()
                }
            };
            ServerSideDataMock.Setup(m => m.GetPromotions()).ReturnsAsync(list);

            // Act
            vm.GetPromotionsCommand.Execute(null);

            // Assert
            ServerSideDataMock.Verify(m => m.GetPromotions(), Times.Once());
            Assert.IsNotNull(vm.PromotionsList);
            Assert.AreEqual(1, vm.PromotionsList.Count);
            Assert.IsFalse(vm.IsBusy);

        }

        [Test]
        public void GetPromotionsCommandIsNotSuccessful()
        {
            // Arrange
            var vm = new PromotionsListViewModel(NavigationMock.Object, DependencyService);
            List<Promotion> list = new List<Promotion>();
            ServerSideDataMock.Setup(m => m.GetPromotions()).ReturnsAsync(list);

            // Act
            vm.GetPromotionsCommand.Execute(null);

            // Assert
            ServerSideDataMock.Verify(m => m.GetPromotions(), Times.Once());
            Assert.IsNull(vm.PromotionsList);
            //Assert.IsFalse(vm.IsBusy);
            Assert.IsFalse(true);

        }
    }
}
