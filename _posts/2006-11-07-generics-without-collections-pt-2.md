---
layout: post
title: Generics without Collections, pt. 2
categories: code c# .net programming generics-without-collections
tags: code c# .net programming generics-without-collections
---

  My  [previous article](/blog/2006/10/02/Generics-without-Collections/  "Generics without Collections") on this subject dealt with creating a lazy-loaded data type.  But, if you think about it, that's really just a collection, with just one item.  I promised you use of generics without collections, so let's move this to the next step, using the type to affect the behavior of the code, without ever storing an object of that type.
  
  You've probably written some code like this:

    foreach (Control ctrl in this.Controls)
    {
        ComboBox cb = ctrl as ComboBox;
        if (cb != null)
        {
             MessageBox.Show("CheckBox " + (chb.Checked   "IS" : "is NOT") + " checked");                
 
        }
    }
    
This gives a list of all ComboBoxes in the current control set.  To handle that, we've got to go through all the controls and filter out the ones that are not ComboBoxes.  Wouldn't it be great if we could handle this filtering automatically.

    foreach (CheckBox chb in ControlFilter.Only<CheckBox>(this.Controls))
    {
        MessageBox.Show("CheckBox " + (chb.Checked   "IS" : "is NOT") + " checked");
    }

We start by defining the static class to hold this method, and the method itself. You'll note that nowhere in this block do we use the type parameter directly.

	using System.Windows.Forms;
	static class ControlFilter
	{
	   public static IEnumerable<T> Only<T>(Control.ControlCollection coll) where T:Control
	   {


We need to return an object here, one which implements IEnumerable&lt;T&gt;, so let's create one, and pass all the work on to it:

    return new ControlFilter_impl<T>(coll);
   }
   
Since ControlFIlter_impl has no meaning outside of ControlFilter, while just define it as an internal class inside ControlFilter.


	class ControlFilter_impl<T> : IEnumerable<T> where T : Control
	{
	   private Control.ControlCollection m_Coll;
	   public ControlFilter_impl(Control.ControlCollection coll)
	   {
		  m_Coll = coll;
	   }


So far, all we've done is store a reference to the collection we plan on enumerating -- and we still haven't actually used type T for anything, but now we do that in the next step.


	public IEnumerator<T> GetEnumerator()
	{
		foreach (Control ctrl in m_Coll)
		{
			T ctrlT = ctrl as T;
			if (ctrlT != null)
				yield return ctrlT;
		}
	}

Here, we use the "yield return" statement to create an iterator block.  It is essentially the same algorithm as we started with, but here we finally use the passed-in type to filter the controls as they go by.   That is the important message here.  We are using the passed in type to modify what the algorithm does, instead of to just hold a bunch of them. 


The rest is just boiler plate to keep the compiler happy.

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}

<a href="http://www.dotnetkicks.com/kick/?url=http://honestillusion.com/blogs/blog_0/archive/2006/11/07/Generics-without-Collections-_2800_pt-2_2900_.aspx"><img alt="kick it on DotNetKicks.com" src="http://www.dotnetkicks.com/Services/Images/KickItImageGenerator.ashx?url=http://honestillusion.com/blogs/blog_0/archive/2006/11/07/Generics-without-Collections-_2800_pt-2_2900_.aspx" border="0" /></a>