﻿#pragma checksum "C:\Users\Yoran\Desktop\QMI\QMIGenerator\QMIGenerator\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6F221E5A61A9160E1C0F2E7C2B3CB9CD"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QMIGenerator
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.NavLeft = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 32 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.NavLeft).Click += this.NavLeft_Click;
                    #line default
                }
                break;
            case 2:
                {
                    this.NavRight = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 40 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.NavRight).Click += this.NavRight_Click;
                    #line default
                }
                break;
            case 3:
                {
                    this.PageContent = (global::Windows.UI.Xaml.Controls.ContentControl)(target);
                }
                break;
            case 4:
                {
                    this.MenuContent = (global::Windows.UI.Xaml.Controls.ContentControl)(target);
                }
                break;
            case 5:
                {
                    this.ContentTitle = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

