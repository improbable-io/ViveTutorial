package improbable.behaviours

import improbable.papi.entity.{Entity, EntityBehaviour}
import improbable.sounds.Sounds
import improbable.unity.fabric.PhysicsEngineConstraint


class DelegateSoundsToPhysicsEngineBehaviour(entity: Entity) extends EntityBehaviour {

  override def onReady() {
    entity.delegateState[Sounds](PhysicsEngineConstraint)
  }

}