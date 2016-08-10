package improbable.behaviours
import improbable.corelib.util.EntityOwnerDelegation.entityOwnerDelegation
import improbable.papi.entity.{Entity, EntityBehaviour}
import improbable.player.LocalPlayerCheck

class DelegateLocalPlayerCheckToClientBehaviour(entity: Entity) extends EntityBehaviour {

  override def onReady() {
    entity.delegateStateToOwner[LocalPlayerCheck]
  }
}
