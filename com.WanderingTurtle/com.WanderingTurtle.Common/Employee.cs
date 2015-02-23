﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.WanderingTurtle.Common
{
    /// <summary>
    /// Ryan Blake
    /// Created: 2015/02/16
    ///
    /// Class for the creation of Employee Objects with set data fields
    /// </summary>
    /// <remarks>
    /// Miguel Santana
    /// Updated: 2015/02/22
    ///
    /// Created non-default constructor to be used in assigning values.
    /// </remarks>
    public class Employee
    {
        public int? EmployeeID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public bool Active { get; set; }

        public RoleData Level { get; set; }

        public Employee(int EmployeeID, string FirstName, string LastName, string Password, int Level, bool Active = true)
        {
            SetValues(EmployeeID, FirstName, LastName, Password, Level, Active);
        }

        public Employee(string FirstName, string LastName, string Password, int Level, bool Active = true)
        {
            SetValues(null, FirstName, LastName, Password, Level, Active);
        }

        public Employee() { }

        private void SetValues(int? EmployeeID, string FirstName, string LastName, string Password, int Level, bool Active)
        {
            this.EmployeeID = EmployeeID;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Password = Password;
            this.Level = (RoleData)Level;
            this.Active = Active;
        }

        public string GetFullName { get { return string.Format("{0} {1}", this.FirstName, this.LastName); } }
    }
}