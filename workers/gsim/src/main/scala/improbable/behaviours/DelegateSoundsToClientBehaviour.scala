package improbable.behaviours

import improbable.corelib.util.EntityOwnerDelegation.entityOwnerDelegation
import improbable.papi.entity.{Entity, EntityBehaviour}
import improbable.sounds.Sounds


class DelegateSoundsToClientBehaviour(entity: Entity) extends EntityBehaviour {

  override def onReady() {
    entity.delegateStateToOwner[Sounds]
  }

}