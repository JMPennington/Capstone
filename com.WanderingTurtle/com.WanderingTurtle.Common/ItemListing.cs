﻿using System;

namespace com.WanderingTurtle.Common
{
    /// <summary>
    /// Matthew Lapka 
    /// Created: 2015/02/05
    /// 
    /// Creates an ItemListing Object. Contains specific information about the event listing
    /// </summary>
    public class ItemListing
    {
        public ItemListing()
        {
            //default constructor
        }

        //new constructor, leaving old one intact just in case there's someone else using it for who knows why - Kelsey B
        public ItemListing(int itemListID, int eventID, int supplierID, DateTime startDate, DateTime endDate, decimal price, int maxNumGuests, int minNumGuests, int currentNumGuests)
        {
            ItemListID = itemListID;
            EventID = eventID;
            SupplierID = supplierID;
            StartDate = startDate;
            EndDate = endDate;
            Price = price;
            MaxNumGuests = maxNumGuests;
            MinNumGuests = minNumGuests;
            CurrentNumGuests = currentNumGuests;
        }

        public ItemListing(int itemListID, int eventID, DateTime startDate, DateTime endDate, decimal price, int quantityOffered, string productSize, int maxNumGuests, int minNumGuests, int currentNumGuests)
        {
            ItemListID = itemListID;
            EventID = eventID;
            StartDate = startDate;
            EndDate = endDate;
            Price = price;
            MaxNumGuests = maxNumGuests;
            MinNumGuests = minNumGuests;
            CurrentNumGuests = currentNumGuests;
        }

        public int CurrentNumGuests { get; set; }

        public DateTime EndDate { get; set; }

        public int EventID { get; set; }

        public string EventName { get; set; }

        public int ItemListID { get; set; }

        public int MaxNumGuests { get; set; }

        public int MinNumGuests { get; set; }

        public decimal Price { get; set; }

        public int Seats { get; set; }

        public DateTime StartDate { get; set; }

        public int SupplierID { get; set; }

        public string SupplierName { get; set; }
    }

    
    /// <summary>
    /// Anthony Noel 
    /// Created: 2015/02/16
    /// 
    /// List Item Object, extends booking.
    /// Has a default constructor, and one to accept 2 ints, 2 strings, another int, and two datetimes
    /// Extends booking so that all information on a booking can be linked to the event information.
    /// </summary>
    public class ItemListingDetails : ItemListing
    {
        public ItemListingDetails()
            : base()
        {
        }

        public ItemListingDetails(int itemListID, int eventID, string eventName, string eventDescription, int qOffered, DateTime start, DateTime end)
        {
            ItemListID = itemListID;
            EventID = eventID;
            EventName = eventName;
            EventDescription = eventDescription;
            QuantityOffered = qOffered;
            StartDate = start;
            EndDate = end;
        }

        public string EventDescription { get; set; }

        public int QuantityOffered { get; set; }
    }
}