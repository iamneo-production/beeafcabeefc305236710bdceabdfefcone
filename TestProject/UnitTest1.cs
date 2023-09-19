using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using MusicianBookingSystem.Controllers;
using MusicianBookingSystem.Data;
using MusicianBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NUnit.Framework;


namespace MusicianBookingSystem.Tests
{
    [TestFixture]
    public class MusicianBookingSystemTest
    {
        private DbContextOptions<SlotDbContext> _dbContextOptions;

        [SetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<SlotDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var dbContext = new SlotDbContext(_dbContextOptions))
            {
                // Add test data to the in-memory database
                var ride = new Slot
                {
                    SlotID = 1,
                    Time = DateTime.Parse("2023-08-30"),
                    //Name = DateTime.Parse("2023-08-30"),
					Duration = 60,
                    Capacity = 10,
                };

                dbContext.Slot.Add(ride);
                dbContext.SaveChanges();
            }
        }

        [TearDown]
        public void TearDown()
        {
            using (var dbContext = new SlotDbContext(_dbContextOptions))
            {
                // Clear the in-memory database after each test
                dbContext.Database.EnsureDeleted();
            }
        }


        [Test]
        public void CreateNewSlot_ValidDetails_JoinsSuccessfully_BookingTable()
        {
            using (var dbContext = new SlotDbContext(_dbContextOptions))
            {
                // Arrange
                var controllerType = typeof(SlotController);
                var controller = Activator.CreateInstance(controllerType, dbContext);

                // Specify the method signature (parameter types)
                MethodInfo method = controllerType.GetMethod("Create", new[] { typeof(int), typeof(DateTime), typeof(string), typeof(int) });

                //var student = new Slot
                //{
                //    Name = "John Doe",
                //    Email = "johndoe@example.com",
                //};

                // Act
                var result = method.Invoke(controller, new object[] { 1, DateTime.Parse("2023-08-30"), "60", 5 });

                // Assert
                Assert.IsNotNull(result);

                var ride = dbContext.Booking.Include(r => r.Slots).FirstOrDefault(r => r.SlotID == 1);
                //Assert.IsNotNull(ride);
                Console.WriteLine(ride.Slots.Count);
                Assert.AreEqual(1, ride.Slots.Count);
            }
        }

        [Test]
        public void CreateNewSlot_ValidDetails_JoinsSuccessfully_SlotTable()
        {
            using (var dbContext = new SlotDbContext(_dbContextOptions))
            {
                // Arrange
                var controllerType = typeof(SlotController);
                var controller = Activator.CreateInstance(controllerType, dbContext);

                // Specify the method signature (parameter types)
                MethodInfo method = controllerType.GetMethod("Create", new[] { typeof(int), typeof(DateTime), typeof(TimeSpan), typeof(string) });

                //var student = new Student
                //{
                //    Name = "John Doe",
                //    Email = "johndoe@example.com",
                //};

                // Act
                var result = method.Invoke(controller, new object[] { 1, DateTime.Parse("2023-08-30"), "60", 3 });

                var ride = dbContext.Slots.FirstOrDefault(r => r.SlotID == 1);
                Assert.IsNotNull(ride);
                Assert.AreEqual(1, ride.SpaceID);
            }
        }

        // [Test]
        // public void CreateNewSlot_ValidDetails_JoinsSuccessfully_SlotTable1()
        // {
        //     using (var dbContext = new SlotDbContext(_dbContextOptions))
        //     {
        //         // Arrange
        //         var controllerType = typeof(SlotController);
        //         var controller = Activator.CreateInstance(controllerType, dbContext);

        //         // Specify the method signature (parameter types)
        //         MethodInfo method = controllerType.GetMethod("Create", new[] { typeof(int), typeof(DateTime), typeof(TimeSpan), typeof(string) });

        //         //var student = new Student
        //         //{
        //         //    Name = "John Doe",
        //         //    Email = "johndoe@example.com",
        //         //};

        //         // Act
        //         var result = method.Invoke(controller, new object[] { 1, DateTime.Parse("2023-08-30"), TimeSpan.Parse("10:00"), "123" });

