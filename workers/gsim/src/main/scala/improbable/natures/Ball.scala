package improbable.natures

import improbable.corelib.natures.{BaseNature, NatureApplication, NatureDescription}
import improbable.corelibrary.transforms.TransformNature
import improbable.math.{Coordinates, Vector3d}
import improbable.papi.entity.EntityPrefab
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor

object Ball extends NatureDescription {

  override val dependencies = Set[NatureDescription](BaseNature, TransformNature)

  override def activeBehaviours: Set[EntityBehaviourDescriptor] = {
    Set.empty
  }

  def apply(spawnPosition: Coordinates): NatureApplication = {
    application(
      states = Seq(
      ),
      natures = Seq(
        BaseNature(entityPrefab = EntityPrefab("Ball"), isPhysical = true),
        TransformNature(globalPosition = spawnPosition.toVector3d)
      )
    )
  }
}