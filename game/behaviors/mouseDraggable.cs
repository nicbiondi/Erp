//-----------------------------------------------------------------------------
// Torque Game Builder
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

if (!isObject(MouseDraggableBehavior))
{
   %template = new BehaviorTemplate(MouseDraggableBehavior);
   
   %template.friendlyName = "Mouse Draggable";
   %template.behaviorType = "Mouse Input";
   %template.description = "Make an object draggable by the mouse";
   
   %template.addBehaviorField(centerOnMouse, "Center the object on the mouse", bool, false);
   %template.addBehaviorField(toggleDragState, "Start dragging on one click, stop dragging on the next click", bool, false);
}

function MouseDraggableBehavior::onBehaviorAdd(%this)
{
   %this.dragging = false;
   %this.cancelOnMouseUp = true;
   %this.offset = "0 0";
   %this.owner.setUseMouseEvents(true);
}

function MouseDraggableBehavior::onMouseDown(%this, %modifier, %worldPos)
{
   
    $isOutOfBounds = false;
   // Toggle the drag status.
   %this.dragging = !%this.dragging;
   
   // We always stop dragging on mouse up unless the toggle option is set.
   %this.cancelOnMouseUp = !%this.toggleDragState;
   
   // Schedule this or else we'll get two onMouseDowns. One for the locked
   // objects, and again for the not locked objects.
   %this.owner.schedule(0, setMouseLocked, %this.dragging);
   
   if (!%this.centerOnMouse)
      %this.offset = t2dVectorSub(%this.owner.position, %worldPos);
}

function MouseDraggableBehavior::onMouseUp(%this, %modifier, %worldPos)
{
   if (%this.cancelOnMouseUp)
   {
      %this.dragging = false;
      %this.owner.setMouseLocked(false);
   }
}

function MouseDraggableBehavior::onMouseDragged(%this, %modifier, %worldPos)
{
   if (%this.dragging && $isOutOfBounds == false)
   {
      %this.moveToMouse(%worldPos);
      
      // Once we have dragged, then dragging will always stop on mouse up.
      %this.cancelOnMouseUp = true;
      $dragLastPos=%worldPos;
   }
   else
   {
      %this.moveToMouse($dragLastPos);
   }
}

function MouseDraggableBehavior::onMouseMove(%this, %modifier, %worldPos)
{
   if (%this.dragging&& $isOutOfBounds == false)
   {
      %this.moveToMouse(%worldPos);
          $dragLastPos=%worldPos;
   }

}

function MouseDraggableBehavior::moveToMouse(%this, %worldPos)
{
   %this.owner.position = t2dVectorAdd(%worldPos, %this.offset);
}


