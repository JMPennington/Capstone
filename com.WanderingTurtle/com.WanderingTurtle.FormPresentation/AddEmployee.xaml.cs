﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using com.WanderingTurtle.Common;
using com.WanderingTurtle.BusinessLogic;

namespace com.WanderingTurtle.FormPresentation
{
    /// <summary>
    /// Interaction logic for AddEmployee.xaml
    /// </summary>
    public partial class AddEmployee : Window
    {
        public static AddEmployee Instance;
        Employee newEmployeeUser = new Employee();
        EmployeeManager myManager = new EmployeeManager();

        /// <summary>
        /// Initialize form 
        /// 
        /// </summary>
        public AddEmployee()
        {
            InitializeComponent();

            //creating a list for the dropdown userLevel
            cboUserLevel.ItemsSource = RetrieveUserLevelList();

            Instance = this;
            this.Title = "Add Employee";
        }

        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            addEmployee();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Instance = null;

        }

        // Pat Banks - February 15, 2015
        // Parameters: text fields from form
        // Desc.: Method takes values for a new employee from the form and passes values
        //        into the AddNewEmployee method of the EmployeeManager class
        // Failure: Exception is thrown if database is not available or 
        //          new employee cannot be created in the database for any reason
        // Success: A user is added to the database
        private void addEmployee()
        {
            int result;

            if (!Validator.ValidateString(txtFirstName.Text))
            {
                MessageBox.Show("Please fill out the first name field.");
                txtFirstName.Focus();
                return;
            }
            if (!Validator.ValidateString(txtLastName.Text))
            {
                MessageBox.Show("Please fill out the last name field.");
                txtLastName.Focus();
                return;
            }
            if (!Validator.ValidateAlphaNumeric(txtPassword.Text, 4, 8))
            {
                MessageBox.Show("Please enter a password with between 4 and 8 characters.");
                txtPassword.Focus();
                return;
            }

            if (cboUserLevel.Text == "" || cboUserLevel.Text == null)
            {
                MessageBox.Show("Please select a user level.");
                cboUserLevel.Focus();
                return;
            }
            
            try
            {
                // collect the values from the form
                newEmployeeUser.FirstName = this.txtFirstName.Text;
                newEmployeeUser.LastName = this.txtLastName.Text;
                newEmployeeUser.Active = this.chkActiveEmployee.IsChecked.Value;
                newEmployeeUser.Password = this.txtPassword.Text;
                newEmployeeUser.Level = int.Parse(this.cboUserLevel.SelectedValue.ToString());

                result = myManager.AddNewEmployee(newEmployeeUser);

            } catch (Exception ax)
            {
                throw ax;
            }


            if (result == 1)
            {
                MessageBox.Show("Employee added successfully");

            }
            else
            {
                MessageBox.Show("There was problem adding the employee to the database");
            }
        }

        private List<RoleData> RetrieveUserLevelList()
        {
            List<RoleData> ListData = new List<RoleData>();
            ListData.Add(new RoleData { id="Admin", value= 1 });
            ListData.Add(new RoleData { id = "Concierge", value = 2 });
            ListData.Add(new RoleData { id="DeskClerk", value=3 });
            ListData.Add(new RoleData { id = "Valet", value = 4 });

            return ListData;

        }
    }
}