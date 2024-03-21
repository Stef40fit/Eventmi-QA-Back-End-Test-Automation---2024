//using Eventmi.Core.Models.Event;
//using Eventmi.Infrastructure.Data.Contexts;
//using Eventmi.Infrastructure.Models;
//using Microsoft.EntityFrameworkCore;
//using RestSharp;


//namespace Eventmi.Tests

//{
//    public class Tests
//    {
//        private  RestClient _client;
//        private const string baseUrl = "https://localhost:7236";

//        [SetUp]
//        public void Setup()
//        {
//            _client = new RestClient(baseUrl);
//        }

//        [Test]
//        public void GetAllEvents_ReturnsSuccessStatusCode()
//        {
//           //Arrange
//           var request = new RestRequest("/Event/All",Method.Get);

//            //Act
//            var response = _client.Execute(request);

//            //Assert
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
//            Console.WriteLine(response.StatusCode);
//        }

//        [Test]
//        public void Add_GetRequest_ReturnsAddView()
//        {
//            //Arrange
//            var request = new RestRequest("/Event/Add", Method.Get);

//            //Act
//            var response = _client.Execute(request);

//            //Assert
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
//            Console.WriteLine(response.StatusCode);
//        }



//        [Test]
//        public void Add_PostRequest_AddsNewRequestAndRedirect()
//        {
//            //Arrange
//            var input = new EventFormModel()
//            {
//                Name = "somName",
//                Place = "SoftUni",
//                Start = new DateTime(2024, 12, 12, 12, 0, 0),
//                End = new DateTime(2024, 12, 12, 18, 0, 0)

//            };
//            var request = new RestRequest("/Event/Add", Method.Post);
//            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
//            request.AddParameter("Name", input.Name);
//            request.AddParameter("Place", input.Place);
//            request.AddParameter("Start", input.Start.ToString("yyyy-MM-ddTHH:mm:ss"));
//            request.AddParameter("End", input.End.ToString("yyyy-MM-ddTHH:mm:ss"));
//            //Act
//            var response = _client.Execute(request);

//            //Assert
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
//            Assert.IsTrue(ChekIfEventExist(input.Name));
//        }

//        [Test]
//        public void GetEventDetails_ReturnsSuccessAndExpectContent()
//        {
//            //Arrange
//            var eventId = 1;
//            var request = new RestRequest($"/Event/Details/{eventId} ", Method.Get);

//            //Act
//            var response = _client.Execute(request);

//            //Assert
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
//            Console.WriteLine(response.StatusCode);
//        }

//        [Test]
//        public void EditAction_ReturnsViewFromValidId()
//        {
//            //Arrange
//            var eventId = 1;
//            var request = new RestRequest($"/Event/Edit/{eventId} ", Method.Get);

//            //Act
//            var response = _client.Execute(request);

//            //Assert
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
//            Console.WriteLine(response.StatusCode);
//        }

//        [Test]
//        public async Task Edit_PostRequest_ShouldEditAnEvent()
//        {
//            //Arrange
//            var eventId = 6;
//            var dbEvent = GetEventById(eventId);
//            var input = new EventFormModel()

//            {
//                Id = dbEvent.Id,
//                End = dbEvent.End,
//                Name = $"{dbEvent.Name} UPDATED",
//                Place = dbEvent.Place,
//                Start = dbEvent.Start,


//            };           
//            var request = new RestRequest($"/Event/Edit{dbEvent.Id}", Method.Post);
//            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");


//            request.AddParameter("Id", input.Id);
//            request.AddParameter("Name", input.Name);
//            request.AddParameter("Place", input.Place);
//            request.AddParameter("Start", input.Start.ToString("MM/dd/yyyy hh:mm tt"));
//            request.AddParameter("End", input.End.ToString("MM/dd/yyyy hh:mm tt"));



//            //Act
//            var response = await _client.ExecuteAsync(request);



//            //Assert
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
//            var updatedDbEvent = GetEventById(eventId);
//            Assert.That(updatedDbEvent.Name, Is.EqualTo(input.Name));




//        }

//        [Test]
//        public async Task EditIdMisematsh_ShouldReturnNotFound()
//        {
//            //Arrange
//            var eventId = 1;
//            var dbEvent = GetEventById(eventId);
//            var input = new EventFormModel()
//            {
//                Id = 444,
//                End = dbEvent.End,
//                Name = $"{dbEvent.Name} UPDATED",
//                Place = dbEvent.Place,
//                Start = dbEvent.Start
//            };
//            var request = new RestRequest($"/Event/Edit{dbEvent.Id}", Method.Post);
//            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
//            request.AddParameter("Id", input.Id);
//            request.AddParameter("Name", input.Name);
//            request.AddParameter("Place", input.Place);
//            request.AddParameter("Start", input.Start.ToString("yyyy-MM-ddTHH:mm:ss"));
//            request.AddParameter("End", input.End.ToString("yyyy-MM-ddTHH:mm:ss"));
//            //Act
//            var response = await _client.ExecuteAsync(request);
//            //Assert
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