        //         var ride = dbContext.Slots.FirstOrDefault(r => r.SpaceID == 1);
        //         Assert.IsNotNull(ride);
        //         Assert.AreEqual(DateTime.Parse("2023-08-30"), ride.EventDate);
        //         Assert.AreEqual(TimeSpan.Parse("10:00"), ride.TimeSlot);
        //     }
        // }

        //[Test]
        //public void ClassEnrollmentForm_Returns_EnrollmentConfirmation_Action()
        //{
        //    using (var dbContext = new SlotDbContext(_dbContextOptions))
        //    {
        //        // Arrange
        //        var controllerType = typeof(SlotController);
        //        var controller = Activator.CreateInstance(controllerType, dbContext);

        //        // Specify the method signature (parameter types)
        //        MethodInfo method = controllerType.GetMethod("Create", new[] { typeof(int), typeof(DateTime), typeof(TimeSpan), typeof(string) });

        //        //var student = new Student
        //        //{
        //        //    Name = "John Doe",
        //        //    Email = "johndoe@example.com",
        //        //};

        //        // Act
        //        var result = method.Invoke(controller, new object[] { 1, DateTime.Parse("2023-08-30"), TimeSpan.Parse("10:00"), "123" });

        //        //var ride = dbContext.Students.FirstOrDefault(r => r.StudentID == 1);
        //        Console.WriteLine(result);
        //        Assert.IsNotNull(result);
        //        //Assert.AreEqual("EnrollmentConfirmation", result.ActionName);

        //        //if (result is IActionResult redirectToAction)
        //        //{
        //        //    Console.WriteLine("dai");
        //        //    Assert.AreEqual("EnrollmentConfirmation", redirectToAction.ActionName);
        //        //}
        //    }
        //}

        //[Test]
        //public void EnrollmentConfirmation_InvalidID_Returns_NotFound()
        //{
        //    using (var dbContext = new ApplicationDbContext(_dbContextOptions))
        //    {
        //        // Arrange
        //        var controllerType = typeof(SlotController);
        //        var controller = Activator.CreateInstance(controllerType, dbContext);

        //        // Specify the method signature (parameter types)
        //        MethodInfo method = controllerType.GetMethod("ClassEnrollmentForm", new[] { typeof(int), typeof(string), typeof(string) });

        //        MethodInfo method1 = controllerType.GetMethod("EnrollmentConfirmation", new[] { typeof(int) });

        //        var student = new Student
        //        {
        //            Name = "John Doe",
        //            Email = "johndoe@example.com",
        //        };

        //        // Act
        //        var result = method.Invoke(controller, new object[] { 1, "John Doe", "johndoe@example.com" });
        //        var result1 = method1.Invoke(controller, new object[] { 2 }) as NotFoundResult;

        //        var ride = dbContext.Students.FirstOrDefault(r => r.StudentID == 1);

        //        Assert.IsNotNull(result1);
        //        //Assert.AreEqual("EnrollmentConfirmation", result1.ActionName);
        //    }
        //}


        //[Test]
        //public void EnrollmentConfirmation_validID_Returns_View()
        //{
        //    using (var dbContext = new ApplicationDbContext(_dbContextOptions))
        //    {
        //        // Arrange
        //        var controllerType = typeof(SlotController);
        //        var controller = Activator.CreateInstance(controllerType, dbContext);

        //        // Specify the method signature (parameter types)
        //        MethodInfo method = controllerType.GetMethod("ClassEnrollmentForm", new[] { typeof(int), typeof(string), typeof(string) });

        //        MethodInfo method1 = controllerType.GetMethod("EnrollmentConfirmation", new[] { typeof(int) });

        //        var student = new Student
        //        {
        //            Name = "John Doe",
        //            Email = "johndoe@example.com",
        //        };

        //        // Act
        //        var result = method.Invoke(controller, new object[] { 1, "John Doe", "johndoe@example.com" });
        //        var result1 = method1.Invoke(controller, new object[] { 1 }) as ViewResult;

        //        var ride = dbContext.Students.FirstOrDefault(r => r.StudentID == 1);

