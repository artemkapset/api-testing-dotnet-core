using NUnit.Framework;
using project_m10_task01.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace project_m10_task01.SpecFlowStepDefenition
{
    public class StepsDefinition
    {
        private StoreClient _client = new StoreClient();
        private Order _order;

        [Given(@"the following orders in the store")]
        public void GivenTheFollowingOrdersInTheStore(Table table)
        {
            var orders = table.CreateDynamicSet().ToList();
            foreach (var order in orders)
            {
                _client.PlaceOrderAsync(order).Wait();
            }

            //table.CreateDynamicSet().ToList().ForEach(order => _client.PlaceOrderAsync(order).Wait());
        }

        [When(@"i get order by '(.*)'")]
        public void WhenIGetOrderBy(long id)
        {
            _order = _client.GetOrderByIdAsync(id).Result;
        }

        [Then(@"order status should be '(.*)'")]
        public void ThenOrderStatusShouldBe(string status)
        {
            Assert.True(_order.Status.ToString().Equals(status));
        }
    }
}