//        }

//        [Test]
//        public async Task Delete_PostRequest_RedirectToAllEvent()
//        {
//            // Arrange
//            var input = new EventFormModel()
//            {
//                Name = "Name to Deleting",
//                Place = "SoftUni",
//                Start = new DateTime(2024, 12, 12, 12, 0, 0),
//                End = new DateTime(2024, 12, 12, 18, 0, 0)

//            };
//            var request = new RestRequest("/Event/Add", Method.Post);
//            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
//            request.AddParameter("Name", input.Name);
//            request.AddParameter("Place", input.Place);
//            request.AddParameter("Start", input.Start.ToString("yyyy-MM-ddTHH:mm:ss"));
//            request.AddParameter("End", input.End.ToString("yyyy-MM-ddTHH:mm:ss"));

//            await _client.ExecuteAsync(request);
//            var eventInDb = GetEventById(input.Id);
//            var eventIdToDelete = eventInDb.Id;
//            var deleteRequest = new RestRequest($"/Event/Delete{eventIdToDelete}",Method.Delete);

//            // Act
//            var response = await _client.ExecuteAsync(deleteRequest);
//            // Assert
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

//        }
//        [Test]
//        public async Task Delete_PostRequest_RedirectToAllEventChatGPT()
//        {
//            // Arrange
//            var input = new EventFormModel()
//            {
//                Name = "Name to Deleting",
//                Place = "SoftUni",
//                Start = new DateTime(2024, 12, 12, 12, 0, 0),
//                End = new DateTime(2024, 12, 12, 18, 0, 0)
//            };

//            // Add the event to the database
//            var eventInDb = GetEventById(input.Id);

//            var eventIdToDelete = eventInDb.Id;
//            var deleteRequest = new RestRequest($"/Event/Delete/{eventIdToDelete}", Method.Delete);

//            // Act
//            var response = await _client.ExecuteAsync(deleteRequest);

//            // Assert
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
//            // Add additional assertions if needed to ensure proper deletion


//        }

//        private Event GetEventById(int id)
//        {
//            var options = new DbContextOptionsBuilder<EventmiContext>().UseSqlServer("Server=.;Database=Eventmi;Trusted_Connection=True;MultipleActiveResultSets=true").Options;
//            using var context = new EventmiContext(options);

//                return context.Events.FirstOrDefault(e => e.Id == id);

//        }




//        private bool ChekIfEventExist(string name)
//        {
//            var options = new DbContextOptionsBuilder<EventmiContext>().UseSqlServer("Server=.;Database=Eventmi;Trusted_Connection=True;MultipleActiveResultSets=true").Options;
//            using (var context = new EventmiContext(options))
//            {
//                return context.Events.Any(e => e.Name == name);
//            }
//        }

//    }
//}

using Eventmi.Core.Models.Event;
using Eventmi.Infrastructure.Data.Contexts;
using Eventmi.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using RestSharp;

namespace Eventmi.Tests
{
    public class Tests
    {
        private RestClient _client;
        private readonly string _baseUrl = "https://localhost:7236";

        [SetUp]
        public void Setup()
        {
            _client = new RestClient(_baseUrl);
        }

