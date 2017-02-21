using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Tritalk.Client
{
    static class ExtMethods
    {
        public static FrameworkElement GetRoot(this FrameworkElement element)
        {
            FrameworkElement root = element;
            while (element.Parent != null)
            {
                if (!(element.Parent is FrameworkElement)) break;
                root = element.Parent as FrameworkElement;
            }
            return root;
        }
    }
}
