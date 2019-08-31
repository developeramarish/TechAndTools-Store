﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechAndTools.Data;
using TechAndTools.Data.Models;
using TechAndTools.Services.Contracts;
using TechAndTools.Services.Models;
using TechAndTools.Services.Tests.Common;
using Xunit;

namespace TechAndTools.Services.Tests
{
    public class ContactServiceTests
    {
        private List<Contact> GetContactsData()
        {
            return new List<Contact>
            {
                new Contact
                {
                    Id = 1,
                    FullName = "Full Name Test 1",
                    Email = "test1@email.com",
                    Message = "test1 message"
                },
                new Contact
                {
                    Id = 2,
                    FullName = "Full Name Test 2",
                    Email = "test2@email.com",
                    Message = "test2 message"
                },
                new Contact
                {
                    Id = 3,
                    FullName = "Full Name Test 3",
                    Email = "test3@email.com",
                    Message = "test3 message"
                }
            };
        }

        private async Task SeedData(TechAndToolsDbContext context)
        {
            context.AddRange(GetContactsData());
            await context.SaveChangesAsync();
        }

        public ContactServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateAContact()
        {
            var options = new DbContextOptionsBuilder<TechAndToolsDbContext>()
                .UseInMemoryDatabase(databaseName: "CreateAsync_ShouldCreateAContact")
                .Options;

            TechAndToolsDbContext context = new TechAndToolsDbContext(options);

            IContactService contactService = new ContactService(context);

            await contactService.CreateAsync(new ContactServiceModel
            {
                FullName = "Full Name Test 1",
                Email = "test1@email.com",
                Message = "test1 message"
            });

            await contactService.CreateAsync(new ContactServiceModel
            {
                FullName = "Full Name Test 2",
                Email = "test2@email.com",
                Message = "test2 message"
            });

            const int expectedResult = 2;

            int actualResult = context.Contacts.Count();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteContactById()
        {
            var options = new DbContextOptionsBuilder<TechAndToolsDbContext>()
                .UseInMemoryDatabase(databaseName: "DeleteAsync_ShouldDeleteContactById")
                .Options;

            TechAndToolsDbContext context = new TechAndToolsDbContext(options);
            await SeedData(context);
            IContactService contactService = new ContactService(context);

            const int contactId = 1;

            bool result = await contactService.DeleteAsync(contactId);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_WithIncorrectContactShouldThrowException()
        {
            var options = new DbContextOptionsBuilder<TechAndToolsDbContext>()
                .UseInMemoryDatabase(databaseName: "DeleteAsync_WithIncorrectContactShouldThrowException")
                .Options;

            TechAndToolsDbContext context = new TechAndToolsDbContext(options);
            await SeedData(context);
            IContactService contactService = new ContactService(context);

            const int contactId = 0;

            await Assert.ThrowsAsync<ArgumentNullException>(() => contactService.DeleteAsync(contactId));
        }

        [Fact]
        public async Task MarkAsRead_ShouldChangeToTrueMarkAsReadContactById()
        {
            var options = new DbContextOptionsBuilder<TechAndToolsDbContext>()
                .UseInMemoryDatabase(databaseName: "MarkAsRead_ShouldMarkAsReadContactById")
                .Options;

            TechAndToolsDbContext context = new TechAndToolsDbContext(options);
            await SeedData(context);
            IContactService contactService = new ContactService(context);

            const int contactId = 1;

            bool result = await contactService.MarkAsRead(contactId);

            Assert.True(result);
        }
        
        [Fact]
        public async Task MarkAsRead_WithIncorrectContactShouldThrowException()
        {
            var options = new DbContextOptionsBuilder<TechAndToolsDbContext>()
                .UseInMemoryDatabase(databaseName: "MarkAsRead_WithIncorrectContactShouldThrowException")
                .Options;

            TechAndToolsDbContext context = new TechAndToolsDbContext(options);
            await SeedData(context);
            IContactService contactService = new ContactService(context);

            const int contactId = 0;

            await Assert.ThrowsAsync<ArgumentNullException>(() => contactService.MarkAsRead(contactId));
        }

        [Fact]
        public async Task GetAllContacts_ShouldReturnAllContactsFromDatabase()
        {
            var options = new DbContextOptionsBuilder<TechAndToolsDbContext>()
                .UseInMemoryDatabase(databaseName: "GetAllContacts_ShouldReturnAllContactsFromDatabase")
                .Options;

            TechAndToolsDbContext context = new TechAndToolsDbContext(options);
            await SeedData(context);
            IContactService contactService = new ContactService(context);
            
            int expectedResult = context.Contacts.Count();
            int actualResult = contactService.GetAllContacts().Count();

            Assert.Equal(expectedResult, actualResult);
        }
    }
}
