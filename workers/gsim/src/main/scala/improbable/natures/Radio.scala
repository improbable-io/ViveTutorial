package improbable.natures

import improbable.behaviours.DelegateSoundsToPhysicsEngineBehaviour
import improbable.corelib.natures.{BaseNature, NatureApplication, NatureDescription}
import improbable.corelibrary.transforms.TransformNature
import improbable.math.Coordinates
import improbable.papi.entity.EntityPrefab
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.sounds.Sounds

object Radio extends NatureDescription {

  override val dependencies = Set[NatureDescription](BaseNature, TransformNature)

  override def activeBehaviours: Set[EntityBehaviourDescriptor] = {
    Set(descriptorOf[DelegateSoundsToPhysicsEngineBehaviour])
  }

  def apply(spawnPosition: Coordinates): NatureApplication = {
    application(
      states = Seq(
        Sounds()
      ),
      natures = Seq(
        BaseNature(entityPrefab = EntityPrefab("Radio"), isPhysical = true),
        TransformNature(globalPosition = spawnPosition.toVector3d)
      )
    )
  }
}