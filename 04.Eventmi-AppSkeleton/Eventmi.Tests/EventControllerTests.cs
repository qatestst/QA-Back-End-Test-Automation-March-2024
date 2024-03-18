using Eventmi.Core.Models.Event;
using Eventmi.Infrastructure.Data.Contexts;
using Eventmi.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Internal.Execution;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Event = Eventmi.Infrastructure.Models.Event;

namespace Eventmi.Tests
{
    public class EventControllerTests
    {
        private RestClient _client;
        private string baseUrl = "https://localhost:7236";

        [SetUp]
        public void Setup()
        {
            _client = new RestClient(baseUrl);
        }

        [Test]
        public async Task GetAllEvents_ReturnSuccessfulCode()
        {
            //Arrange
            var request = new RestRequest("/Events/All", Method.Get);

            //Act
            var response = await _client.ExecuteAsync(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
                       
        }

        [Test]
        public async Task Add_GetRequest_ReturnsAddViewe()
        {
            //Arrange
            var request = new RestRequest("/Events/Add", Method.Get);

            //Act
            var response = await _client.ExecuteAsync(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

        }

        [Test]
        public async Task Add_PostRequest_AddsNewEventAndRedirects()
        {
            //Arrange
            var input = new EventFormModel() 
            { 
            Name = "Soft Uni Conf",
            Place = "SoftUni",
            Start = new DateTime(2024, 12,12,12,0,0),
            End = new DateTime(2024,12,12,16,0,0)
            };
            
            var request = new RestRequest("/Events/Add", Method.Post);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            //Add form data to the request
            request.AddParameter("Name", input.Name);
            request.AddParameter("Place", input.Place);
            request.AddParameter("Start", input.Start.ToString("MM/dd/yyyy hh:mm:ss tt"));
            request.AddParameter("End", input.End.ToString("MM/dd/yyyy hh:mm:ss tt"));

            //Act
            var response = await _client.ExecuteAsync(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
            Assert.That(CheckIfEventExists(input.Name), Is.True, "Event was not added to database");

        }

        private bool CheckIfEventExists(string name)
        {
            var options = new DbContextOptionsBuilder<EventmiContext>().UseSqlServer("Server=.;Database=Eventmi;Trusted_Connection=True;MultipleActiveResultSets=true").Options;
            using var context = new EventmiContext(options);

            return context.Events.Any(x => x.Name == name);
        }

        [Test]
        public async Task Details_GetRequest_ShouldReturnDetailedView()
        {
            //Arrange
            var eventId = 1;
            var request = new RestRequest($"/Events/Details/{eventId}", Method.Get);

            //Act
            var response = await _client.ExecuteAsync(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Edit_GetRequest_ShouldReturnEditView()
        {
            //Arrange
            var eventId = 1;
            var request = new RestRequest($"/Event/Edit/{eventId}", Method.Get);

            //Act
            var response = await _client.ExecuteAsync(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        }

        [Test]
        public async Task Details_GetRequest_ShouldReturnNotFoundIfNoIdIsGiven()
        {
            //Arrange
            var request = new RestRequest($"/Events/Details/", Method.Get);

            //Act
            var response = await _client.ExecuteAsync(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Edit_GetRequest_ShouldReturnNotFoundIfNoIdIsGiven()
        {
            //Arrange
            
            var request = new RestRequest($"/Event/Edit/", Method.Get);

            //Act
            var response = await _client.ExecuteAsync(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        }

        private Event GetEventById(int id)
        {
            var options = new DbContextOptionsBuilder<EventmiContext>().UseSqlServer("Server=.;Database=Eventmi;Trusted_Connection=True;MultipleActiveResultSets=true").Options;
            using var context = new EventmiContext(options);

            return context.Events.FirstOrDefault(x => x.Id == id);
        }

        [Test]
        public async Task Edit_PostRequest_SHouldEditAnEvent()
        {
            //Arrange
            var eventId = 1;
            var dbEvent = GetEventById(eventId);

            var input = new EventFormModel()
            {
                Id = dbEvent.Id,
                End = dbEvent.End,
                Name = $"{dbEvent.Name} UPDATED!!!",
                Place = dbEvent.Place,
                Start = dbEvent.Start,
            };

            var request = new RestRequest($"/Event/Edit/{dbEvent.End}", Method.Post);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("Id", input.Id);
            request.AddParameter("Place", input.Place);
            request.AddParameter("Start", input.Start.ToString("MM/dd/yyyy hh:mm:ss tt"));
            request.AddParameter("End", input.End.ToString("MM/dd/yyyy hh:mm:ss tt"));

            //Act
            var response = await _client.ExecuteAsync(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var updatedDbEvent = GetEventById(eventId);
            Assert.That(updatedDbEvent.Name, Is.EqualTo(input.Name));
        }

        [Test]
        public async Task Edit_WithIdMismatch_ShoudReturnNotFound()
        {
            //Arrange
            var eventId = 1;
            var dbEvent = GetEventById(eventId);
            var input = new EventFormModel()
            {
                Id = 445,
                End = dbEvent.End,
                Name = $"{dbEvent.Name} UPDATED!!!",
                Place = dbEvent.Place,
                Start = dbEvent.Start,
            };
            var request = new RestRequest($"/Event/Edit/{eventId}", Method.Post);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("Id", input.Id);
            request.AddParameter("Place", input.Place);
            request.AddParameter("Start", input.Start.ToString("MM/dd/yyyy hh:mm:ss tt"));
            request.AddParameter("End", input.End.ToString("MM/dd/yyyy hh:mm:ss tt"));

            //Act
            var response = await _client.ExecuteAsync(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Edit_PostRequest_ShouldReturnBackTheSameViewIfModelErrorsArePresent()
        {
            //Arrange
            var eventId = 1;
            var dbEvent = GetEventById(eventId);

            var input = new EventFormModel()
            {
                Id = dbEvent.Id,
                Place = dbEvent.Place,
                
            };
            var request = new RestRequest($"/Event/Edit/{eventId}", Method.Post);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("Id", input.Id);
            request.AddParameter("Place", input.Place);
       
            //Act
            var response = await _client.ExecuteAsync(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Delete_WithValidId_ShoudRedirectToAllItems()
        {
            //Arrange
            var input = new EventFormModel()
            {
                Name = "Event For Delete",
                Place = "SoftUni",
                Start = new DateTime(2024, 12, 12, 12, 0, 0),
                End = new DateTime(2024, 12, 12, 16, 0, 0)
            };

            var request = new RestRequest("/Events/Add", Method.Post);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            //Add form data to the request
            request.AddParameter("Name", input.Name);
            request.AddParameter("Place", input.Place);
            request.AddParameter("Start", input.Start.ToString("MM/dd/yyyy hh:mm:ss tt"));
            request.AddParameter("End", input.End.ToString("MM/dd/yyyy hh:mm:ss tt"));

            await _client.ExecuteAsync(request);

            var eventInDb = GetEventByName(input.Name);
            var eventIdToDelete = eventInDb.Id;

            var deleteRequest = new RestRequest($"/Event/Delete/{eventIdToDelete}", Method.Post);

            //Act
            var response = await _client.ExecuteAsync(deleteRequest);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));


        }

        private Event GetEventByName(string name)
        {
            var options = new DbContextOptionsBuilder<EventmiContext>().UseSqlServer("Server=.;Database=Eventmi;Trusted_Connection=True;MultipleActiveResultSets=true").Options;
            using var context = new EventmiContext(options);

            return context.Events.FirstOrDefault(x => x.Name == name);
        }

        [Test]
        public async Task Delete_WithNoId_ShoulReturnNotFound()
        {
            //Arrange
            var request = new RestRequest("/Event/Delete/", Method.Post);
            //Act
            var response = await _client.ExecuteAsync(request);
            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }





    }
}