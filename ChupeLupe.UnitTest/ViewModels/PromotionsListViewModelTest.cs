using System;
using ChupeLupe.UnitTest.Helpers;
using NUnit.Framework;
using Xamarin.Forms;
using Xamarin.Forms.Mocks;
using ChupeLupe.Services;
using Moq;
using ChupeLupe.ViewModels;
using ChupeLupe.Models;
using System.Collections.Generic;
using AutoFixture;

namespace ChupeLupe.UnitTest.ViewModels
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

            vm.GetPromotionsCommand.Execute(null);

            ServerSideDataMock.Verify(m => m.GetPromotions(), Times.Once());
            Assert.IsNotNull(vm.PromotionsList);
            Assert.AreEqual(1, vm.PromotionsList.Count);

        }
    }
}
