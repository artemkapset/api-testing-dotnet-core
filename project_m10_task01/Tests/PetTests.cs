using NUnit.Framework;
using project_m10_task01.Client;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using AutoFixture.NUnit3;
using AutoFixture;
using KellermanSoftware.CompareNetObjects;
using System.Linq;

namespace project_m10_task01.Tests
{
    public class PetTests
    {
        private StoreClient _client;
        private Fixture _fixture;
        private CompareLogic _compareLogic;        

        [OneTimeSetUp]
        public void CommonSetUp()
        {
            _client = new StoreClient();
            _fixture = new Fixture();

            _compareLogic = new CompareLogic();
            _compareLogic.Config.MaxDifferences = 10;
            _compareLogic.Config.IgnoreObjectTypes = true;
        }

        [Test]
        public void GetPetByIDTest()
        {            
            var petForAdd = _fixture.Create<Pet>();
            var id = petForAdd.Id;

            _client.AddPetAsync(petForAdd).Wait();

            var petAfterGet = _client.GetPetByIdAsync((long)id).Result;

            var _result = _compareLogic.Compare(petAfterGet, petForAdd);

            Assert.True(_result.AreEqual);
        }

        [Test]
        public void UpdateTest()
        {
            var petBeforeUpdate = _fixture.Create<Pet>();
            var id = petBeforeUpdate.Id;

            _client.AddPetAsync(petBeforeUpdate).Wait();
                        
            var newName = $"new{petBeforeUpdate.Name}";
                        
            var newStatus = Enum
                .GetNames(typeof(PetStatus))
                .ToList()
                .First(s => s != petBeforeUpdate.Status.ToString());

            _client.UpdatePetWithFormAsync((long)id, newName, newStatus).Wait();

            var petAfetrUpdate = _client.GetPetByIdAsync((long)id).Result;

            Assert.Multiple(() =>
            {
                Assert.True(petAfetrUpdate.Name.Equals(newName));
                Assert.True(petAfetrUpdate.Status.ToString().Equals(newStatus));
            });
        }

        [Test]
        public void DeleteTest()
        {
            var pet = _fixture.Create<Pet>();
            var id = pet.Id;

            _client.AddPetAsync(pet).Wait();

            var getPet = _client.GetPetByIdAsync((long)id).Result;
            
            Assert.True(_compareLogic.Compare(pet, getPet).AreEqual);

            _client.DeletePetAsync("special-key", (long)id).Wait();

            Assert.Multiple(() =>
            {
                var exc = Assert.ThrowsAsync<ApiException>(() => _client.GetPetByIdAsync((long)id));
                Assert.True(exc.Message.Contains("Pet not found"));
                Assert.True(exc.Message.Contains("Status: 404"));
            });
        }
    }
}