        //        Assert.IsNotNull(result1);
        //    }
        //}


        [Test]
        public void AvailableIndex_Method_Exists_SlotController()
        {
            using (var dbContext = new SlotDbContext(_dbContextOptions))
            {
                // Arrange
                var controllerType = typeof(SlotController);
                var controller = Activator.CreateInstance(controllerType, dbContext);

                // Specify the method signature (parameter types)
                MethodInfo method = controllerType.GetMethod("Index");
                Assert.IsNotNull(method);
            }
        }

        [Test]
        public void Index_Method_Exists_BookingController()
        {
            using (var dbContext = new SlotDbContext(_dbContextOptions))
            {
                // Arrange
                var controllerType = typeof(BookingController);
                var controller = Activator.CreateInstance(controllerType, dbContext);

                // Specify the method signature (parameter types)
                MethodInfo method = controllerType.GetMethod("Index");
                Assert.IsNotNull(method);
            }
        }

        [Test]
        public void Book_Method_Exists_BookingController_1_parameter()
        {
            using (var dbContext = new SlotDbContext(_dbContextOptions))
            {
                // Arrange
                var controllerType = typeof(BppkingController);
                var controller = Activator.CreateInstance(controllerType, dbContext);

                // Specify the method signature (parameter types)
                MethodInfo method = controllerType.GetMethod("Book", new[] { typeof(int) });
                Assert.IsNotNull(method);
            }
        }

        [Test]
        public void Book_Method_Exists_BookingController_2_parameter()
        {
            using (var dbContext = new SlotDbContext(_dbContextOptions))
            {
                // Arrange
                var controllerType = typeof(BookingController);
                var controller = Activator.CreateInstance(controllerType, dbContext);



                // Specify the method signature (parameter types)
                MethodInfo method = controllerType.GetMethod("Book", new[] { typeof(int), typeof(int) });
                Assert.IsNotNull(method);
            }
        }
        ////// [Test]
        ////// public void JoinRide_InvalidCommuter_ModelStateInvalid()
        ////// {
        //////     using (var dbContext = new RideSharingDbContext(_dbContextOptions))
        //////     {
        //////         // Arrange
        //////         var slotController = new SlotController(dbContext);
        //////         var commuter = new Commuter(); // Invalid commuter with missing required fields

        //////         // Act
        //////         slotController.ModelState.AddModelError("Name", "Name is required");
        //////         var result = slotController.JoinRide(1, commuter) as ViewResult;

        //////         // Assert
        //////         Assert.IsNotNull(result);
        //////         Assert.AreEqual("", result.ViewName); // Returns the same view
        //////         Assert.IsFalse(result.ViewData.ModelState.IsValid);
        //////         Assert.AreEqual(1, result.ViewData.ModelState.ErrorCount);
        //////         Assert.IsTrue(result.ViewData.ModelState.ContainsKey("Name"));
        //////     }
        ////// }

        // [Test]
        // public void Confirmation_InvaalidID_ReturnsNotFoundResult()
        // {
        //     using (var dbContext = new SlotDbContext(_dbContextOptions))
        //     {
        //         var controllerType = typeof(SlotController);
        //         var controller = Activator.CreateInstance(controllerType, dbContext);

        //         MethodInfo method1 = controllerType.GetMethod("Confirmation", new[] { typeof(int) });
        //         var result = method1.Invoke(controller, new object[] { 1 }) as NotFoundResult;
        //         Assert.IsNotNull(result);
        //     }
        // }

        // [Test]
        // public void Confirmation_ValidID_Returns_ViewResule_Slot()
        // {
        //     using (var dbContext = new SlotDbContext(_dbContextOptions))
        //     {
        //         // Arrange
        //         var controllerType = typeof(SlotController);
        //         var controller = Activator.CreateInstance(controllerType, dbContext);

        //         MethodInfo method = controllerType.GetMethod("Create", new[] { typeof(int), typeof(DateTime), typeof(TimeSpan), typeof(string) });
        //         var result1 = method.Invoke(controller, new object[] { 1, DateTime.Parse("2023-08-30"), TimeSpan.Parse("10:00"), "123" });

