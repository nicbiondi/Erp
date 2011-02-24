//-----------------------------------------------------------------------------
// Torque Game Builder
// Copyright (C) GarageGames.com, Inc.
// Behavior by Mike Lilligreen
//-----------------------------------------------------------------------------

if (!isObject(SplashScreenBehavior))
{
   %template = new BehaviorTemplate(SplashScreenBehavior);
   
   %template.friendlyName = "Splash Screen";
   %template.behaviorType = "GUI";
   %template.description  = "Turns the object into a splash screen";
   
   %template.addBehaviorField(fadeInTime, "Length of time in seconds to fade in", float, 2.0);
   %template.addBehaviorField(waitTime, "Length of time in seconds to display screen", float, 1.0);
   %template.addBehaviorField(fadeOutTime, "Length of time in seconds to fade out", float, 1.0);
   %template.addBehaviorField(level, "The scene to load next", string, "");
   %template.addBehaviorField(clickThrough, "Allow player to end display by mouse click", bool, 0);
}

function SplashScreenBehavior::onAddToScene(%this, %scenegraph)
{
   %this.owner.setBlendAlpha(0);
   %this.duration = %this.waitTime * 1000;
   %this.fadeInDuration = %this.fadeInTime * 1000;
   %this.fadeOutDuration = %this.fadeOutTime * 1000;
   %this.schedule(10, "fadeIn", 1, %this.fadeInDuration);
   
   if (%this.clickThrough)
      %this.owner.setUseMouseEvents(true);
}

function SplashScreenBehavior::onMouseDown(%this, %modifier, %worldPos)
{
   schedule(10, 0, "nextToLoad", %this.level);
}

function SplashScreenBehavior::onRightMouseDown(%this, %modifier, %worldPos)
{
   schedule(10, 0, "nextToLoad", %this.level);
}

function SplashScreenBehavior::fadeIn(%this, %toAlpha, %time)
{
   if(%time > 30)
   {
      %alpha = %this.owner.getBlendAlpha();
      %updatesRemaining = %time / 30;
      %alpha += (%toAlpha - %alpha) / %updatesRemaining;
      %this.owner.setBlendAlpha(%alpha);
      %this.schedule(30, "fadeIn", %toAlpha, %time - 30);
   }else
   {
      %this.owner.setBlendAlpha(%toAlpha);
      %this.schedule(%this.duration, "fadeOut", 0, %this.fadeOutDuration);
   }
}

function SplashScreenBehavior::fadeOut(%this, %toAlpha, %time)
{
   if(%time > 30)
   {
         %alpha = %this.owner.getBlendAlpha();
         %updatesRemaining = %time / 30;
         %alpha += (%toAlpha - %alpha) / %updatesRemaining;
         %this.owner.setBlendAlpha(%alpha);
         %this.schedule(30, "fadeOut", %toAlpha, %time - 30);
   }else
   {
         %this.owner.setBlendAlpha(%toAlpha);
         schedule(10, 0, "nextToLoad", %this.level);
   }
}

function nextToLoad(%level)
{
   %scene = "game/data/levels/" @ %level;
   
   if (isFile(%scene))
   {
      sceneWindow2D.loadLevel(%scene);
   }
}