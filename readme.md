#LimitlessLED Older RGBW Light Control
via Windows Forms and LimitlessLED local wifi API

This controls the older RGBW bulbs.

##Clarification
Since the website uses confusing terminology let's clarify what's there:

[No Color - Soft or Cool White Only - "Dual White LED"](http://www.limitlessled.com/shop/warm-white-e26-b22-e27-led-light-bulb/)

[Saturated Color Only - White and Color can't mix "RGBW"](http://www.limitlessled.com/shop/rgbw-color-led-light-bulb/)

[What You'd Expect - Color and White All Working Non-Exclusively - "RGBWW/CW"](http://www.limitlessled.com/shop/rgbw-ww-cw-light-bulb/)

This controls the middle option, the oddball "RGBW"-but-not-RGB-and-W-at-the-same-time older lights. As the only color lights on the site for several years it's likely many LimitlessLED customers have these lights.

#Form
The Form lays out a pretty simple interface. You enter the IP of the Wifi Bridge (check your Router's NAT list), pick a Light Group (1-4) and drag the brightness up or down. Color is coded behind the scenes but no interface is presented here. The special Off command is issued for the bottom value (all the way left on the slider).

#Code
This is mostly here to show how to talk to LimitlessLED via C# so let's lay out the code.

###Form1 - Design View
Textbox, Radios and a Slider (Horiz scrollbar)

###Form1 - Code View
The top half is just handling Windows Forms oddities like setting the right value for the radio buttons and throttling the double-fire of events on the scrollbar.

The important part is at the bottom, `handleBrightness()`. When the form is valid, it kicks off a Task (to get us off the UI thread - there are important 100ms delays in these commands to make them work properly). Inside the Task we create a LimitlessLed object for our Bridge IP and send it either the White then Brightness commands, or Off, depending on the value. Note that since these have 2-27 for valid brightness, we set the scrollbar to 1-27 and treat 1 as Off, and pass-through the other 26 possible brightness values.

###LimitlessLed
LimitlessLED at its base is just UDP packets on your local wifi, so LimitlessLed is just a disposable wrapper around a UdpClient. Note that everything here is Task-based, so the delays in UDP send and the required 100ms delays are handled concisely.

There's a generic Command format laid out in the first `SendAsync()` that bundles up the standard 3-byte control code, 100ms delay, 3-byte value code, but this turns out to be not-so-standard so Color and White below immediately break from fully leveraging this method.

`SendColorAsync` is the least-safe - the color byte you send is up to the caller to correctly bound so it doesn't break the API.

The rest are based on Group number so if you were to send a Group higher than 4 or less than 1 you'd also break the API.

It's possible there are undocumented commands some that brick the lights so the risk could be very real - or perhaps all commands that are out of these bounds are harmless. Unknown.



Dev info:
http://www.limitlessled.com/dev/