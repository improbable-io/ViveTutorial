package improbable.behaviours

import improbable.corelib.util.EntityOwner
import improbable.papi.engine.EngineId
import improbable.papi.entity.{Entity, EntityBehaviour}
import improbable.papi.world.World
import improbable.papi.world.messaging.CustomMsg
import improbable.player.PlayerControls

case class DelegateToPlayer(clientId: EngineId) extends CustomMsg
case class HeartbeatFromPlayer(clientId: EngineId) extends CustomMsg

class RequestClientControlBehaviour(entity: Entity, world: World) extends EntityBehaviour {

  override def onReady() {
    entity.watch[PlayerControls].onPickUpEvent {
     pickUpObjectData =>
       entity.watch[EntityOwner].bind.ownerId {
         case None =>

         case Some(owner) =>
           world.messaging.sendToEntity(pickUpObjectData.pickUpTargetId, DelegateToPlayer(owner))
       }
    }

    entity.watch[PlayerControls].onGrabbingHeartbeatEvent {
      heartbeatEventData =>
        entity.watch[EntityOwner].bind.ownerId {
          case None =>

          case Some(owner) =>
            world.messaging.sendToEntity(heartbeatEventData.heldTargetId, HeartbeatFromPlayer(owner))
        }
      }
  }
}