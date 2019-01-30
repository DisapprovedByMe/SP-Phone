# Single Player Scaleform Phone

This researches how the SP phone works in GTA V and is re-created from scratch.

Not finished and missing documentation for some apps however if you decompile the .gfx scaleform files yourself it is pretty easy to make sense of the data and arguments passed around.



Important stuff to know;  
The phone position in SP on the screen is (99.62f, -45.305f, -113f).  
The phone rotation in SP on the screen is (-90f, 0f, 0f).  
The phone scale is 500f.  
The scaleform MUST be located at 0.1f, 0.179f and have a size of 0.2f, 0.356f when you are drawing it.

YOU HAVE TO REQUEST "cellphone_ifruit"/"cellphone_facade"/"cellphone_badget" BEFORE YOU CREATE THE CELLPHONE. THIS WILL CAUSE THE GAME TO LOAD UP THE SP PHONE ON THE PED AS WELL, IF YOU DO NOT DO THIS THE PED WILL HAVE A FAKE PHONE INSTEAD

Main Process;

RAGE.Game.Graphics.RequestScaleformMovie("cellphone_ifruit");  
//important to wait for scaleform to load first, check script to see how  
RAGE.Game.Mobile.CreateMobilePhone(0); //phone type 0, 1, 2, 4  
RAGE.Game.Mobile.SetMobilePhonePosition(position);  
RAGE.Game.Mobile.SetMobilePhoneRotation(rotation, 0);  
RAGE.Game.Mobile.SetMobilePhoneScale(500f);  
RAGE.Game.Mobile.GetMobilePhoneRenderId(ref PhoneRenderID); //important  
RAGE.Game.Mobile.ScriptIsMovingMobilePhoneOffscreen(false); //turns phone on ped  

The inside the draw event you draw the phone like this;  

RAGE.Game.Ui.SetTextRenderId(PhoneRenderID);  
RAGE.Game.Graphics.Set2dLayer(4);  
RAGE.Game.Graphics.DrawScaleformMovie(PhoneScaleform, 0.1f, 0.179f, 0.2f, 0.356f, 255, 0, 255, 255, 0);  
RAGE.Game.Ui.SetTextRenderId(1); //important to reset back in case you play around with other render ids  

NOTES:  
YOU CAN DRAW OTHER STUFF TO THE PHONE, AFTER YOU DRAW THE SCALEFORM  
MOVING THE PHONE ON AND OFF THE SCREEN WAS DONE USING A CUSTOM LERP FUNCTION  

Then the script shows ways to make the phone work.  