        //         // Specify the method signature (parameter types)
        //         MethodInfo method1 = controllerType.GetMethod("Confirmation", new[] { typeof(int) });
        //         var result = method1.Invoke(controller, new object[] { 1 }) as ViewResult;
        //         Assert.IsNotNull(result);
        //     }
        // }

        //[Test]
        //public void ClassEnrollmentForm_ThrowsException_fullyBooked_Class()
        //{
        //    using (var dbContext = new SlotDbContext(_dbContextOptions))
        //    {
        //        // Arrange
        //        var controllerType = typeof(SlotController);
        //        var controller = Activator.CreateInstance(controllerType, dbContext);

        //        MethodInfo method = controllerType.GetMethod("Create", new[] { typeof(int), typeof(DateTime), typeof(TimeSpan), typeof(string) });

        //        // Simulate Slot for a fully booked class
        //        var result1 = method.Invoke(controller, new object[] { 1, DateTime.Parse("2023-08-30"), TimeSpan.Parse("10:00"), "123" });

        //        // Check the database state
        //        var ride = dbContext.Slots.FirstOrDefault(r => r.SpaceID == 1);
        //        Console.WriteLine(ride.EventDate);
        //        var res2=method.Invoke(controller, new object[] { 1, DateTime.Parse("2023-08-30"), TimeSpan.Parse("10:00"), "123" });
        //        Console.WriteLine("jkl"+res2);

        //        // Attempt to book the same class again, which should throw an exception
        //        var exception = Assert.Throws<TargetInvocationException>(() =>
        //        {
        //            method.Invoke(controller, new object[] { 1, DateTime.Parse("2023-08-30"), TimeSpan.Parse("10:00"), "123" });
        //        });

        //        // Check the exception message and type
        //        Console.WriteLine(exception.Message);
        //        Assert.IsInstanceOf<EventSlotException>(exception.InnerException);
        //    }
        //}



        //[Test]
        //public void ClassEnrollmentForm_ThrowsException_fullyBooked_Class_With_Message()
        //{
        //    using (var dbContext = new SlotDbContext(_dbContextOptions))
        //    {
        //        // Arrange
        //        var controllerType = typeof(SlotController);
        //        var controller = Activator.CreateInstance(controllerType, dbContext);

        //        MethodInfo method = controllerType.GetMethod("ClassEnrollmentForm", new[] { typeof(int), typeof(string), typeof(string) });

        //        // Act and Assert for the first scenario

        //        var result = method.Invoke(controller, new object[] { 1, "John Doe", "johndoe@example.com" }) as RedirectToActionResult;

        //        // Assert for the first scenario
        //        Assert.IsNotNull(result);

        //        // Act and Assert for the second scenario
        //        MethodInfo method1 = controllerType.GetMethod("ClassEnrollmentForm", new[] { typeof(int) });

        //        var exception = Assert.Throws<TargetInvocationException>(() =>
        //        {
        //            method1.Invoke(controller, new object[] { 1 });
        //        });

        //        // Assert that the inner exception is of type KathakClassSlotException
        //        Assert.IsInstanceOf<KathakClassSlotException>(exception.InnerException);
        //        Assert.AreEqual("Class is fully booked.", exception.InnerException.Message);
        //    }
        //}

        //////     [Test]
        ////// public void JoinRide_DestinationSameAsDeparture_ReturnsViewWithValidationError()
        ////// {
        //////     using (var dbContext = new RideSharingDbContext(_dbContextOptions))
        //////     {
        //////         // Arrange
        //////         var slotController = new SlotController(dbContext);
        //////         var commuter = new Commuter
        //////         {
        //////             Name = "John Doe",
        //////             Email = "johndoe@example.com",
        //////             Phone = "1234567890"
        //////         };

        //////         // Act
        //////         var ride = dbContext.Rides.FirstOrDefault(r => r.RideID == 1);
        //////         ride.Destination = ride.DepartureLocation; // Set the destination as the same as departure
        //////         dbContext.SaveChanges();

