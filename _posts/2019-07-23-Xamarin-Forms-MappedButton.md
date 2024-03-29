---
layout: post
title: Simplify Data Entry in Xamarin Forms with MappedButton
tags: code programming csharp dotnet xamarin forms
---

Back when Steven Thewissen proposed the 
[ "Xamarin UI July"](https://www.thewissen.io/introducing-xamarin-ui-july/), I thought "Great! That would be the perfect place to talk about that custom control I created for that Xamarin project I did.  I then spent the next month working on a Vue-based website (my first time use VueJs).  Now I have to shift my brain back into Xamarin mode.  (I'm getting too old for this....)
![#xamarinuijuly](/images/XamarinUIJuly.png)


In my particular app, the user would be filling out a form about a number of people.  One question would be if the person is male or female (we'll be ignoring the non-binary).  This could be handled with a switch control,  but they're really intended for yes/no questions, nor either/or question.  It also needed for  "Lead", "Supporting" or "Featured"  (the app is for reviewing stage shows).

These really wanted to be handled with something like a RadioButton.  Or, more precisely a collection of RadioButtons in a RadioGroup.  Basically, a whole bunch of controls to just a little bit of information.

And I do mean a little bit,  and that's another complication.  While it would be displaying "Male" and "Female", it would actually be storing "M" and "F", so that what goes into the bound variable, which needs to be two-way
![screenshot](/images/MappedButtonExample.png)

And I wanted to make setting the options as simple as possible.  I got it down to a simple string ("M=Male, F=Female") which can be hard-coded in the XAML, or bound to a variable in the C# code.

## Properties

| **Options** | Gets or sets the list of options with associated text for each button.  String, in the form "Key1=Text1, Key2=Text2".  Number of options limited only by memory.  This is a bindable property  |
| **SelectedKey** | Gets or sets the key of the selected button.  This is a bindable property. |
| **SelectedColor** | Gets or sets the color which will fill the background of a selected button. This is a bindable property. | 
| **ButtonColor** | Gets or sets the color which will fill the background of a unselected buttons. This is a bindable property. |

<script src="https://gist.github.com/jamescurran/b506f2ee146cebe4a4836bc47623ff6a.js">   </script>

The buttons themselves aren't very artful -- My not much of a graphic designer.  Any help from more creative readers would  be get (see GitHub repo below)

[GitHub repository here](https://github.com/jamescurran/MappedButton)


