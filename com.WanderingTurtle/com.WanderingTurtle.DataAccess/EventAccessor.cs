﻿using com.WanderingTurtle.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace com.WanderingTurtle.DataAccess
{
    public class EventAccessor
    {
        /// <summary>
        /// Justin Pennington
        /// Created:  2015/02/14
        /// Adds a new event item in the database, using an event object that is passed in
        /// </summary>
        /// <param name="newEvent">The Event object to be added</param>
        /// <returns>number of rows updated in db</returns>
        public static int AddEvent(Event newEvent)
        {
            //Connect To Database
            var conn = DatabaseConnection.GetDatabaseConnection();
            var cmdText = "spInsertEventItem";
            var cmd = new SqlCommand(cmdText, conn);
            var rowsAffected = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            //Set up Parameters For the Stored Procedure
            cmd.Parameters.AddWithValue("@EventItemName", newEvent.EventItemName);
            cmd.Parameters.AddWithValue("@EventTypeID", newEvent.EventTypeID);
            cmd.Parameters.AddWithValue("@EventOnsite", newEvent.OnSite);
            cmd.Parameters.AddWithValue("@Transportation", newEvent.Transportation);
            cmd.Parameters.AddWithValue("@EventDescription", newEvent.Description);

            try
            {
                conn.Open();
                rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    throw new ApplicationException("Concurrency Violation");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return rowsAffected;
        }

        /// <summary>
        /// Justin Pennington 
        /// Created:  2015/02/04
        /// 
        /// Updates an Event object/record
        /// </summary>
        /// <param name="oldEvent">The Event object to be updated</param>
        /// <param name="newEvent">The Event object with the updated information</param>
        /// <returns>Returns the number of rows affected (should be 1)</returns>
        public static int UpdateEvent(Event oldEvent, Event newEvent)
        {
            var conn = DatabaseConnection.GetDatabaseConnection();
            var cmdText = "spUpdateEventItem";
            var cmd = new SqlCommand(cmdText, conn);
            var rowsAffected = 0;

            // set command type to stored procedure and add parameters
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@EventItemName", newEvent.EventItemName);
            cmd.Parameters.AddWithValue("@EventTypeID", newEvent.EventTypeID);
            cmd.Parameters.AddWithValue("@EventOnsite", newEvent.OnSite);
            cmd.Parameters.AddWithValue("@Transportation", newEvent.Transportation);
            cmd.Parameters.AddWithValue("@EventDescription", newEvent.Description);

            cmd.Parameters.AddWithValue("@EventItemID", oldEvent.EventItemID);
            cmd.Parameters.AddWithValue("@originalEventItemName", oldEvent.EventItemName);
            cmd.Parameters.AddWithValue("@originalEventTypeID", oldEvent.EventTypeID);
            cmd.Parameters.AddWithValue("@originalEventOnsite", oldEvent.OnSite);
            cmd.Parameters.AddWithValue("@originalTransportation", oldEvent.Transportation);
            cmd.Parameters.AddWithValue("@originalEventDescription", oldEvent.Description);
            try
            {
                conn.Open();
                rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    throw new ApplicationException("Concurrency Violation");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return rowsAffected;  // needs to be rows affected
        }

        /// <summary>
        /// Justin Pennington
        /// Created:  2015/02/14
        /// Changes the event from active to inactive
        /// </summary>
        /// <param name="eventToBeDeleted">The Event to be set inactive</param>
        /// <returns>returns number of rows affected</returns>
        public static int DeleteEventItem(Event eventToBeDeleted)
        {
            var conn = DatabaseConnection.GetDatabaseConnection();
            var cmdText = "spDeleteEventItem";
            var cmd = new SqlCommand(cmdText, conn);
            var rowsAffected = 0;

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@EventItemName", eventToBeDeleted.EventItemName);
            cmd.Parameters.AddWithValue("@EventItemID", eventToBeDeleted.EventItemID);
            cmd.Parameters.AddWithValue("@EventTypeID", eventToBeDeleted.EventTypeID);
            cmd.Parameters.AddWithValue("@Transportation", eventToBeDeleted.Transportation);
            cmd.Parameters.AddWithValue("@EventDescription", eventToBeDeleted.Description);
            cmd.Parameters.AddWithValue("@EventOnsite", eventToBeDeleted.OnSite);
            try
            {
                conn.Open();
                rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    throw new ApplicationException("Concurrency Violation");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return rowsAffected;  // needs to be rows affected
        }


        /// <summary>
        /// Justin Pennington 
        /// Created:  2015/02/14 
        /// Retrieves all Active Events and returns a List of Event objects
        /// </summary>
        /// <returns>List object containing Event objects returned by the database</returns>
        public static List<Event> GetEventList()
        {
            var EventList = new List<Event>();

            // set up the database call
            var conn = DatabaseConnection.GetDatabaseConnection();
            string cmdText = "spSelectAllEventItems";
            var cmd = new SqlCommand(cmdText, conn);

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {
                        var currentEvent = new Event();

                        currentEvent.EventItemID = reader.GetInt32(0);
                        currentEvent.EventItemName = reader.GetString(1);
                        currentEvent.EventTypeID = reader.GetInt32(2);
                        currentEvent.OnSite = reader.GetBoolean(3);
                        currentEvent.Transportation = reader.GetBoolean(4);
                        currentEvent.Description = reader.GetString(5);
                        currentEvent.Active = reader.GetBoolean(6);
                        currentEvent.EventTypeName = reader.GetString(7);
                        EventList.Add(currentEvent);
                    }
                }
                else
                {
                    var ax = new ApplicationException("Data not found!");
                    throw ax;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return EventList;
        }

        /// <summary>
        /// Justin Pennington 
        /// Created:  2015/02/14
        /// Retrieve data for an Event, create an object using data with retrieved data, and return the object that is created
        /// </summary>
        /// <remarks>
        /// Bryan Hurst
        /// Updated: 2015/04/24
        /// 
        /// Added while loop and CommandType.StoredProcedure setting
        /// </remarks>
        /// <param name="eventID">The EventID matching the Event to be retrieved</param>
        /// <returns>The Event requested via the EventID</returns>
        public static Event GetEvent(string eventID)
        {
            var theEvent = new Event();
            // set up the database call
            var conn = DatabaseConnection.GetDatabaseConnection();
            string cmdText = "spSelectEventItem";
            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@EventItemID", eventID);
            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {
                        theEvent.EventItemID = reader.GetInt32(0);
                        theEvent.EventItemName = reader.GetString(1);
                        theEvent.EventTypeID = reader.GetInt32(2);
                        theEvent.OnSite = reader.GetBoolean(3);
                        theEvent.Transportation = reader.GetBoolean(4);
                        theEvent.Description = reader.GetString(5);
                        theEvent.Active = reader.GetBoolean(6);
                    }
                    
                }
                else
                {
                    var ax = new ApplicationException("Data not found!");
                    throw ax;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return theEvent;
        }

        /// <summary>
        ///  Hunter Lind
        ///  Created:  2015/04/03
        ///  Method for deletion of test records created with the unit tests
        /// </summary>
        /// <param name="TestEvent">The Event object used for testing -- to be deleted</param>
        /// <returns>number of rows affected</returns>
        public static int DeleteEventTestItem(Event TestEvent)
        {
            var conn = DatabaseConnection.GetDatabaseConnection();
            var cmdText = "spDeleteTestEvent";
            var cmd = new SqlCommand(cmdText, conn);
            var rowsAffected = 0;

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@EventItemName", TestEvent.EventItemName);

            try
            {
                conn.Open();
                rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    throw new ApplicationException("Concurrency Violation");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return rowsAffected;  // needs to be rows affected
        }
    }
}