        //////         var result = slotController.JoinRide(1, commuter) as ViewResult;

        //////         // Assert
        //////         Assert.IsNotNull(result);
        //////         Assert.IsFalse(result.ViewData.ModelState.IsValid);
        //////         Assert.IsTrue(result.ViewData.ModelState.ContainsKey("Destination"));
        //////     }
        ////// }

        ////// [Test]
        ////// public void JoinRide_MaximumCapacityNotPositiveInteger_ReturnsViewWithValidationError()
        ////// {
        //////     using (var dbContext = new RideSharingDbContext(_dbContextOptions))
        //////     {
        //////         // Arrange
        //////         var slotController = new SlotController(dbContext);
        //////         var commuter = new Commuter
        //////         {
        //////             Name = "John Doe",
        //////             Email = "johndoe@example.com",
        //////             Phone = "1234567890"
        //////         };

        //////         // Act
        //////         var ride = dbContext.Rides.FirstOrDefault(r => r.RideID == 1);
        //////         ride.MaximumCapacity = -5; // Set a negative value for MaximumCapacity
        //////         dbContext.SaveChanges();

        //////         var result = slotController.JoinRide(1, commuter) as ViewResult;

        //////         // Assert
        //////         Assert.IsNotNull(result);
        //////         Assert.IsFalse(result.ViewData.ModelState.IsValid);
        //////         Assert.IsTrue(result.ViewData.ModelState.ContainsKey("MaximumCapacity"));
        //////     }
        ////// }


        [Test]
        public void Slot_ClassExists()
        {
            string assemblyName = "MusicianBookingSystem";
            string typeName = "MusicianBookingSystem.Models.Slot";
            Assembly assembly = Assembly.Load(assemblyName);
            Type rideType = assembly.GetType(typeName);
            Assert.IsNotNull(rideType);
            var ride = Activator.CreateInstance(rideType);
            Assert.IsNotNull(ride);
        }

        [Test]
        public void Booking_ClassExists()
        {
            string assemblyName = "MusicianBookingSystem";
            string typeName = "MusicianBookingSystem.Models.Booking";
            Assembly assembly = Assembly.Load(assemblyName);
            Type rideType = assembly.GetType(typeName);
            Assert.IsNotNull(rideType);
            var ride = Activator.CreateInstance(rideType);
            Assert.IsNotNull(ride);
        }
        [Test]
        public void SlotBookingException_ClassExists()
        {
            string assemblyName = "MusicianBookingSystem";
            string typeName = "MusicianBookingSystem.Models.SlotBookingException";
            Assembly assembly = Assembly.Load(assemblyName);
            Type rideType = assembly.GetType(typeName);
            Assert.IsNotNull(rideType);
        }



        [Test]
        public void SlotDbContextContainsDbSetBookingProperty()
        {
            // var context = new ApplicationDbContext();
            using (var dbContext = new SlotDbContext(_dbContextOptions))
            {
                var propertyInfo = dbContext.GetType().GetProperty("Bookings");

                Assert.IsNotNull(propertyInfo);
                Assert.AreEqual(typeof(DbSet<Booking>), propertyInfo.PropertyType);
            }
        }

        [Test]
        public void SlotDbContextContainsDbSetSlotsProperty()
        {
            // var context = new ApplicationDbContext();
            using (var dbContext = new SlotDbContext(_dbContextOptions))
            {
                var propertyInfo = dbContext.GetType().GetProperty("Slots");
                Assert.IsNotNull(propertyInfo);
                Assert.AreEqual(typeof(DbSet<Slot>), propertyInfo.PropertyType);
            }
        }


        [Test]
        public void Slot_Properties_SlotID_ReturnExpectedDataTypes_int()
        {
            string assemblyName = "MusicianBookingSystem";
            string typeName = "MusicianBookingSystem.Models.Slot";
            Assembly assembly = Assembly.Load(assemblyName);
            Type commuterType = assembly.GetType(typeName);
            PropertyInfo propertyInfo = commuterType.GetProperty("SlotID");
            Assert.IsNotNull(propertyInfo, "The property 'SpaceID' was not found on the Slot class.");
            Type propertyType = propertyInfo.PropertyType;
            Assert.AreEqual(typeof(int), propertyType, "The data type of 'SpaceID' property is not as expected (int).");
        }

