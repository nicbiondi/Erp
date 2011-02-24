//-----------------------------------------------------------------------------
// Torque Game Builder
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

if (!isObject(ClockBehavior))
{
   %template = new BehaviorTemplate(ClockBehavior);
   
   %template.friendlyName = "Clock";
   %template.behaviorType = "Game Logic";
   %template.description  = "Move the hand of a clock 360 degrees and then stop";

   %template.addBehaviorField(minutes, "minutes to get around the clock", float, ".25");
}

function ClockBehavior::onBehaviorAdd(%this)
{
   %this.time = (%this.minutes * 60000); 
   %this.speed = %this.time / (10000 * (%this.minutes * %this.minutes));
   %this.schedule(100, "start");
}

function ClockBehavior::start(%this)
{
   //move arrow
   %this.owner.setAngularVelocity(%this.speed);
   %this.owner.setRotationTarget(180, true, true, true, 0.001);  
}

function ClockBehavior::onRotationTarget(%this)
{ 
   %this.owner.setAngularVelocity(%this.speed);
   %this.owner.setRotationTarget(360, true, true, true, 0.001);  
}