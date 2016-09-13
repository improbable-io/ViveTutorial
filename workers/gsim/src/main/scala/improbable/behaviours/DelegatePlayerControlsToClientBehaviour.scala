package improbable.behaviours


import improbable.corelib.util.EntityOwnerDelegation.entityOwnerDelegation
import improbable.papi.entity.{Entity, EntityBehaviour}
import improbable.player.PlayerControls


class DelegatePlayerControlsToClientBehaviour(entity: Entity) extends EntityBehaviour {

  override def onReady() {
    entity.delegateStateToOwner[PlayerControls]
  }

}