        [Test]
        public void Slot_Properties_Name_ReturnExpectedDataTypes_string()
        {
            string assemblyName = "MusicianBookingSystem";
            string typeName = "MusicianBookingSystem.Models.Slot";
            Assembly assembly = Assembly.Load(assemblyName);
            Type commuterType = assembly.GetType(typeName);
            PropertyInfo propertyInfo = commuterType.GetProperty("Capacity");
            Assert.IsNotNull(propertyInfo, "The property 'Name' was not found on the Slot class.");
            Type propertyType = propertyInfo.PropertyType;
            Assert.AreEqual(typeof(int), propertyType, "The data type of 'Name' property is not as expected (string).");
        }
        [Test]
        public void Slot_Properties_Capacity_ReturnExpectedDataTypes_DateTime()
        {
            string assemblyName = "MusicianBookingSystem";
            string typeName = "MusicianBookingSystem.Models.Slot";
            Assembly assembly = Assembly.Load(assemblyName);
            Type commuterType = assembly.GetType(typeName);
            PropertyInfo propertyInfo = commuterType.GetProperty("Duration");
            Assert.IsNotNull(propertyInfo, "The property 'Capacity' was not found on the Booking class.");
            Type propertyType = propertyInfo.PropertyType;
            Assert.AreEqual(typeof(string), propertyType, "The data type of 'Capacity' property is not as expected (int).");
        }
        [Test]
        public void Booking_Properties_BookingID_ReturnExpectedDataTypes_Bool()
        {
            string assemblyName = "MusicianBookingSystem";
            string typeName = "MusicianBookingSystem.Models.Booking";
            Assembly assembly = Assembly.Load(assemblyName);
            Type commuterType = assembly.GetType(typeName);
            PropertyInfo propertyInfo = commuterType.GetProperty("BooingID");
            Assert.IsNotNull(propertyInfo, "The property 'Availability' was not found on the Slot class.");
            Type propertyType = propertyInfo.PropertyType;
            Assert.AreEqual(typeof(int), propertyType, "The data type of 'Availability' property is not as expected (Student).");
        }
        [Test]
        public void Booking_Properties_SlotID_ReturnExpectedDataTypes_ICollection_Slot()
        {
            string assemblyName = "MusicianBookingSystem";
            string typeName = "MusicianBookingSystem.Models.Booking";
            Assembly assembly = Assembly.Load(assemblyName);
            Type commuterType = assembly.GetType(typeName);
            PropertyInfo propertyInfo = commuterType.GetProperty("SlotID");
            Assert.IsNotNull(propertyInfo, "The property 'Availability' was not found on the Slot class.");
            Type propertyType = propertyInfo.PropertyType;
            Assert.AreEqual(typeof(int), propertyType, "The data type of 'Slots' property is as expected (ICollection_Slot).");
        }

