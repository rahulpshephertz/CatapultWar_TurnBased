﻿#pragma checksum "E:\WP7\CatapultWarTurnBased\CatapultWar\CatapultWar\JoinPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "35D78797F14AAAA994595110CD43E9AE"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18051
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace CatapultWar {
    
    
    public partial class JoinPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal Microsoft.Phone.Controls.PhoneApplicationPage Join;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBox txtUserName;
        
        internal System.Windows.Controls.Button joinButton;
        
        internal System.Windows.Controls.Grid MessagePopup;
        
        internal System.Windows.Controls.TextBlock messageTB;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/CatapultWar;component/JoinPage.xaml", System.UriKind.Relative));
            this.Join = ((Microsoft.Phone.Controls.PhoneApplicationPage)(this.FindName("Join")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.txtUserName = ((System.Windows.Controls.TextBox)(this.FindName("txtUserName")));
            this.joinButton = ((System.Windows.Controls.Button)(this.FindName("joinButton")));
            this.MessagePopup = ((System.Windows.Controls.Grid)(this.FindName("MessagePopup")));
            this.messageTB = ((System.Windows.Controls.TextBlock)(this.FindName("messageTB")));
        }
    }
}

