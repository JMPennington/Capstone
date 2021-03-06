﻿using com.WanderingTurtle.BusinessLogic;
using com.WanderingTurtle.Common;
using com.WanderingTurtle.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace com.WanderingTurtle.Tests
{///Created- Tony Noel, 2015/3/27, Updated: 2015/4/7, Updated: 2015/4/10, Updated 2015/05/01
    /// <summary>
    /// Booking Manager Tests- Creates a fake booking record by using a dummy ItemListing from the database
    /// (ItemListing record 100). Performs actions on this booking based upon manager methods.
    /// </summary>
    [TestClass]
    public class BookingManagerTests
    {
        //
        private int BookingID;

        private int guestID = 100;
        private int empID = 100;
        private int itemID = 100;

        private int bQuantity = 2;
        private DateTime dateBooked = DateTime.Now;
        private decimal ticket = 1234m;
        private decimal extended = 40m;
        private decimal discount = .1m;
        private decimal total = 36m;
        private Booking booking = new Booking();
        private BookingDetails bookingDetails;
        private BookingManager myBook = new BookingManager();
        private List<Booking> getThem = new List<Booking>();
        public void TestBookingConstructor()
        {
            //booking object created
            booking = new Booking(guestID, empID, itemID, bQuantity, dateBooked, ticket, extended, discount, total);
        }

        [TestInitialize]
        public void BookingTestSetup()
        {//Sets up the booking record and calls for the record to be added
            TestBookingConstructor();
            AddBookingResult();
            createBookingList();

        }
        public void AddBookingResult()
        {
            ResultsEdit result = myBook.AddBookingResult(booking);
        }
        public void createBookingList()
        {
            getThem = TestCleanupAccessor.GetAllBookings();
        }
        [TestMethod]
        public void TestAddBookingResult()
        {
            //booking object created

            TestBookingConstructor();
            ResultsEdit result = myBook.AddBookingResult(booking);
            ResultsEdit expected = ResultsEdit.Success;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestEditBooking()
        {
            // Updates the dummy booking in the database, first captures the dummy bookingID from database
            //using a TestAccessor
            BookingID = TestCleanupAccessor.GetBooking();
            //Assigns one booking object to be the old record and one to be the new record
            Booking newB = myBook.RetrieveBooking(BookingID);
            //Updates the booking with new quantity
            newB.Quantity = 3;
            int rows = myBook.EditBooking(newB);
            int expected = 3;
            //Grabs the record to test and see if the update went through
            Booking toCheck = myBook.RetrieveBooking(BookingID);
            Assert.AreEqual(expected, toCheck.Quantity);
        }

        [TestMethod]
        public void TestRetrieveActiveItemListings()
        {
            //A test to retrieve a listing of ItemListingDetails, checks to ensure that a list of ItemListingDetails
            //is being returned and that the count is greater than 1 record.
            List<ItemListingDetails> activeEventListings = myBook.RetrieveActiveItemListingDetailsList();
            bool worked = false;
            if (activeEventListings.Count > 1)
            {
                worked = true;
            }
            Assert.IsTrue(worked);
        }

        [TestMethod]
        public void TestRetrieveEventListing()
        {   //Assigns int item to the ItemListID from the first booking record in the getThem list of all bookings
            int item = getThem[getThem.Count - 1].ItemListID;
            //A test to retrieve a single listing by ID from the ItemListing table.
            ItemListingDetails detail = myBook.RetrieveItemListingDetailsList(item);
            Assert.IsNotNull(detail);
        }

        [TestMethod]
        public void TestRetrieveBooking()
        {
            // Retrieves a booking from the database by ID, first captures the dummy booking from database
            //using a TestAccessor, then uses a real manager method to be tested.
            BookingID = TestCleanupAccessor.GetBooking();
            Booking booking2 = myBook.RetrieveBooking(BookingID);
            int expected = 1234;
            Assert.AreEqual(expected, booking2.TicketPrice);
        }

        [TestMethod]
        public void TestCalcTotalCharge()
        {   //Checks to see what the total price returned will be.
            decimal result = myBook.CalcTotalCharge(discount, extended);
            decimal expected = 36m;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestAvailableQuantity()
        {
            //Tests to see what the available quantity will be.
            int max = 10;
            int current = 5;
            int quantity = myBook.AvailableQuantity(max, current);
            int expected = 5;
            Assert.AreEqual(expected, quantity);
        }

        [TestMethod]
        public void TestSpotsReserved()
        {
            //Tests to see what the spots reserved will be.
            int newQuantity = 6;
            int oldQuantity = 4;
            int quantity = myBook.SpotsReservedDifference(newQuantity, oldQuantity);
            int expected = 2;
            Assert.AreEqual(expected, quantity);
        }
        [TestMethod]
        public void TestBookingCalculateTimeFee0()
        {
            //Creates a Bookingdetails object to pass to BookingManager to check calculation of CalculateTime
            BookingDetails bookingD = new BookingDetails();
            DateTime value = new DateTime(2100, 01, 31, 17, 30, 00);
            bookingD.StartDate = value;
            decimal result = myBook.CalculateTime(bookingD);
            decimal expected = 0m;
            //Asserts that the expected and the result will be 0M
            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void TestBookingCalculateTimeFee1()
        {
            //Creates a Bookingdetails object to pass to BookingManager to check calculation of CalculateTime
            BookingDetails bookingD = new BookingDetails();
            DateTime value = new DateTime(2000, 01, 31, 17, 30, 00);
            bookingD.StartDate = value;
            decimal result = myBook.CalculateTime(bookingD);
            decimal expected = 1.0m;
            //Asserts that the expected and the result will be 1.0M
            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void TestBookingCalculateTimeFeeHalf()
        {
            //Creates a Bookingdetails object to pass to BookingManager to check calculation of CalculateTime
            BookingDetails bookingD = new BookingDetails();
            DateTime today = DateTime.Now;
            DateTime value = today.AddDays(2);
            bookingD.StartDate = value;
            decimal result = myBook.CalculateTime(bookingD);
            decimal expected = .5m;
            //Asserts that the expected and the result will be .5M
            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void TestBookingCalcExtendedPrice()
        {
            //Creates a decimal and an int to be calculated
            decimal numA = 2.0m;
            int numB = 2;
            decimal expected = 4.0m;
            decimal result = myBook.CalcExtendedPrice(numA, numB);
            //Asserts that the expected and the result will be .5M
            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void TestBookingCalcCancellationFee()
        {
            //Creates a bookingDetails object used to perform calculations. Method uses the TotalCharge and the startdate
            BookingDetails bookingD = new BookingDetails();
            DateTime value = new DateTime(2000, 01, 31);
            bookingD.StartDate = value;
            bookingD.TotalCharge = 3.0m;
            decimal expected = 3.0m;
            decimal result = myBook.CalculateCancellationFee(bookingD);
            //Asserts that the expected and the result will be .5M
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CheckToEditBooking()
        {
            //Checks the edit booking and makes sure a result is returned.
            bookingDetails = new BookingDetails();
            bookingDetails.StartDate = DateTime.Now;
            bookingDetails.Quantity = 2;
            ResultsEdit result = myBook.CheckToEditBooking(bookingDetails);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestCancelBookingResults()
        {
            //Tests the Cancel Booking Results method in the Booking Manager- which takes a BookingDetails object.
            //Grabs the ID for the dummy booking
            int id = TestCleanupAccessor.GetBooking();
            //retrieves the full booking information
            Booking booking1 = myBook.RetrieveBooking(id);
            //Creates a BookingDetails object and assigns variables from the booking to it.
            bookingDetails = new BookingDetails();
            bookingDetails.BookingID = id;
            bookingDetails.GuestID = guestID;
            bookingDetails.EmployeeID = empID;
            bookingDetails.ItemListID = itemID;
            bookingDetails.Quantity = bQuantity;
            bookingDetails.DateBooked = dateBooked;
            bookingDetails.TicketPrice = ticket;
            bookingDetails.ExtendedPrice = extended;
            bookingDetails.Discount = discount;
            bookingDetails.TotalCharge = total;
            //Passes the object to the CancelBookingResults method and asserts that that cancel will be successful
            ResultsEdit result = myBook.CancelBookingResults(bookingDetails);
            ResultsEdit expected = ResultsEdit.Success;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestEditBookingResultsSuccess()
        {
            //Grabs the List of all active bookings in DB and assigns the first record from the listing to a int id 
            int id = getThem[getThem.Count - 1].BookingID;
            //retrieves the full booking information, assigns the initial quantity to an int
            //Does not change the record, just checks to make sure all steps going through.
            Booking booking1 = myBook.RetrieveBooking(id);
            int original = booking1.Quantity;
            //Passes the object to the EditBookingResults method and asserts that that result will be successful
            ResultsEdit result = myBook.EditBookingResult(original, booking1);
            ResultsEdit expected = ResultsEdit.Success;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestEditBookingResultsQuantityZero()
        {
            //Tests the Edit Booking Results method in the Booking Manager- which takes an int and a Booking object.
            //Grabs the ID for the dummy booking
            int id = getThem[getThem.Count - 1].BookingID;
            //retrieves the full booking information, assigns the initial quantity to an int
            Booking booking1 = myBook.RetrieveBooking(id);
            int original = booking1.Quantity;
            booking1.Quantity = 0;
            //Passes the object to the EditBookingResults method and asserts that that result will be 0
            ResultsEdit result = myBook.EditBookingResult(original, booking1);
            ResultsEdit expected = ResultsEdit.QuantityZero;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestEditBookingResultsListingFull()
        {   //grabs the BookingID from the first booking from the active bookings list
            int id = getThem[getThem.Count - 1].BookingID;
            Booking booking1 = myBook.RetrieveBooking(id);//Retrieves
            int original = booking1.Quantity;
            booking1.Quantity = 10000;//sets to new quantity
            ////Passes the object to the EditBookingResults method and asserts that that result will be full
            ResultsEdit result = myBook.EditBookingResult(original, booking1);
            ResultsEdit expected = ResultsEdit.ListingFull;
            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void TestBookingGuestPIN()
        {
            //Pulls a guest from the database and collects the guest information
            List<HotelGuest> guest1 = HotelGuestAccessor.HotelGuestGet(100);
            //Checks using a pin in the database, stores guest info from database into a guest object
            //Asserts that a record is found, that guest is not null by passing the guest1 guest pin
            HotelGuest guest = myBook.CheckValidPIN(guest1[guest1.Count - 1].GuestPIN);
            Assert.IsNotNull(guest);
        }
        [TestMethod]
        public void TestBookingNumbers()
        {
            //Checks using the dummy itemListing, returns a BookingNumbers object(nums)  
            //Asserts that a record is found, that nums is not null
            List<BookingNumbers> nums = myBook.RetrieveBookingNumbers(100);
            Assert.IsNotNull(nums);
        }
        [TestCleanup]
        public void CleanupTest()
        {
            TestCleanupAccessor.resetItemListing100();
            TestCleanupAccessor.testBook(booking);
        }
    }
}