        [Test]
        public void Booking_Properties_UserID_ReturnExpectedDataTypes_int()
        {
            string assemblyName = "MusicianBookingSystem";
            string typeName = "MusicianBookingSystem.Models.Booking";
            Assembly assembly = Assembly.Load(assemblyName);
            Type commuterType = assembly.GetType(typeName);
            PropertyInfo propertyInfo = commuterType.GetProperty("UserID");
            Assert.IsNotNull(propertyInfo, "The property 'SlotID' was not found on the Commuter class.");
            Type propertyType = propertyInfo.PropertyType;
            Assert.AreEqual(typeof(int), propertyType, "The data type of 'SlotID' property is not as expected (int).");
        }
        // [Test]
        // public void Slot_Properties_TimeSlot_ReturnExpectedDataTypes_TimeSpan()
        // {
        //     string assemblyName = "MusicianBookingSystem";
        //     string typeName = "MusicianBookingSystem.Models.Slot";
        //     Assembly assembly = Assembly.Load(assemblyName);
        //     Type commuterType = assembly.GetType(typeName);
        //     PropertyInfo propertyInfo = commuterType.GetProperty("TimeSlot");
        //     Assert.IsNotNull(propertyInfo, "The property 'Name' was not found on the Commuter class.");
        //     Type propertyType = propertyInfo.PropertyType;
        //     Assert.AreEqual(typeof(TimeSpan), propertyType, "The data type of 'Name' property is not as expected (string).");
        // }
        // [Test]
        // public void Slot_Properties_EventDate_ReturnExpectedDataTypes_DateTime()
        // {
        //     string assemblyName = "MusicianBookingSystem";
        //     string typeName = "MusicianBookingSystem.Models.Slot";
        //     Assembly assembly = Assembly.Load(assemblyName);
        //     Type commuterType = assembly.GetType(typeName);
        //     PropertyInfo propertyInfo = commuterType.GetProperty("EventDate");
        //     Assert.IsNotNull(propertyInfo, "The property 'EventDate' was not found on the Commuter class.");
        //     Type propertyType = propertyInfo.PropertyType;
        //     Assert.AreEqual(typeof(DateTime), propertyType, "The data type of 'EventDate' property is not as expected (EventDate).");
        // }

        

