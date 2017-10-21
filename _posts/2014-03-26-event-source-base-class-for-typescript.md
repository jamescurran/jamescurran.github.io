---
layout: post
title: Event Source Base class for Typescript
tags: code javascript programming typescript
---

HTML elements have an extensive event system, and jQuery gives us a nice interface to access it.  

However, as our Javascript coding gets more elaborate and more object-oriented -- particularly through Typescript -- the more we need a similar event system in objects which do not derive from HTML elements.

I created EventSource for such a need I recently had at work.  In Typescript, it's used as the base class for classes which need to trigger events. Since pure Javascript doesn't offer base classes (it technically doesn't offer classes at all, except via an abuse of an implementation detail), using EventSource with pure Javascript code is a bit tricky, but it can be done.

The programming interface is a simplified version of the API from jQuery.  It don't have all the options jQuery does, but it was sufficient for my project. If there's an outcry, I'll extend it.

###API for using a EventSource-derived class:


 **Syntax:**

    objSomeObj.on("event", context, handlerFunction, [optional data])

 __Example:__

	status.on("changed", this, function(ev) { alert ("changed");} );

 **Parameters:**

  - event - `string`, name of the subscribed event.  
  - 	context - `any` - will be the "this" object when the handler function is called.  Most like you'd want it to be the "this" property of the calling class.
  -	handleFunction - `(args: EventArgs, data?: any) => void` function called when event is triggered.  An EventArgs object (described below) is passed as it's argument
  -	data - `any` -an optional object which is included with in the EventArgs when the event is triggered.

*EventArgs:*

  - source - `any` -the object triggering the event.
  -	type  - `string`, the name of the event triggered
  -	data - `any` - extra data (from above), might include more properties from the triggering object.


 **Return Value:**

  - `EventSource` - The object that's triggering the events is returned, so on() method calls can be chained.


---
**Syntax:**

    objSomeObj.one("event", context, handlerFunction, [optional data])

**Example:**

	status.one("changed", this, function(ev) { alert ("changed");} );

Parameters and return are as above.

`handlerFunction` is called only once, on the next time the event is triggers.

---
**Syntax:**

    objSomeObj.off("event", [context], [handlerFunction])

**Example:**

	// remove all "changed" handlers.
	status.off("changed");   

	// remove all "changed" handlers for this context.
	status.off("changed", this );

	// remove the onStatusChanged handlers for this context.
	// Note: this cannot be an inline function.
	status.off("changed", this, onStatusChanged);


 - Parameters are the same as with on(), except here, only `type` is required. All event handlers matching as many parameters as given will be removed.
 
---
###API for defining an EventSource-derived class:


Inherit from the noveltheory.EventSource base class

	class MyClass extends noveltheory.EventSource
	{

In the ctor, call the superclass.

	constructor()
	{
		super();

Next, you should (be are not required) to set the name of the class.

        this.SetName('MyClass');

Once the name is set, event may be prefixed with the class name for clarity.

Also in the ctor, you may optionally define what event are handled.
If any are specified, `on()` will throw an exception if any other 
 string is given for type.

If none are specified, `on()` will accept any string.

		this.AddAllowedEvent("changed");
		this.AddAllowedEvent("MyClass.changing");
	}

In a method of class, call trigger:

	edit()
	{
		if (hasChanged)
			this.trigger("changing", {Text: newText} ); 
	}

###API

    trigger(type, data?)

**Parameters**

   - type : string - Name of event to be triggered. 
   - data : any - (optional) merged with data given in on() call, and passed to event handler.


### Full example:
<script src="https://gist.github.com/jamescurran/9766860.js">   </script>

The EventSource.ts code is available on my GitHub repository:
[https://github.com/jamescurran/HonestIllusion/tree/master/EventSource](https://github.com/jamescurran/HonestIllusion/tree/master/EventSource)

