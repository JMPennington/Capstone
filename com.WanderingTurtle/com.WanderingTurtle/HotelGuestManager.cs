﻿using com.WanderingTurtle.Common;
using com.WanderingTurtle.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.WanderingTurtle
{
    public class HotelGuestManager
    {
        /// <summary>
        /// Creates a new Hotel Guest in the database
        /// </summary>
        /// <param name="newHotelGuest">Object containing new hotel guest information</param>
        /// <returns>Number of rows effected</returns>
        /// Miguel Santana 2/18/2015
        public bool AddHotelGuest(NewHotelGuest newHotelGuest)
        {
            try
            {
                return HotelGuestAccessor.HotelGuestAdd(newHotelGuest) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets a hotel guest by id
        /// </summary>
        /// <param name="hotelGuestID">the id of a hotel guest to retrieve</param>
        /// <returns>HotelGuest object retrieved from database</returns>
        /// Miguel Santana 2/18/2015
        public HotelGuest GetHotelGuest(int hotelGuestID)
        {
            try
            {
                List<HotelGuest> list = HotelGuestAccessor.HotelGuestGet(hotelGuestID);
                return (list.Count == 1) ? list.ElementAt(0) : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets a list of all Hotel Guests
        /// </summary>
        /// <returns>List of HotelGuest Objects</returns>
        /// Miguel Santana 2/18/2015
        public List<HotelGuest> GetHotelGuestList()
        {
            try
            {
                return HotelGuestAccessor.HotelGuestGet();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        /********************  Methods not used in Sprint 1 ************************************************/
        /// <summary>
        /// Updates a hotel guest with new informatino
        /// </summary>
        /// <param name="oldHotelGuest">Object containing original information about a hotel guest</param>
        /// <param name="newHotelGuest">Object containing new hotel guest information</param>
        /// <returns>Number of rows effected</returns>
        /// Miguel Santana 2/18/2015
        public bool UpdateHotelGuest(HotelGuest oldHotelGuest, NewHotelGuest newHotelGuest)
        {
            try
            {
                return HotelGuestAccessor.HotelGuestUpdate(oldHotelGuest, newHotelGuest) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}