        [Test]
        public void Test_SlotDisplayViewFile_Exists_Slot()
        {
            string indexPath = Path.Combine(@"/home/coder/project/workspace/MusicianBookingSystem\Views\Slot\", "Index.cshtml");
            bool indexViewExists = File.Exists(indexPath);

            Assert.IsTrue(indexViewExists, "Create.cshtml view file does not exist.");
        }
        [Test]
        public void Test_BookingSummaryViewFile_Exists_Slot()
        {
            string indexPath = Path.Combine(@"/home/coder/project/workspace/MusicianBookingSystem\Views\Booking\", "Index.cshtml");
            bool indexViewExists = File.Exists(indexPath);

            Assert.IsTrue(indexViewExists, "Create.cshtml view file does not exist.");
        }

        //[Test]
        //public void Student_Properties_ClassID_ReturnExpectedDataTypes_int()
        //{
        //    string assemblyName = "MusicianBookingSystem";
        //    string typeName = "MusicianBookingSystem.Models.Student";
        //    Assembly assembly = Assembly.Load(assemblyName);
        //    Type commuterType = assembly.GetType(typeName);
        //    PropertyInfo propertyInfo = commuterType.GetProperty("ClassID");
        //    Assert.IsNotNull(propertyInfo, "The property 'ClassID' was not found on the Student class.");
        //    Type propertyType = propertyInfo.PropertyType;
        //    Assert.AreEqual(typeof(int), propertyType, "The data type of 'ClassID' property is not as expected (int).");
        //}

        //[Test]
        //public void Student_Properties_Class_ReturnExpectedDataTypes_Class()
        //{
        //    string assemblyName = "MusicianBookingSystem";
        //    string typeName = "MusicianBookingSystem.Models.Student";
        //    Assembly assembly = Assembly.Load(assemblyName);
        //    Type commuterType = assembly.GetType(typeName);
        //    PropertyInfo propertyInfo = commuterType.GetProperty("Class");
        //    Assert.IsNotNull(propertyInfo, "The property 'Class' was not found on the Student class.");
        //    Type propertyType = propertyInfo.PropertyType;
        //    Assert.AreEqual(typeof(Class), propertyType, "The data type of 'Class' property is not as expected (Class).");
        //}

        ////[Test]
        ////public void Ride_Properties_MaximumCapacity_ReturnExpectedDataTypes()
        ////{
        ////    // Arrange
        ////    Ride ride = new Ride();

        ////    Assert.That(ride.MaximumCapacity, Is.TypeOf<int>());
        ////}

        ////[Test]
        ////public void Commuter_Ride_ReturnsExpectedValue()
        ////{
        ////    // Arrange
        ////    Ride expectedRide = new Ride { RideID = 2 };
        ////    Commuter commuter = new Commuter { Ride = expectedRide };

        ////    // Assert
        ////    Assert.AreEqual(expectedRide, commuter.Ride);
        ////}

        ////[Test]
        ////public void Commuter_Properties_CommuterID_ReturnExpectedValues()
        ////{
        ////    // Arrange
        ////    int expectedCommuterID = 1;

        ////    // Act
        ////    Commuter commuter = new Commuter
        ////    {
        ////        CommuterID = expectedCommuterID,
        ////    };

        ////    // Assert
        ////    Assert.AreEqual(expectedCommuterID, commuter.CommuterID);
        ////}

        ////[Test]
        ////public void Commuter_Properties_Name_ReturnExpectedValues()
        ////{

        ////    string expectedName = "John Doe";

        ////    // Act
        ////    Commuter commuter = new Commuter
        ////    {
        ////        Name = expectedName,
        ////    };

        ////    // Assert
        ////    Assert.AreEqual(expectedName, commuter.Name);
        ////}

        ////[Test]
        ////public void Commuter_Properties_Email_ReturnExpectedValues()
        ////{
        ////    string expectedEmail = "john@example.com";

        ////    // Act
        ////    Commuter commuter = new Commuter
        ////    {
        ////        Email = expectedEmail,
        ////    };

        ////    Assert.AreEqual(expectedEmail, commuter.Email);
        ////}

        ////[Test]
        ////public void Commuter_Properties_Phone_ReturnExpectedValues()
        ////{

        ////    string expectedPhone = "1234567890";

        ////    // Act
        ////    Commuter commuter = new Commuter
        ////    {
        ////        Phone = expectedPhone,
        ////    };

        ////    Assert.AreEqual(expectedPhone, commuter.Phone);
        ////}

        ////[Test]
        ////public void Commuter_Properties_RideID_ReturnExpectedValues()
        ////{
        ////    int expectedRideID = 2;

        ////    // Act
        ////    Commuter commuter = new Commuter
        ////    {
        ////        RideID = expectedRideID
        ////    };
        ////    Assert.AreEqual(expectedRideID, commuter.RideID);
        ////}

        ////[Test]
        ////public void test_case12()
        ////{
        ////    using (var dbContext = new RideSharingDbContext(_dbContextOptions))
        ////    {
        ////        // Arrange
        ////        var slotController = new SlotController(dbContext);
        ////        var commuter = new Commuter
        ////        {
        ////            Name = "John Doe",
        ////            Email = "johndoe@example.com",
        ////            Phone = "1234567890"
        ////        };

        ////        // Act
        ////        var ride = dbContext.Rides.FirstOrDefault(r => r.RideID == 1);
        ////        ride.Destination = ride.DepartureLocation; // Set the destination as the same as departure
        ////        dbContext.SaveChanges();

        ////        var result = slotController.JoinRide(1, commuter) as ViewResult;

        ////        // Assert
        ////        Assert.IsNotNull(result);
        ////        Assert.IsFalse(result.ViewData.ModelState.IsValid);
        ////        Assert.IsTrue(result.ViewData.ModelState.ContainsKey("Destination"));
        ////    }
        ////}

        ////[Test]
        ////public void test_case13()
        ////{
        ////    using (var dbContext = new RideSharingDbContext(_dbContextOptions))
        ////    {
        ////        // Arrange
        ////        var slotController = new SlotController(dbContext);
        ////        var commuter = new Commuter
        ////        {
        ////            Name = "John Doe",
        ////            Email = "johndoe@example.com",
        ////            Phone = "1234567890"
        ////        };

        ////        // Act
        ////        var ride = dbContext.Rides.FirstOrDefault(r => r.RideID == 1);
        ////        ride.MaximumCapacity = -5; // Set a negative value for MaximumCapacity
        ////        dbContext.SaveChanges();

        ////        var result = slotController.JoinRide(1, commuter) as ViewResult;

        ////        // Assert
        ////        Assert.IsNotNull(result);
        ////        Assert.IsFalse(result.ViewData.ModelState.IsValid);
        ////        Assert.IsTrue(result.ViewData.ModelState.ContainsKey("MaximumCapacity"));
        ////    }
        ////}
    }

}