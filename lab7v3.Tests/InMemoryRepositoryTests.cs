using System;
using System.Collections.Generic;
using System.Linq;
using lab7v3;
using Xunit;

namespace lab7v3.Tests
{
    public class InMemoryRepositoryTests
    {
        private Order CreateOrder(int id, decimal total = 100m, int customerId = 1)
        {
            return new Order
            {
                Id = id,
                OrderDate = new DateTime(2025, 1, 1),
                TotalAmount = total,
                CustomerId = customerId
            };
        }

        [Fact]
        public void Add_NewItem_ItemIsStored()
        {
            var repo = new InMemoryRepository<Order>();
            var order = CreateOrder(1);

            repo.Add(order);

            Assert.Single(repo.GetAll());
            Assert.Same(order, repo.GetById(1));
        }

        [Fact]
        public void Add_DuplicateId_ThrowsArgumentException()
        {
            var repo = new InMemoryRepository<Order>();
            repo.Add(CreateOrder(1));

            Assert.Throws<ArgumentException>(() => repo.Add(CreateOrder(1)));
        }

        [Fact]
        public void Add_Null_ThrowsArgumentNullException()
        {
            var repo = new InMemoryRepository<Order>();

            Assert.Throws<ArgumentNullException>(() => repo.Add(null!));
        }

        [Fact]
        public void GetById_ExistingId_ReturnsCorrectItem()
        {
            var repo = new InMemoryRepository<Order>();
            repo.Add(CreateOrder(1, 50m));
            repo.Add(CreateOrder(2, 75m));

            Order result = repo.GetById(2);

            Assert.Equal(2, result.Id);
            Assert.Equal(75m, result.TotalAmount);
        }

        [Fact]
        public void GetById_NonExistingId_ThrowsKeyNotFoundException()
        {
            var repo = new InMemoryRepository<Order>();
            repo.Add(CreateOrder(1));

            Assert.Throws<KeyNotFoundException>(() => repo.GetById(99));
        }

        [Fact]
        public void GetAll_EmptyRepository_ReturnsEmptyCollection()
        {
            var repo = new InMemoryRepository<Order>();

            Assert.Empty(repo.GetAll());
        }

        [Fact]
        public void GetAll_ReturnsAllAddedItems()
        {
            var repo = new InMemoryRepository<Order>();
            repo.Add(CreateOrder(1));
            repo.Add(CreateOrder(2));
            repo.Add(CreateOrder(3));

            List<Order> all = repo.GetAll().ToList();

            Assert.Equal(3, all.Count);
            Assert.Contains(all, o => o.Id == 1);
            Assert.Contains(all, o => o.Id == 2);
            Assert.Contains(all, o => o.Id == 3);
        }

        [Fact]
        public void Update_ExistingItem_ChangesData()
        {
            var repo = new InMemoryRepository<Order>();
            repo.Add(CreateOrder(1, 100m, 5));

            repo.Update(CreateOrder(1, 250m, 7));

            Order result = repo.GetById(1);
            Assert.Equal(250m, result.TotalAmount);
            Assert.Equal(7, result.CustomerId);
            Assert.Single(repo.GetAll());
        }

        [Fact]
        public void Update_NonExistingItem_ThrowsKeyNotFoundException()
        {
            var repo = new InMemoryRepository<Order>();
            repo.Add(CreateOrder(1));

            Assert.Throws<KeyNotFoundException>(() => repo.Update(CreateOrder(42)));
        }

        [Fact]
        public void Delete_ExistingId_RemovesItem()
        {
            var repo = new InMemoryRepository<Order>();
            repo.Add(CreateOrder(1));
            repo.Add(CreateOrder(2));

            repo.Delete(1);

            Assert.Single(repo.GetAll());
            Assert.Throws<KeyNotFoundException>(() => repo.GetById(1));
            Assert.Equal(2, repo.GetById(2).Id);
        }

        [Fact]
        public void Delete_NonExistingId_ThrowsKeyNotFoundException()
        {
            var repo = new InMemoryRepository<Order>();
            repo.Add(CreateOrder(1));

            Assert.Throws<KeyNotFoundException>(() => repo.Delete(99));
            Assert.Single(repo.GetAll());
        }
    }
}