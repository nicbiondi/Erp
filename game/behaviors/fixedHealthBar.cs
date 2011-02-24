//-----------------------------------------------------------------------------
// Torque Game Builder
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

if (!isObject(FixedHealthBar))
{
   %template = new BehaviorTemplate(FixedHealthBar);
   
   %template.friendlyName = "Fixed Health Bar";
   %template.behaviorType = "Game";
   %template.description  = "Display current health with a health bar for an object using the Takes Damage behavior.";

   %template.addBehaviorField(healthObject, "The object which needs a health bar", object, "", t2dSceneObject);
   
   %fixedEdges = "Top" TAB "Bottom" TAB "Left" TAB "Right" TAB "Center Horizontal" TAB "Center Vertical";
   %template.addBehaviorField(fixedEdge, "The edge of the health bar you want to be fixed in place", enum, "Left", %fixedEdges);
}

function FixedHealthBar::onAddToScene(%this)
{
   %this.owner.enableUpdateCallback();
   %this.maxSizeX = %this.owner.getSizeX();
   %this.maxSizeY = %this.owner.getSizeY();
   %this.startPosX = %this.owner.getPositionX();
   %this.startPosY = %this.owner.getPositionY();
   
   %hObject = %this.healthObject.getBehavior("TakesDamageBehavior");
   if (!isObject(%hObject))
      return;
   %this.maxHealth = %hObject.health;
}

function FixedHealthBar::onUpdate(%this)
{

   %hObject = %this.healthObject.getBehavior("TakesDamageBehavior");
   if (!isObject(%hObject))
      return;

   %currentHealth=%hObject.health;

   if (%currentHealth<0)
      %currentHealth = 0;
      
   %healthRatio = (%currentHealth) / (%this.maxHealth);
   
   %this.updateSizeAndPosition(%healthRatio);
}

function FixedHealthBar::updateSizeAndPosition(%this, %hRatio)
{
   %xPos = %this.owner.getPositionX();
   %yPos = %this.owner.getPositionY();
   %fixedEdge = %this.fixedEdge;
   
   switch$ (%fixedEdge)
   {
      case "Top":
         %newSize = (%this.maxSizeY * %hRatio);
         %sizeDiff = (%this.maxSizeY - %newSize);
         %this.owner.setSizeY(%newSize);
         %this.owner.setPositionY(%this.startPosY - (%sizeDiff / 2));
      case "Bottom":
         %newSize = (%this.maxSizeY * %hRatio);
         %sizeDiff = (%this.maxSizeY - %newSize);
         %this.owner.setSizeY(%newSize);
         %this.owner.setPositionY(%this.startPosY + (%sizeDiff / 2));
      case "Left":
         %newSize = (%this.maxSizeX * %hRatio);
         %sizeDiff = (%this.maxSizeX - %newSize);
         %this.owner.setSizeX(%newSize);
         %this.owner.setPositionX(%this.startPosX - (%sizeDiff / 2));
      case "Right":
         %newSize = (%this.maxSizeX * %hRatio);
         %sizeDiff = (%this.maxSizeX - %newSize);
         %this.owner.setSizeX(%newSize);
         %this.owner.setPositionX(%this.startPosX + (%sizeDiff / 2));
      case "Center Horizontal":
         %newSize = (%this.maxSizeX * %hRatio);
         %this.owner.setSizeX(%newSize);
      case "Center Vertical":
         %newSize = (%this.maxSizeY * %hRatio);
         %this.owner.setSizeY(%newSize);     
   }   

}