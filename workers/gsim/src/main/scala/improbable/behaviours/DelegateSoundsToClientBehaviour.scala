package improbable.behaviours

import improbable.papi.entity.{Entity, EntityBehaviour}
import improbable.sounds.Sounds
import improbable.corelib.util.EntityOwnerDelegation.entityOwnerDelegation

class DelegateSoundsToClientBehaviour(entity: Entity) extends EntityBehaviour {

  override def onReady() {
    entity.delegateStateToOwner[Sounds]
  }

}