        [Test]
        public async Task GetAllEvents_ReturnsSuccessStatusCode()
        {
            // Arrange
            var request = new RestRequest("/Event/All", Method.Get);

            // Act
            var respone = await _client.ExecuteAsync(request);

            // Assert
            Assert.That(respone.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Add_GetRequest_ReturnsAddView()
        {
            // Arrange
            var request = new RestRequest("/Event/Add", Method.Get);

            // Act
            var response = await _client.ExecuteAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Add_PostRequest_AddsNewEventAndRedirects()
        {
            // Arrange
            var input = new EventFormModel()
            {
                Name = "Soft Uni Conf",
                Place = "Soft Uni",
                Start = new DateTime(2024, 12, 12, 12, 0, 0),
                End = new DateTime(2024, 12, 12, 16, 0, 0)
            };

            var request = new RestRequest("/Event/Add", Method.Post);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            request.AddParameter("Name", input.Name);
            request.AddParameter("Place", input.Place);
            request.AddParameter("Start", input.Start.ToString("yyyy-MM-ddTHH:mm:ss"));
            request.AddParameter("End", input.End.ToString("yyyy-MM-ddTHH:mm:ss"));

            // Act
            var response = await _client.ExecuteAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.True(CheckIfEventExists(input.Name), "Event was not added to the database");
        }

        [Test]
        public async Task Details_GetRequest_ShouldReturnDetailedView()
        {
            // Arrange
            var eventId = 1;
            var request = new RestRequest($"/Event/Details/{eventId}", Method.Get);

            // Act
            var response = await _client.ExecuteAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Details_GetRequest_ShouldReturnNotFoundIfNoIdIsGiven()
        {
            // Arrange
            var request = new RestRequest($"/Event/Details/", Method.Get);

            // Act
            var response = await _client.ExecuteAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Edit_GetRequest_ShouldReturnEditView()
        {
            // Arrange
            var eventId = 1;
            var request = new RestRequest($"/Event/Edit/{eventId}", Method.Get);

            // Act
            var response = await _client.ExecuteAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Edit_GetRequest_ShouldReturnNotFoundIfNoIdIsGiven()
        {
            // Arrange
            var request = new RestRequest($"/Event/Edit/", Method.Get);

            // Act
            var response = await _client.ExecuteAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Edit_PostRequest_ShouldEditAnEvent()
        {
            // Arrange
            var eventId = 1;
            var dbEvent = GetEventById(eventId);

            var input = new EventFormModel()
            {
                Id = dbEvent.Id,
                End = dbEvent.End,
                Name = dbEvent.Name,
                Place = dbEvent.Place,
                Start = dbEvent.Start,
            };

            var request = new RestRequest($"/Event/Edit/{dbEvent.Id}", Method.Post);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            request.AddParameter("Id", input.Id);
            request.AddParameter("Name", input.Name);
            request.AddParameter("Place", input.Place);
            request.AddParameter("Start", input.Start.ToString("yyyy-MM-ddTHH:mm:ss"));
            request.AddParameter("End", input.End.ToString("yyyy-MM-ddTHH:mm:ss"));

            // Act
            var response = await _client.ExecuteAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var updatedDbEvent = GetEventById(eventId);
            Assert.That(updatedDbEvent.Name, Is.EqualTo(input.Name));
        }

        [Test]
        public async Task Edit_PostRequest_ShouldReturnBackTheSameViewIfModelErrorsArePresent()
        {
            // Arrange
            var eventId = 1;
            var dbEvent = GetEventById(eventId);

            var input = new EventFormModel()
            {
                Id = dbEvent.Id,
                Place = dbEvent.Place,
            };

            var request = new RestRequest($"/Event/Edit/{dbEvent.Id}", Method.Post);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            request.AddParameter("Id", input.Id);
            request.AddParameter("Name", input.Name);

            // Act
            var response = await _client.ExecuteAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Edit_WtihIdMismatch_ShouldReturnNotFound()
        {
            // Arrange
            var eventId = 1;
            var dbEvent = GetEventById(eventId);

            var input = new EventFormModel()
            {
                Id = 445,
                End = dbEvent.End,
                Name = dbEvent.Name,
                Place = dbEvent.Place,
                Start = dbEvent.Start,
            };

            var request = new RestRequest($"/Event/Edit/{eventId}", Method.Post);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            request.AddParameter("Id", input.Id);
            request.AddParameter("Name", input.Name);
            request.AddParameter("Place", input.Place);
            request.AddParameter("Start", input.Start.ToString("yyyy-MM-ddTHH:mm:ss"));
            request.AddParameter("End", input.End.ToString("yyyy-MM-ddTHH:mm:ss"));

            // Act
            var response = await _client.ExecuteAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Delete_WithValidId_ShouldRedirectToAllItems()
        {
            // Arrange
            var input = new EventFormModel()
            {
                Name = "Event For Deleting",
                Place = "Soft Uni",
                Start = new DateTime(2024, 12, 12, 12, 0, 0),
                End = new DateTime(2024, 12, 12, 16, 0, 0)
            };

            var request = new RestRequest("/Event/Add", Method.Post);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            request.AddParameter("Name", input.Name);
            request.AddParameter("Place", input.Place);
            request.AddParameter("Start", input.Start.ToString("yyyy-MM-ddTHH:mm:ss"));
            request.AddParameter("End", input.End.ToString("yyyy-MM-ddTHH:mm:ss"));

            await _client.ExecuteAsync(request);

            var eventInDb = GetEventByName(input.Name);
            var eventIdToDelete = eventInDb.Id;

            var deleteRequest = new RestRequest($"/Event/Delete/{eventIdToDelete}", Method.Post);

            // Act
            var response = await _client.ExecuteAsync(deleteRequest);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

       

        private bool CheckIfEventExists(string name)
        {
            var options = new DbContextOptionsBuilder<EventmiContext>().UseSqlServer("Server=.;Database=Eventmi;Trusted_Connection=True;MultipleActiveResultSets=true").Options;

            using var context = new EventmiContext(options);

            return context.Events.Any(x => x.Name == name);
        }

        private Event GetEventByName(string name)
        {
            var options = new DbContextOptionsBuilder<EventmiContext>().UseSqlServer("Server=.;Database=Eventmi;Trusted_Connection=True;MultipleActiveResultSets=true").Options;

            using var context = new EventmiContext(options);

            return context.Events.FirstOrDefault(x => x.Name == name);
        }

        private Event GetEventById(int id)
        {
            var options = new DbContextOptionsBuilder<EventmiContext>().UseSqlServer("Server=.;Database=Eventmi;Trusted_Connection=True;MultipleActiveResultSets=true").Options;

            using var context = new EventmiContext(options);

            return context.Events.FirstOrDefault(x => x.Id == id);
        }
    }
}