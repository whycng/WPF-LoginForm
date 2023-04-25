using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace WPF_LoginForm.ViewModels
{
    public static class VisualTreeHelperExtensions
    {
        public static T FindChild<T>(this DependencyObject parent, string childName) where T : DependencyObject
        {
            // 检查参数是否有效
            if (parent == null || string.IsNullOrEmpty(childName))
            {
                return null;
            }

            // 从可视化树中找到名为 childName 的子元素，并返回其引用
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                var frameworkElement = child as FrameworkElement;

                if (frameworkElement != null && frameworkElement.Name == childName)
                {
                    return (T)child;
                }

                var result = FindChild<T>(child, childName);

                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        //public static List<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
        //{
        //    var children = new List<T>();
        //    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        //    {
        //        var child = VisualTreeHelper.GetChild(parent, i);
        //        if (child is T typedChild)
        //        {
        //            children.Add(typedChild);
        //        }
        //        children.AddRange(FindVisualChildren<T>(child));
        //    }
        //    return children;
        //}

        //public static T FindChildByXName<T>(DependencyObject parent, string xname) where T : FrameworkElement
        //{
        //    if (parent == null || string.IsNullOrEmpty(xname))
        //    {
        //        return null;
        //    }

        //    var children = FindVisualChildren<T>(parent);

        //    foreach (var child in children)
        //    {
        //        if (child.Name == xname)
        //        {
        //            return child;
        //        }
        //    }

        //    return null;
        //}


    }


}
