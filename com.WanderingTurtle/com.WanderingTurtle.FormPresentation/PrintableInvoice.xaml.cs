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

namespace com.WanderingTurtle.FormPresentation
{
    /// <summary>
    /// Interaction logic for PrintableInvoice.xaml
    /// </summary>
    public partial class PrintableInvoice
    {
        private int _guestID;
        public PrintableInvoice(int guestID)
        {
            InitializeComponent();
            _guestID = guestID;
            BindData();
        }

        private void BindData()
        {
            report.BuildInvoice(_guestID);
        }
    }
}
